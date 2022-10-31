$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    GetDatos()

    function GetDatos() {
        var tipo = 9;
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
                                GetOpcion(2)
                                GetInputsUpdate(e.row.data['ID_PROVEEDOR'], e.row.data['NOMBRE'], e.row.data['DESCRIPCION'], e.row.data['TELEFONO'], e.row.data['DIRECCION'], e.row.data['CONTACTO'])
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
                                Delete(e.row.data['ID_PROVEEDOR'], 5)
                            }
                        }
                    ]
                },
                {
                    dataField: "ID_PROVEEDOR",
                    caption: "ID",
                    alignment: "center",
                    visible: false
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
                    dataField: "TELEFONO",
                    caption: "TELEFONO"
                },
                {
                    dataField: "DIRECCION",
                    caption: "DIRECCION"
                },
                {
                    dataField: "CONTACTO",
                    caption: "CONTACTO"
                }
            ]
        }).dxDataGrid('instance');
    }
    function Procesar(nombre, descripcion, tipo, telefono, direccion, contacto, id, opcion) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Guardar",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                nombre, descripcion, tipo, telefono, direccion, id, contacto
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    if (opcion == 1)
                        ShowAlertMessage('success', 'Datos creados correctamente')
                    else
                        ShowAlertMessage('success', 'Datos actualizados correctamente')
                    $('#txtNombre').val('');
                    $('#txtDescripcion').val('');
                    $('#txtTelefono').val('');
                    $('#txtDireccion').val('');
                    $('#txtContacto').val('');
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
                    ShowAlertMessage('success', 'El proveedor seleccionado se inactivó correctamente.')
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function GetOpcion(opcion) {
        $('#hfOpcion').val(opcion);
        if (opcion == 1) {
            $('#txtNombre').val('');
            $('#txtDescripcion').val('');
            $('#titleModal').html('CREAR PROVEEDOR')
        }
        else if (opcion == 2) {
            $('#titleModal').html('MODIFICAR PROVEEDOR')
        }

        $('#modalDatos').modal('show');
    }
    function GetInputsUpdate(id, nombre, descripcion, telefono, direccion, contacto) {
        $('#hfID').val(id);
        $('#txtNombre').val(nombre);
        $('#txtDescripcion').val(descripcion);
        $('#txtTelefono').val(telefono);
        $('#txtDireccion').val(direccion);
        $('#txtContacto').val(contacto);
    }

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
        var telefono = $('#txtTelefono').val();
        var direccion = $('#txtDireccion').val();
        var contacto = $('#txtContacto').val();

        if (nombre == '' ) {
            ShowAlertMessage('info', 'Debe ingresar un nombre de proveedor.')
            return;
        }
        if (descripcion == '' ) {
            ShowAlertMessage('info', 'Debe ingresar una descripcion.')
            return;
        }
        if (telefono == '') {
            ShowAlertMessage('info', 'Debe ingresar un telefono.')
            return;
        }

        if (opcion == 1) {
            Procesar(nombre, descripcion, 5, telefono, direccion, contacto, 0, opcion);
        }
        else if (opcion == 2) {
            Procesar(nombre, descripcion, 6, telefono, direccion, contacto, id, opcion);
        }
    });

});