﻿$(document).ready(function () {
    GetDatos()

    GetLists('#selCategoria', 4)
    GetLists('#selModelo', 8)
    GetLists('#selTipo', 12)
    GetLists('#selBodega', 16)


    function Update_Delete_Producto(id, tipo, ID_CATEGORIA, ID_MODELO, ID_TIPO, ID_BODEGA, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, ANIO_FABRICADO, CODIGO) {
        var mensaje = '';
        if (tipo != 20)
            mensaje = 'Datos actualizados correctamente';
        else
            mensaje = 'Se inactivó el item seleccionado';
        
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Update_Delete_Producto",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                id, tipo, ID_CATEGORIA, ID_MODELO, ID_TIPO, ID_BODEGA, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, ANIO_FABRICADO, CODIGO
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', mensaje)
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function GetDatos() {
        var tipo = 18;
        $('#tbodyDatos').empty();
        $.ajax({
            type: 'GET',
            url: '/INVMantenimiento/GetDatosTable',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { tipo },
            cache: false,
            success: function (data) {
                var lista = data["data"];
                var state = data["State"];
                if (state == 1) {
                    AddRows(lista);
                    $('#modalDatos').modal('hide');
                    //DatatableActive();
                }
                else if (state == -1)
                    alert(data["Message"])
            }
        });
    }
    function AddRows(lista) {
        $.each(lista, function (i, l) {
            var estatus = '', buttons = '';
            if (l.ESTADO == 1) {
                estatus = '<span class="badge badge-success">ACTIVO</span>';
                buttons = '<button class="btn btn-sm btn-circle btn-outline-primary edit" data-toggle="modal" data-target="#modalDatos"><i class="far fa-pencil-alt"></i></button>' +
                    '<button style="margin-left:5px;" class="btn btn-sm btn-circle btn-outline-danger remove" ><i class="far fa-trash-alt"></i></button>';
            }
            else
                estatus = '<span class="badge badge-danger">INACTIVO</span>';

            $('#tbodyProductos').append('<tr class="table">' +
                '<td class="d-none">' + l.ID_PRODUCTO + '</td>' +
                '<td>' + l.NOMBRE + '</td>' +
                '<td>' + l.DESCRIPCION + '</td>' +
                '<td>' + l.PRECIO_COSTO + '</td>' +
                '<td>' + l.PRECIO_VENTA + '</td>' +
                '<td>' + l.STOCK + '</td>' +
                '<td>' + l.ANIO_FABRICADO + '</td>' +
                '<td>' + l.CODIGO + '</td>' +
                '<td class="d-none">' + l.ID_CATEGORIA + '</td>' +
                '<td>' + l.NOMBRE_CATEGORIA + '</td>' +
                '<td class="d-none">' + l.ID_MODELO + '</td>' +
                '<td>' + l.NOMBRE_MODELO + '</td>' +
                '<td class="d-none">' + l.ID_TIPO + '</td>' +
                '<td>' + l.NOMBRE_TIPO + '</td>' +
                '<td class="d-none">' + l.ID_BODEGA + '</td>' +
                '<td>' + l.NOMBRE_BODEGA + '</td>' +
                '<td class="text-center">' + estatus + '</td>' +
                '<td class="text-center">' + buttons + '</td>' +
                '</tr>'
            );
        });
    }
    function DatatableActive() {
        tablaIni = $("#tblProductos").DataTable({
            scrollY: (window.innerHeight - 200) + 'px',
            scrollX: true,
            scrollCollapse: true,
            fixedHeader: true,
            language: {
                "lengthMenu": "Registros por pagina _MENU_",
                "zeroRecords": "No existen registros",
                "info": "Pagina _PAGE_ de _PAGES_",
                "infoEmpty": "No existen registros",
                "search": "<strong>Buscar...</strong>",
                "paginate": {
                    "first": "Primero",
                    "last": "Ultimo",
                    "next": "Siguiente",
                    "previous": "Anterior"
                },
            },
            "lengthMenu": [[20, 25, 50, -1], [20, 25, 50, "Todos"]],
            ordering: false,
            info: false,
            paginate: false,
            searching: true,
            paging: false,
            searching: false,
            destroy: true
        });
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

    $('#tbodyProductos').on('click', '.edit', function (e) {
        var ID_PRODUCTO = $(this).closest("tr").find("td").eq(0).text();
        var NOMBRE = $(this).closest("tr").find("td").eq(1).text();
        var DESCRIPCION = $(this).closest("tr").find("td").eq(2).text();
        var PRECIO_COSTO = $(this).closest("tr").find("td").eq(3).text();
        var PRECIO_VENTA = $(this).closest("tr").find("td").eq(4).text();
        var STOCK = $(this).closest("tr").find("td").eq(5).text();
        var ANIO_FABRICADO = $(this).closest("tr").find("td").eq(6).text();
        var CODIGO = $(this).closest("tr").find("td").eq(7).text();
        var ID_CATEGORIA = $(this).closest("tr").find("td").eq(8).text();
        var ID_MODELO = $(this).closest("tr").find("td").eq(10).text();
        var ID_TIPO = $(this).closest("tr").find("td").eq(12).text();
        var ID_BODEGA = $(this).closest("tr").find("td").eq(14).text();

        $('#hfIdProducto').val(ID_PRODUCTO);
        $('#txtNombre').val(NOMBRE);
        $('#txtDescripcion').val(DESCRIPCION);
        $('#txtPrecioCosto').val(PRECIO_COSTO);
        $('#txtPrecioVenta').val(PRECIO_VENTA);
        $('#txtStock').val(STOCK);
        $('#txtAnioFabricacion').val(ANIO_FABRICADO);
        $('#txtCodigo').val(CODIGO);
        $('#selCategoria').val(ID_CATEGORIA);
        $('#selModelo').val(ID_MODELO);
        $('#selTipo').val(ID_TIPO);
        $('#selBodega').val(ID_BODEGA);

        $('#modalDatos').modal('show');
    });
    $('#tbodyProductos').on('click', '.remove', function (e) {
        var id = $(this).closest("tr").find("td").eq(0).text();
        Swal.fire({
            icon: 'warning',
            title: '¿Estás seguro de inactivar el item seleccionado?',
            //showDenyButton: true,
            //denyButtonText: 'No, cancelar',
            showConfirmButton: true,
            showCancelButton: true,
            confirmButtonText: 'Si',
            cancelButtonText: 'No, cancelar',
        }).then((result) => {
            if (result.isConfirmed) {
                Update_Delete_Producto(id, 20);
            }
        })
    });
    $('#formGuardarProducto').submit(function (e) {
        e.preventDefault();
        
        var ID_PRODUCTO = $('#hfIdProducto').val();
        var ID_CATEGORIA = $('#selCategoria').val();
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
        Update_Delete_Producto(ID_PRODUCTO, 19, ID_CATEGORIA, ID_MODELO, ID_TIPO, ID_BODEGA, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, ANIO_FABRICADO, CODIGO);
    });
});