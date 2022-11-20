$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    /*-------------------------FUNCION DE FECHAS ----------------------------------*/
    function reporteProductoUnico(codigo) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/REPProductoUnico/getProductoUnico',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: {codigo},
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
                var worksheet = workbook.addWorksheet('REPORTE DE PRODUCTOS VENDIDOS EL EDEN');
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
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'REPORTE_PRODUCTOS.xlsx');
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
                    dataField: "CODIGO",
                    caption: "CODIGO"
                },
                {
                    dataField: "CODIGO2",
                    caption: "CODIGO2"
                },
               
                {
                    dataField: "STOCK",
                    caption: "STOCK",
                    alignment: "center"
                },
                {
                    dataField: "PRECIO_COSTO",
                    caption: "PRECIO COSTO",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 },
                    alignment: "center"
                },
                {
                    dataField: "PRECIO_VENTA",
                    caption: "PRECIO VENTA",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 },
                    alignment: "center"
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

    /*----------------------------boton informe-----------------------------*/
    $('#botonInforme').on('click', function (e) {
        e.preventDefault();
        var codigo = $('#txtProducto2').val();
        console.log(codigo);
        if (codigo != "") {
            reporteProductoUnico(codigo);
        }
        
        else {
            ShowAlertMessage('warning', 'INGRESE UN CODIGO DE PRODUCTO')
        }
    })
});