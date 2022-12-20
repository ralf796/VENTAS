let fecha;
$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    var cont = 0;

    function DateFormat(date) {
        try {
            let d = date;
            let month = (d.getMonth() + 1).toString().padStart(2, '0');
            let day = d.getDate().toString().padStart(2, '0');
            let year = d.getFullYear();
            return [year, month, day].join('-');
        } catch (err) {
            return '';
        }
    }
    var f = new Date();
    fecha = new AirDatepicker('#txtFecha', {
        autoClose: true,
        autoClose: true,
        view: 'days',
        minView: 'days',
        minDate: f.setDate(f.getDate() -7),
        maxDate: new Date(),
        dateFormat: 'dd/MM/yyyy',
        selectedDates: [new Date()]
    });

    function GetDatos(fecha) {
        var tipo = 1;
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/ANUCrearAnulacionVenta/GetDatosSP',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { fecha, tipo },
                    cache: false,
                    success: function (data) {

                        var state = data["State"];
                        if (state == 1) {
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
        var salesPivotGrid = $("#gridContainer").dxDataGrid({
            dataSource: new DevExpress.data.DataSource(customStore),
            showBorders: true,
            loadPanel: {
                text: "Cargando..."
            },
            scrolling: {
                useNative: false,
                scrollByContent: true,
                scrollByThumb: true,
                showScrollbar: "always" // or "onScroll" | "always" | "never"
            },
            searchPanel: {
                visible: true,
                width: 200,
                placeholder: "Buscar..."
            },
            headerFilter: {
                visible: true
            },
            columnAutoWidth: true,
            export: {
                enabled: false
            },
            columns: [
                {
                    dataField: "ID_VENTA",
                    caption: "ORDEN DE COMPRA",
                    alignment: "center"
                },
                {
                    dataField: "FEL",
                    caption: "FEL",
                    alignment: "center"
                },
                {
                    dataField: "FECHA_CREACION_STRING",
                    caption: "FECHA"
                },
                {
                    dataField: "NOMBRE",
                    caption: "CLIENTE"
                },
                {
                    dataField: "CREADO_POR",
                    caption: "VENDEDOR",
                    alignment: "center"
                },
                {
                    dataField: "TOTAL",
                    caption: "TOTAL SIN DESCUENTO",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "SUBTOTAL",
                    caption: "TOTAL CON DESCUENTO",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },

                {
                    caption: "DETALLE VENTA",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        var classTmp = 'detalle' + cont;
                        var classBTN = 'hvr-grow text-dark far fa-eye btn btn-info ' + classTmp;

                        $("<span>").addClass(classBTN).prop('title', 'Ver detalle').appendTo(container);
                        $('.detalle' + cont).click(function (e) {

                            mostrarDetalle(fieldData.ID_VENTA);
                        })
                        cont++;
                    }
                },
                {
                    caption: "ANULAR VENTA",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        var classTmp = 'anular' + cont;
                        var classBTN = 'hvr-grow text-dark far fa-trash-alt btn btn-danger ' + classTmp;

                        $("<span>").addClass(classBTN).prop('title', 'cobrar').appendTo(container);
                        $('.anular' + cont).click(function (e) {
                            var id = parseInt(fieldData.ID_VENTA);
                            var fel = parseInt(fieldData.FEL);
                            Swal.fire({
                                title: 'ANULACION DE VENTA',
                                text: '¿Quieres anular la venta ' + id + '?',
                                icon: 'warning',
                                showCancelButton: true,
                                confirmButtonColor: '#3085d6',
                                cancelButtonColor: '#d33',
                                confirmButtonText: 'Si, anular',
                                cancelButtonText: 'No'
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    Anular(id, 2, fel);
                                }
                            })
                        })
                        cont++;
                    }
                }
            ]

        }).dxDataGrid('instance');
    }

    function mostrarDetalle(id) {
        var id_venta = parseInt(id);
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/CAJCobro/GetDetalle',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { id_venta },
                    cache: false,
                    success: function (data) {
                        var state = data["State"];
                        if (state == 1) {
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
        var salesPivotGrid = $("#gridContainer2").dxDataGrid({
            dataSource: new DevExpress.data.DataSource(customStore),
            showBorders: true,
            loadPanel: {
                text: "Cargando..."
            },
            scrolling: {
                useNative: false,
                scrollByContent: true,
                scrollByThumb: true,
                showScrollbar: "always" // or "onScroll" | "always" | "never"
            },
            searchPanel: {
                visible: true,
                width: 200,
                placeholder: "Buscar..."
            },
            headerFilter: {
                visible: true
            },
            columnAutoWidth: true,
            export: {
                enabled: false
            },
            columns: [
                {
                    dataField: "NOMBRE",
                    caption: "PRODUCTO"
                },
                {
                    dataField: "CANTIDAD",
                    caption: "CANTIDAD",
                    alignment: "center"
                },
                {
                    dataField: "PRECIO_UNITARIO",
                    caption: "P/U",
                    alignment: "center"
                },
                {
                    dataField: "DESCUENTO",
                    caption: "D/U",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "TOTAL",
                    caption: "TOTAL SIN DESCUENTO",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                },
                {
                    dataField: "SUBTOTAL",
                    caption: "TOTAL CON DESCUENTO",
                    alignment: "center",
                    dataType: "number",
                    format: { type: 'fixedPoint', precision: 2 }
                }
            ],
        }).dxDataGrid('instance');
        $('#modalDetalle').modal('show');
    }

    $('#btnBuscar').on('click', function (e) {
        e.preventDefault();
        GetDatos(DateFormat(fecha.lastSelectedDate));
    })

    function Anular(idVenta, tipo, fel) {
        CallLoadingFire('Anulando venta, por favor espere...');
        $.ajax({
            type: 'GET',
            url: "/ANUCrearAnulacionVenta/GetDatosSP",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { idVenta, tipo, fel},
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    GetDatos(DateFormat(fecha.lastSelectedDate));
                    ShowAlertMessage('success', 'Se anuló la venta seleccionada')
                }
                else if (state == -1) 
                    ShowAlertMessage('warning', data['Message'])
                else if (state == 2) 
                    ShowAlertMessage('warning', 'No se pudo anular la venta en BD, consulta con el Administrador')
                else if (state == 3) 
                    ShowAlertMessage('warning', 'No se pudo anular la venta en FEL, consulta con el Administrador, Mensaje: ' + data['Message'])
                
            }
        });
    }
});

