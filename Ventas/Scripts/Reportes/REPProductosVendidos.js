let fecha;
$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    var cont = 0;
    var f = new Date();
    fecha = new AirDatepicker('#txtFecha', {
        autoClose: true,
        autoClose: true,
        view: 'days',
        minView: 'days',
        minDate: f.setDate(f.getDate() - 7),
        maxDate: new Date(),
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date()]
    });

    function GetDatos(fechaInicial) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/REPProductosVendidos/GetDatosSP',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { fechaInicial},
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
                var worksheet = workbook.addWorksheet('REPORTE BITACORAS EL EDEN');
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
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'REPORTE_BITACORA.xlsx');
                    });
                });
                e.cancel = true;
            },
            columns: [
                {
                    dataField: "CODIGO_INTERNO",
                    caption: "CODIGO INTERNO"
                },
                {
                    dataField: "FECHA_CREACION_STRING",
                    caption: "FECHA Y HORA"
                },
                {
                    dataField: "CANTIDAD",
                    caption: "CANTIDAD"
                },
                {
                    dataField: "CODIGO",
                    caption: "CODIGO 1",
                    alignment: "center"
                },
                {
                    dataField: "CODIGO2",
                    caption: "CODIGO 2",
                    alignment: "center"
                },
                {
                    dataField: "NOMBRE",
                    caption: "NOMBRE"
                },
                {
                    dataField: "PRECIO_UNITARIO",
                    caption: "PRECIO UNITARIO",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "TOTAL",
                    caption: "TOTAL SIN DESCUENTO",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "TOTAL_DESCUENTO",
                    caption: "DESCUENTO TOTAL",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "SUBTOTAL",
                    caption: "TOTAL CON DESCUENTO",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
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

    $('#btnBuscar').on('click', function (e) {
        e.preventDefault();
        GetDatos(DateFormat(fecha.lastSelectedDate));
    })
});

