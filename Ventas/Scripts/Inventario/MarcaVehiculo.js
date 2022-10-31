$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    GetDatos()

    function GetDatos() {
        var tipo = 13;
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
                                GetInputsUpdate(e.row.data['ID_MARCA_VEHICULO'], e.row.data['NOMBRE'], e.row.data['DESCRIPCION'])
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
                                Delete(e.row.data['ID_MARCA_VEHICULO'], 7)
                            }
                        }
                    ]
                },
                {
                    dataField: "ID_MARCA_VEHICULO",
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
                }
            ]
        }).dxDataGrid('instance');
    }
    function Procesar(nombre, descripcion, tipo, id, opcion) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Guardar",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                nombre, descripcion, tipo, id
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
    function GetOpcion(opcion) {
        $('#hfOpcion').val(opcion);
        if (opcion == 1) {
            $('#txtNombre').val('');
            $('#txtDescripcion').val('');
            $('#titleModal').html('CREAR MARCA DE VEHICULO')
        }
        else if (opcion == 2)
            $('#titleModal').html('MODIFICAR MARCA DE VEHICULO')

        $('#modalDatos').modal('show');
    }
    function GetInputsUpdate(id, nombre, descripcion) {
        $('#hfID').val(id);
        $('#txtNombre').val(nombre);
        $('#txtDescripcion').val(descripcion);
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


        if (nombre == '') {
            ShowAlertMessage('info', 'Debe ingresar un nombre.')
            return;
        }
        if (descripcion == '') {
            ShowAlertMessage('info', 'Debe ingresar una descripcion.')
            return;
        }

        if (opcion == 1) {
            Procesar(nombre, descripcion, 13, 0, opcion);
        }
        else if (opcion == 2) {
            Procesar(nombre, descripcion, 14, id, opcion);
        }
    });

});