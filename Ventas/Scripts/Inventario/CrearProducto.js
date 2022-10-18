$(document).ready(function () {
    GetLists('#selCategoria', 4)
    GetLists('#selModelo', 8)
    GetLists('#selTipo', 12)
    GetLists('#selBodega', 16)

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
                            if (tipo == 4) {
                                $(selObject).append('<option value="' + dato.ID_CATEGORIA + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 8) {
                                $(selObject).append('<option value="' + dato.ID_MODELO + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 12) {
                                $(selObject).append('<option value="' + dato.ID_TIPO + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 16) {
                                $(selObject).append('<option value="' + dato.ID_BODEGA + '">' + dato.NOMBRE + '</option>');
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

    function GuardarProducto(ID_CATEGORIA, ID_MODELO, ID_TIPO, ID_BODEGA, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, ANIO_FABRICADO, CODIGO) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/GuardarProducto",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                ID_CATEGORIA, ID_MODELO, ID_TIPO, ID_BODEGA, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, ANIO_FABRICADO, CODIGO
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', '¡Producto creado correctamente!')
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    $('#formGuardarProducto').submit(function (e) {
        e.preventDefault();

        var ID_CATEGORIA = $('#selCategoria').val();
        var ID_MODELO = $('#selModelo').val();
        var ID_TIPO = $('#selTipo').val();
        var ID_BODEGA = $('#selBodega').val();
        var NOMBRE = $('#txtNombre').val();
        var DESCRIPCION = $('#txtDescripcion').val();
        var PRECIO_COSTO = $('#txtPrecioCosto').val();
        var PRECIO_VENTA = $('#txtPrecioVenta').val();
        var STOCK = $('#txtStock').val();
        var ANIO_FABRICADO = $('#txtAnioFabricacion').val();
        var CODIGO = $('#txtCodigo').val(); 
        GuardarProducto(ID_CATEGORIA, ID_MODELO, ID_TIPO, ID_BODEGA, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, ANIO_FABRICADO, CODIGO);
    });


});