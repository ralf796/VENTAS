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

    /*-------------------------FUNCION DE FECHAS ----------------------------------*/
    function GetDatos(fechaInicial, fechaFinal) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/REPAnulaciones/GetDatos',
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
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'REPORTE DE ANULACIONES.xlsx');
                    });
                });
                e.cancel = true;
            },
            columns: [
                {
                    dataField: "ID_VENTA",
                    caption: "NO. DE COMPRA",
                    alignment: "center"
                },
                {
                    dataField: "SERIE",
                    caption: "SERIE"
                },
                {
                    dataField: "CORRELATIVO",
                    caption: "CORRELATIVO"
                },
                {
                    dataField: "FECHA_CREACION_STRING",
                    caption: "FECHA",
                    alignment: "center"
                },
                {
                    dataField: "SUBTOTAL",
                    caption: "SUBTOTAL",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "TOTAL_DESCUENTO",
                    caption: "DESCUENTO",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "TOTAL",
                    caption: "TOTAL",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "TIPO_COBRO",
                    caption: "MODO DE PAGO"
                },
                {
                    dataField: "UUID",
                    caption: "FACTURA FEL"
                },
                {
                    dataField: "CREADO_POR",
                    caption: "VENDEDOR",
                    alignment: "center"
                },
                {
                    dataField: "NOMBRE",
                    caption: "NOMBRE CLIENTE"
                },
                {
                    dataField: "NIT",
                    caption: "NIT CLIENTE"
                },
                {
                    dataField: "ID_COBRO",
                    caption: "COBRO"
                },
                {
                    dataField: "ID_CORTE",
                    caption: "CORTE"
                }
            ],
            onCellPrepared: function (e) {
                if (e.rowType === 'header') {
                    //e.cellElement.css("background", "var(--secondary)");
                    e.cellElement.css("background", "#5F6A6A");
                    e.cellElement.css("color", "#FFFFFF");
                }
            }
        }).dxDataGrid('instance');
    }

    $('#botonInforme').on('click', function (e) {
        e.preventDefault();

        if (fechaI.lastSelectedDate != undefined && fechaF.lastSelectedDate != undefined) {
            let fechaInicial = DateFormat(fechaI.lastSelectedDate);
            let fechaFinal = DateFormat(fechaF.lastSelectedDate);
            GetDatos(fechaInicial, fechaFinal);
        }
        else {
            ShowAlertMessage('warning', 'EL INGRESO DE AMBAS FECHAS ES OBLIGATORIO')
        }

        // 
    })

});