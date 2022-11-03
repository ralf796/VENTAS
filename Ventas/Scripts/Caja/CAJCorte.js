$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    GetDatos();
    function GetDatos() {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/CAJCorte/GetCorte',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: {},
                    cache: false,
                    success: function (data) {
                        console.log(data);
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
                    dataField: "ID_VENTA",
                    caption: "ID_VENTA",
                    alignment: "center",
                    visible: false
                },
                {
                    dataField: "TOTAL",
                    caption: "TOTAL"
                },
                {
                    dataField: "FECHA_CREACION_STRING",
                    caption: "FECHA",
                    alignment: "center"
                },
                {
                    dataField: "CREADO_POR",
                    caption: "VENDEDOR",
                    alignment: "center"
                }
            ]
        }).dxDataGrid('instance');
    }
    /*-------------------------------funcion corte------------------------------*/
    function aplicarCorte() {
        $.ajax({
            type: 'GET',
            url: "/CAJCorte/GetAplicarCorte",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {},
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Corte realizada exitosamente')
                    $('#modalCerrarCorte').modal('hide');
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    /*------------------------------boton corte----------------------------------*/
    $('#btnAbrirCorte').on('click', function (e) {
        e.preventDefault();
        $('#modalCerrarCorte').modal('show');
    })
    $('#btnAnular').on('click', function () {
        aplicarCorte();
    })
})