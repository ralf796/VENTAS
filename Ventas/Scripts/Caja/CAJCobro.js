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
                    alignment: "center"
                },

                {
                    caption: "DETALLE VENTA",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        var classTmp = 'detalle' + cont;
                        var classBTN = 'btn btn-success ' + classTmp;

                        $("<button>").addClass(classBTN).text('VER DETALLE').appendTo(container);
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
                        var classBTN = 'btn btn-danger ' + classTmp;

                        $("<button>").addClass(classBTN).text('COBRAR').appendTo(container);
                        $('.cobrar' + cont).click(function (e) {
                            var total = parseFloat(fieldData.TOTAL);
                            var id = parseInt(fieldData.ID_VENTA);
                            var creadoPor = fieldData.CREADO_POR;
                            //mostrarCobro(fieldData.TOTAL, fieldData.ID_VENTA);
                            document.querySelector('#totalCobro').textContent = total;
                            $('#hfID').val(id);
                            $('#hfOpcion').val(creadoPor);
                            $('#modalCobro').modal('show');

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
                    dataField: "CANTIDAD",
                    caption: "CANTIDAD",
                    alignment: "center"
                },
                {
                    dataField: "PRECIO_UNITARIO",
                    caption: "PRECIO UNITARIO",
                    alignment: "center"
                },
                {
                    dataField: "TOTAL",
                    caption: "TOTAL",
                    alignment: "center"
                },
                {
                    dataField: "SUBTOTAL",
                    caption: "SUBTOTAL",
                    alignment: "center"
                }
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
                    ShowAlertMessage('success', 'Datos ingresados correctamente')
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
        var cobro = $('#txtCobro').val();
        var formaPago = $('#selTipoPago').val();
        var id = parseInt($('#hfID').val());
        var creado_por = $('#hfOpcion').val();
        if (cobro == "") {
            alert("Debe de ingresar un Monto valido")
            $('#modalCobro').modal('show');
        }
        else {
            getCobro(id, cobro, formaPago, creado_por);
        }

    });


});

