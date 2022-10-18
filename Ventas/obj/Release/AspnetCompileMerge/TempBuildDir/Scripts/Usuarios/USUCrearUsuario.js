$(document).ready(function () {
    GetLists('#selGuardarTipoEmpleado', 2);
    GetLists('#selGuardarRol', 3);


    function GetLists(selObject, tipo) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: 'GET',
                url: '/USUCrearUsuario/GetLists',
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

    function GuardarUsuario() {
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

        $.ajax({
            type: 'GET',
            url: "/USUCrearUsuario/GuardarUsuario",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                primerNombre, segundoNombre, primerApellido, segundoApellido, celular, telefono, direccion, idTipoEmpleado, email, usuario, password, idRol, urlFoto
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Datos ingresados correctamente')
                    //$('#txtGuardarPrimerNombre').val('');
                    //$('#txtGuardarSegundoNombre').val('');
                    //$('#txtGuardarPrimerApellido').val('');
                    //$('#txtGuardarSegundoApellido').val('');
                    //$('#txtGuardarCelular').val('');
                    //$('#txtGuardarTelCasa').val('');
                    //$('#txtGuardarDireccion').val('');
                    //$('#selGuardarTipoEmpleado').val(-1);
                    //$('#txtGuardarEmail').val('');
                    //$('#txtGuardarUsuario').val('');
                    //$('#txtGuardarPassword').val('');
                    //$('#selGuardarRol').val(-1);
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    $('#formGuardarEmpleadoUsuario').submit(function (e) {
        e.preventDefault();
        //GuardarUsuario();
        GuardarUsuarioFoto();
    });


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
            url: '/USUCrearUsuario/GuardarUsuarioFoto',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                //----------------ERROR CATCH----------------
                if (data["State"] == -1)
                    alert(data["State"])
                else if (data["State"] == 1) {
                    console.log(data['path_foto'])
                    $('#idFotografia').val('');
                    $('#imgModal').attr('src', data['path_foto']);

                    ShowAlertMessage('success', 'Datos ingresados correctamente')
                    //$('#txtGuardarPrimerNombre').val('');
                    //$('#txtGuardarSegundoNombre').val('');
                    //$('#txtGuardarPrimerApellido').val('');
                    //$('#txtGuardarSegundoApellido').val('');
                    //$('#txtGuardarCelular').val('');
                    //$('#txtGuardarTelCasa').val('');
                    //$('#txtGuardarDireccion').val('');
                    //$('#selGuardarTipoEmpleado').val(-1);
                    //$('#txtGuardarEmail').val('');
                    //$('#txtGuardarUsuario').val('');
                    //$('#txtGuardarPassword').val('');
                    //$('#selGuardarRol').val(-1);
                }
            }
        });
    }
});