let fechaI;
$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    fechaI = new AirDatepicker('#txtFecha', {
        autoClose: true,
        autoClose: true,
        view: 'days',
        minView: 'days',
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date()]

        /* //onSelect: GetDataTable*/
    });

    function Excel() {
        let fechaInicial = DateFormat(fechaI.lastSelectedDate);
        CallLoadingFire('Descargando productos...');
        $.post('/REPProductosSinStock/GenerarExcel', { fechaInicial }, function (result) {
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
            //CallToast('Descarga realizada con éxito.', true, 2300, '#9EC600')
            //HideLoader();
        });
    }

    $('#botonInforme').on('click', function (e) {
        e.preventDefault();

        //if (fechaI.lastSelectedDate != undefined) {
            Excel()
        //}
        //else {
        //    ShowAlertMessage('warning', 'DEBES SELECCIONAR UNA FECHA')
        //}
    })
});