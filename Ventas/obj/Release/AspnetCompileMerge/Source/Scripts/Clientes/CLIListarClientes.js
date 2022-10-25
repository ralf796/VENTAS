$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    GetDatos();
    function GetDatos() {
        var tipo = 4;
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/CLIListarClientes/GetClientes',
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
                    dataField: "DIRECCION",
                    caption: "DIRECCION"
                },
                {
                    dataField: "TELEFONO",
                    caption: "TELEFONO",
                    alignment: "center"
                },
                {
                    dataField: "EMAIL",
                    caption: "EMAIL"
                },
                {
                    dataField: "NIT",
                    caption: "NIT",
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
                                Delete(e.row.data['ID_CLIENTE'], 3)
                            }
                        }
                    ]
                }
            ]
        }).dxDataGrid('instance');

    }

    // funcion guardar datos
    function guardarCliente(nombre, direccion, telefono, email, nit) {
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
                    $('#txtGuardarNombre').val('');
                    $('#txtGuardarDireccion').val('');
                    $('#txtGuardarTelefono').val('');
                    $('#txtGuardarEmail').val('');
                    $('#txtGuardarNit').val('');
                    $('#modalCliente').modal('hide');
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    //funcion delete
    function Delete(id, tipo) {
        $.ajax({
            type: 'GET',
            url: "/CLIListarClientes/Delete",
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
    // funcion para abrir modal
    function GetOpcion(opcion) {
        $('#hfOpcion').val(opcion);
        if (opcion == 1) {
            $('#txtGuardarNombre').val('');
            $('#txtGuardarDireccion').val('');
            $('#txtGuardarTelefono').val('');
            $('#txtGuardarEmail').val('');
            $('#txtGuartxtGuardarNitdarDireccion').val('');
            $('#titleModal').html('CREAR CLIENTE')
        }
        else if (opcion == 2)
            $('#titleModal').html('MODIFICAR CLIENTE')

        $('#modalCliente').modal('show');
    }
    // boton para recibir datos desde el modal ------------------
    $('#guardarClienteModal').on('click', function (e) {
        e.preventDefault();
        var opcion = $('#hfOpcion').val();
        var id = $('#hfID').val();
        var nombre = $('#txtGuardarNombre').val();
        var direccion = $('#txtGuardarDireccion').val();
        var telefono = $('#txtGuardarTelefono').val();
        var email = $('#txtGuardarEmail').val();
        var nit = $('#txtGuardarNit').val();
        if (opcion == 1) {
            guardarCliente(nombre, direccion, telefono, email, nit);
        }
        else if (opcion == 2) {
            Update_Delete(nid, nombre, direccion, telefono, email, nit, 3);
        }
    });

    //boton para abrir modal
    $('#btnAbrirModal').on('click', function (e) {
        e.preventDefault();
        GetOpcion(1);
    });
});