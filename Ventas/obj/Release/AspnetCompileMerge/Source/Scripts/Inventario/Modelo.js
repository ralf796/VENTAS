$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);

    GetDatos()

    function GetDatos() {
        var tipo = 5;
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
                    dataField: "ID_MODELO",
                    caption: "ID",
                    alignment: "center",
                    visible: false
                },
                {
                    dataField: "ANIO_INICIAL",
                    caption: "AÑO INICIAL"
                },
                {
                    dataField: "ANIO_FINAL",
                    caption: "AÑO FINAL"
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
                                Delete(e.row.data['ID_MODELO'], 3)
                            }
                        }
                    ]
                }
            ]
        }).dxDataGrid('instance');
    }
    function Guardar(anioI, anioF, tipo) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Guardar",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                anioI, anioF, tipo
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    $('#txtAnioF').val('');
                    $('#txtAnioI').val('');
                    $('#modalDatos').modal('hide');
                    GetDatos()
                    ShowAlertMessage('success', 'Datos creados correctamente')
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
            $('#titleModal').html('CREAR MODELO')
        }
        else if (opcion == 2)
            $('#titleModal').html('MODIFICAR MODELO')

        $('#modalDatos').modal('show');
    }

    $('#btnAbrirModal').on('click', function (e) {
        e.preventDefault();
        GetOpcion(1);
    });
    $('#btnProcesar').on('click', function (e) {
        e.preventDefault();
        var opcion = $('#hfOpcion').val();
        var id = $('#hfID').val();
        var anioI = $('#txtAnioI').val();
        var anioF = $('#txtAnioF').val();

        if (opcion == 1) {
            Guardar(anioI, anioF, 3);
        }
        else if (opcion == 2) {
            Update_Delete(nombre, 14, id, estanteria, nivel);
        }
    });

});