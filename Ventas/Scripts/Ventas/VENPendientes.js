function addCode(lista) {
    $("#divMaster").empty();

    lista.forEach(function (row) {
        document.getElementById("divMaster").innerHTML +=
            '<div class="col-md-4">' +
            '<div class="card card-widget widget-user-2">' +
            '<div class="widget-user-header bg-dark">' +
            '<div class="widget-user-image">' +
            '<img class="img-circle elevation-2" src="https://pondokindahmall.co.id/assets/img/default.png" alt="User Avatar">' +
            '</div>' +
            '<h3 class="widget-user-username">'+row.CREADO_POR+'</h3>' +
            '<h5 class="widget-user-desc">'+row.ID_VENTA+'</h5>' +
            '</div>' +
            '<div class="card-footer p-0">' +
            '<ul class="nav flex-column">' +
            '<li class="nav-item">' +
            '<a href="#" class="nav-link"> Projects <span class="float-right badge bg-primary">31</span> </a>' +
            '</li>' +
            '<li class="nav-item">' +
            '<a href="#" class="nav-link">Tasks <span class="float-right badge bg-info">5</span> </a>' +
            '</li>' +
            '<li class="nav-item">' +
            '<a href="#" class="nav-link"> Completed Projects <span class="float-right badge bg-success">12</span> </a>' +
            '</li>' +
            '<li class="nav-item">' +
            '<a href="#" class="nav-link"> Followers <span class="float-right badge bg-danger">842</span> </a>' +
            '</li>' +
            '</ul>' +
            '</div>' +
            '</div>' +
            '</div>';
    });
}

$(document).ready(function () {
    function GetPendientes() {
        var tipo = 8;
        $.ajax({
            type: 'GET',
            url: '/VENCrearVenta/GetList',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { tipo },
            cache: false,
            success: function (data) {
                var state = data["State"];
                var lista = data["data"];
                if (state == 1) {
                    addCode(lista)
                }
                else if (state == -1)
                    alert(data["Message"])
            },
            error: function (jqXHR, exception) {
                getErrorMessage(jqXHR, exception);
            }
        });
    }


    setInterval(GetPendientes, 5000);
});