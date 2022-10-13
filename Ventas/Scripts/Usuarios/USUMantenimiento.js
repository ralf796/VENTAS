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
                        $(selObject).append('<option disabled">Seleccione una opción</option>');
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

});