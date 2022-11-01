$(document).ready(function () {
    GetDatos();

    function GetDatos() {
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/USUListarUsuario/GetUsuarios',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: {},
                    cache: false,
                    success: function (data) {
                        var state = data["State"];
                        if (state == 1) {
                            data = JSON && JSON.parse(JSON.stringify(data)) || $.parseJSON(data);
                            d.resolve(data);
                        }
                        else if (state == -1)
                            ShowAlertMessage(data["Message"])
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
            columns: [
                {
                    dataField: 'PATH_IMAGEN',
                    width: 90,
                    cellTemplate(container, options) {
                        $('<div>').append($('<img>', { src: options.value })).appendTo(container);
                    },
                },
                {
                    caption: "ESTADO",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        container.addClass(fieldData.ESTADO != 1 ? "dec" : "");

                        if (fieldData.ESTADO == 1)
                            $("<span>").addClass("badge badge-success").text('ACTIVO').appendTo(container);
                        else
                            $("<span>").addClass("badge badge-danger").text('INACTIVO').appendTo(container);

                    }
                },
                {
                    caption: "ACCIONES",
                    type: "buttons",
                    alignment: "center",
                    buttons: [
                        {
                            visible: function (e) {
                                var visible = false;
                                if (e.row.data.ESTADO == 1)
                                    visible = true;
                                return visible;
                            },
                            hint: "Editar",
                            icon: "edit",
                            onClick: function (e) {
                                GetOpcion(2)
                                GetInputsUpdate(e.row.data['ID_BODEGA'], e.row.data['NOMBRE'], e.row.data['ESTANTERIA'], e.row.data['NIVEL'])
                            }
                        },
                        {
                            visible: function (e) {
                                var visible = false;
                                if (e.row.data.ESTADO == 1)
                                    visible = true;
                                return visible;
                            },
                            hint: "Inactivar",
                            icon: "clear",
                            onClick: function (e) {
                                Delete(e.row.data['ID_USUARIO'], 4)
                            }
                        }
                    ]
                },
                {
                    dataField: "ID_USUARIO",
                    caption: "ID",
                    visible: false
                },
                {
                    dataField: "USUARIO",
                    caption: "USUARIO",
                    alignment: "center",
                    visible: false
                },
                {
                    dataField: "NOMBRE_ROL",
                    caption: "ROL"
                },
                {
                    dataField: "NOMBRE_TIPO_EMPLEADO",
                    caption: "TIPO EMPLEADO"
                },
                {
                    dataField: "PRIMER_NOMBRE",
                    caption: "PRIMER NOMBRE"
                },
                {
                    dataField: "SEGUNDO_NOMBRE",
                    caption: "SEGUNDO NOMBRE"
                },
                {
                    dataField: "PRIMER_APELLIDO",
                    caption: "PRIMER APELLIDO"
                },
                {
                    dataField: "SEGUNDO_APELLIDO",
                    caption: "SEGUNDO_APELLIDO"
                },
                {
                    dataField: "TELEFONO",
                    caption: "TELEFONO"
                },
                {
                    dataField: "DIRECION",
                    caption: "DIRECCION"
                },
                {
                    dataField: "URL_PANTALLA",
                    caption: "URL DEFAULT"
                },
                {
                    dataField: "EMAIL",
                    caption: "EMAIL"
                }
            ]
        }).dxDataGrid('instance');

    }

});