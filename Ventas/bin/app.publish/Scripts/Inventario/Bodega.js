$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);

    GetDatos()

    function GetDatos() {
        var tipo = 7;
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/INVMantenimiento/GetDatosTable',
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
                            alert(data["Message"])
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
                    dataField:"ID_BODEGA",
                    caption: "ID",
                    alignment: "center",
                    visible:false
                },
                {
                    dataField:"NOMBRE",
                    caption: "NOMBRE"
                },
                {
                    dataField:"ESTANTERIA",
                    caption: "ESTANTERIA",
                    alignment: "center"
                },
                {
                    dataField:"NIVEL",
                    caption: "NIVEL",
                    alignment: "center"
                },
                {
                    caption: "ESTADO",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        container.addClass(fieldData.ESTADO != 1 ? "dec" : "");

                        if (fieldData.ESTADO == 1)
                            $("<span>").addClass("badge badge-success").text('ACTIVO').appendTo(container);
                        else
                            $("<span>").addClass("badge badge-danger").text('INACTIVO').appendTo(container);

                    }
                },
                {
                    caption: "ACCIONES",
                    type: "buttons",
                    alignment: "center",
                    buttons: [
                        {
                            visible: function (e) {
                                var visible = false;
                                if (e.row.data.ESTADO == 1)
                                    visible = true;
                                return visible;
                            },
                            hint: "Editar",
                            icon: "edit",
                            onClick: function (e) {
                                alert('en desarrollo')
                            }
                        },
                        {
                            visible: function (e) {
                                var visible = false;
                                if (e.row.data.ESTADO == 1)
                                    visible = true;
                                return visible;
                            },
                            hint: "Inactivar",
                            icon: "clear",
                            onClick: function (e) {
                                Delete(e.row.data['ID_BODEGA'], 4)
                            }
                        }
                    ]
                }
            ]
        }).dxDataGrid('instance');

    }
    function Guardar(nombre, descripcion, tipo, estanteria, nivel) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Guardar",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                nombre, descripcion, tipo, estanteria, nivel
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Datos creados correctamente')
                    $('#txtNombre').val('');
                    $('#txtDescripcion').val('');
                    $('#modalDatos').modal('hide');
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function Delete(id, tipo) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Delete",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { id, tipo },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'La bodega seleccionada se inactivó correctamente.')
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function Update_Delete(nombre, descripcion, tipo, id, estanteria, nivel) {
        var mensaje = 'Se inactivó el item seleccionado';
        if (tipo != 15) {
            if (nombre == '' || nombre == null) {
                ShowAlertMessage('warning', '¡DEBES INGRESAR UN NOMBRE!');
                return;
            }
            if (descripcion == '' || descripcion == null) {
                ShowAlertMessage('warning', '¡DEBES INGRESAR UNA DESCRIPCIÓN!');
                return;
            }
            mensaje = 'Datos actualizados correctamente';
        }
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Update_Delete",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                nombre, descripcion, tipo, id, estanteria, nivel
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
                '<td class="d-none">' + l.ID_BODEGA + '</td>' +
                '<td>' + l.NOMBRE + '</td>' +
                '<td>' + l.ESTANTERIA + '</td>' +
                '<td>' + l.NIVEL + '</td>' +
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
            $('#titleModal').html('CREAR BODEGA')
        }
        else if (opcion == 2)
            $('#titleModal').html('MODIFICAR BODEGA')

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
                Delete(id, 4)
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
        var estanteria = $('#selEstanteria').val();
        var nivel = $('#selNivel').val();
        if (opcion == 1) {
            Guardar(nombre, '', 4, estanteria, nivel);
        }
        else if (opcion == 2) {
            Update_Delete(nombre, 14, id, estanteria, nivel);
        }
    });

});