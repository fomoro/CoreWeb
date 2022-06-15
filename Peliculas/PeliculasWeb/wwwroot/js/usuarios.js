var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblUsuarios').DataTable({

        "ajax": {
            "url": "/Usuarios/GetTodosUsuarios",
            "type": "GET",
            "datatype": "json"
        },
        "columns":[
            { "data": "id", "width": "20%" },
            { "data": "usuarioA", "width": "40%" },
            { "data": "passwordHash", "width": "20%" }            
        ]
    });
}