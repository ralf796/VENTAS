$(document).ready(function () {

    $("#txtUploadExcel").change(function () {
        Swal.fire({
            title: 'Carga masiva de modificación de precios',
            text: "Se cargaran todos los productos con precios nuevos. ¿Quiere continuar con la carga?",
            type: 'info',
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si',
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.value) {

                var file = $("#txtUploadExcel").val();
                var formData = new FormData();
                var totalFiles = document.getElementById("txtUploadExcel").files.length;
                for (var i = 0; i < totalFiles; i++) {
                    var file = document.getElementById("txtUploadExcel").files[i];
                    formData.append("FileUpload", file);
                }
                if (file != null && file != "") {
                    CallLoadingFire('Cargando subida de datos, por favor espere.')
                    $.ajax({
                        type: "POST",
                        url: "/INVModificarPreciosMasivo/CargarExcel",
                        data: formData,
                        dataType: 'json',
                        contentType: false,
                        processData: false,
                        success: function (data) {
                            var state = data['State'];
                            var contRows = data['dataGroup'];
                            var contRowsDetails = data['dataGroupDetail'];
                            if (state == 1) {
                                $("#txtUploadExcel").val('');
                                Swal.fire({
                                    icon: 'success',
                                    type: 'success',
                                    html: '¡Se modificaron ' + contRows + ' precios correctamente.!<br/>Se cargaron ' + contRowsDetails + ' filas del archivo excel seleccionado.',
                                    showCancelButton: true,
                                    cancelButtonText: 'Cerrar',
                                    showConfirmButton: false,
                                })
                            }
                            else if (state == -1) {
                                ShowAlertMessage('warning', data['Message']);
                            }
                        },
                        error: function (jqXHR, ex) {
                            console.log(jqXHR);
                            console.log(ex);
                        }
                    });
                }
            }
        })
    });
});