﻿$(document).ready(function () {
    $('#txtCodigo').on('click', function (e) {
        //$('#modalProductos').modal('show');
    });

    function ClearCustomer() {
        $('#hfIdCliente').val('');
        $('#txtNombreCliente').val('');
        $('#txtNit').val('');
    }
    function ClearProduct() {
        $('#hfIdProducto').val('');
        $('#txtCodigo').val('');
        $('#txtNombreProducto').val('');
        $('#txtCantidad').val(0);
        $('#txtStock').val('');
        $('#txtPrecio').val('');
        $('#txtDescuento').val(0);
    }

    var DetalleVenta = $("#tbodyDetalleVenta");
    function AddDetail(id, producto, cantidad, precio, descuento) {
        if (cantidad == '')
            cantidad = 0;
        if (precio == '')
            precio = 0;
        if (descuento == '')
            descuento = 0;

        var subtotal = (parseFloat(cantidad) * parseFloat(precio)) - parseFloat(descuento);
        var remove = '<a title="Eliminar" class="btn btn-outline-danger removeDetail" style="margin-top:-10px"><i class="far fa-times"></i></a>';
        DetalleVenta.append('<tr>' +
            '<td class="text-center">' + remove + '</td>' +
            '<td class="d-none">' + id + '</td>' +
            '<td class="pl-2">' + producto + '</td>' +
            '<td class="pl-2 pr-2 text-center">' + cantidad + '</td>' +
            '<td class="pl-2 pr-2 text-right">' + formatNumber(parseFloat(precio).toFixed(2)) + '</td>' +
            '<td class="pl-2 pr-2 text-right">' + formatNumber(parseFloat(descuento).toFixed(2)) + '</td>' +
            '<td class="pl-2 pr-2 text-right">' + formatNumber(parseFloat(subtotal).toFixed(2)) + '</td>' +
            '</tr>');

        ClearProduct();
        RefreshSum();
    }

    $('#btnAgregarDetalle').on('click', function (e) {
        var id = $('#hfIdProducto').val();
        var producto = $('#txtCodigo').val() + ' - ' + $('#txtNombreProducto').val();
        var cantidad = $('#txtCantidad').val();
        var precio = $('#txtPrecio').val();
        var descuento = $('#txtDescuento').val();
        var stock = $('#txtStock').val();

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

        AddDetail(id, producto, cantidad, precio, descuento);
    });

    DetalleVenta.on('click', '.removeDetail', function () {
        $(this).closest('tr').remove();
        RefreshSum();
    });

    function RefreshSum() {
        var total = 0
        $('#tbodyDetalleVenta tr').each(function () {
            total = total + parseFloat($(this).find("td").eq(6).text().replace(",", ""));
        });
        $('#txtTotal').html('TOTAL: ' + formatNumber(parseFloat(total).toFixed(2)));
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
    function DETALLE(ID_PRODUCTO, CANTIDAD, PRECIO_UNITARIO, TOTALTOTAL, IVA, DESCUENTO, SUBTOTAL) {
        this.ID_PRODUCTO = ID_PRODUCTO;
        this.CANTIDAD = CANTIDAD;
        this.PRECIO_UNITARIO = PRECIO_UNITARIO;
        this.TOTALTOTAL = TOTALTOTAL;
        this.IVA = IVA;
        this.DESCUENTO = DESCUENTO;
        this.SUBTOTAL = SUBTOTAL;
    }
    $('#btnGuardarVenta').on('click', function (e) {
        e.preventDefault();
        var serie = '1';
        var correlativo = '1';
        var idCliente = 0;
        var total = 10;
        var subtotal = 9;
        var totalIva = 1;
        var totalDescuento = 2;

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

        if (listDetalles.length == 0)
            ShowAlertMessage('info', 'Debes agregar al menos un producto.');
        else {
            GuardarParticipantes(encabezado, listDetalles)
        }
    });

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
                    }
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    $("#txtNit").keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            e.preventDefault();
            var nit = $(this).val();
            GetCliente(nit);
        }
    });

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
                $('#modalClientes').modal('hide');
            }
        }).dxDataGrid('instance');
    }
    function GetListProductos() {
        var tipo = 1;
        var ID_MARCA_REPUESTO = $('#selMarcaRepuesto').val();
        var ID_SUBCATEGORIA = $('#selSubcategoria').val();
        var ID_CATEGORIA = $('#selCategoria').val();
        var ID_SERIE_VEHICULO = $('#selSerieVehiculo').val();
        var ID_MARCA_VEHICULO = $('#selMarcaVehiculo').val();
        var ID_MODELO = $('#selModelo').val();

        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/VENCrearVenta/GetList',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { tipo, ID_MARCA_REPUESTO, ID_SUBCATEGORIA, ID_CATEGORIA, ID_SERIE_VEHICULO, ID_MARCA_VEHICULO, ID_MODELO },
                    cache: false,
                    success: function (data) {
                        var state = data["State"];
                        if (state == 1) {
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
        var salesPivotGrid = $("#gridProductos").dxDataGrid({
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
            rowAlternationEnabled: true,

            columns: [
                {
                    dataField: "ID_PRODUCTO",
                    caption: "ID_PRODUCTO",
                    visible: false
                },
                {
                    dataField: "CODIGO",
                    caption: "CODIGO"
                },
                {
                    dataField: "NOMBRE",
                    caption: "NOMBRE"
                },
                {
                    dataField: "DESCRIPCION",
                    caption: "DESCRIPCION"
                },
                {
                    dataField: "PRECIO_VENTA",
                    caption: "PRECIO"
                },
                {
                    dataField: "STOCK",
                    caption: "STOCK",
                    alignment: "center"
                },
                {
                    dataField: "NOMBRE_MARCA_REPUESTO",
                    caption: "MARCA REPUESTO",
                    visible: false
                },
                {
                    dataField: "NOMBRE_CATEGORIA",
                    caption: "CATEGORIA",
                    visible: false
                },
                {
                    dataField: "NOMBRE_SUBCATEGORIA",
                    caption: "SUBCATEGORIA",
                    visible: false
                },
                {
                    dataField: "NOMBRE_MARCA_VEHICULO",
                    caption: "MARCA VEHICULO",
                    visible: false
                },
                {
                    dataField: "NOMBRE_SERIE_VEHICULO",
                    caption: "SERIE VEHICULO",
                    visible: false
                }
            ],
            onRowDblClick: function (e) {
                $('#hfIdProducto').val(e.data["ID_PRODUCTO"]);
                $('#txtCodigo').val(e.data["CODIGO"]);
                $('#txtNombreProducto').val(e.data["NOMBRE"]);
                $('#txtStock').val(e.data["STOCK"]);
                $('#txtPrecio').val(e.data["PRECIO_VENTA"]);
                $('#modalProductos').modal('hide');
            }
        }).dxDataGrid('instance');
    }

    $('#btnBuscarClientes').on('click', function (e) {
        e.preventDefault();
        GetListClientes();
    });
    $('#btnBuscarProductos').on('click', function (e) {
        e.preventDefault();
        //GetListProductos();
        GetLists('#selMarcaRepuesto', 12)
        GetLists('#selCategoria', 2)
        GetLists('#selModelo', 6)
        GetLists('#selMarcaVehiculo', 14)
        $('#modalProductos').modal('show');
    });
    $('#txtNit').on('click', function (e) {
        e.preventDefault();
        ClearCustomer();
    });

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
                        $(selObject).append('<option value="0">Todos</option>');
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
                        $(selObject).append('<option value="0">Todos</option>');
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
        GetListProductos();
    });
    $('#selMarcaVehiculo').on('change', function (e) {
        e.preventDefault();
        var id = $(this).val();
        GetListsID('#selSerieVehiculo', 16, id)
        GetListProductos();
    });
    $('#selMarcaRepuesto').on('change', function (e) {
        e.preventDefault();
        GetListProductos();
    });
    $('#selSubcategoria').on('change', function (e) {
        e.preventDefault();
        GetListProductos();
    });
    $('#selSerieVehiculo').on('change', function (e) {
        e.preventDefault();
        GetListProductos();
    });
    $('#selModelo').on('change', function (e) {
        e.preventDefault();
        GetListProductos();
    });



});