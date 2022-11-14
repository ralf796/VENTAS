let fecha;
$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    var cont = 0;

    function DateFormat(date) {
        try {
            let d = date;
            let month = (d.getMonth() + 1).toString().padStart(2, '0');
            let day = d.getDate().toString().padStart(2, '0');
            let year = d.getFullYear();
            return [year, month, day].join('-');
        } catch (err) {
            return '';
        }
    }
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
                    dataField: "FECHA_CREACION_STRING",
                    caption: "FECHA"
                },
                {
                    dataField: "NOMBRE_COMPLETO",
                    caption: "NOMBRE PRODUCTO"
                },
                {
                    dataField: "DESCRIPCION_BITACORA",
                    caption: "DESCRIPCION BITACORA"
                },
                {
                    dataField: "STOCK",
                    caption: "STOCK"
                },
                {
                    dataField: "CREADO_POR",
                    caption: "USUARIO MODIFICA"
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
                }
            ]

        }).dxDataGrid('instance');
    }

    $('#btnBuscar').on('click', function (e) {
        e.preventDefault();
        GetDatos(DateFormat(fecha.lastSelectedDate));
    })

});

