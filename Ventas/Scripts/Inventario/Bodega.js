$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    GetDatos()
    var cont = 0;
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

                /*{
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
                                GetInputsUpdate(e.row.data['ID_BODEGA'], e.row.data['NOMBRE'], e.row.data['ESTANTERIA'], e.row.data['NIVEL'])
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
                     
                },*/

                {
                    caption: 'ACCIONES',
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        if (fieldData.ESTADO == 1)
                            $("<span>").addClass("badge badge-success").text('ACTIVO').appendTo(container);
                        else
                            $("<span>").addClass("badge badge-danger").text('INACTIVO').appendTo(container);

                        var classTmp1 = 'edit' + cont;
                        var classBTN1 = 'ml-2 hvr-grow far fa-edit btn btn-success ' + classTmp1;

                        if (fieldData.ESTADO == 1) {
                            $("<span>").addClass(classBTN1).prop('title', 'Editar').appendTo(container);
                            $('.edit' + cont).click(function (e) {
                                var id = parseInt(fieldData.ID_BODEGA);
                                GetOpcion(2)
                                GetInputsUpdate(id, fieldData.NOMBRE, fieldData.ESTANTERIA, fieldData.NIVEL)
                            })
                        }

                        var classTmp2 = 'remove' + cont;
                        var classBTN2 = 'ml-2 hvr-grow far fa-trash-alt btn btn-danger ' + classTmp2;
                        if (fieldData.ESTADO == 1) {

                            $("<span>").addClass(classBTN2).prop('title', 'Inactivar').appendTo(container);
                            $('.remove' + cont).click(function (e) {
                                var id = parseInt(fieldData.ID_BODEGA);
                                Delete(id, 4);
                            })
                        }
                        cont++;
                    }
                },
                {
                    dataField: "ID_BODEGA",
                    caption: "ID",
                    alignment: "center",
                    visible: false
                },
                {
                    dataField: "NOMBRE",
                    caption: "NOMBRE"
                },
                {
                    dataField: "NIVEL",
                    caption: "NIVEL",
                    alignment: "center"
                },
                {
                    dataField: "ESTANTERIA",
                    caption: "ESTANTERIA",
                    alignment: "center"
                }
            ]
        }).dxDataGrid('instance');

    }
    function Procesar(nombre, id, tipo, estanteria, nivel, opcion) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Guardar",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                nombre, id, tipo, estanteria, nivel
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
            $('#titleModal').html('CREAR BODEGA')
        }
        else if (opcion == 2) {
            $('#titleModal').html('MODIFICAR BODEGA')
        }
        $('#modalDatos').modal('show');
    }
    function GetInputsUpdate(id, nombre, estante, nivel) {
        $('#hfID').val(id);
        $('#txtNombre').val(nombre);
        $('#selEstanteria').val(estante);
        $('#selNivel').val(nivel);
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
        var estanteria = $('#selEstanteria').val();
        var nivel = $('#selNivel').val();

        if (nombre == '') {
            ShowAlertMessage('info', 'Debe ingresar un nombre.')
            return;
        }
        if (nivel == '' || nivel < 1) {
            ShowAlertMessage('info', 'Debe seleccionar un nivel.')
            return;
        }
        if (estanteria == '' || estanteria < 1) {
            ShowAlertMessage('info', 'Debe seleccionar una estanteria.')
            return;
        }

        if (opcion == 1) {
            Procesar(nombre, 0, 1, estanteria, nivel, opcion);
        }
        else if (opcion == 2) {
            Procesar(nombre, id, 2, estanteria, nivel, opcion);
        }
    });

});