$(document).ready(function () {
    GetUsuarios();

    function GetUsuarios() {
        $('#tbodyUsuarios').empty();
        $.ajax({
            type: 'GET',
            url: '/USUListarUsuario/GetUsuarios',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {},
            cache: false,
            success: function (data) {
                var lista = data["data"];
                var state = data["State"];
                if (state == 1) {
                    AddRows(lista);
                    DatatableActive();
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

            $('#tbodyUsuarios').append('<tr class="table">' +
                '<td>' + l.USUARIO + '</td>' +
                '<td>' + l.NOMBRE_ROL + '</td>' +
                '<td>' + l.NOMBRE_TIPO_EMPLEADO + '</td>' +
                '<td>' + l.PRIMER_NOMBRE + ' ' + l.SEGUNDO_NOMBRE + ' ' + l.PRIMER_APELLIDO + ' ' + l.SEGUNDO_APELLIDO + '</td>' +
                '<td>' + l.TELEFONO + '</td>' +
                '<td>' + l.DIRECCION + '</td>' +
                '<td>' + l.URL_PANTALLA + '</td>' +
                '<td>' + l.EMAIL + '</td>' +
                '<td class="text-center">' + estatus + '</td>' +
                '<td class="text-center">' + buttons + '</td>' +
                '</tr>'
            );
        });
    }
    function DatatableActive() {
        tablaIni = $("#tblUsuarios").DataTable({
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