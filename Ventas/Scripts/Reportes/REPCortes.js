let fechaI;
let fechaF;
$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
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
    var cont = 0;

    /*-------------------------FUNCION DE FECHAS ----------------------------------*/
    function cobroReporte(fechaInicial, fechaFinal) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/REPCortes/getCortes',
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
                var worksheet = workbook.addWorksheet('Hoja 1');
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
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'REPORTE DE CORTES.xlsx');
                    });
                });
                e.cancel = true;
            },
            columns: [
                {
                    dataField: "ID_CORTE",
                    caption: "NO. CORTE",
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
                    dataField: "TOTAL",
                    caption: "TOTAL",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    caption: "DETALLE CORTE",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        var classTmp = 'detalle' + cont;
                        var classBTN = 'hvr-grow text-dark far fa-eye btn btn-info ' + classTmp;

                        $("<span>").addClass(classBTN).prop('title', 'Ver detalle').appendTo(container);
                        $('.detalle' + cont).click(function (e) {
                            mostrarDetalle(fieldData.ID_CORTE);
                        })

                        var classTmp2 = 'detalle_corte' + cont;
                        var classBTN2 = 'hvr-grow text-dark fad fa-print btn btn-danger ' + classTmp2;

                        $("<span>").addClass(classBTN2).prop('title', 'Imprimir corte').appendTo(container);
                        $('.detalle_corte' + cont).click(function (e) {
                            PDF_Corte(fieldData.ID_CORTE);
                        })
                        cont++;
                    }
                },
            ],
            onCellPrepared: function (e) {
                if (e.rowType === 'header') {
                    //e.cellElement.css("background", "var(--secondary)");
                    e.cellElement.css("background", "#5F6A6A");
                    e.cellElement.css("color", "#FFFFFF");
                }
            }
        }).dxDataGrid('instance');
        $('#chart').dxPieChart({
            palette: 'bright',
            dataSource: new DevExpress.data.DataSource(customStore),
            title: 'PRODUCTOS MAS VENDIDOS',
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
                argumentField: 'FECHA_CREACION_STRING',
                valueField: 'TOTAL',
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
            url: '/REPCortes/getTotalCorte',
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
                    document.querySelector('#totalCortes').textContent = formatNumber(parseFloat(venta_T).toFixed(2));

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
        }
        else {
            ShowAlertMessage('warning', 'EL INGRESO DE AMBAS FECHAS ES OBLIGATORIO')
        }

        // 
    })

    function mostrarDetalle(id) {
        var id_venta = parseInt(id);
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/REPCortes/GetDetalle',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { id_venta },
                    cache: false,
                    success: function (data) {
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
        var salesPivotGrid = $("#gridContainer2").dxDataGrid({
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
                enabled: false
            },
            columns: [
                {
                    dataField: "ID_CORTE",
                    caption: "CORTE"
                },
                {
                    dataField: "ID_COBRO",
                    caption: "COBRO / RECIBO",
                    alignment: "center"
                },
                {
                    dataField: "ID_VENTA",
                    caption: "NO. VENTA",
                    alignment: "center"
                },
                {
                    dataField: "DESCRIPCION",
                    caption: "DESCRIPCION"
                },
                {
                    dataField: "MONTO",
                    caption: "MONTO",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "CREADO_POR",
                    caption: "CREADO POR"
                },
                {
                    dataField: "FECHA_CREACION_STRING",
                    caption: "FECHA"
                }
            ],
        }).dxDataGrid('instance');
        $('#modalDetalle').modal('show');
    }


    function PDF_Corte(corte) {
        CallLoadingFire('Generando impresión de corte, por favor espere.');
        $.post("/REPCortes/PDF_Corte", {
            corte
        }, function (result) {
            var pom = document.createElement('a');
            pom.setAttribute('href', 'data:' + result.MimeType + ';base64,' + result.File);
            pom.setAttribute('download', result.FileName);
            if (document.createEvent) {
                var event = document.createEvent('MouseEvents');
                event.initEvent('click', true, true);
                pom.dispatchEvent(event);
            }
            else {
                pom.click();
            }
            CallToast('Descarga realizada con éxito.', true, 2300, '#9EC600');
        });
    }
});