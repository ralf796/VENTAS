$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    GetDatos()

    function GetLists(selObject, tipo) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: 'GET',
                url: '/INVMantenimiento/GetDatosTable',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: { tipo },
                cache: false,
                success: function (data) {
                    var list = data["data"];
                    var state = data["State"];
                    if (state == 1) {
                        $(selObject).empty();
                        $(selObject).append('<option selected value="-1" disabled>Seleccione una opción</option>');
                        list.forEach(function (dato) {                            
                                $(selObject).append('<option value="' + dato.ID_CATEGORIA + '">' + dato.NOMBRE + '</option>');
                        });
                        resolve(1);
                    }
                    else if (state == -1)
                        alert(data["Message"])
                }
            });
        });
    }
    function GetDatos() {
        var tipo = 3;
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/INVMantenimiento/GetDatosTable',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { tipo },
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
                                GetInputsUpdate(e.row.data['ID_PROVEEDOR'], e.row.data['NOMBRE'], e.row.data['DESCRIPCION'], e.row.data['ID_CATEGORIA'])
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
                                Delete(e.row.data['ID_SUBCATEGORIA'], 2)
                            }
                        }
                    ]
                },
                {
                    dataField: "ID_SUBCATEGORIA",
                    caption: "ID",
                    alignment: "center",
                    visible: false
                },
                {
                    dataField: "NOMBRE",
                    caption: "NOMBRE"
                },
                {
                    dataField: "DESCRIPCION",
                    caption: "DESCRIPCION"
                },
                {
                    dataField: "ID_CATEGORIA",
                    caption: "ID_CATEGORIA",
                    visible:false
                },
                {
                    dataField: "NOMBRE_CATEGORIA",
                    caption: "CATEGORIA"
                }                
            ]
        }).dxDataGrid('instance');
    }
    function Procesar(nombre, descripcion, categoria, tipo, id, opcion) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Guardar",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                nombre, descripcion, tipo, categoria, id
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    if (opcion == 1)
                        ShowAlertMessage('success', 'Datos creados correctamente')
                    else
                        ShowAlertMessage('success', 'Datos actualizados correctamente')
                    $('#txtNombre').val('');
                    $('#txtDescripcion').val('');
                    $('#selCategoria').val(-1);
                    $('#modalDatos').modal('hide');
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function Delete(id, tipo) {
        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Delete",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { id, tipo },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'La bodega seleccionada se inactivó correctamente.')
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    function GetOpcion(opcion) {
        $('#hfOpcion').val(opcion);
        if (opcion == 1) {
            $('#txtNombre').val('');
            $('#txtDescripcion').val('');
            $('#titleModal').html('CREAR SUBCATEGORIA')
        }
        else if (opcion == 2)
            $('#titleModal').html('MODIFICAR SUBCATEGORIA')

        $('#modalDatos').modal('show');
    }
    function GetInputsUpdate(id, nombre, descripcion, categoria) {
        $('#hfID').val(id);
        $('#txtNombre').val(nombre);
        $('#txtDescripcion').val(descripcion);
        $('#selCategoria').val(categoria);
    }

    $('#btnAbrirModal').on('click', function (e) {
        e.preventDefault();
        GetLists('#selCategoria',2)
        GetOpcion(1);
    });
    $('#btnProcesar').on('click', function (e) {
        e.preventDefault();
        var opcion = $('#hfOpcion').val();
        var id = $('#hfID').val();
        var nombre = $('#txtNombre').val();
        var descripcion = $('#txtDescripcion').val();
        var categoria = $('#selCategoria').val();

        if (opcion == 1) {
            Procesar(nombre, descripcion, categoria, 11, 0, opcion);
        }
        else if (opcion == 2) {
            Procesar(nombre, descripcion, categoria, 12, id, opcion);
        }
    });

});