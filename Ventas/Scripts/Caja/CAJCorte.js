$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    GetDatos();
    GetTotalTarjeta();
    GetTotalEfectivo();
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
                    dataField: "ID_VENTA",
                    caption: "ID_VENTA",
                    alignment: "center",
                    visible: false
                },
                {
                    dataField: "TOTAL",
                    caption: "TOTAL",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
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
                    ShowAlertMessage('success', 'Corte realizado exitosamente')
                    $('#modalCerrarCorte').modal('hide');
                    $('#totalCobrosTarjeta').text(0.00);
                    $('#totalCobrosEfectivo').text(0.00);
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    /*-----------------------------FUNCION TOTAL VENTA----------------------------*/
    function totalVenta() {
        $.ajax({
            type: 'GET',
            url: "/CAJCorte/TotalVentaCorte",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {},
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);
                    var venta_T = data.data[0].TOTAL_VENTA2;
                    document.querySelector('#ventaTotal').textContent = formatNumber(parseFloat(venta_T).toFixed(2));
                    $('#modalCerrarCorte').modal('show');
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
        totalVenta();
      
    })
    $('#btnAnular').on('click', function () {
        aplicarCorte();
    })

    /*-*--------------------------------------función total tarjeta-----------------------*/
    function GetTotalTarjeta() {
        $.ajax({
            type: 'GET',
            url: "/CAJCorte/GetTotalTarjeta",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {},
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);
                    console.log(data);
                    var montoTotal = data.data[0].MONTO_TARJETA;
                    document.querySelector('#totalCobrosTarjeta').textContent = formatNumber(parseFloat(montoTotal).toFixed(2));
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    /*-*--------------------------------------función total efectivo-----------------------*/
    function GetTotalEfectivo() {
        $.ajax({
            type: 'GET',
            url: "/CAJCorte/GetTotalEfectivo",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {},
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);
                    console.log(data);
                    var montoTotal = data.data[0].MONTO_EFECTIVO;
                    document.querySelector('#totalCobrosEfectivo').textContent = formatNumber(parseFloat(montoTotal).toFixed(2));
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
})