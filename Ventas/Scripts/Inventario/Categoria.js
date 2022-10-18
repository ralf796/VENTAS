$(document).ready(function () {
    GetDatos()

    function Guardar(nombre, descripcion, tipo) {
        if (nombre == '' || nombre == null) {
            ShowAlertMessage('warning', '¡DEBES INGRESAR UN NOMBRE!');
            return;
        }
        if (descripcion == '' || descripcion == null) {
            ShowAlertMessage('warning', '¡DEBES INGRESAR UNA DESCRIPCIÓN!');
            return;
        }

        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Guardar",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                nombre, descripcion, tipo
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Datos creados correctamente')
                    $('#txtNombre').val('');
                    $('#txtDescripcion').val('');
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function Update_Delete(nombre, descripcion, tipo, id) {
        if (tipo != 3) {
            if (nombre == '' || nombre == null) {
                ShowAlertMessage('warning', '¡DEBES INGRESAR UN NOMBRE!');
                return;
            }
            if (descripcion == '' || descripcion == null) {
                ShowAlertMessage('warning', '¡DEBES INGRESAR UNA DESCRIPCIÓN!');
                return;
            }
        }
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Update_Delete",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                nombre, descripcion, tipo, id
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Datos actualizados correctamente')
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function GetDatos() {
        $('#tbodyDatos').empty();
        $.ajax({
            type: 'GET',
            url: '/INVMantenimiento/GetDatosTable',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {},
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

            $('#tbodyDatos').append('<tr class="table">' +
                '<td>' + l.ID_CATEGORIA + '</td>' +
                '<td>' + l.NOMBRE + '</td>' +
                '<td>' + l.DESCRIPCION + '</td>' +
                '<td class="text-center">' + estatus + '</td>' +
                '<td class="text-center">' + buttons + '</td>' +
                '</tr>'
            );
        });
    }
    function DatatableActive() {
        tablaIni = $("#tblDatos").DataTable({
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
    function GetOpcion(opcion) {
        $('#hfOpcion').val(opcion);
        if (opcion == 1) {
            $('#txtNombre').val('');
            $('#txtDescripcion').val('');
            $('#titleModal').html('CREAR CATEGORIA')
        }
        else if (opcion == 2)
            $('#titleModal').html('MODIFICAR CATEGORIA')

        $('#modalDatos').modal('show');
    }

    $('#tbodyDatos').on('click', '.edit', function (e) {
        var id = $(this).closest("tr").find("td").eq(0).text();
        var nombre = $(this).closest("tr").find("td").eq(1).text();
        var descripcion = $(this).closest("tr").find("td").eq(2).text();
        $('#txtNombre').val(nombre);
        $('#txtDescripcion').val(descripcion);
        GetOpcion(2);
        $('#hfID').val(id);
    });
    $('#tbodyDatos').on('click', '.remove', function (e) {
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
                Update_Delete('', '', 3, id);
            }
        })
    });
    $('#btnAbrirModal').on('click', function (e) {
        e.preventDefault();
        GetOpcion(1);
    });
    $('#btnProcesar').on('click', function (e) {
        e.preventDefault();
        var opcion = $('#hfOpcion').val();
        var id = $('#hfID').val();
        var nombre = $('#txtNombre').val();
        var descripcion = $('#txtDescripcion').val();
        if (opcion == 1) {
            Guardar(nombre, descripcion, 1);
        }
        else if (opcion == 2) {
            Update_Delete(nombre, descripcion, 2, id);
        }
    });

});