$(document).ready(function () {

    function formatNumber(num) {
        return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    }

    function ClearDatos() {
        $('#txtSerie').val('');
        $('#txtCorrelativo').val('');
        $('#txtIdentificador').val('');
        $('#txtFechaVenta').val('');
        $('#txtEstado').val('');
        $('#txtUUID').val('');
        $('#txtSerieFel').val('');
        $('#txtNumeroFel').val('');
        $('#txtNitCliente').val('');
        $('#txtNombreCliente').val('');
        $('#txtDireccionCliente').val('');
        $('#txtTotalVenta').val('');
        $('#txtSaldoPendiente').val('');
    }

    function GetVenta(ID_VENTA) {
        CallLoadingFire();
        var MTIPO = 2;
        $.ajax({
            type: 'GET',
            url: '/CARCrearCredito/GetVenta',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { MTIPO, ID_VENTA },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    var VENTA = data["data"];
                    console.log(VENTA)
                    if (VENTA == null) {
                        ShowAlertMessage('warning', 'La órden de compra ' + ID_VENTA + ' no existe o no es venta al crédito.');
                        ClearDatos();
                    }
                    else {
                        debugger
                        if ((VENTA.SALDO == 0)) {
                            $('#textoSaldo').removeClass('d-none');
                            $('#divAbono2').addClass('d-none');
                        }
                        else {
                            $('#textoSaldo').addClass('d-none');
                            $('#divAbono2').removeClass('d-none');
                        }

                        $('#txtAbono').val('');
                        $('#txtSerie').val(VENTA.SERIE);
                        $('#txtCorrelativo').val(VENTA.CORRELATIVO);
                        $('#txtIdentificador').val(VENTA.IDENTIFICADOR_UNICO);
                        $('#txtFechaVenta').val(VENTA.FECHA_STRING);
                        $('#txtEstado').val(VENTA.ESTADO);
                        $('#txtUUID').val(VENTA.UUID);
                        $('#txtSerieFel').val(VENTA.SERIE_FEL);
                        $('#txtNumeroFel').val(VENTA.NUMERO_FEL);
                        $('#txtNitCliente').val(VENTA.NIT);
                        $('#txtNombreCliente').val(VENTA.NOMBRE);
                        $('#txtDireccionCliente').val(VENTA.DIRECCION);
                        $('#txtTotalVenta').val(formatNumber(parseFloat(VENTA.TOTAL_VENTA).toFixed(2)));
                        $('#txtSaldoPendiente').val(formatNumber(parseFloat(VENTA.SALDO).toFixed(2)));
                    }
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    function CrearRecibo(ID_VENTA, ABONO, OBSERVACIONES) {
        $.ajax({
            type: 'GET',
            url: "/CARCrearCredito/CrearRecibo",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { ID_VENTA, ABONO, OBSERVACIONES },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', '¡Se agregó un abono correctamente!')
                    GetVenta(ID_VENTA);
                    GenerarRecibo(ID_VENTA);
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }

    function GenerarRecibo(id_venta) {
        CallLoadingFire('Generando recibo, por favor espera...');
        $.post("/CARCrearCredito/GenerarReciboPDF", { id_venta }, function (result) {
            var pom = document.createElement('a');
            pom.setAttribute('href', 'data:' + result.MimeType + ';base64,' + result.File);
            pom.setAttribute('download', result.FileName);
            if (document.createEvent) {
                var event = document.createEvent('MouseEvents');
                event.initEvent('click', true, true);
                pom.dispatchEvent(event);
            }
            else {
                pom.click();
            }
            CallToast('Descarga realizada con éxito.', true, 2300, '#9EC600')
        });
    }

    $('#btnBuscarVenta').on('click', function (e) {
        e.preventDefault();
        var idVenta = $('#txtVenta').val();

        if (idVenta != '')
            GetVenta(idVenta);
    });

    $('#btnCrearAbono').on('click', function (e) {
        e.preventDefault();
        var ID_VENTA = $('#txtVenta').val();
        var ABONO = $('#txtAbono').val().replace(',', '');
        var SALDO = $('#txtSaldoPendiente').val().replace(',', '');

        if (parseFloat(ABONO) > parseFloat(SALDO)) {
            ShowAlertMessage('warning', '¡El abono no puede ser mayor al saldo disponible!')
            return;
        }
        if (ABONO == 0 || ABONO == '' || ABONO == null) {
            ShowAlertMessage('warning', '¡El abono no puede ser 0!')
            return;
        }

        Swal.fire({
            title: 'Crear Abono de Crédito',
            html: 'Se creará un recibo por Q' + formatNumber(parseFloat(ABONO).toFixed(2)) + ' a la órden de compra ' + ID_VENTA + '. </h3><br/>¿Quieres continuar?<br> Ingresa una observación para la creación del recibo',
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si',
            cancelButtonText: 'No',
            input: 'text'
        }).then((result) => {
            if (result.isConfirmed) {
                CrearRecibo(ID_VENTA, ABONO, result.value)
            }
            //else if (result.dismiss === Swal.DismissReason.cancel) {
            //    SaveOrder(encabezado, listDetalles, 1)
            //}
        })
    });

});