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
        maxDate: new Date(),
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date()]
    });

    function GetDatos(fechaInicial) {
        var tipo = 13;
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/REPBitacora/GetDatosSP',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { fechaInicial, tipo },
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
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'BITACORA DE TRANSACCIONES.xlsx');
                    });
                });
                e.cancel = true;
            },
            columns: [
                {
                    dataField: "TIPO_REPORTE",
                    caption: "TIPO BITACORA"
                },
                {
                    dataField: "FECHA_CREACION_STRING",
                    caption: "FECHA"
                },
                {
                    dataField: "DESCRIPCION_BITACORA",
                    caption: "DESCRIPCION BITACORA"
                },
                {
                    dataField: "CREADO_POR",
                    caption: "USUARIO MODIFICA"
                },
                {
                    dataField: "CODIGO_INTERNO",
                    caption: "CODIGO INTERNO"
                },
                {
                    dataField: "NOMBRE_COMPLETO",
                    caption: "NOMBRE PRODUCTO"
                },                
                {
                    dataField: "STOCK",
                    caption: "STOCK"
                },                
                {
                    dataField: "PRECIO_COSTO",
                    caption: "PRECIO COSTO",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "PRECIO_VENTA",
                    caption: "PRECIO VENTA",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "NOMBRE_CLIENTE",
                    caption: "NOMBRE CLIENTE"
                },
                {
                    dataField: "NIT",
                    caption: "NIT CLIENTE"
                },
                {
                    dataField: "FECHA_VENTA_STRING",
                    caption: "FECHA VENTA"
                },
            ],
            onRowPrepared(e) {
                if (e.rowType == 'data' && e.data.TIPO_REPORTE == 'BITACORA ANULACIONES') {
                    e.rowElement.css("background-color", "#EC7063");
                    e.rowElement.css("color", "#FFFFFF");
                }
                if (e.rowType == 'data' && e.data.TIPO_REPORTE == 'BITACORA PRODUCTOS') {
                    e.rowElement.css("background-color", "#45B39D");
                    e.rowElement.css("color", "#FFFFFF");
                }
                if (e.rowType == 'data' && e.data.TIPO_REPORTE == 'BITACORA PRODUCTOS-VENTAS') {
                    e.rowElement.css("background-color", "#45B39D");
                    e.rowElement.css("color", "#FFFFFF");
                }
            },
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