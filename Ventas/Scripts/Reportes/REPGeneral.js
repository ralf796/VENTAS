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
    function ventaReporte(fechaInicial, fechaFinal) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/REPGeneral/getGeneral',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { fechaInicial, fechaFinal },
                    cache: false,
                    success: function (data) {

                        /* console.log(data);*/
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
                var worksheet = workbook.addWorksheet('REPORTE GENERAL');
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
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'REPORTE_GENERAL.xlsx');
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
                    caption: "FECHA CORTE"
                },
                {
                    dataField: "CAJERO_CORTE",
                    caption: "CAJERO CORTE",
                    alignment: "left"
                },
                {
                    dataField: "ID_COBRO",
                    caption: "NO. COBRO",
                    alignment: "center"
                },
                {
                    dataField: "CAJERO_COBRO",
                    caption: "CAJERO COBRO",
                    alignment: "left"
                },
                {
                    dataField: "ID_VENTA",
                    caption: "NO. COMPRA",
                    alignment: "center"
                },
                {
                    dataField: "SERIE",
                    caption: "SERIE",
                    alignment: "left"
                },
                {
                    dataField: "CORRELATIVO",
                    caption: "CORRELATIVO",
                    alignment: "left"
                },
                {
                    dataField: "NOMBRE",
                    caption: "NOMBRE",
                    alignment: "left"
                },
                {
                    dataField: "NIT",
                    caption: "NIT",
                    alignment: "left"
                },
                {
                    dataField: "TELEFONO",
                    caption: "TELEFONO",
                    alignment: "center",
                    visible:false
                },
                {
                    dataField: "DIRECCION",
                    caption: "DIRECCION",
                    alignment: "left"
                },
                {
                    dataField: "CANTIDAD",
                    caption: "CANTIDAD",
                    alignment: "center"
                },
                {
                    dataField: "NOMBRE_PRODUCTO",
                    caption: "NOMBRE PRODUCTO",
                },
                {
                    dataField: "PRECIO_UNITARIO",
                    caption: "PRECIO UNITARIO",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 },
                    alignment: "center"
                },
                {
                    dataField: "DESCUENTO",
                    caption: "DESCUENTO",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 },
                    alignment: "center"
                },
                {
                    dataField: "SUBTOTAL",
                    caption: "SUBTOTAL",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 },
                    alignment: "center"
                }
            ]
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
                valueField: 'SUBTOTAL',
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

    /*----------------------------boton informe-----------------------------*/
    $('#botonInforme').on('click', function (e) {
        e.preventDefault();
        
        if (fechaI.lastSelectedDate != undefined && fechaF.lastSelectedDate != undefined) {
            let fechaInicial = DateFormat(fechaI.lastSelectedDate);
            let fechaFinal = DateFormat(fechaF.lastSelectedDate);
            ventaReporte(fechaInicial, fechaFinal);
        }
        else {
            ShowAlertMessage('warning', 'EL INGRESO DE AMBAS FECHAS ES OBLIGATORIO')
        }
    })
});