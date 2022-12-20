var gridProductos = null;
$(document).ready(function () {
    var cont = 0;

    function GetListClientes() {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/CARSaldos/GetClientes',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: {},
                    cache: false,
                    success: function (data) {
                        var state = data["State"];
                        if (state == 1) {
                            $('#modalClientes').modal('show');
                            data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);
                            d.resolve(data);
                        }
                        else if (state == -1)
                            ShowAlertMessage('warning', data["Message"])
                    },
                    error: function (jqXHR, exception) {
                        getErrorMessage(jqXHR, exception);
                    }
                });
                return d.promise();
            }
        });
        var salesPivotGrid = $("#gridClientes").dxDataGrid({
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
                width: 240,
                placeholder: "Buscar..."
            },
            headerFilter: {
                visible: true
            },
            columnAutoWidth: true,
            export: {
                enabled: false
            },
            paging: {
                pageSize: 10,
            },
            pager: {
                showPageSizeSelector: true,
                allowedPageSizes: [10, 25, 50, 100],
            },
            remoteOperations: false,
            searchPanel: {
                visible: true,
                highlightCaseSensitive: true,
            },
            allowColumnReordering: true,
            rowAlternationEnabled: true,
            columns: [
                {
                    dataField: "ID_CLIENTE",
                    caption: "ID",
                    alignment: "center",
                    visible: false
                },
                {
                    dataField: "NOMBRE",
                    caption: "NOMBRE"
                },
                {
                    dataField: "NIT",
                    caption: "NIT",
                    alignment: "center"
                },
                {
                    dataField: "DIRECCION",
                    caption: "DIRECCION",
                    alignment: "center"
                },
                {
                    dataField: "TELEFONO",
                    caption: "TELEFONO",
                    alignment: "center"
                }
            ],
            onRowDblClick: function (e) {
                $('#hfIdCliente').val(e.data["ID_CLIENTE"]);
                $('#txtNombreCliente').val(e.data["NOMBRE"]);
                $('#txtNit').val(e.data["NIT"]);
                $('#txtDireccionCliente').val(e.data["DIRECCION"]);
                $('#modalClientes').modal('hide');

                GetEstadoCuentaCliente(e.data["ID_CLIENTE"])
            },
            onCellPrepared: function (e) {
                if (e.rowType === 'header') {
                    e.cellElement.css("background", "var(--secondary)");
                    e.cellElement.css("color", "#FFFFFF");
                }
            }
        }).dxDataGrid('instance');
    }

    function GetEstadoCuentaCliente(ID_CLIENTE) {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/CARSaldos/GetEstadoCuentaCliente',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { ID_CLIENTE },
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
        var gridestadoCuenta = $("#gridestadoCuenta").dxDataGrid({
            dataSource: new DevExpress.data.DataSource(customStore),
            showBorders: true,
            loadPanel: {
                text: "Cargando..."
            },
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: "Buscar..."
            },
            scrolling: {
                useNative: false,
                scrollByContent: true,
                scrollByThumb: true,
                showScrollbar: "always" // or "onScroll" | "always" | "never"
            },
            columnAutoWidth: true,
            onRowPrepared(e) {
                //e.rowElement.css("background-color", "#A7BCD6");
                //e.rowElement.css("color", "#000000");
            },
            columns: [
                {
                    caption: '',
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        var classTmp1 = 'print' + cont;
                        var classBTN1 = 'ml-2 hvr-grow fal fa-print btn btn-success ' + classTmp1;
                        $("<span>").addClass(classBTN1).prop('title', 'Imprimir').appendTo(container);
                        $('.print' + cont).click(function (e) {
                            var venta = parseInt(fieldData.ID_VENTA);
                            GenerarImpresionCredito(venta)
                        })
                        cont++;
                    }
                },
                {
                    dataField: "ID_ESTADO_CUENTA",
                    caption: "NO. CRÉDITO",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        if (fieldData.DIAS_ATRASO > 0)
                            $("<span>").addClass("badge badge-danger").text(fieldData.ID_ESTADO_CUENTA).css('font-size', 15).appendTo(container);
                        else
                            $("<span>").text(fieldData.ID_ESTADO_CUENTA).appendTo(container);
                    },
                    alignment: 'center'
                },
                {
                    dataField: "FECHA_PAGO_STRING",
                    caption: "FECHA DE PAGO",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        if (fieldData.DIAS_ATRASO > 0)
                            $("<span>").addClass("badge badge-danger").text(fieldData.FECHA_PAGO_STRING).css('font-size', 15).appendTo(container);
                        else
                            $("<span>").text(fieldData.FECHA_PAGO_STRING).appendTo(container);
                    },
                    alignment: 'center'
                },
                {
                    dataField: "SALDO",
                    caption: "SALDO PENDIENTE",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        if (fieldData.SALDO == 0)
                            $("<span>").addClass("badge badge-success").text('0.00').css('font-size', 15).appendTo(container);
                        else
                            $("<span>").addClass("badge badge-danger").text(fieldData.SALDO).css('font-size', 15).appendTo(container);
                    },
                    alignment: 'center'
                },
                {
                    dataField: "ABONO",
                    caption: "ABONADO",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        if (fieldData.ABONO == 0)
                            $("<span>").addClass("badge badge-danger").text('0.00').css('font-size', 15).appendTo(container);
                        else
                            $("<span>").addClass("badge badge-success").text(fieldData.ABONO).css('font-size', 15).appendTo(container);
                    },
                    alignment: 'center'
                },
                {
                    dataField: "DIAS_ATRASO",
                    caption: "DÍAS DE ATRASO",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        if (fieldData.DIAS_ATRASO > 0)
                            $("<span>").addClass("badge badge-danger").text(fieldData.DIAS_ATRASO).css('font-size', 15).appendTo(container);
                        else
                            $("<span>").addClass("badge badge-success").text(fieldData.DIAS_ATRASO).css('font-size', 15).appendTo(container);
                    },
                    alignment: 'center'
                },
                {
                    dataField: "ID_VENTA",
                    caption: "ÓRDEN DE COMPRA"
                },
                {
                    dataField: "FECHA_CREACION_CREDITO_STRING",
                    caption: "FECHA CREACIÓN CRÉDITO"
                },
                {
                    dataField: "TOTAL_VENTA",
                    caption: "TOTAL VENTA"
                },
                {
                    dataField: "CREDITO_CREADO_POR",
                    caption: "CAJERO"
                },
                
                {
                    dataField: "ESTADO",
                    caption: "ESTADO DE VENTA"
                },
                {
                    dataField: "FECHA_VENTA_STRING",
                    caption: "FECHA DE VENTA"
                },
                {
                    dataField: "VENTA_CREADO_POR",
                    caption: "VENDEDOR"
                }
            ]
        }).dxDataGrid('instance');

    }

    function GenerarImpresionCredito(id_venta) {
        CallLoadingFire('Generando estado de cuenta, por favor espera...');
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

    $('#btnBuscarCliente').on('click', function (e) {
        e.preventDefault();
        GetListClientes()
    });
});