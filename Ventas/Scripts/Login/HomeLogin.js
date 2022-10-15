$(document).ready(function () {

    function ValidarLogin(usuario, password) {
        $.ajax({
            type: 'GET',
            url: 'Home/ValidarLogin',
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

    $('#btnValidarLogin').on('click', function (e) {
        e.preventDefault();
        var usuario = $('#txtUsuario').val();
        var pass = $('#txtPassword').val();

        ValidarLogin(usuario, pass);
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