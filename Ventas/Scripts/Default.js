$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    function ventaReporte(fechaInicial, fechaFinal) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/REPBitacora/GetDefault',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { },
                    cache: false,
                    success: function (data) {

                        /* console.log(data);*/
                        var state = data["State"];
                        if (state == 1) {
                            $('#textVentasDia').html(data['ventasDia']);
                            $('#textUsuarios').html(data['usuarios']);

                            data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);

                            d.resolve(data);

                        }
                        else if (state == -1)
                            alert(data["Message"])
                    },
                    error: function (jqXHR, exception) {
                        getErrorMessage(jqXHR, exception);
                    }
                });
                return d.promise();
            }
        });


        $('#chart').dxChart({
            palette: 'Harmony Light',
            dataSource: new DevExpress.data.DataSource(customStore),
            commonSeriesSettings: {
                
                argumentField: 'FECHA_CREACION_STRING',
            },
            series: [{
                name:'VENTAS DEL DÍA',
                argumentField: 'FECHA_CREACION_STRING',
                valueField: 'CANTIDAD',
                label: {
                    visible: true,
                    font: {
                        size: 16,
                    },
                    connector: {
                        visible: true,
                        width: 0.5,
                    },
                    position: 'columns',
                    //customizeText(arg) {
                    //    return `${arg.valueText} (${arg.percentText})`;
                    //},
                },
            }],
            margin: {
                bottom: 20,
            },
            /*title: 'Population: Age Structure (2018)',*/
            argumentAxis: {
                valueMarginsEnabled: false,
            },
            export: {
                enabled: true,
            },
            legend: {
                verticalAlignment: 'bottom',
                horizontalAlignment: 'center',
            },
        }).dxChart('instance');

    }
    ventaReporte('2022-12-01', '2022-12-30')
});