$(document).ready(function () {
    GetDatos()

    GetLists('#selCategoria', 4)
    GetLists('#selModelo', 8)
    GetLists('#selTipo', 12)
    GetLists('#selBodega', 16)

    var cont = 0;
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
                    caption: 'IMAGEN',
                    //width: 90,
                    /*
                    cellTemplate(container, options) {
                        $('<div>').append($('<img>', { src: options.value })).appendTo(container);
                    },
                    */
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        $("<img>").attr('src', fieldData.PATH_IMAGEN).css('width', '70px').appendTo(container);
                        //$("<img>").attr('src','https://itpromklao.com/wp-content/uploads/2020/03/Slide1-135.jpg').css('width','70px').appendTo(container);
                    }
                },
                {
                    caption: 'ACCIONES',
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;

                        if (fieldData.ESTADO == 1)
                            $("<span>").addClass("badge badge-success").text('ACTIVO').appendTo(container);
                        else
                            $("<span>").addClass("badge badge-danger").text('INACTIVO').appendTo(container);

                        var classTmp1 = 'edit' + cont;
                        var classBTN1 = 'ml-2 hvr-grow far fa-edit btn btn-success ' + classTmp1;
                        if (fieldData.ESTADO == 1) {
                            $("<span>").addClass(classBTN1).prop('title', 'Editar').appendTo(container);
                            $('.edit' + cont).click(function (e) {
                                var id = parseInt(fieldData.ID_BODEGA);
                                GetOpcion(2)
                                GetInputsUpdate(id, fieldData.NOMBRE, fieldData.ESTANTERIA, fieldData.NIVEL)
                            })
                        }

                        var classTmp2 = 'remove' + cont;
                        var classBTN2 = 'ml-2 hvr-grow far fa-trash-alt btn btn-danger ' + classTmp2;
                        if (fieldData.ESTADO == 1) {

                            $("<span>").addClass(classBTN2).prop('title', 'Inactivar').appendTo(container);
                            $('.remove' + cont).click(function (e) {
                                var id = parseInt(fieldData.ID_BODEGA);
                                Delete(id, 4);
                            })
                        }
                        cont++;
                    }
                },
                //{
                //    caption: "ACCIONES",
                //    type: "buttons",
                //    alignment: "center",
                //    buttons: [
                //        {
                //            visible: function (e) {
                //                var visible = false;
                //                if (e.row.data.ESTADO == 1)
                //                    visible = true;
                //                return visible;
                //            },
                //            hint: "Editar",
                //            icon: "edit",
                //            onClick: function (e) {
                //                alert('en desarrollo')
                //            }
                //        },
                //        {
                //            visible: function (e) {
                //                var visible = false;
                //                if (e.row.data.ESTADO == 1)
                //                    visible = true;
                //                return visible;
                //            },
                //            hint: "Inactivar",
                //            icon: "clear",
                //            onClick: function (e) {
                //                Delete(e.row.data['ID_PRODUCTO'], 2)
                //            }
                //        },
                //        {
                //            visible: function (e) {
                //                var visible = false;
                //                if (e.row.data.ESTADO == 1)
                //                    visible = true;
                //                return visible;
                //            },
                //            hint: "Agregar",
                //            icon: "add",
                //            onClick: function (e) {
                //                Delete(e.row.data['ID_PRODUCTO'], 2)
                //            }
                //        }
                //    ]
                //},
                //{
                //    caption: "ESTADO",
                //    alignment: "center",
                //    cellTemplate: function (container, options) {
                //        var fieldData = options.data;
                //        container.addClass(fieldData.ESTADO != 1 ? "dec" : "");

                //        if (fieldData.ESTADO == 1)
                //            $("<span>").addClass("badge badge-success").text('ACTIVO').appendTo(container);
                //        else
                //            $("<span>").addClass("badge badge-danger").text('INACTIVO').appendTo(container);
                //    }
                //},
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
                    dataField: "NOMBRE_MODELO",
                    caption: "MODELO"
                },
                {
                    dataField: "STOCK",
                    caption: "STOCK",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        container.addClass(fieldData.ESTADO != 1 ? "dec" : "");

                        if (fieldData.STOCK > 0)
                            $("<span>").addClass("badge badge-success").text(fieldData.STOCK).appendTo(container);
                        else
                            $("<span>").addClass("badge badge-danger").text('SIN STOCK').appendTo(container);
                    }
                },
                {
                    dataField: "PRECIO_COSTO",
                    caption: "PRECIO COSTO",
                    alignment: "right",
                    format: ",##0.00"
                },
                {
                    dataField: "PRECIO_VENTA",
                    caption: "PRECIO VENTA",
                    alignment: "right",
                    alignment: "right",
                    format: ",##0.00"
                },
                {
                    dataField: "CREADO_POR",
                    caption: "CREADO_POR",
                    visible: false
                },
                {
                    dataField: "NOMBRE_DISTRIBUIDOR",
                    caption: "DISTRIBUIDOR"
                },
                {
                    dataField: "NOMBRE_MARCA_REPUESTO",
                    caption: "MARCA PRODUCTO"
                },
                {
                    dataField: "NOMBRE_MARCA_VEHICULO",
                    caption: "MARCA VEHICULO"
                },
                {
                    dataField: "NOMBRE_LINEA_VEHICULO",
                    caption: "LINEA VEHICULO"
                }
            ]
        }).dxDataGrid('instance');

    }
});