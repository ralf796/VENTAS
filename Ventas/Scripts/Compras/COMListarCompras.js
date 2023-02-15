let fechaI;
let fechaF;
$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    fechaI = new AirDatepicker('#txtFechaI', {
        autoClose: true,
        autoClose: true,
        view: 'days',
        minView: 'days',
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date(new Date().getFullYear(), new Date().getMonth(), 1)]
        /* //onSelect: GetDataTable*/
    });
    fechaF = new AirDatepicker('#txtFechaF', {
        autoClose: true,
        autoClose: true,
        view: 'days',
        minView: 'days',
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0)]

        /*     onSelect: GetDataTable*/
    });

    var cont = 0;
    function GenerarReporte(fechaInicial, fechaFinal) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/COMListarCompras/GetDatos',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { fechaInicial, fechaFinal },
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
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'COMPRAS.xlsx');
                    });
                });
                e.cancel = true;
            },
            columns: [
                {
                    dataField: "ID_COMPRA",
                    caption: "NO. DE COMPRA",
                    alignment: "center"
                },
                {
                    dataField: "NOMBRE_PROVEEDOR",
                    caption: "NOMBRE PROVEEDOR"
                },
                {
                    dataField: "SERIE_FACTURA",
                    caption: "SERIE",
                    alignment: "center"
                },
                {
                    dataField: "MONTO_FACTURA",
                    caption: "TOTAL",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "FECHA_ENTREGA_STRING",
                    caption: "FECHA INGRESO",
                    alignment: "center"
                },
                {
                    dataField: "TELEFONO_PROVEEDOR",
                    caption: "TIPO PAGO",
                    alignment: "center"
                },
                {
                    dataField: "CONTACTO_PROVEEDOR",
                    caption: "OBSERVACIONES",
                    alignment: "center"
                },
                {
                    caption: 'DOCUMENTO 1',
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        //<a href="http://www.example.org" target="_blank">http://www.example.org</a>
                        if (fieldData.FILE1 != '')
                            $("<a>").attr('href', fieldData.FILE1).attr('target', '_blank').addClass('form-control text-center').text('VER DOCUMENTO').appendTo(container);
                    }
                },
                {
                    caption: 'DOCUMENTO 2',
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        //<a href="http://www.example.org" target="_blank">http://www.example.org</a>
                        if (fieldData.FILE2 != '')
                            $("<a>").attr('href', fieldData.FILE2).attr('target', '_blank').addClass('form-control text-center').text('VER DOCUMENTO').appendTo(container);
                    }
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

    $('#btnGenerar').on('click', function (e) {
        e.preventDefault();

        if (fechaI.lastSelectedDate != undefined && fechaF.lastSelectedDate != undefined) {
            let fechaInicial = DateFormat(fechaI.lastSelectedDate);
            let fechaFinal = DateFormat(fechaF.lastSelectedDate);
            GenerarReporte(fechaInicial, fechaFinal);
        }
        else {
            ShowAlertMessage('warning', 'EL INGRESO DE AMBAS FECHAS ES OBLIGATORIO')
        }
    })

});