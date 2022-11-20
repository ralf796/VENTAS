﻿var gridProductos = null;
$(document).ready(function () {
    //CallLoadingFire('Cargandoo-.')
    //GetListProductos();


    function ClearCustomer() {
        $('#hfIdCliente').val('');
        $('#txtNombreCliente').val('');
        $('#txtNit').val('');
        $('#txtDireccion').val('');
    }
    function ClearProduct() {
        $('#hfIdProducto').val('');
        $('#txtCodigo').val('');
        $('#txtNombreProducto').val('');
        $('#txtCantidad').val(0);
        $('#txtStock').val('');
        $('#txtPrecio').val('');
        $('#txtDescuento').val('');
        $('#txtDescuentoTotal').val('');
        $('#txtSinDescuento').val('');
        $('#txtConDescuento').val('');
        $('#txtMarcaRepuesto').val('');
        $('#txtMarcaVehiculo').val('');
        $('#txtLineaVehiculo').val('');
        $('#checkAutorizaDescuento').prop('checked', false);
    }
    function GetCliente(nit) {
        var tipo = 2;
        $.ajax({
            type: 'GET',
            url: '/VENCrearVenta/GetCliente',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { nit, tipo },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    var CLIENTE = data["data"];
                    console.log(CLIENTE)
                    if (CLIENTE == null) {
                        ShowAlertMessage('warning', 'No existe el cliente con el nit ingresado.');
                        ClearCustomer();
                    }
                    else {
                        $('#hfIdCliente').val(CLIENTE.ID_CLIENTE);
                        $('#txtNombreCliente').val(CLIENTE.NOMBRE);
                        $('#txtDireccion').val(CLIENTE.DIRECCION);
                    }
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function GetClienteId(id) {

        var tipo = 9;
        $.ajax({
            type: 'GET',
            url: '/VENCrearVenta/GetClienteID',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { id, tipo },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    var CLIENTE = data["data"];
                    console.log(CLIENTE)
                    if (CLIENTE == null) {
                        ShowAlertMessage('warning', 'No existe el cliente con el nit ingresado.');
                        ClearCustomer();
                    }
                    else {
                        $('#txtNit').val(CLIENTE.NIT);
                        $('#hfIdCliente').val(CLIENTE.ID_CLIENTE);
                        $('#txtNombreCliente').val(CLIENTE.NOMBRE);
                        $('#txtNombreCrear').val('');
                        $('#txtDireccionCrear').val('');
                        $('#txtEmailCrear').val('');
                        $('#txtNitCrear').val('');
                    }
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function GetListClientes() {
        var tipo = 3;
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/VENCrearVenta/GetList',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { tipo },
                    cache: false,
                    success: function (data) {
                        var state = data["State"];
                        if (state == 1) {
                            $('#modalClientes').modal('show');
                            data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);
                            d.resolve(data);
                        }
                        else if (state == -1)
                            ShowAlertMessage('warning', data["Message"])
                    },
                    error: function (jqXHR, exception) {
                        getErrorMessage(jqXHR, exception);
                    }
                });
                return d.promise();
            }
        });
        var salesPivotGrid = $("#gridClientes").dxDataGrid({
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
                width: 240,
                placeholder: "Buscar..."
            },
            headerFilter: {
                visible: true
            },
            columnAutoWidth: true,
            export: {
                enabled: false
            },
            paging: {
                pageSize: 10,
            },
            pager: {
                showPageSizeSelector: true,
                allowedPageSizes: [10, 25, 50, 100],
            },
            remoteOperations: false,
            searchPanel: {
                visible: true,
                highlightCaseSensitive: true,
            },
            allowColumnReordering: true,
            rowAlternationEnabled: true,
            columns: [
                {
                    dataField: "ID_CLIENTE",
                    caption: "ID",
                    alignment: "center",
                    visible: false
                },
                {
                    dataField: "NOMBRE",
                    caption: "NOMBRE"
                },
                {
                    dataField: "NIT",
                    caption: "NIT",
                    alignment: "center"
                },
                {
                    dataField: "DIRECCION",
                    caption: "DIRECCION",
                    alignment: "center"
                },
                {
                    dataField: "TELEFONO",
                    caption: "TELEFONO",
                    alignment: "center"
                }
            ],
            onRowDblClick: function (e) {
                $('#hfIdCliente').val(e.data["ID_CLIENTE"]);
                $('#txtNombreCliente').val(e.data["NOMBRE"]);
                $('#txtNit').val(e.data["NIT"]);
                $('#hfDireccion').val(e.data["DIRECCION"]);
                $('#txtDireccion').val(e.data["DIRECCION"]);
                $('#modalClientes').modal('hide');
            },
            onCellPrepared: function (e) {
                if (e.rowType === 'header') {
                    e.cellElement.css("background", "var(--secondary)");
                    e.cellElement.css("color", "#FFFFFF");
                }
            }
        }).dxDataGrid('instance');
    }

    function SaveCustomer(nombre, direccion, telefono, email, nit) {

        $.ajax({
            type: 'GET',
            url: "/VENCrearVenta/GuardarCliente",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                nombre, direccion, telefono, email, nit
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    var idC = data["ID_CLIENTE"];


                    ShowAlertMessage('success', 'Datos ingresados correctamente')
                    //$('#txtNit').val(nit);
                    GetClienteId(idC);
                    $('#modalCrearCliente').modal('hide');
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    function ENCABEZADO(SERIE, CORRELATIVO, ID_CLIENTE, TOTAL, SUBTOTAL, TOTAL_IVA, TOTAL_DESCUENTO) {
        this.SERIE = SERIE;
        this.CORRELATIVO = CORRELATIVO;
        this.ID_CLIENTE = ID_CLIENTE;
        this.TOTAL = TOTAL;
        this.SUBTOTAL = SUBTOTAL;
        this.TOTAL_IVA = TOTAL_IVA;
        this.TOTAL_DESCUENTO = TOTAL_DESCUENTO;
    }
    function DETALLE(ID_PRODUCTO, CANTIDAD, PRECIO_VENTA, TOTAL, IVA, TOTAL_DESCUENTO, SUBTOTAL) {
        this.ID_PRODUCTO = ID_PRODUCTO;
        this.CANTIDAD = CANTIDAD;
        this.PRECIO_VENTA = PRECIO_VENTA;
        this.TOTAL = TOTAL;
        this.IVA = IVA;
        this.TOTAL_DESCUENTO = TOTAL_DESCUENTO;
        this.SUBTOTAL = SUBTOTAL;
    }

    function SaveOrder(jsonEncabezado, jsonDetalles) {
        $.ajax({
            type: 'GET',
            url: '/VENCrearVenta/SaveOrder',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            //data: { tipo, ID_CLIENTE, TOTAL, SUBTOTAL, TOTAL_DESCUENTO },
            data: {
                encabezado: JSON.stringify(jsonEncabezado),
                detalles: JSON.stringify(jsonDetalles)
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                var compra = data["ORDEN_COMPRA"];
                if (state == 1) {
                    //ShowAlertMessage('success', 'Se creó la orden de compra: ' + compra.ID_VENTA)

                    Swal.fire({
                        icon: 'success',
                        type: 'success',
                        html: 'Se creó la orden de compra: ' + compra,
                        showCancelButton: true,
                        cancelButtonText: 'Cerrar',
                        showConfirmButton: false,
                    })

                    ClearCustomer();
                    ClearProduct();
                    $('#txtTotal').html('');
                    $('#tbodyDetalleVenta').empty();
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                    return;
                }
                else if (data == null) {
                    ShowAlertMessage('warning', 'La orden de compra no se pudo procesar!!')
                    return;
                }
            }
        });
    }

    function RefreshSum() {
        var total = 0, subtotal = 0, descuento = 0;
        $('#tbodyDetalleVenta tr').each(function () {
            total = total + parseFloat($(this).find("td").eq(6).text().replace(",", ""));
            subtotal = subtotal + parseFloat($(this).find("td").eq(6).text().replace(",", ""));
            descuento = descuento + parseFloat($(this).find("td").eq(5).text().replace(",", ""));
        });
        $('#txtTotal').html('TOTAL: ' + formatNumber(parseFloat(total).toFixed(2)));

        total = parseFloat(descuento) + parseFloat(subtotal);

        $('#hfTotal').val(parseFloat(total).toFixed(2));
        $('#hfTotalDescuento').val(parseFloat(descuento).toFixed(2));
        $('#hfSubtotal').val(parseFloat(subtotal).toFixed(2));
    }
    var DetalleVenta = $("#tbodyDetalleVenta");
    function AddDetail(id, producto, cantidad, precio, descuento, codigo1, codigo2, marca, hfNombre) {
        if (cantidad == '')
            cantidad = 0;
        if (precio == '')
            precio = 0;
        if (descuento == '')
            descuento = 0;

        var total = 0, subtotal = 0;
        total = parseFloat(precio) * parseFloat(cantidad);
        descuento = (descuento / 100) * total;
        subtotal = total - descuento;

        var remove = '<a title="Eliminar" class="btn btn-outline-danger removeDetail" style="margin-top:-10px"><i class="far fa-times"></i></a>';
        DetalleVenta.append('<tr>' +
            '<td class="text-center">' + remove + '</td>' +
            '<td class="d-none">' + id + '</td>' +
            '<td class="pl-2">' + producto + '</td>' +
            '<td class="pl-2 pr-2 text-center">' + cantidad + '</td>' +
            '<td class="pl-2 pr-2 text-right">' + formatNumber(parseFloat(precio).toFixed(2)) + '</td>' +
            '<td class="pl-2 pr-2 text-right">' + formatNumber(parseFloat(descuento).toFixed(2)) + '</td>' +
            '<td class="pl-2 pr-2 text-right">' + formatNumber(parseFloat(subtotal).toFixed(2)) + '</td>' +
            '<td class="d-none">' + codigo1 + '</td>' +
            '<td class="d-none">' + codigo2 + '</td>' +
            '<td class="d-none">' + marca + '</td>' +
            '<td class="d-none">' + hfNombre + '</td>' +
            '</tr>');
        ClearProduct();
        RefreshSum();
    }
    DetalleVenta.on('click', '.removeDetail', function () {
        $(this).closest('tr').remove();
        RefreshSum();
    });

    function ZoomImage(nombre, descripcion, url) {
        Swal.fire({
            title: nombre,
            text: descripcion,
            imageUrl: url,
            imageWidth: 400,
            imageHeight: 400,
            imageAlt: 'Custom image',
            confirmButtonText: 'Cerrar'
        })
    }
    function GetProducto(codigo) {
        var tipo = 10;
        $.ajax({
            type: 'GET',
            url: '/VENCrearVenta/GetProductoPorCodigo',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { codigo, tipo },
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
                        $('#hfIdProducto').val(PROD.ID_PRODUCTO);
                        //$('#txtCodigo').val(PROD.CODIGO);
                        $('#txtNombreProducto').val(PROD.NOMBRE_COMPLETO);
                        $('#txtStock').val(PROD.STOCK);
                        $('#txtPrecio').val(PROD.PRECIO_VENTA);
                        $('#hfCodigo1').val(PROD.CODIGO);
                        $('#hfCodigo2').val(PROD.CODIGO2);
                        $('#hfMarcaR').val(PROD.NOMBRE_MARCA_REPUESTO);
                        $('#hfNombre').val(PROD.NOMBRE);
                    }
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function ValidarLogin() {
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
                    $('#checkAutorizaDescuento').prop('checked', true)
                    $('#modalValidarAutorizacion').modal('hide');
                }
                else {
                    $('#checkAutorizaDescuento').prop('checked', false);
                    $('#txtUser').val('');
                    $('#txtPassword').val('');
                }
            }
        });
    }
    function ClearProducto() {
        $('#hfIdProducto').val('');
        $('#txtCodigo').val('');
        $('#txtNombreProducto').val('');
        $('#txtStock').val('');
        $('#txtPrecio').val('');
        $('#hfCodigo1').val('');
        $('#hfCodigo2').val('');
        $('#hfMarcaR').val('');
        $('#hfNombre').val('');
    }

    $("#txtNit").keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            e.preventDefault();
            var nit = $(this).val();
            GetCliente(nit);
        }
    });
    $('#txtNit').on('click', function (e) {
        e.preventDefault();
        ClearCustomer();
    });
    $('#btnBuscarClientes').on('click', function (e) {
        e.preventDefault();
        GetListClientes();
    });
    $('#btnBuscarProductos').on('click', function (e) {
        e.preventDefault();
        if (gridProductos != null)
            gridProductos.option('dataSource', []);

        $('#txtFiltro').val('');
        $('#modalProductos').modal('show');

    });
    $('#btnAgregarDetalle').on('click', function (e) {
        var id = $('#hfIdProducto').val();
        var producto = $('#txtCodigo').val() + ' - ' + $('#txtNombreProducto').val();
        var cantidad = $('#txtCantidad').val();
        var precio = $('#txtPrecio').val();
        var descuento = $('#txtDescuento').val();
        var stock = $('#txtStock').val();
        var codigo1 = $('#hfCodigo1').val();
        var codigo2 = $('#hfCodigo2').val();
        var marca = $('#hfMarcaR').val();
        var nombre = $('#hfNombre').val();


        if (id == '' || id == null) {
            ShowAlertMessage('info', 'Debes seleccionar un producto.')
            return;
        }
        if (cantidad == 0 || cantidad == '' || cantidad == null) {
            ShowAlertMessage('info', 'Debes ingresar una cantidad válida.')
            return;
        }
        if (parseFloat(cantidad) > parseFloat(stock)) {
            ShowAlertMessage('info', 'La cantidad que solicita no puede ser mayor al stock(' + stock + ') disponible.')
            return;
        }

        var bandera = false;
        $('#tbodyDetalleVenta tr').each(function () {
            if (bandera == false) {
                if ($(this).find("td").eq(1).text() == id)
                    bandera = true;
            }
        });
        if (bandera == true) {
            ShowAlertMessage('info', 'El producto ' + producto + ' ya ha sido agregado a la orden de compra.')
            return;
        }

        AddDetail(id, producto, cantidad, precio, descuento, codigo1, codigo2, marca, nombre);
    });
    $('#btnAgregarCliente').on('click', function (e) {
        e.preventDefault();
        $('#modalCrearCliente').modal('show');
    });

    $('#btnCancelarVenta').on('click', function (e) {
        e.preventDefault();
        ClearCustomer();
        ClearProduct();
        $('#tbodyDetalleVenta').empty();
    });
    $('#btnGuardarVenta').on('click', function (e) {
        e.preventDefault();
        var serie = '1';
        var correlativo = '1';
        var idCliente = $('#hfIdCliente').val();
        var total = $('#hfTotal').val();
        var subtotal = $('#hfSubtotal').val();
        var totalIva = 0;
        var totalDescuento = $('#hfTotalDescuento').val();

        var encabezado = new ENCABEZADO(serie, correlativo, idCliente, total, subtotal, totalIva, totalDescuento);

        var listDetalles = [];
        $('#tbodyDetalleVenta tr').each(function () {
            var vId = $(this).find("td").eq(1).text();
            var vCantidad = $(this).find("td").eq(3).text();
            var vPrecio = $(this).find("td").eq(4).text();
            var vIva = 0;
            var vDescuento = $(this).find("td").eq(5).text();
            var vSubtotal = $(this).find("td").eq(6).text();
            var vTotal = parseFloat(vIva) + parseFloat(vDescuento) + parseFloat(vSubtotal);
            var listado = new DETALLE(vId, vCantidad, vPrecio, vTotal, vIva, vDescuento, vSubtotal);
            listDetalles.push(listado);
        });
        console.log(listDetalles)

        if (idCliente == '' || idCliente == null) {
            ShowAlertMessage('info', 'Debes seleccionar un cliente para la venta.');
            return;
        }

        if (listDetalles.length == 0)
            ShowAlertMessage('info', 'Debes agregar al menos un producto.');
        else {
            SaveOrder(encabezado, listDetalles)
        }
    });
    $('#btnGuardarCliente').on('click', function (e) {
        e.preventDefault();
        var nombre = $('#txtNombreCrear').val();
        var direccion = $('#txtDireccionCrear').val();
        var telefono = $('#txtTelefonoCrear').val();
        var email = $('#txtEmailCrear').val();
        var nit = $('#txtNitCrear').val();

        if (nombre == '' || nombre != '') {
            ShowAlertMessage('warning', 'Debes ingresar un nombre de cliente.')
            return;
        }

        SaveCustomer(nombre, direccion, telefono, email, nit);
    });

    $('#txtDescuento').on('keyup', function (e) {
        e.preventDefault();
        var cantidad = $('#txtCantidad').val();
        var precio = $('#txtPrecio').val();
        var descuento = $(this).val();
        var total = 0, subtotal = 0;

        if (descuento == '')
            descuento = 0;
        if (!$('#checkAutorizaDescuento').is(':checked')) {
            if (parseFloat(descuento) > 35) {
                $('#txtDescuento').val();
                descuento = 0;
                ShowAlertMessage('warning', 'El máximo descuento a aplicar es de 35%.');
                $('#txtDescuentoTotal').val('');
                $('#txtConDescuento').val('');
                $('#txtSinDescuento').val('');
                $('#txtDescuento').val('');
                return;
            }
        }

        if ($('#txtCantidad').val() == '')
            $('#txtCantidad').val() = 0;
        if ($('#txtPrecio').val() == '')
            $('#txtPrecio').val() = 0;

        total = parseFloat(precio) * parseFloat(cantidad);
        var totalAux = total;
        descuento = (descuento / 100) * total;
        total = total - descuento;
        $('#txtDescuentoTotal').val(formatNumber(parseFloat(descuento).toFixed(2)));
        $('#txtConDescuento').val(formatNumber(parseFloat(total).toFixed(2)));
        $('#txtSinDescuento').val(formatNumber(parseFloat(totalAux).toFixed(2)));
    });
    $('#txtCantidad').on('keyup', function (e) {
        e.preventDefault();
        var cantidad = $('#txtCantidad').val();
        var precio = $('#txtPrecio').val();
        var descuento = $('#txtDescuento').val();
        var total = 0, subtotal = 0;



        if (descuento == '') {
            descuento = 0;
        }
        if (!$('#checkAutorizaDescuento').is(':checked')) {
            if (parseFloat(descuento) > 35) {
                $('#txtDescuento').val();
                descuento = 0;
                ShowAlertMessage('warning', 'El máximo descuento a aplicar es de 35%.');
                $('#txtDescuentoTotal').val('');
                $('#txtConDescuento').val('');
                $('#txtSinDescuento').val('');
                $('#txtDescuento').val('');
                return;
            }
        }

        if ($('#txtCantidad').val() == '')
            $('#txtCantidad').val() = 0;
        if ($('#txtPrecio').val() == '')
            $('#txtPrecio').val() = 0;

        total = parseFloat(precio) * parseFloat(cantidad);
        var totalAux = total;
        descuento = (descuento / 100) * total;
        total = total - descuento;
        $('#txtDescuentoTotal').val(formatNumber(parseFloat(descuento).toFixed(2)));
        $('#txtConDescuento').val(formatNumber(parseFloat(total).toFixed(2)));
        $('#txtSinDescuento').val(formatNumber(parseFloat(totalAux).toFixed(2)));
    });

    $("#txtCodigo").keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            e.preventDefault();
            var codigo = $(this).val();
            GetProducto(codigo);
        }
    });
    $('#txtCodigo').on('click', function (e) {
        e.preventDefault();
        ClearProducto()
    });
    $('#checkAutorizaDescuento').on('change', function (e) {
        e.preventDefault();
        if ($('#checkAutorizaDescuento').is(':checked')) {
            $('#txtUser').val('');
            $('#txtPassword').val('');
            $('#modalValidarAutorizacion').modal('show');
            $('#checkAutorizaDescuento').prop('checked', false);
        }
        else {
            $('#txtDescuentoTotal').val('');
            $('#txtConDescuento').val('');
            $('#txtSinDescuento').val('');
            $('#txtDescuento').val('');
        }
    });
    $('#btnValidarUsuario').on('click', function (e) {
        e.preventDefault();
        ValidarLogin()
    });

    $('#btnAbrirModalAutProducto').on('click', function (e) {
        e.preventDefault();
        $('#txtUserAutProd').val('');
        $('#txtPasswordAutProd').val('');
        $('#txtNombreAutProd').val('');
        $('#txtDescripcionAutProd').val('');
        $('#txtAutPrecioCosto').val('');
        $('#txtAutPrecioVenta').val('');

        var nombre = $('#hfNombre').val();
        var descripcion = $('#hfDescripcion').val();
        var precioCosto = $('#hfPrecioCosto').val();
        var precioVenta = $('#hfPrecioVenta').val();
        $('#txtNombreAutProd').val(nombre);
        $('#txtDescripcionAutProd').val(descripcion);
        $('#txtAutPrecioCosto').val(precioCosto);
        $('#txtAutPrecioVenta').val(precioVenta);

        var ID_PRODUCTO = $('#hfIdProducto').val();
        if (ID_PRODUCTO == '')
            ShowAlertMessage('warning', 'Debes seleccionar un producto.')
        else
            $('#modalAutProd').modal('show');
    });

    function ValidarAutorizacionEditarProducto(ID_PRODUCTO, nombre, descripcion, precioCosto, precioVenta, usuario) {
        var usuario = $('#txtUserAutProd').val();
        var password = $('#txtPasswordAutProd').val();
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
                    UpdateProduct(ID_PRODUCTO, nombre, descripcion, precioCosto, precioVenta, usuario)
                }
                else {
                    $('#txtUserAutProd').val('');
                    $('#txtPasswordAutProd').val('');
                    $('#txtNombreAutProd').val('');
                    $('#txtDescripcionAutProd').val('');
                    $('#txtAutPrecioCosto').val('');
                    $('#txtAutPrecioVenta').val('');
                }
            }
        });
    }
    function UpdateProduct(ID_PRODUCTO, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, usuarioModifica) {
        $.ajax({
            type: 'GET',
            url: '/VENCrearVenta/UpdateProducto',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            //data: { usuario, nombre, descripcion, precioCosto, precioVenta},
            data: {
                ID_PRODUCTO, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, usuarioModifica
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Se actualizó el producto correctamente.');
                    var codigo = data["CODIGO"];
                    GetProducto(codigo);
                    $('#modalAutProd').modal('hide');
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                    return;
                }
                else if (state == 2) {
                    ShowAlertMessage('warning', 'No se ha podido actualizar el producto, consulta a sistemas.');
                    return;
                }
            }
        });
    }
    $('#btnAutProd').on('click', function (e) {
        e.preventDefault();
        var ID_PRODUCTO = $('#hfIdProducto').val();
        var usuarioModifica = $('#txtUserAutProd').val();
        var NOMBRE = $('#txtNombreAutProd').val();
        var DESCRIPCION = $('#txtDescripcionAutProd').val();
        var PRECIO_COSTO = $('#txtAutPrecioCosto').val();
        var PRECIO_VENTA = $('#txtAutPrecioVenta').val();
        ValidarAutorizacionEditarProducto(ID_PRODUCTO, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, usuarioModifica)
    });

    function GetDatosGridProductos(filtro) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/VENCrearVenta/GetProductosTable',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { filtro },
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
                        $("<img>").addClass('zoom hover').attr('src', fieldData.PATH_IMAGEN).css('width', '70px').appendTo(container);
                    }
                },
                {
                    dataField: "ID_PRODUCTO",
                    caption: "ID PRODUCTO",
                    visible: false
                },
                {
                    dataField: "CODIGO_INTERNO",
                    caption: "COD. INTERNO",
                    alignment: "center",
                    width: 150
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
                    },
                    width: 115
                },
                {
                    dataField: "PRECIO_VENTA",
                    caption: "PRECIO",
                    alignment: "right",
                    format: "###,###.00",
                    width: 115
                },
                {
                    dataField: "PRECIO_COSTO",
                    caption: "PRECIO COSTO",
                    alignment: "right",
                    format: "###,###.00",
                    width: 115,
                    visible: false
                },
                {
                    dataField: "NOMBRE_COMPLETO",
                    caption: "NOMBRE",
                    width: 400
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
                    dataField: "DESCRIPCION",
                    caption: "DESCRIPCION",
                    visible: false
                },

                {
                    dataField: "NOMBRE_MODELO",
                    caption: "MODELO",
                    width: 200
                },
                {
                    dataField: "CREADO_POR",
                    caption: "CREADO_POR",
                    visible: false
                },
                {
                    dataField: "NOMBRE_DISTRIBUIDOR",
                    caption: "DISTRIBUIDOR",
                    width: 200
                },
                {
                    dataField: "NOMBRE_MARCA_REPUESTO",
                    caption: "MARCA PRODUCTO",
                    width: 200
                },
                {
                    dataField: "SOLO_NOMBRE",
                    visible: false
                }
            ],
            onCellClick: function (e) {
                if (e.column.dataField == 'PATH_IMAGEN') {
                    console.log(e.data)
                    ZoomImage((e.data['NOMBRE_COMPLETO']), 'MARCA: ' + e.data['NOMBRE_MARCA_REPUESTO'] + '.     Stock: ' + e.data['STOCK'] + '.     Precio: Q' + e.data['PRECIO_VENTA'], e.data['PATH_IMAGEN'])
                }
            },
            onRowDblClick: function (e) {
                $('#hfIdProducto').val(e.data["ID_PRODUCTO"]);
                $('#txtCodigo').val(e.data["CODIGO"]);
                $('#txtNombreProducto').val(e.data["NOMBRE_COMPLETO"]);
                $('#txtStock').val(e.data["STOCK"]);
                $('#txtPrecio').val(e.data["PRECIO_VENTA"]);
                $('#txtPrecio').attr('title', 'PRECIO COSTO:  ' + e.data["PRECIO_COSTO"]);
                $('#hfCodigo1').val(e.data["CODIGO"]);
                $('#hfCodigo2').val(e.data["CODIGO2"]);
                $('#hfMarcaR').val(e.data["NOMBRE_MARCA_REPUESTO"]);
                $('#hfNombre').val(e.data["NOMBRE"]);
                $('#txtMarcaRepuesto').val(e.data["NOMBRE_MARCA_REPUESTO"]);
                $('#txtMarcaVehiculo').val(e.data["NOMBRE_MARCA_VEHICULO"]);
                $('#txtLineaVehiculo').val(e.data["NOMBRE_LINEA_VEHICULO"]);
                $('#hfNombre').val(e.data["SOLO_NOMBRE"]);
                $('#hfDescripcion').val(e.data["DESCRIPCION"]);
                $('#hfPrecioCosto').val(e.data["PRECIO_COSTO"]);
                $('#hfPrecioVenta').val(e.data["PRECIO_VENTA"]);
                $('#modalProductos').modal('hide');
            }
        }).dxDataGrid('instance');

    }
    $("#txtFiltro").keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            e.preventDefault();
            var filtro = $(this).val();
            if (filtro.length > 3)
                GetDatosGridProductos(filtro);
        }
    });
    $('#txtFiltro').on('keyup', function (e) {
        e.preventDefault();
        var filtro = $(this).val();
        if (filtro.length > 3)
            GetDatosGridProductos(filtro);
        else {
            if (gridProductos != null) {
                gridProductos.option('dataSource', []);
            }
        }
    });
});