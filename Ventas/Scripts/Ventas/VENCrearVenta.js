$(document).ready(function () {
    $('#txtCodigo').on('click', function (e) {
        //$('#modalProductos').modal('show');
    });


    var DetalleVenta = $("#tbodyDetalleVenta");
    function AddDetail(id, producto, cantidad, precio, descuento) {
        if (cantidad == '')
            cantidad = 0;
        if (precio == '')
            precio = 0;
        if (descuento == '')
            descuento = 0;

        var subtotal = (parseFloat(cantidad) * parseFloat(precio)) - parseFloat(descuento);
        var remove = '<a title="Eliminar" class="btn btn-link btn-danger removeDetail" style="margin: 0 0 !important"><i class="far fa-times"></i></a>';
        DetalleVenta.append('<tr>' +
            '<td class="text-center">' + remove + '</td>' +
            '<td>' + id + '</td>' +
            '<td>' + producto + '</td>' +
            '<td class="text-center">' + cantidad + '</td>' +
            '<td class="text-right">' + formatNumber(precio) + '</td>' +
            '<td class="text-right">' + formatNumber(descuento) + '</td>' +
            '<td class="text-right">' + formatNumber(subtotal) + '</td>' +
            '</tr>');
    }

    $('#btnAgregarDetalle').on('click', function (e) {
        var id = $('#hfIdProducto').val();
        var producto = $('#txtCodigo').val() + ' - ' + $('#txtNombreProducto').val();
        var cantidad = $('#txtCantidad').val();
        var precio = $('#txtPrecio').val();
        var descuento = $('#txtDescuento').val();

        console.log(cantidad);
        console.log(precio);
        console.log(descuento);
        AddDetail(id, producto, cantidad, precio, descuento);
    });

    DetalleVenta.on('click', '.removeDetail', function () {
        $(this).closest('tr').remove();
        RefreshSum();
    });

    function RefreshSum() {
        var total = 0
        $('#tbodyDetalleVenta tr').each(function () {
            total = total + parseFloat($(this).find("td").eq(5).text().replace(",", ""));
        });
        $('#txtTotal').val(formatNumber(total));
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
        $.ajax({
            type: 'GET',
            url: '/VENCrearVenta/GetCliente',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { nit },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    var CLIENTE = data["data"];

                    if (CLIENTE == null) {
                        ShowAlertMessage('warning', 'No existe el cliente con el nit ingresado.');
                    }
                    else if (CLIENTE.codigo_respuesta == '01') {
                        $('#txtNombreCliente').val(CLIENTE.NOMBRE);
                    }
                    else if (CLIENTE.CODIGO_RESPUESTA != '00') {
                        ShowAlertMessage('warning', CLIENTE.MENSAJE_RESPUESTA);
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
        var tipo = 2;
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
            onRowClick: function (e) {
                //OBTAIN YOUR GRID DATA HERE
                var grid = $("#gridContainer").dxDataGrid('instance');
                var rows = grid.getSelectedRowsData();

                if (clickTimer && lastRowCLickedId === e.rowIndex) {
                    clearTimeout(clickTimer);
                    clickTimer = null;
                    lastRowCLickedId = e.rowIndex;
                    //YOUR DOUBLE CLICK EVENT HERE
                    alert('double clicked!');
                } else {
                    clickTimer = setTimeout(function () { }, 250);
                }

                lastRowCLickedId = e.rowIndex;
            }
        }).dxDataGrid('instance');

    }
});