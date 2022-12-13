$(document).ready(function () {
    var cont = 0;

    function GetProducto(codigo) {
        $.ajax({
            type: 'GET',
            url: '/INVMantenimiento/GetProductoEditar',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { codigo },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    var PROD = data["data"];
                    console.log(PROD)
                    if (PROD == null) {
                        ShowAlertMessage('warning', 'No existen productos con el código ingresado.');
                    }
                    else {
                        GetDetallesCodigo(codigo)
                        $('#hfIdProducto').val(PROD.ID_PRODUCTO);
                        $('#txtCodigo1').val(PROD.CODIGO);
                        $('#txtCodigo2').val(PROD.CODIGO2);
                        $('#txtNombre').val(PROD.NOMBRE);
                        $('#txtDescripcion').val(PROD.DESCRIPCION);
                        $('#txtPrecioCosto').val(PROD.PRECIO_COSTO);
                        $('#txtPrecioVenta').val(PROD.PRECIO_VENTA);
                        $('#txtStock').val(PROD.STOCK);
                        $('#txtMarcaRepuesto').val(PROD.NOMBRE_MARCA_REPUESTO);
                        $('#txtDistribuidor').val(PROD.NOMBRE_DISTRIBUIDOR);
                    }
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    function GetDetallesCodigo(codigo) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/INVMantenimiento/GetDetallesProductoEditar',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { codigo },
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
            scrolling: {
                useNative: false,
                scrollByContent: true,
                scrollByThumb: true,
                showScrollbar: "always" // or "onScroll" | "always" | "never"
            },
            columnAutoWidth: true,
            columns: [
                {
                    caption: '',
                    alignment: "center",
                    width: 80,
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        var classTmp2 = 'remove' + cont;
                        var classBTN2 = 'ml-2 hvr-grow far fa-trash-alt btn btn-danger ' + classTmp2;
                        $("<span>").addClass(classBTN2).prop('title', 'Inactivar').appendTo(container);
                        $('.remove' + cont).click(function (e) {
                            var id = parseInt(fieldData.ID_DELETE);
                            EliminarDetalleProducto(id)
                        })
                        cont++;
                    }
                },
                {
                    dataField: "ID_DELETE",
                    caption: "ID",
                    alignment: "center",
                    width: 150,
                    visible: false
                },
                {
                    dataField: "CODIGO",
                    caption: "CODIGO 1",
                    alignment: "center",
                    width: 150
                },
                {
                    dataField: "CODIGO2",
                    caption: "CODIGO 2",
                    alignment: "center",
                    width: 150
                },
                {
                    dataField: "NOMBRE_MARCA_VEHICULO",
                    caption: "MARCA VEHICULO",
                    width: 200
                },
                {
                    dataField: "NOMBRE_LINEA_VEHICULO",
                    caption: "LINEA VEHICULO",
                    width: 200
                },
                {
                    dataField: "NOMBRE_MODELO",
                    caption: "MODELO",
                    width: 200
                }
            ]
        }).dxDataGrid('instance');

    }

    function EliminarDetalleProducto(ID_DETALLE) {
        var codigo = $('#txtBuscarCodigo').val();
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/EliminarDetalleProducto",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { ID_DETALLE },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    GetDetallesCodigo(codigo);
                    ShowAlertMessage('success', 'Detalle eliminado correctamente');
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    function AgregarDetalleProducto(anioI, anioF, marcaV, linea, codigo1, codigo2) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/AgregarDetalleProducto",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { anioI, anioF, marcaV, linea, codigo1, codigo2 },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    GetDetallesCodigo($('#txtBuscarCodigo').val());
                    ShowAlertMessage('success', 'Detalle agregado correctamente');
                    $('#modalAgregarDetalle').modal('hide');
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    function CambiarURLImagenProducto(id_producto, url) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/CambiarURLImagenProducto",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { id_producto, url },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    $('#txtURLProducto').val('');
                    ShowAlertMessage('success', 'Se actualizó la imagen correctamente.');
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    function ActualizarEncabezadoProducto(ID_PRODUCTO, NOMBRE, DESCRIPCION, MARCA_REPUESTO, STOCK, PRECIO_COSTO, PRECIO_VENTA, CODIGO, CODIGO2, DISTRIBUIDOR) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/ActualizarEncabezadoProducto",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { ID_PRODUCTO, NOMBRE, DESCRIPCION, MARCA_REPUESTO, STOCK, PRECIO_COSTO, PRECIO_VENTA, CODIGO, CODIGO2, DISTRIBUIDOR},
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    $('#txtURLProducto').val('');
                    ShowAlertMessage('success', 'Se actualizaron los datos correctamente.');
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    $("#txtBuscarCodigo").keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            e.preventDefault();
            var codigo = $(this).val();
            GetProducto(codigo);
        }
    });

    $('#btnModalAgregarDetalle').on('click', function (e) {
        e.preventDefault();
        $('#txtAnioInicial').val('');
        $('#txtAnioFinal').val('');
        $('#txtMarcaVehiculo').val('');
        $('#txtLineaVehiculo').val('');
        if ($('#txtBuscarCodigo').val() != '')
            $('#modalAgregarDetalle').modal('show');
    });

    $('#btnGuardarDetalle').on('click', function (e) {
        e.preventDefault();
        var anioI = $('#txtAnioInicial').val();
        var anioF = $('#txtAnioFinal').val();
        var marcaV = $('#txtMarcaVehiculo').val();
        var linea = $('#txtLineaVehiculo').val();
        var codigo1 = $('#txtCodigo1').val();
        var codigo2 = $('#txtCodigo2').val();
        AgregarDetalleProducto(anioI, anioF, marcaV, linea, codigo1, codigo2);
    });

    $("#idPathImagen").change(function () {
        Swal.fire({
            title: 'Actualización de imagen de producto',
            text: "Se actualizará la imagen del producto seleccionado. ¿Quieres continuar?",
            type: 'info',
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si',
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.value) {

                var ID = $("#hfIdProducto").val();
                var file = $("#idPathImagen").val();
                var formData = new FormData();
                var totalFiles = document.getElementById("idPathImagen").files.length;
                for (var i = 0; i < totalFiles; i++) {
                    var file = document.getElementById("idPathImagen").files[i];
                    formData.append("ID_PRODUCTO", ID);
                    formData.append("FileUpload", file);
                }
                if (file != null && file != "") {
                    CallLoadingFire('Cargando subida de datos, por favor espere.')
                    $.ajax({
                        type: "POST",
                        url: "/INVMantenimiento/CambiarImagenProducto",
                        data: formData,
                        dataType: 'json',
                        contentType: false,
                        processData: false,
                        success: function (data) {
                            var state = data['State'];
                            if (state == 1) {
                                $("#idPathImagen").val('');
                                ShowAlertMessage('success', 'Se cambió la imagen correctamente.');
                            }
                            else if (state == -1) {
                                ShowAlertMessage('warning', data['Message']);
                            }
                        },
                        error: function (jqXHR, ex) {
                            console.log(jqXHR);
                            console.log(ex);
                        }
                    });
                }
            }
        })
    });

    $('#btnActualizarURL').on('click', function (e) {
        e.preventDefault();
        var id = $('#hfIdProducto').val();
        var url = $('#txtURLProducto').val();

        if (url != '')
            CambiarURLImagenProducto(id, url)
    });

    $('#btnGuardarEncabezado').on('click', function (e) {
        e.preventDefault();
        var ID_PRODUCTO = $('#hfIdProducto').val();
        var NOMBRE = $('#txtNombre').val();
        var DESCRIPCION = $('#txtDescripcion').val();
        var MARCA_REPUESTO = $('#txtMarcaRepuesto').val();
        var STOCK = $('#txtStock').val();
        var PRECIO_COSTO = $('#txtPrecioCosto').val();
        var PRECIO_VENTA = $('#txtPrecioVenta').val();
        var CODIGO = $('#txtCodigo1').val();
        var CODIGO2 = $('#txtCodigo2').val();
        var DISTRIBUIDOR = $('#txtDistribuidor').val();

        ActualizarEncabezadoProducto(ID_PRODUCTO, NOMBRE, DESCRIPCION, MARCA_REPUESTO, STOCK, PRECIO_COSTO, PRECIO_VENTA, CODIGO, CODIGO2, DISTRIBUIDOR)

    });

});