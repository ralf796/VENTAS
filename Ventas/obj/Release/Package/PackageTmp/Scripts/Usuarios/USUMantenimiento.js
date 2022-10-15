$(document).ready(function () {
    GetLists('#selGuardarTipoEmpleado', 2);
    GetLists('#selGuardarRol', 3);


    function GetLists(selObject, tipo) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: 'GET',
                url: '/USUMantenimiento/GetLists',
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
            url: "/USUMantenimiento/GuardarUsuario",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                primerNombre, segundoNombre, primerApellido, segundoApellido, celular, telefono, direccion, idTipoEmpleado, email, usuario, password, idRol
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Datos ingresados correctamente')
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
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    $('#formGuardarEmpleadoUsuario').submit(function (e) {
        e.preventDefault();
        GuardarUsuario();
    });

});