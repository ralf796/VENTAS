$(document).ready(function () {

    function ValidarLogin(usuario, password) {
        $.ajax({
            type: 'GET',
            url: '/Home/ValidarLogin',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { usuario, password },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    var datoUsuario = data["data"];
                    if (datoUsuario != null) {
                        window.location.href = datoUsuario.URL_PANTALLA;
                    }
                    else {
                        $('#txtUsuarioChange').val('');
                        $('#txtUsuario').val('');
                        $('#txtPassword').val('');
                        ShowAlertMessage('warning', '¡CREDENCIALES INCORRECTAS!')
                    }
                }
                else if (state == -1) {
                    alert(data['Message'])
                }
            }
        });
    }
    function ChangePassword(correo, usuario, password) {
        $.ajax({
            type: 'GET',
            url: '/Home/ChangePassword',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { correo, usuario, password },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    var datoUsuario = data["data"];
                    if (datoUsuario != null) {
                        ShowAlertMessage('success', 'Se han cambiado las credenciales correctamente.')
                        $('#modalRecuperarPass').modal('hide');
                    }
                    else {
                        ShowAlertMessage('info','El correo ingresado no es válido en el sistema SALESMENT.')

                        $('#txtUsuarioChange').val('');
                        $('#txtCorreo').val('');
                        $('#txtpass1').val('');
                        $('#txtpass2').val('');
                    }
                }
                else if (state == -1) {
                    alert(data['Message'])
                }
            }
        });
    }

    $('#btnValidarLogin').on('click', function (e) {
        e.preventDefault();
        var usuario = $('#txtUsuario').val();
        var pass = $('#txtPassword').val();

        ValidarLogin(usuario, pass);
    });
    $('#btnCambiarPass').on('click', function (e) {
        e.preventDefault();
        var usuario = $('#txtUsuarioChange').val();
        var correo = $('#txtCorreo').val();
        var pass1 = $('#txtpass1').val();
        var pass2 = $('#txtpass2').val();

        if (pass1 != pass2) {
            ShowAlertMessage('warning','Las contraseñas no coinciden.')
            return;
        }

        ChangePassword(correo,usuario, pass1);
    });

    $('#btnAbrirModalPass').on('click', function (e) {
        e.preventDefault();
        $('#txtUsuarioChange').val('');
        $('#txtCorreo').val('');
        $('#txtpass1').val('');
        $('#txtpass2').val('');
        $('#modalRecuperarPass').modal('show');
    });

    function ShowAlertMessage(icon, descripcion) {
        Swal.fire({
            icon: icon,
            html: descripcion,
            showCancelButton: true,
            cancelButtonText: 'Cerrar',
            showConfirmButton: false,
        })
    }

});