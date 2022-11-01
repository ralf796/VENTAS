$(document).ready(function () {
    GetDatos()

    GetLists('#selCategoria', 4)
    GetLists('#selModelo', 8)
    GetLists('#selTipo', 12)
    GetLists('#selBodega', 16)


    function Update_Delete_Producto(id, tipo, ID_CATEGORIA, ID_MODELO, ID_TIPO, ID_BODEGA, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, ANIO_FABRICADO, CODIGO) {
        var mensaje = '';
        if (tipo != 20)
            mensaje = 'Datos actualizados correctamente';
        else
            mensaje = 'Se inactivó el item seleccionado';

        $.ajax({
            type: 'GET',
            url: "/INVMantenimiento/Update_Delete_Producto",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                id, tipo, ID_CATEGORIA, ID_MODELO, ID_TIPO, ID_BODEGA, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, ANIO_FABRICADO, CODIGO
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', mensaje)
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
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
                            if (tipo == 4) {
                                $(selObject).append('<option value="' + dato.ID_CATEGORIA + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 8) {
                                $(selObject).append('<option value="' + dato.ID_MODELO + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 12) {
                                $(selObject).append('<option value="' + dato.ID_TIPO + '">' + dato.NOMBRE + '</option>');
                            }
                            else if (tipo == 16) {
                                $(selObject).append('<option value="' + dato.ID_BODEGA + '">' + dato.NOMBRE + '</option>');
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

    $('#formGuardarProducto').submit(function (e) {
        e.preventDefault();

        var ID_PRODUCTO = $('#hfIdProducto').val();
        var ID_CATEGORIA = $('#selCategoria').val();
        var ID_CATEGORIA = $('#selCategoria').val();
        var ID_MODELO = $('#selModelo').val();
        var ID_TIPO = $('#selTipo').val();
        var ID_BODEGA = $('#selBodega').val();
        var NOMBRE = $('#txtNombre').val();
        var DESCRIPCION = $('#txtDescripcion').val();
        var PRECIO_COSTO = $('#txtPrecioCosto').val();
        var PRECIO_VENTA = $('#txtPrecioVenta').val();
        var STOCK = $('#txtStock').val();
        var ANIO_FABRICADO = $('#txtAnioFabricacion').val();
        var CODIGO = $('#txtCodigo').val();
        Update_Delete_Producto(ID_PRODUCTO, 19, ID_CATEGORIA, ID_MODELO, ID_TIPO, ID_BODEGA, NOMBRE, DESCRIPCION, PRECIO_COSTO, PRECIO_VENTA, STOCK, ANIO_FABRICADO, CODIGO);
    });

    function GetDatos() {
        var tipo = 20;
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
                    dataField: 'PATH_IMAGEN',
                    width: 90,
                    cellTemplate(container, options) {
                        $('<div>').append($('<img>', { src: options.value })).appendTo(container);
                    },
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
                                alert('en desarrollo')
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
                                Delete(e.row.data['ID_PRODUCTO'], 2)
                            }
                        },
                        {
                            visible: function (e) {
                                var visible = false;
                                if (e.row.data.ESTADO == 1)
                                    visible = true;
                                return visible;
                            },
                            hint: "Agregar",
                            icon: "add",
                            onClick: function (e) {
                                Delete(e.row.data['ID_PRODUCTO'], 2)
                            }
                        }
                    ]
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
                    dataField: "ID_PRODUCTO",
                    caption: "ID PRODUCTO",
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
                    dataField: "CODIGO",
                    caption: "CODIGO 1",
                    alignment: "center"
                },
                {
                    dataField: "CODIGO2",
                    caption: "CODIGO 2",
                    alignment: "center"
                },
                {
                    dataField: "ID_MODELO",
                    caption: "ID_MODELO",
                    visible: false
                },
                {
                    dataField: "NOMBRE_MODELO",
                    caption: "NOMBRE_MODELO"
                },
                {
                    dataField: "ID_PROVEEDOR",
                    caption: "ID_PROVEEDOR",
                    visible: false
                },
                {
                    dataField: "NOMBRE_PROVEEDOR",
                    caption: "PROVEEDOR"
                },
                {
                    dataField: "ID_MARCA_REPUESTO",
                    caption: "ID_MARCA_REPUESTO",
                    visible: false
                },
                {
                    dataField: "NOMBRE_MARCA_REPUESTO",
                    caption: "MARCA PRODUCTO"
                },
                {
                    dataField: "STOCK",
                    caption: "STOCK",
                    alignment: "center"
                },
                {
                    dataField: "PRECIO_COSTO",
                    caption: "PRECIO COSTO",
                    alignment: "center"
                },
                {
                    dataField: "PRECIO_VENTA",
                    caption: "PRECIO VENTA",
                    alignment: "center"
                },
                {
                    dataField: "CREADO_POR",
                    caption: "CREADO_POR",
                    visible: false
                },
                {
                    dataField: "ID_BODEGA",
                    caption: "ID_BODEGA",
                    visible: false
                },
                {
                    dataField: "NOMBRE_BODEGA",
                    caption: "BODEGA"
                },
                /*
                {
                    dataField: "ID_CATEGORIA",
                    caption: "ID_CATEGORIA",
                    visible: false
                },
                {
                    dataField: "NOMBRE_CATEGORIA",
                    caption: "CATEGORIA",
                    visible:false
                },
                {
                    dataField: "ID_SUBCATEGORIA",
                    caption: "ID_SUBCATEGORIA",
                    visible: false
                },
                {
                    dataField: "NOMBRE_SUBCATEGORIA",
                    caption: "SUBCATEGORIA",
                    visible:false
                },                
                {
                    dataField: "ID_MARCA_VEHICULO",
                    caption: "ID_MARCA_VEHICULO",
                    visible: false
                },
                {
                    dataField: "NOMBRE_MARCA_VEHICULO",
                    caption: "MARCA VEHICULO"
                },
                {
                    dataField: "ID_SERIE_VEHICULO",
                    caption: "ID_SERIE_VEHICULO",
                    visible: false
                },
                {
                    dataField: "NOMBRE_SERIE_VEHICULO",
                    caption: "SERIE VEHICULO"
                }
                */
            ]
        }).dxDataGrid('instance');

    }
});