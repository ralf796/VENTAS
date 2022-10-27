$(document).ready(function () {

    GetLists('#selBodega', 8)
    GetLists('#selModelo', 6)
    GetLists('#selProveedor', 10)
    GetLists('#selMarcaRepuesto', 12)
    GetLists('#selCategoria', 2)
    GetLists('#selMarcaVehiculo', 14)

    function GetLists(selObject, tipo) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: 'GET',
                url: '/INVMantenimiento/GetDatosTable',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: { tipo },
                cache: false,
                success: function (data) {
                    var list = data["data"];
                    var state = data["State"];
                    if (state == 1) {
                        $(selObject).empty();
                        $(selObject).append('<option selected value="-1" disabled>Seleccione una opción</option>');
                        list.forEach(function (dato) {
                            if (tipo == 8) {
                                $(selObject).append('<option value="' + dato.ID_BODEGA + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 6) {
                                $(selObject).append('<option class="text-center" value="' + dato.ID_MODELO + '">' + + dato.ANIO_INICIAL + ' - ' + dato.ANIO_FINAL + '</option>');
                            }
                            else if (tipo == 10) {
                                $(selObject).append('<option value="' + dato.ID_PROVEEDOR + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 12) {
                                $(selObject).append('<option value="' + dato.ID_MARCA_REPUESTO + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 2) {
                                $(selObject).append('<option value="' + dato.ID_CATEGORIA + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 14) {
                                $(selObject).append('<option value="' + dato.ID_MARCA_VEHICULO + '">' + dato.NOMBRE + '</option>');
                            }
                        });
                        resolve(1);
                    }
                    else if (state == -1)
                        alert(data["Message"])
                }
            });
        });
    }

    function GetListsID(selObject, tipo, id) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: 'GET',
                url: '/INVMantenimiento/GetDatosTable',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: { tipo, id },
                cache: false,
                success: function (data) {
                    var list = data["data"];
                    var state = data["State"];
                    if (state == 1) {
                        $(selObject).empty();
                        $(selObject).append('<option selected value="-1" disabled>Seleccione una opción</option>');
                        list.forEach(function (dato) {
                            if (tipo == 4) {
                                $(selObject).append('<option value="' + dato.ID_SUBCATEGORIA + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 16) {
                                $(selObject).append('<option value="' + dato.ID_SERIE_VEHICULO + '">' + dato.NOMBRE + '</option>');
                            }
                        });
                        resolve(1);
                    }
                    else if (state == -1)
                        alert(data["Message"])
                }
            });
        });
    }

    $('#selCategoria').on('change', function (e) {
        e.preventDefault();
        var id = $(this).val();
        GetListsID('#selSubcategoria', 4, id)
    });
    $('#selMarcaVehiculo').on('change', function (e) {
        e.preventDefault();
        var id = $(this).val();
        GetListsID('#selSerieVehiculo', 16, id)
    });

    function GuardarProducto(NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, CODIGO, ID_BODEGA, ID_MODELO, ID_PROVEEDOR, ID_MARCA_REPUESTO, ID_SUBCATEGORIA, ID_SERIE_VEHICULO, ID_PRODUCTO, tipo) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/OperacionesProducto",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, CODIGO, ID_BODEGA, ID_MODELO, ID_PROVEEDOR, ID_MARCA_REPUESTO, ID_SUBCATEGORIA, ID_SERIE_VEHICULO, ID_PRODUCTO, tipo
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    $('#txtNombre').val('');
                    $('#txtDescripcion').val('');
                    $('#txtPrecioCosto').val('');
                    $('#txtPrecioVenta').val('');
                    $('#txtStock').val('');
                    $('#txtCodigo').val('');

                    $('#selBodega').val(-1);
                    $('#selModelo').val(-1);
                    $('#selProveedor').val(-1);
                    $('#selMarcaRepuesto').val(-1);
                    $('#selSubcategoria').empty();
                    $('#selSerieVehiculo').empty();
                    ShowAlertMessage('success', '¡Producto creado correctamente!');
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    $('#formGuardarProducto').submit(function (e) {
        e.preventDefault();
        var NOMBRE = $('#txtNombre').val();
        var DESCRIPCION = $('#txtDescripcion').val();
        var PRECIO_COSTO = $('#txtPrecioCosto').val();
        var PRECIO_VENTA = $('#txtPrecioVenta').val();
        var STOCK = $('#txtStock').val();
        var CODIGO = $('#txtCodigo').val();

        var ID_BODEGA = $('#selBodega').val();
        var ID_MODELO = $('#selModelo').val();
        var ID_PROVEEDOR = $('#selProveedor').val();
        var ID_MARCA_REPUESTO = $('#selMarcaRepuesto').val();
        var ID_SUBCATEGORIA = $('#selSubcategoria').val();
        var ID_SERIE_VEHICULO = $('#selSerieVehiculo').val();
        var ID_PRODUCTO = 0;
        var tipo = 1;
        GuardarProducto(NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, CODIGO, ID_BODEGA, ID_MODELO, ID_PROVEEDOR, ID_MARCA_REPUESTO, ID_SUBCATEGORIA, ID_SERIE_VEHICULO, ID_PRODUCTO, tipo);
    });


});