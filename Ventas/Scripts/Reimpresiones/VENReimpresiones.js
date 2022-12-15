let fecha;
$(document).ready(function () {
    var cont = 0;
    DevExpress.localization.locale(navigator.language);
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

    function GetDatos(fecha) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/VENReimpresiones/GetDatos',
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
                enabled: false
            },
            columns: [
                {
                    caption: '',
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        var classTmp3 = 'reimpreFEL' + cont;
                        var classBTN3 = 'ml-2 hvr-grow far fa-print btn btn-primary ' + classTmp3;
                        $("<span>").addClass(classBTN3).prop('title', 'Reimpresión FEL').appendTo(container);
                        $('.reimpreFEL' + cont).click(function (e) {
                            var url = "https://report.feel.com.gt/ingfacereport/ingfacereport_documento?uuid=" + fieldData.UUID;
                            window.open(url, '_blank');
                        })
                        cont++;
                    }
                },
                {
                    dataField: "ID_VENTA",
                    caption: "VENTA"
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
                    caption: "FECHA"
                },
                {
                    dataField: "NIT",
                    caption: "NIT"
                },
                {
                    dataField: "NOMBRE",
                    caption: "CLIENTE"
                },
                {
                    dataField: "CREADO_POR",
                    caption: "VENDEDOR"
                },
                {
                    dataField: "UUID",
                    caption: "UUID"
                },
                {
                    dataField: "ESTADO_STRING",
                    caption: "ESTADO"
                },
                {
                    dataField: "TOTAL",
                    caption: "TOTAL",
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
    }

    $('#btnGenerar').on('click', function (e) {
        e.preventDefault();
        GetDatos(DateFormat(fecha.lastSelectedDate));
    })
});