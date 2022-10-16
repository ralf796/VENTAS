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

    $('#btnGuardar').on('click', function (e) {
        e.preventDefault();
        Guardar($('#txtNombre').val(), $('#txtDescripcion').val(), 1);
    });

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
                    $('#modalCrear').modal('hide');
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
                buttons = '<button class="btn btn-sm btn-circle btn-outline-primary"><i class="far fa-pencil-alt"></i></button><button style="margin-left:5px;" class="btn btn-sm btn-circle btn-outline-danger"><i class="far fa-trash-alt"></i></button>';
            }
            else
                estatus = '<span class="badge badge-danger">INACTIVO</span>';

            $('#tbodyDatos').append('<tr class="table">' +
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
                "search": "<strong>Buscar por nombre de cliente</strong>",
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
            searching: false,
            paging: false,
            searching: false,
            destroy: true
        });
    }
});