// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showOrderDetails(id) {
    $.ajax({
        url: "/Admin/Order/DetailsOrder/" + id,
        type: "GET",
        //data: { id: id },
        success: function (response) {
            $('#orderDetailsModalBody').html(response);
            $('#orderDetailsModal').show();
        },
        error: function () {
        }
    });
}

//function showOrderDetails(id) {
//    $.ajax({
//        url: "/Admin/Order/DetailsOrder",
//        type: "GET",
//        data: { id: id },
//        success: function (response) {
//            var modalBody = $('#orderDetailsModal').find('.modal-content');
//            modalBody.html('');

//            if (response != null && response.length > 0) {

//                var table = $('<table>').addClass('table');
//                var thead = $('<thead>').appendTo(table);
//                var tbody = $('<tbody>').appendTo(table);

//                response.forEach(function (detail) {
//                    var row = $('<tr>').appendTo(tbody);
//                    $('<td>').text(detail.Property1).appendTo(row);
//                    $('<td>').text(detail.Property2).appendTo(row);
//                });

//                modalBody.append(table);
//            } else {

//                modalBody.text('Sipariş detayı bulunamadı.');
//            }

//            $('#orderDetailsModal').modal('show');
//        },
//        error: function () {

//        }
//    });
//}