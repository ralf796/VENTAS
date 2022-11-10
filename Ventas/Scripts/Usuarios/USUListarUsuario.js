$(document).ready(function () {
    GetLists('#selGuardarTipoEmpleado', 2);
    GetLists('#selGuardarRol', 3);
    GetDatos();

    function ClearFormCreate() {
        $('#txtGuardarPrimerNombre').val('');
        $('#txtGuardarSegundoNombre').val('');
        $('#txtGuardarPrimerApellido').val('');
        $('#txtGuardarSegundoApellido').val('');
        $('#txtGuardarCelular').val('');
        $('#txtGuardarTelCasa').val('');
        $('#txtGuardarDireccion').val('');
        $('#selGuardarTipoEmpleado').val(-1);
        $('#txtGuardarEmail').val('');
        $('#txtGuardarUsuario').val('');
        $('#txtGuardarPassword').val('');
        $('#selGuardarRol').val(-1);
    }

    function ClearFormEditar() {
        $('#txtEditarPrimerNombre').val('');
        $('#txtEditarSegundoNombre').val('');
        $('#txtEditarPrimerApellido').val('');
        $('#txtEditarSegundoApellido').val('');
        $('#txtEditarCelular').val('');
        $('#txtEditarTelCasa').val('');
        $('#txtEditarDireccion').val('');
        $('#selEditarTipoEmpleado').val(-1);
        $('#txtEditarEmail').val('');
        $('#txtEditarUsuario').val('');
        $('#txtEditarPassword').val('');
        $('#selEditarRol').val(-1);
    }

    function CallBtnEdit(primerNombre, segundoNombre, primerApellido, segundoApellido, celular, telefono, direccion, email, tipoEmpleado, rol, idPath) {
        $('#txtEditarPrimerNombre').val(primerNombre);
        $('#txtEditarSegundoNombre').val(segundoNombre);
        $('#txtEditarPrimerApellido').val(primerApellido);
        $('#txtEditarSegundoApellido').val(segundoApellido);
        $('#txtEditarCelular').val(celular);
        $('#txtEditarTelCasa').val(telefono);
        $('#txtEditarDireccion').val(direccion);
        $('#selEditarTipoEmpleado').val(tipoEmpleado);
        $('#txtEditarEmail').val(email);
        $('#selEditarRol').val(rol);
        $('#idFotografiaEditar').val(idPath);

        $('#modalEditarUsuario').modal('show')
    }

    function GetLists(selObject, tipo) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: 'GET',
                url: '/USUListarUsuario/GetLists',
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
                            if (tipo == 2) {
                                $(selObject).append('<option data-descripcion="' + dato.DESCRIPCION + '" value="' + dato.ID_TIPO_EMPLEADO + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 3) {
                                $(selObject).append('<option data-descripcion="' + dato.DESCRIPCION + '" value="' + dato.ID_ROL + '">' + dato.NOMBRE + '</option>');
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
    var cont = 0;
    function GetDatos() {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/USUListarUsuario/GetUsuarios',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: {},
                    cache: false,
                    success: function (data) {
                        var state = data["State"];
                        if (state == 1) {
                            data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);
                            d.resolve(data);
                        }
                        else if (state == -1)
                            ShowAlertMessage(data["Message"])
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


            filterRow: {
                visible: true,
                applyFilter: "auto"
            },
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: "Buscar..."
            },
            headerFilter: {
                visible: true
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
            columnAutoWidth: true,

            onRowPrepared(e) {
                //e.rowElement.css("background-color", "#A7BCD6");
                //e.rowElement.css("color", "#000000");
            },
            columns: [
                {
                    dataField: 'PATH_IMAGEN',
                    width: 90,
                    cellTemplate(container, options) {
                        $('<div>').append($('<img>', { src: options.value })).appendTo(container);
                    },
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
                    caption: 'ACCIONES',
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        //ESTADO
                        if (fieldData.ESTADO == 1)
                            $("<span>").addClass("badge badge-success").text('ACTIVO').appendTo(container);
                        else
                            $("<span>").addClass("badge badge-danger").text('INACTIVO').appendTo(container);

                        //BTN EDITAR
                        if (fieldData.ESTADO == 1) {
                            var classTmp1 = 'edit' + cont;
                            var classBTN1 = 'ml-2 hvr-grow far fa-edit btn btn-success ' + classTmp1;
                            $("<span>").addClass(classBTN1).prop('title', 'Editar').appendTo(container);
                            $('.edit' + cont).click(function (e) {
                                var id = parseInt(fieldData.ID_USUARIO);
                                ClearFormCreate();
                                GetLists('#selEditarTipoEmpleado', 2);
                                GetLists('#selEditarRol', 3);
                                CallBtnEdit(fieldData.PRIMER_NOMBRE, fieldData.SEGUNDO_NOMBRE, fieldData.PRIMER_APELLIDO, fieldData.SEGUNDO_APELLIDO, dataField.CELULAR, dataField.TELEFONO, dataField.EMAIL, dataField.ID_TIPO_EMPLEADO, dataField.ID_ROL, dataField.PATH_IMAGEN);
                            })

                            //BTN ELIMINAR
                            var classTmp2 = 'remove' + cont;
                            var classBTN2 = 'ml-2 hvr-grow far fa-trash-alt btn btn-danger ' + classTmp2;
                            $("<span>").addClass(classBTN2).prop('title', 'Inactivar').appendTo(container);
                            $('.remove' + cont).click(function (e) {
                                var id = parseInt(fieldData.ID_USUARIO);
                                Delete(id);
                            })
                        }
                        cont++;
                    }
                },
                {
                    dataField: "ID_USUARIO",
                    caption: "ID",
                    visible: false
                },
                {
                    dataField: "ID_ROL",
                    caption: "ID ROL",
                    visible: false
                },
                {
                    dataField: "ID_TIPO_EMPLEADO",
                    caption: "ID TIPO EMPLEADO",
                    visible: false
                },
                {
                    dataField: "USUARIO",
                    caption: "USUARIO",
                    alignment: "center"
                },
                {
                    dataField: "NOMBRE_ROL",
                    caption: "ROL"
                },
                {
                    dataField: "NOMBRE_TIPO_EMPLEADO",
                    caption: "TIPO EMPLEADO"
                },
                {
                    dataField: "PRIMER_NOMBRE",
                    caption: "PRIMER NOMBRE"
                },
                {
                    dataField: "SEGUNDO_NOMBRE",
                    caption: "SEGUNDO NOMBRE"
                },
                {
                    dataField: "PRIMER_APELLIDO",
                    caption: "PRIMER APELLIDO"
                },
                {
                    dataField: "SEGUNDO_APELLIDO",
                    caption: "SEGUNDO_APELLIDO"
                },
                {
                    dataField: "CELULAR",
                    caption: "CELULAR"
                },
                {
                    dataField: "TELEFONO",
                    caption: "TELEFONO"
                },
                {
                    dataField: "DIRECION",
                    caption: "DIRECCION"
                },
                {
                    dataField: "URL_PANTALLA",
                    caption: "URL DEFAULT",
                    visible: false
                },
                {
                    dataField: "EMAIL",
                    caption: "EMAIL"
                }
            ]
        }).dxDataGrid('instance');

    }

    function GuardarUsuarioFoto() {
        var primerNombre = $('#txtGuardarPrimerNombre').val();
        var segundoNombre = $('#txtGuardarSegundoNombre').val();
        var primerApellido = $('#txtGuardarPrimerApellido').val();
        var segundoApellido = $('#txtGuardarSegundoApellido').val();
        var celular = $('#txtGuardarCelular').val();
        var telefono = $('#txtGuardarTelCasa').val();
        var direccion = $('#txtGuardarDireccion').val();
        var idTipoEmpleado = $('#selGuardarTipoEmpleado').val();
        var email = $('#txtGuardarEmail').val();
        var usuario = $('#txtGuardarUsuario').val();
        var password = $('#txtGuardarPassword').val();
        var urlFoto = $('#idFotografia').val();
        var idRol = $('#selGuardarRol').val();

        if (idTipoEmpleado == '' || idTipoEmpleado == null) {
            ShowAlertMessage('warning', '¡DEBES SELECCIONAR EL TIPO DE EMPLEADO!');
            $('#selGuardarTipoEmpleado').focus();
            return;
        }
        if (idRol == '' || idRol == null) {
            ShowAlertMessage('warning', '¡DEBES SELECCIONAR EL ROL DE ACCESOS!');
            $('#selGuardarRol').focus();
            return;
        }

        var formData = new FormData();
        formData.append('primerNombre', primerNombre);
        formData.append('segundoNombre', segundoNombre);
        formData.append('primerApellido', primerApellido);
        formData.append('segundoApellido', segundoApellido);
        formData.append('celular', celular);
        formData.append('telefono', telefono);
        formData.append('direccion', direccion);
        formData.append('idTipoEmpleado', idTipoEmpleado);
        formData.append('email', email);
        formData.append('usuario', usuario);
        formData.append('password', password);
        formData.append('idRol', idRol);
        formData.append('urlFoto', urlFoto);
        if ($('#idFotografia')[0].files != undefined) {
            let files = $('#idFotografia')[0].files;
            formData.append('foto', files[0]);
        }
        $.ajax({
            type: 'POST',
            url: '/USUListarUsuario/GuardarUsuarioFoto',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                //----------------ERROR CATCH----------------
                if (data["State"] == -1)
                    ShowAlertMessage('warning', data['Message'])
                else if (data["State"] == 1) {
                    console.log(data['path_foto'])
                    $('#idFotografia').val('');
                    //$('#imgModal').attr('src', data['path_foto']);
                    ShowAlertMessage('success', 'Datos ingresados correctamente')
                    ClearFormCreate();
                    GetDatos();
                    $('#modalCrearUsuario').modal('hide');
                }
            }
        });
    }

    function EditarUsuarioFoto() {
        var primerNombre = $('#txtGuardarPrimerNombre').val();
        var segundoNombre = $('#txtGuardarSegundoNombre').val();
        var primerApellido = $('#txtGuardarPrimerApellido').val();
        var segundoApellido = $('#txtGuardarSegundoApellido').val();
        var celular = $('#txtGuardarCelular').val();
        var telefono = $('#txtGuardarTelCasa').val();
        var direccion = $('#txtGuardarDireccion').val();
        var idTipoEmpleado = $('#selGuardarTipoEmpleado').val();
        var email = $('#txtGuardarEmail').val();
        var urlFoto = $('#idFotografia').val();
        var idRol = $('#selGuardarRol').val();
        var id = $('#hfId').val();

        if (idTipoEmpleado == '' || idTipoEmpleado == null) {
            ShowAlertMessage('warning', '¡DEBES SELECCIONAR EL TIPO DE EMPLEADO!');
            $('#selGuardarTipoEmpleado').focus();
            return;
        }
        if (idRol == '' || idRol == null) {
            ShowAlertMessage('warning', '¡DEBES SELECCIONAR EL ROL DE ACCESOS!');
            $('#selGuardarRol').focus();
            return;
        }

        var formData = new FormData();
        formData.append('primerNombre', primerNombre);
        formData.append('segundoNombre', segundoNombre);
        formData.append('primerApellido', primerApellido);
        formData.append('segundoApellido', segundoApellido);
        formData.append('celular', celular);
        formData.append('telefono', telefono);
        formData.append('direccion', direccion);
        formData.append('idTipoEmpleado', idTipoEmpleado);
        formData.append('email', email);
        formData.append('idRol', idRol);
        formData.append('urlFoto', urlFoto);
        formData.append('id', id);
        if ($('#idFotografia')[0].files != undefined) {
            let files = $('#idFotografia')[0].files;
            formData.append('foto', files[0]);
        }
        $.ajax({
            type: 'POST',
            url: '/USUListarUsuario/EditarUsuarioFoto',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                //----------------ERROR CATCH----------------
                if (data["State"] == -1)
                    ShowAlertMessage('warning', data['Message'])
                else if (data["State"] == 1) {
                    console.log(data['path_foto'])
                    $('#idFotografia').val('');
                    $('#imgModal').attr('src', data['path_foto']);
                    ShowAlertMessage('success', 'Datos actualizados correctamente')
                    ClearFormEditar();
                    GetDatos();
                    $('#modalEditarUsuario').modal('hide');
                }
            }
        });
    }

    function Delete(id) {
        $.ajax({
            type: 'GET',
            url: "/USUListarUsuario/Delete",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { id },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'El usuario seleccionado se inactivó correctamente.')
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    $('#btnAbrirModalCrear').on('click', function (e) {
        e.preventDefault();
        $('#modalCrearUsuario').modal('show')
        ClearFormCreate()
        GetLists('#selGuardarTipoEmpleado', 2);
        GetLists('#selGuardarRol', 3);
    });

    $('#formGuardarEmpleadoUsuario').submit(function (e) {
        e.preventDefault();
        GuardarUsuarioFoto();
    });

    $('#formEditarEmpleadoUsuario').submit(function (e) {
        e.preventDefault();
        EditarUsuarioFoto();
    });

});