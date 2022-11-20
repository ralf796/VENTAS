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

    function ventaReporte(fechaInicial, fechaFinal) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/REPProductos/getProducto',
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
                    dataField: "NOMBRE",
                    caption: "NOMBRE"
                },
                {
                    dataField: "DESCRIPCION",
                    caption: "DESCRIPCION",
                    visible:false
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
                },
                {
                    dataField: "STOCK",
                    caption: "STOCK",
                    alignment: "center"
                },
                {
                    dataField: "CANTIDAD",
                    caption: "CANTIDAD VENDIDA",
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
                argumentField: 'NOMBRE',
                valueField: 'CANTIDAD',
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