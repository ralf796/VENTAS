$(document).ready(function () {
    //$("#acordion").accordion({
    //    collapsible: true
    //});
    DevExpress.localization.locale(navigator.language);
    GetDatos();
    var cont = 0;
    function GetDatos() {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/CAJCobro/GetCobro',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: {},
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
                enabled: false
            },
            columns: [
                {
                    dataField: "ID_VENTA",
                    caption: "ORDEN DE COMPRA",
                    alignment: "center"
                },
                {
                    dataField: "FECHA_CREACION_STRING",
                    caption: "FECHA"
                },
                {
                    dataField: "NOMBRE",
                    caption: "CLIENTE"
                },
                {
                    dataField: "CREADO_POR",
                    caption: "VENDEDOR",
                    alignment: "center"
                },
                {
                    dataField: "TOTAL",
                    caption: "TOTAL",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },

                {
                    caption: "DETALLE VENTA",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        var classTmp = 'detalle' + cont;
                        var classBTN = 'hvr-grow text-dark far fa-eye btn btn-success ' + classTmp;

                        $("<span>").addClass(classBTN).prop('title', 'Ver detalle').appendTo(container);
                        $('.detalle' + cont).click(function (e) {

                            mostrarDetalle(fieldData.ID_VENTA);
                        })
                        cont++;
                    }
                },
                {
                    caption: "COBRAR VENTA",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        var classTmp = 'cobrar' + cont;
                        var classBTN = 'hvr-grow text-dark far fa-money-bill-alt btn btn-danger ' + classTmp;

                        $("<span>").addClass(classBTN).prop('title', 'cobrar').appendTo(container);
                        $('.cobrar' + cont).click(function (e) {
                            var total = parseFloat(fieldData.TOTAL);
                            var id = parseInt(fieldData.ID_VENTA);
                            var creadoPor = fieldData.CREADO_POR;
                            //mostrarCobro(fieldData.TOTAL, fieldData.ID_VENTA);
                            document.querySelector('#totalCobro').textContent = formatNumber(parseFloat(total).toFixed(2));
                            $('#hfID').val(id);
                            $('#hfOpcion').val(creadoPor);
                            $('#hfMonto').val(total);
                            $('#modalCobro').modal('show');

                        })
                        cont++;
                    }
                },
                {
                    caption: "ANULAR VENTA",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        var classTmp = 'anular' + cont;
                        var classBTN = 'hvr-grow text-dark far fa-trash-alt btn btn-warning ' + classTmp;

                        $("<span>").addClass(classBTN).prop('title', 'cobrar').appendTo(container);
                        $('.anular' + cont).click(function (e) {
                            var id = parseInt(fieldData.ID_VENTA);
                            $('#hfID').val(id);
                            $('#modalAnulado').modal('show');
                        })
                        cont++;
                    }
                }
            ],
            //masterDetail: {
            //    enabled: true,
            //    template(container, options) {
            //        $('<div>').appendTo(container).dxDataGrid({
            //            dataSource: {
            //                store: ordersStore,
            //                filter: ['ID_VENTA', '=', options.key],

            //                reshapeOnPush: true,
            //            },
            //            repaintChangesOnly: true,
            //            columnAutoWidth: true,
            //            showBorders: true,
            //            paging: {
            //                pageSize: 3,
            //            },
            //            columns: [{
            //                caption: "venta detalle"
            //            }, {
            //                caption: "Peta"
            //            }, {
            //                caption: "ASADO"
            //            }]
            //        })
            //    }
            //}

        }).dxDataGrid('instance');
    }

    /*-----------------------------FUNCION MOSTRAR DETALLE----------------------------------------*/
    function mostrarDetalle(id) {
        var id_venta = parseInt(id);
        console.log("el id es: ", typeof id_venta)
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/CAJCobro/GetDetalle',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { id_venta },
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
        var salesPivotGrid = $("#gridContainer2").dxDataGrid({
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
                    dataField: "NOMBRE",
                    caption: "PRODUCTO"
                },
                {
                    dataField: "PRECIO_UNITARIO",
                    caption: "P/U",
                    alignment: "center"
                },
                {
                    dataField: "CANTIDAD",
                    caption: "CANTIDAD",
                    alignment: "center"
                },
                {
                    dataField: "SUBTOTAL",
                    caption: "D/U",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "TOTAL",
                    caption: "TOTAL",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },

            ],
        }).dxDataGrid('instance');
        $('#modalDetalle').modal('show');
    }
    /*--------------------------------FUNCIÓN COBRO---------------------------------------------*/
    //function mostrarCobro(venta, id) {

    //}

    /*-------------------------------funcion cobrar-----------------------------*/
    function getCobro(id, cobro, formaPago, creado_por) {

        $.ajax({
            type: 'GET',
            url: "/CAJCobro/getCobroEfectuado",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                id, cobro, formaPago, creado_por
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Cobro realizada exitosamente')
                    $('#txtCobro').val('');
                    $('#selTipoPago').val('');
                    $('#modalCobro').modal('hide');
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    /*-----------------------------BOTON  COBRAR------------------------------------*/
    $('#btnCobrar').on('click', function (e) {
        e.preventDefault();
        var cobro = parseFloat($('#hfMonto').val());
        var formaPago = $('#selTipoPago').val();
        var id = parseInt($('#hfID').val());
        var creado_por = $('#hfOpcion').val();
        if (formaPago != null)
            getCobro(id, cobro, formaPago, creado_por);
        else {
            ShowAlertMessage('warning', 'Debe de Elegir una forma de Pago correcto')
        }

    });
    /*-----------------------------------Funcion Anular Venta---------------------------*/
    function anularVenta(id) {
        let id_venta = parseInt(id);
        $.ajax({
            type: 'GET',
            url: "/CAJCobro/getAularVenta",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                id_venta
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Venta anulado correctamente')
                    $('#modalAnulado').modal('hide');
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    /*----------------------------------BOTON ANULAR------------------------------------*/
    $('#btnAnular').on('click', function (e) {
        e.preventDefault();
        let id = $('#hfID').val();
        anularVenta(id)
    })

});

