var gridProductos = null;
$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    var cont = 0;
    function GetDatosGridProductos() {
        var filtro = $('#txtFiltro').val();
        var anioI = $('#anioI').val();
        var anioF = $('#anioF').val();


        if (filtro.length < 4) {
            if (gridProductos != null) {
                gridProductos.option('dataSource', []);
            }
            return;
        }

        if (anioI.length < 4)
            anioI = 0;
        if (anioF.length < 4)
            anioF = 0;

        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/INVMantenimiento/GetProductosTable',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { filtro, anioI, anioF },
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
        gridProductos = $("#gridContainer").dxDataGrid({
            dataSource: new DevExpress.data.DataSource(customStore),
            showBorders: true,
            loadPanel: {
                text: "Cargando..."
            },
            filterRow: {
                visible: true,
                applyFilter: "auto"
            },
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: "Buscar..."
            },
            headerFilter: {
                visible: true
            },
            scrolling: {
                useNative: false,
                scrollByContent: true,
                scrollByThumb: true,
                showScrollbar: "always" // or "onScroll" | "always" | "never"
            },
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: "Buscar..."
            },
            columnAutoWidth: true,

            onRowPrepared(e) {
                //e.rowElement.css("background-color", "#A7BCD6");
                //e.rowElement.css("color", "#000000");
            },
            columns: [
                {
                    dataField: 'PATH_IMAGEN',
                    caption: 'IMAGEN',
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        $("<img>").attr('src', fieldData.PATH_IMAGEN).css('width', '70px').appendTo(container);
                    }
                },
                {
                    caption: 'ACCIONES',
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        if (fieldData.ESTADO == 1)
                            $("<span>").addClass("badge badge-success").text('ACTIVO').appendTo(container);
                        else
                            $("<span>").addClass("badge badge-danger").text('INACTIVO').appendTo(container);
                        /*
                        var classTmp1 = 'edit' + cont;
                        var classBTN1 = 'ml-2 hvr-grow far fa-edit btn btn-success ' + classTmp1;
                        if (fieldData.ESTADO == 1) {
                            $("<span>").addClass(classBTN1).prop('title', 'Editar').appendTo(container);
                            $('.edit' + cont).click(function (e) {
                                var id = parseInt(fieldData.ID_PRODUCTO);
                                $('#modalEditarProducto').modal('show');
                                GetDatosProductoUpdate(id, fieldData.NOMBRE, fieldData.STOCK, fieldData.PRECIO_COSTO, fieldData.PRECIO_VENTA, fieldData.PATH_IMAGEN, fieldData.DESCRIPCION)
                            })
                        }
                        */
                        var classTmp2 = 'remove' + cont;
                        var classBTN2 = 'ml-2 hvr-grow far fa-trash-alt btn btn-danger ' + classTmp2;
                        if (fieldData.ESTADO == 1) {

                            $("<span>").addClass(classBTN2).prop('title', 'Inactivar').appendTo(container);
                            $('.remove' + cont).click(function (e) {
                                var id = parseInt(fieldData.ID_PRODUCTO);
                                Update_Delete_Producto(id, fieldData.NOMBRE, fieldData.STOCK, fieldData.PRECIO_COSTO, fieldData.PRECIO_VENTA, fieldData.PATH_IMAGEN, 3, '')
                            })
                        }

                        var classTmp3 = 'modifyStock' + cont;
                        var classBTN3 = 'ml-2 hvr-grow fas fa-exchange-alt btn btn-warning ' + classTmp3;
                        if (fieldData.ESTADO == 1) {

                            $("<span>").addClass(classBTN3).prop('title', 'Modificar Stock').appendTo(container);
                            $('.modifyStock' + cont).click(function (e) {
                                var id = parseInt(fieldData.ID_PRODUCTO);
                                var stock = parseInt(fieldData.STOCK);
                                $('#hfIdProducto').val(id);
                                $('#txtStockActual').text(stock);
                                $('#txtNuevoStock').val(0);
                                $('#modalAddStock').modal('show');
                                //GetDatosProductoUpdate(id, fieldData.NOMBRE, fieldData.STOCK, fieldData.PRECIO_COSTO, fieldData.PRECIO_VENTA, fieldData.PATH_IMAGEN, 3)
                            })
                        }
                        cont++;
                    }
                },
                {
                    dataField: "CODIGO_INTERNO",
                    caption: "COD. INTERNO",
                    alignment: "center",
                    width: 150
                },
                {
                    dataField: "ID_PRODUCTO",
                    caption: "ID PRODUCTO",
                    visible: false
                },
                {
                    dataField: "CODIGO",
                    caption: "CODIGO 1",
                    alignment: "center"
                },
                {
                    dataField: "CODIGO2",
                    caption: "CODIGO 2",
                    alignment: "center"
                },
                {
                    dataField: "STOCK",
                    caption: "STOCK",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        container.addClass(fieldData.ESTADO != 1 ? "dec" : "");

                        if (fieldData.STOCK > 0)
                            $("<span>").addClass("badge badge-success").text(fieldData.STOCK).appendTo(container);
                        else
                            $("<span>").addClass("badge badge-danger").text('SIN STOCK').appendTo(container);
                    }
                },
                {
                    dataField: "NOMBRE_COMPLETO",
                    caption: "NOMBRE"
                },
                {
                    dataField: "NOMBRE",
                    caption: "NOMBRE",
                    visible: false
                },
                {
                    dataField: "DESCRIPCION",
                    caption: "DESCRIPCION",
                    visible: false
                },

                {
                    dataField: "NOMBRE_MODELO",
                    caption: "MODELO"
                },
                {
                    dataField: "PRECIO_COSTO",
                    caption: "PRECIO COSTO",
                    alignment: "right",
                    format: ",##0.00",
                },
                {
                    dataField: "PRECIO_VENTA",
                    caption: "PRECIO VENTA",
                    alignment: "right",
                    format: "###,###.00",
                },
                {
                    dataField: "CREADO_POR",
                    caption: "CREADO_POR",
                    visible: false
                },
                {
                    dataField: "NOMBRE_DISTRIBUIDOR",
                    caption: "DISTRIBUIDOR"
                },
                {
                    dataField: "NOMBRE_MARCA_REPUESTO",
                    caption: "MARCA PRODUCTO"
                },
                {
                    dataField: "NOMBRE_MARCA_VEHICULO",
                    caption: "MARCA VEHICULO"
                },
                {
                    dataField: "NOMBRE_LINEA_VEHICULO",
                    caption: "LINEA VEHICULO"
                }
            ]
        }).dxDataGrid('instance');

    }
    function GetDatosProductoUpdate(ID_PRODUCTO, NOMBRE, STOCK, PRECIO_COSTO, PRECIO_VENTA, PATH, DESCRIPCION) {
        $('#hfIdProducto').val(ID_PRODUCTO);
        $('#txtNombre').val(NOMBRE);
        $('#txtDescripcion').val(DESCRIPCION);
        $('#txtStock').val(STOCK);
        $('#txtPrecioCosto').val(PRECIO_COSTO);
        $('#txtPrecioVenta').val(PRECIO_VENTA);
        //$('#idFotografia').val(PATH);
    }
    function Update_Delete_Producto(ID_PRODUCTO, NOMBRE, STOCK, PRECIO_COSTO, PRECIO_VENTA, PATH, tipo, DESCRIPCION) {
        var mensaje = '';
        mensaje = 'Proceso realizado correctamente';

        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/OperacionesProducto",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { ID_PRODUCTO, NOMBRE, STOCK, PRECIO_COSTO, PRECIO_VENTA, PATH, tipo, DESCRIPCION },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    GetDatosGridProductos()
                    ShowAlertMessage('success', mensaje)
                    $('#modalAddStock').modal('hide');
                    $('#modalEditarProducto').modal('hide');
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function SwalUpdate(nuevo, STOCK_actual, ID_PRODUCTO, NOMBRE, STOCK, PRECIO_COSTO, PRECIO_VENTA, PATH, tipo) {
        Swal.fire({
            title: 'ACTUALIZACIÓN',
            //text: 'Stock actual: ' + STOCK_actual,
            html: 'Stock actual: <h3>' + STOCK_actual + '</h3><br/>Nuevo stock: <h3>' + nuevo + '</h3><br/>¿Confirmas la actualización?',
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, actualizar',
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.isConfirmed) {
                Update_Delete_Producto(ID_PRODUCTO, NOMBRE, STOCK, PRECIO_COSTO, PRECIO_VENTA, PATH, tipo, '');
            }
        })
    }
    function ValidarLogin(nuevo, STOCK_actual, ID_PRODUCTO, NOMBRE, STOCK, PRECIO_COSTO, PRECIO_VENTA, PATH, tipo) {
        var usuario = $('#txtUser').val();
        var password = $('#txtPassword').val();
        $.ajax({
            type: 'GET',
            url: '/VENCrearVenta/ValidarLogin',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { usuario, password },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    Update_Delete_Producto(ID_PRODUCTO, NOMBRE, STOCK, PRECIO_COSTO, PRECIO_VENTA, PATH, tipo, '');
                    $('#modalValidarAutorizacion').modal('hide');
                }
                else {
                    $('#txtUser').val('');
                    $('#txtPassword').val('');
                }
            }
        });
    }

    $("#txtFiltro").keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            e.preventDefault();
            GetDatosGridProductos();
        }
    });
    $('#txtFiltro').on('keyup', function (e) {
        e.preventDefault();
        GetDatosGridProductos();
    });
    $('#anioI').on('keyup', function (e) {
        e.preventDefault();
        var anioI = $('#anioI').val();
        if (anioI.length > 3) {
            GetDatosGridProductos();
        }
    });
    $('#anioF').on('keyup', function (e) {
        e.preventDefault();
        var anioF = $('#anioF').val();
        if (anioF.length > 3) {
            GetDatosGridProductos();
        }
    });
    $('#formModificarProducto').submit(function (e) {
        e.preventDefault();

        var ID_PRODUCTO = $('#hfIdProducto').val();
        var NOMBRE = $('#txtNombre').val();
        var DESCRIPCION = $('#txtDescripcion').val();
        var STOCK = $('#txtStock').val();
        var PRECIO_COSTO = $('#txtPrecioCosto').val();
        var PRECIO_VENTA = $('#txtPrecioVenta').val();
        var PATH = $('#idFotografia').val();
        var tipo = 2;

        Update_Delete_Producto(ID_PRODUCTO, NOMBRE, STOCK, PRECIO_COSTO, PRECIO_VENTA, PATH, tipo, DESCRIPCION);
    });
    $('#tbnResStock').on('click', function (e) {
        e.preventDefault();
        var stockNuevo = $('#txtNuevoStock').val();

        if (stockNuevo == '' || stockNuevo == null)
            stockNuevo = 0;

        var sum = parseFloat(stockNuevo) - 1;
        $('#txtNuevoStock').val(sum);
    });
    $('#btnAddStock').on('click', function (e) {
        e.preventDefault();
        var stockNuevo = $('#txtNuevoStock').val();

        if (stockNuevo == '' || stockNuevo == null)
            stockNuevo = 0;

        var sum = parseFloat(stockNuevo) + 1;
        $('#txtNuevoStock').val(sum);
    });
    $('#btnUpdateStock').on('click', function (e) {
        e.preventDefault();

        var ID_PRODUCTO = $('#hfIdProducto').val();
        var NOMBRE = '';
        var STOCK = $('#txtNuevoStock').val();
        var STOCK_actual = $('#txtStockActual').text();
        var PRECIO_COSTO = 0;
        var PRECIO_VENTA = 0;
        var PATH = '';
        var tipo = 6;
        var nuevo = parseFloat(STOCK_actual) + parseFloat(STOCK);

        if (parseFloat(nuevo) < parseFloat(STOCK_actual)) {
            $('#txtUser').val('');
            $('#txtPassword').val('');
            $('#modalValidarAutorizacion').modal('show');
        }
        else {
            SwalUpdate(nuevo, STOCK_actual, ID_PRODUCTO, NOMBRE, STOCK, PRECIO_COSTO, PRECIO_VENTA, PATH, tipo)
        }
    });
    $('#btnValidarUsuario').on('click', function (e) {
        e.preventDefault();

        var ID_PRODUCTO = $('#hfIdProducto').val();
        var NOMBRE = '';
        var STOCK = $('#txtNuevoStock').val();
        var STOCK_actual = $('#txtStockActual').text();
        var PRECIO_COSTO = 0;
        var PRECIO_VENTA = 0;
        var PATH = '';
        var tipo = 6;
        var nuevo = parseFloat(STOCK_actual) + parseFloat(STOCK);

        ValidarLogin(nuevo, STOCK_actual, ID_PRODUCTO, NOMBRE, STOCK, PRECIO_COSTO, PRECIO_VENTA, PATH, tipo)

    });
});