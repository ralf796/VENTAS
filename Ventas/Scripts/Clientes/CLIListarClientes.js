$(document).ready(function () {
    DevExpress.localization.locale(navigator.language);
    GetDatos();
    function GetDatos() {
        var cont = 0;
        var tipo = 4;
        var customStore = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                var d = $.Deferred();
                $.ajax({
                    type: 'GET',
                    url: '/CLIListarClientes/GetClientes',
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
                    caption: "ACCIONES",
                    alignment: "center",
                    cellTemplate: function (container, options) {
                        var fieldData = options.data;
                        //ESTADO
                        if (fieldData.ESTADO == 1)
                            $("<span>").addClass("badge badge-success").text('ACTIVO').appendTo(container);
                        else
                            $("<span>").addClass("badge badge-danger").text('INACTIVO').appendTo(container);
                        //BTN EDITAR
                        if (fieldData.ESTADO == 1) {
                            var classTmp1 = 'edit' + cont;
                            var classBTN1 = 'ml-2 hvr-grow far fa-edit btn btn-success ' + classTmp1;
                            $("<span>").addClass(classBTN1).prop('title', 'Editar').appendTo(container);
                            $('.edit' + cont).click(function (e) {
                                var id = fieldData.ID_CLIENTE;
                                var nombre = $(this).closest("tr").find("td").eq(2).text();
                                var direccion = $(this).closest("tr").find("td").eq(3).text();
                                var telefono = $(this).closest("tr").find("td").eq(4).text();
                                var email = $(this).closest("tr").find("td").eq(5).text();
                                var nit = $(this).closest("tr").find("td").eq(6).text();
                                var id_categoria_cliente = $(this).closest("tr").find("td").eq(7).text();
                                
                                $('#hfID').val(id);
                                $('#hfOpcion').val(2);
                                $('#txtGuardarNombre').val(nombre);
                                $('#txtGuardarDireccion').val(direccion);
                                $('#txtGuardarTelefono').val(telefono);
                                $('#txtGuardarEmail').val(email);
                                $('#txtGuardarNit').val(nit);
                                $('#selCategoriaCliente').val(id_categoria_cliente);
                                $('#modalCliente').modal('show');

                            })
                            //BTN ELIMINAR
                            var classTmp2 = 'remove' + cont;
                            var classBTN2 = 'ml-2 hvr-grow far fa-trash-alt btn btn-danger ' + classTmp2;
                            $("<span>").addClass(classBTN2).prop('title', 'Inactivar').appendTo(container);
                            $('.remove' + cont).click(function (e) {
                                var id = parseInt(fieldData.ID_CLIENTE);
                                Delete(id);
                            })
                        }
                        cont++;
                    }
                },
                {
                    dataField: "NOMBRE_CATEGORIA",
                    caption: "CATEGORIA"
                },
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
                    dataField: "DIRECCION",
                    caption: "DIRECCION"
                },
                {
                    dataField: "TELEFONO",
                    caption: "TELEFONO",
                    alignment: "center"
                },
                {
                    dataField: "EMAIL",
                    caption: "EMAIL"
                },
                {
                    dataField: "NIT",
                    caption: "NIT",
                    alignment: "center"
                },
                {
                    dataField: "ID_CATEGORIA_CLIENTE",
                    caption: "ID_CATEGORIA"
                }
            ]
        }).dxDataGrid('instance');

    }

    // funcion guardar datos
    function guardarCliente(nombre, direccion, telefono, email, nit, id_categoria) {
        $.ajax({
            type: 'GET',
            url: "/CLIListarClientes/GuardarCliente",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                nombre, direccion, telefono, email, nit, id_categoria
            },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Datos ingresados correctamente')
                    $('#txtGuardarNombre').val('');
                    $('#txtGuardarDireccion').val('');
                    $('#txtGuardarTelefono').val('');
                    $('#txtGuardarEmail').val('');
                    $('#txtGuardarNit').val('');
                    $('#modalCliente').modal('hide');
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    //funcion actualizar cliente
    function Update_Delete(id, nombre, direccion, telefono, email, nit, id_categoria) {
        $.ajax({
            type: 'GET',
            url: "/CLIListarClientes/UpdateClientes",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { id, nombre, direccion, telefono, email, nit, id_categoria },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'Cliente actualizado correctamente');
                    $('#txtGuardarNombre').val('');
                    $('#txtGuardarDireccion').val('');
                    $('#txtGuardarTelefono').val('');
                    $('#txtGuardarEmail').val('');
                    $('#txtGuardarNit').val('');
                    $('#modalCliente').modal('hide');
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    //funcion delete
    function Delete(id) {
        $.ajax({
            type: 'GET',
            url: "/CLIListarClientes/Delete",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { id },
            cache: false,
            success: function (data) {
                var state = data["State"];
                if (state == 1) {
                    ShowAlertMessage('success', 'El Cliente seleccionado se inactivó correctamente.')
                    GetDatos()
                }
                else if (state == -1) {
                    ShowAlertMessage('warning', data['Message'])
                }
            }
        });
    }
    // funcion para abrir modal
    function GetOpcion(opcion) {
        $('#hfOpcion').val(opcion);
        if (opcion == 1) {
            $('#txtGuardarNombre').val('');
            $('#txtGuardarDireccion').val('');
            $('#txtGuardarTelefono').val('');
            $('#txtGuardarEmail').val('');
            $('#txtGuartxtGuardarNitdarDireccion').val('');
            $('#titleModal').html('CREAR CLIENTE')
        }
        else if (opcion == 2)
            $('#titleModal').html('MODIFICAR CLIENTE')

        $('#modalCliente').modal('show');
    }
    // boton para recibir datos desde el modal ------------------
    $('#guardarClienteModal').on('click', function (e) {
        e.preventDefault();
        var opcion = $('#hfOpcion').val();
        var id = $('#hfID').val();
        var nombre = $('#txtGuardarNombre').val();
        var direccion = $('#txtGuardarDireccion').val();
        var telefono = $('#txtGuardarTelefono').val();
        var email = $('#txtGuardarEmail').val();
        var nit = $('#txtGuardarNit').val();
        var id_categoria = $('#selCategoriaCliente').val();
        if (nombre == "" || telefono == "" || nit == "") {
            ShowAlertMessage('warning', 'Los campos nombre, telefono y nit son obligatorios')
        }
        else {
            if (email.trim() != '' && email.trim()!=null) {
                const regex = /^\w+([.-_+]?\w+)*@\w+([.-]?\w+)*(\.\w{2,10})+$/
                if (regex.test(email) && email!='' && email!=null) {
                    if (opcion == 1) {
                        guardarCliente(nombre, direccion, telefono, email, nit, id_categoria);
                    }
                    else if (opcion == 2) {
                        Update_Delete(id, nombre, direccion, telefono, email, nit, id_categoria);
                    }
                }
                else {
                    ShowAlertMessage('warning', 'EMAIL con formato incorrecto');
                }
            }
            else {
                if (opcion == 1) {
                    guardarCliente(nombre, direccion, telefono, email, nit, id_categoria);
                }
                else if (opcion == 2) {
                    Update_Delete(id, nombre, direccion, telefono, email, nit, id_categoria);
                }
            }

        }


    });

    //boton para abrir modal
    $('#btnAbrirModal').on('click', function (e) {
        e.preventDefault();
        GetOpcion(1);
    });
});