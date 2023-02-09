let fechaI;
let fechaF;
$(document).ready(function () {

    fechaCompra = new AirDatepicker('#txtFechaCompra', {
        autoClose: true,
        view: 'days',
        minView: 'days',
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date()]
    });
    fechaPago = new AirDatepicker('#txtFechaPago', {
        autoClose: true,
        view: 'days',
        minView: 'days',
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date()]
    });
    fechaEntrega = new AirDatepicker('#txtFechaEntrega', {
        autoClose: true,
        view: 'days',
        minView: 'days',
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date()]
    });

    function Guardar() {        
        var nombreProveedor = $('#txtNombreProveedor').val();
        var contactoProveedor = $('#txtContactoProveedor').val();
        var fechaPedido = DateFormat(fechaCompra.lastSelectedDate);
        var fechadePago = DateFormat(fechaPago.lastSelectedDate);
        var fechadeEntrega = DateFormat(fechaEntrega.lastSelectedDate);
        var telefono = $('#txtTelefono').val();
        var serie = $('#txtSerieFactura').val();
        var numero = $('#txtNumero').val();
        var monto = $('#txtValorFactura').val();

        var formData = new FormData();
        formData.append('nombreProveedor', nombreProveedor);
        formData.append('contactoProveedor', contactoProveedor);
        formData.append('fechaPedido', fechaPedido);
        formData.append('fechaPago', fechadePago);
        formData.append('fechaEntrega', fechadeEntrega);
        formData.append('telefono', telefono);
        formData.append('monto', monto);
        formData.append('serie', serie);
        formData.append('numero', numero);

        if ($('#idExcel')[0].files != undefined) {
            let files = $('#idExcel')[0].files;
            formData.append('excel', files[0]);
        }
        if ($('#idAdjunto')[0].files != undefined) {
            let files = $('#idAdjunto')[0].files;
            formData.append('adjunto', files[0]);
        }

        $.ajax({
            type: 'POST',
            url: '/COMRegistrarCompra/Guardar',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    $('#txtNombreProveedor').val('');
                    $('#txtContactoProveedor').val('');                    
                    $('#txtTelefono').val('');
                    $('#txtSerieFactura').val('');
                    $('#txtNumero').val('');
                    $('#txtValorFactura').val('');
                    $('#idAdjunto').val('');
                    $('#infoAdjunto').val('');
                    $('#idExcel').val('');
                    $('#infoExcel').val('');
                    ShowAlertMessage('success', '¡Compra registrada correctamente!');
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }


    $('#btnGuardar').on('click', function (e) {
        e.preventDefault();        
        Guardar()
    });
});