﻿let fechaI;
let fechaF;
$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    fechaI = new AirDatepicker('#txtFecha', {
        autoClose: true,
        autoClose: true,
        view: 'days',
        minView: 'days',
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date(new Date().getFullYear(), new Date().getMonth(), 1)]
        /* //onSelect: GetDataTable*/
    });
    /*-------------------------FUNCION DE FECHAS ----------------------------------*/
    function GetReporte(fecha) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/REPAbonos/GetReporte',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { fecha },
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
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'REPORTE DE ABONOS.xlsx');
                    });
                });
                e.cancel = true;
            },
            columns: [
                {
                    dataField: "ID_COBRO",
                    caption: "NO. RECIBO",
                    alignment: "center"
                },
                {
                    dataField: "ID_ESTADO_CUENTA",
                    caption: "NO. CRÉDITO",
                    alignment: "center"
                },
                {
                    dataField: "ID_VENTA",
                    caption: "NO. VENTA",
                    alignment: "center"
                },
                {
                    dataField: "NOMBRE",
                    caption: "CLIENTE"
                },
                {
                    dataField: "MONTO",
                    caption: "ABONO",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "DESCRIPCION",
                    caption: "OBSERVACIONES"
                },
                {
                    dataField: "CREADO_POR",
                    caption: "CREADO POR",
                    alignment: "center"
                },
                {
                    dataField: "FECHA_CREACION_STRING",
                    caption: "FECHA CREACIÓN"
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
        if (fechaI.lastSelectedDate != undefined ) {
            let fechaInicial = DateFormat(fechaI.lastSelectedDate);
            GetReporte(fechaInicial);            
        }
        else {
            ShowAlertMessage('warning', 'EL INGRESO DE LA FECHA ES OBLIGATORIO')
        }
    })

});