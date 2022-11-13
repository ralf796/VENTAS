let fechaI;
let fechaF;
$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    function DatePicker(obj, options = null) {
        var air = new AirDatepicker(obj, options);
        return air;
    }

    function DateFormat(date) {
        try {
            let d = date;
            let month = (d.getMonth() + 1).toString().padStart(2, '0');
            let day = d.getDate().toString().padStart(2, '0');
            let year = d.getFullYear();
            //console.log(month);
            //console.log(day);
            //console.log(year);
            return [year, month, day].join('-');
        } catch (err) {
            return '';
        }
    }
    fechaI = new AirDatepicker('#txtAnioI', {
        autoClose: true,
        autoClose: true,
        view: 'days',
        minView: 'days',
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date(new Date().getFullYear(), new Date().getMonth(), 1)]

        /* //onSelect: GetDataTable*/
    });
    fechaF = new AirDatepicker('#txtAnioF', {
        autoClose: true,
        autoClose: true,
        view: 'days',
        minView: 'days',
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0)]

        /*     onSelect: GetDataTable*/
    });

    /*-------------------------FUNCION DE FECHAS ----------------------------------*/
    function cobroReporte(fechaInicial, fechaFinal) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/REPCobros/getCobro',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { fechaInicial, fechaFinal },
                    cache: false,
                    success: function (data) {

                         console.log(data);
                        var state = data["State"];
                        if (state == 1) {
                            data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);

                            d.resolve(data);

                        }
                        else if (state == -1)
                            alert(data["Message"])
                    },
                    error: function (jqXHR, exception) {
                        getErrorMessage(jqXHR, exception);
                    }
                });
                return d.promise();
            }
        });
        var salesPivotGrid = $("#gridContainer").dxDataGrid({
            dataSource: new DevExpress.data.DataSource(customStore),
            showBorders: true,
            loadPanel: {
                text: "Cargando..."
            },
            scrolling: {
                useNative: false,
                scrollByContent: true,
                scrollByThumb: true,
                showScrollbar: "always" // or "onScroll" | "always" | "never"
            },
            searchPanel: {
                visible: true,
                width: 200,
                placeholder: "Buscar..."
            },
            headerFilter: {
                visible: true
            },
            columnAutoWidth: true,
            export: {
                enabled: true
            },
            onExporting: function (e) {
                var workbook = new ExcelJS.Workbook();
                var worksheet = workbook.addWorksheet('REPORTE DE COBROS');
                DevExpress.excelExporter.exportDataGrid({
                    worksheet: worksheet,
                    component: e.component,
                    customizeCell: function (options) {
                        var excelCell = options;
                        excelCell.font = { name: 'Arial', size: 12 };
                        excelCell.alignment = { horizontal: 'left' };
                        excelCell.columnCount;
                    }
                }).then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'REPORTE_COBROS.xlsx');
                    });
                });
                e.cancel = true;
            },
            columns: [
                {
                    dataField: "ID_COBRO",
                    caption: "NO. COBRO",
                    alignment: "center"
                },
                {
                    dataField: "ID_VENTA",
                    caption: "NO. COMPRA",
                    alignment: "center"
                },
                {
                    dataField: "DESCRIPCION",
                    caption: "TIPO COBRO",
                    alignment: "center"
                },
                {
                    dataField: "FECHA_CREACION_STRING",
                    caption: "FECHA",
                    alignment: "center"
                },
                {
                    dataField: "CREADO_POR",
                    caption: "CAJERO",
                    alignment: "center"
                },
                {
                    dataField: "MONTO",
                    caption: "MONTO",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 },
                    alignment: "right"
                }          
            ]
        }).dxDataGrid('instance');
        $('#chart').dxPieChart({
            palette: 'bright',
            dataSource: new DevExpress.data.DataSource(customStore),
            title: 'REPORTE DE COBROS',
            legend: {
                orientation: 'horizontal',
                itemTextPosition: 'right',
                horizontalAlignment: 'center',
                verticalAlignment: 'bottom',
                columnCount: 4,
            },
            export: {
                enabled: true,
            },
            series: [{
                argumentField: 'DESCRIPCION',
                valueField: 'MONTO',
                label: {
                    visible: true,
                    font: {
                        size: 16,
                    },
                    connector: {
                        visible: true,
                        width: 0.5,
                    },
                    position: 'columns',
                    customizeText(arg) {
                        return `${arg.valueText} (${arg.percentText})`;
                    },
                },
            }],
        });
    }

    /*------------------------FUNCION TOTAL VENTAS*-------------------------------*/
    function cobroTotal(fechaInicial, fechaFinal) {
        $.ajax({
            type: 'GET',
            url: '/REPCobros/getTotalMonto',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { fechaInicial, fechaFinal },
            cache: false,
            success: function (data) {
                console.log(data);
                
                var state = data["State"];
                if (state == 1) {
                    data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);
                    var venta_T = data.data[0].TOTAL_VENTA;
                    document.querySelector('#totalCobros').textContent = formatNumber(parseFloat(venta_T).toFixed(2));

                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    /*----------------------------funcion total en efectivo-----------------------------*/
    function cobroEfectivo(fechaInicial, fechaFinal) {
        $.ajax({
            type: 'GET',
            url: '/REPCobros/getTotalEfectivo',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { fechaInicial, fechaFinal },
            cache: false,
            success: function (data) {
                console.log(data);

                var state = data["State"];
                if (state == 1) {
                    data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);
                    var venta_T = data.data[0].TOTAL_EFECTIVO;
                    document.querySelector('#totalCobrosEfectivo').textContent = formatNumber(parseFloat(venta_T).toFixed(2));

                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    /*----------------------------funcion total en tarjeta-----------------------------*/
    function cobroTarjeta(fechaInicial, fechaFinal) {
        $.ajax({
            type: 'GET',
            url: '/REPCobros/getTotalTarjeta',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { fechaInicial, fechaFinal },
            cache: false,
            success: function (data) {
                console.log(data);

                var state = data["State"];
                if (state == 1) {
                    data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);
                    var venta_T = data.data[0].TOTAL_TARJETA;
                    document.querySelector('#totalCobrosTarjeta').textContent = formatNumber(parseFloat(venta_T).toFixed(2));

                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    /*----------------------------boton informe-----------------------------*/
    $('#botonInforme').on('click', function (e) {
        e.preventDefault();

        if (fechaI.lastSelectedDate != undefined && fechaF.lastSelectedDate != undefined) {
            let fechaInicial = DateFormat(fechaI.lastSelectedDate);
            let fechaFinal = DateFormat(fechaF.lastSelectedDate);
            /* console.log(fechaInicial);*/
            cobroReporte(fechaInicial, fechaFinal);
            cobroTotal(fechaInicial, fechaFinal);
            cobroEfectivo(fechaInicial, fechaFinal);
            cobroTarjeta(fechaInicial, fechaFinal);
        }
        else {
            ShowAlertMessage('warning', 'EL INGRESO DE AMBAS FECHAS ES OBLIGATORIO')
        }

        // 
    })

});