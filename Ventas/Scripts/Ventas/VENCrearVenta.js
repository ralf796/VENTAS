$(document).ready(function () {
    //CallLoadingFire('Cargandoo-.')
    GetListProductos();


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
            groupPanel: { visible: true },
            grouping: {
                autoExpandAll: false,
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
                    dataField: 'PATH_IMAGEN',
                    caption: 'IMAGEN',
                    width: 130,
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        $("<img>").attr('src', fieldData.PATH_IMAGEN).css('width', '70px').appendTo(container);
                        //$("<img>").attr('src','https://itpromklao.com/wp-content/uploads/2020/03/Slide1-135.jpg').css('width','70px').appendTo(container);
                    }
                },

                {
                    dataField: "ID_PRODUCTO",
                    caption: "ID PRODUCTO",
                    visible: false
                },
                {
                    dataField: "NOMBRE",
                    caption: "NOMBRE",
                    width: 200,
                },
                {
                    dataField: "DESCRIPCION",
                    caption: "DESCRIPCION",
                    width: 230
                },
                {
                    dataField: "CODIGO",
                    caption: "CODIGO 1",
                    alignment: "center",
                    width: 200
                },
                {
                    dataField: "CODIGO2",
                    caption: "CODIGO 2",
                    alignment: "center",
                    width: 200
                },
                {
                    dataField: "NOMBRE_MODELO",
                    caption: "MODELO",
                    width: 200
                },
                {
                    dataField: "STOCK",
                    caption: "STOCK",
                    alignment: "center",
                    width: 200,
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
                    dataField: "PRECIO_VENTA",
                    caption: "PRECIO",
                    alignment: "right",
                    alignment: "right",
                    format: ",##0.00",
                    width: 200
                },
                {
                    dataField: "NOMBRE_DISTRIBUIDOR",
                    caption: "DISTRIBUIDOR",
                    width: 200
                },
                {
                    dataField: "NOMBRE_MARCA_REPUESTO",
                    caption: "MARCA REPUESTO",
                    width: 200
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
                }
            ],
            onRowDblClick: function (e) {
                $('#hfIdProducto').val(e.data["ID_PRODUCTO"]);
                $('#txtCodigo').val(e.data["CODIGO"]);
                $('#txtNombreProducto').val(e.data["NOMBRE"]);
                $('#txtStock').val(e.data["STOCK"]);
                $('#txtPrecio').val(e.data["PRECIO_VENTA"]);
                $('#hfCodigo1').val(e.data["CODIGO"]);
                $('#hfCodigo2').val(e.data["CODIGO2"]);
                $('#hfMarcaR').val(e.data["NOMBRE_MARCA_REPUESTO"]);
                $('#hfNombre').val(e.data["NOMBRE"]);
                $('#modalProductos').modal('hide');
            }
        }).dxDataGrid('instance');
    }
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
                        console.log(list)
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
                                $(selObject).append('<option value="' + dato.ID_MARCA_REPUESTO + '">' + dato.ID_MARCA_REPUESTO + ' -  ' + dato.NOMBRE + '</option>');
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
    function SaveCustomer(nombre, direccion, telefono, email, nit) {
        $.ajax({
            type: 'GET',
            url: "/CLIListarClientes/GuardarCliente",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                nombre, direccion, telefono, email, nit
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Datos ingresados correctamente')
                    $('#txtNit').val(nit);
                    GetCliente(nit);
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
    function DETALLE(ID_PRODUCTO, CANTIDAD, PRECIO_UNITARIO, TOTALTOTAL, IVA, DESCUENTO, SUBTOTAL) {
        this.ID_PRODUCTO = ID_PRODUCTO;
        this.CANTIDAD = CANTIDAD;
        this.PRECIO_UNITARIO = PRECIO_UNITARIO;
        this.TOTALTOTAL = TOTALTOTAL;
        this.IVA = IVA;
        this.DESCUENTO = DESCUENTO;
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
                if (state == 1 ) {
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

        var subtotal = (parseFloat(cantidad) * parseFloat(precio)) - (parseFloat(cantidad) * parseFloat(descuento));
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

        alert(idCliente)
        if (idCliente == '' || idCliente == null) {
            ShowAlertMessage('info', 'Debes seleccionar un cliente para la venta.');
            return;
        }

        alert(listDetalles.length)
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
        SaveCustomer(nombre, direccion, telefono, email, nit);
    });
});