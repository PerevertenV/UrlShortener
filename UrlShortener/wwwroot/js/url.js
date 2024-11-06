var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/home/getall' },
        "columns": [
            {
                data: 'longUrl',
                "render": function (data) {
                    return `<a href="${data}" target="_blank">${data}</a>`;
                },
                width: "50%"
            },
            {
                data: 'shortUrl',
                "render": function (data, type, row) {
                    return `<a href="${row.longUrl}" target="_blank">${data}</a>`;
                },
                width: "30%"
            },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/home/addCheckShortUrl?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-info-lg"></i></a>
                        <a onClick=Delete('/home/delete/${data}') class="btn btn-primary mx-2 btn-danger"> <i class="bi bi-trash3"></i></a>
                    </div>`;
                },
                width: "20%"
            }
        ]
    });
}


function Delete(url) {
    Swal.fire({
        title: "Ви впевнені?",
        text: "Ви не зможете відновити видалине!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Ok"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'Delete',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}