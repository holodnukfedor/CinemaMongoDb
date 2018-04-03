function showErrorDialog(errorMsg) {
    $('#errorDialogMessage').html(errorMsg);
    $('#errorDialog').dialog("open");
}

function initializeMoviesGrid() {
    $("#moviesTable").jqGrid({
        url: 'api/movie/GetMovies',
        datatype: "json",
        autowidth: true,
        height: 'auto',
        ajaxSelectOptions: { type: "GET" },
        colNames: ['Id', 'Имя', "Описание", "Дата релиза", "Рейтинг"],
        colModel: [
            {
                name: 'Id', index: 'Id', width: 30, sortable: true, editable: false,
            },
            {
                name: 'Name', index: 'Name', width: 40, sortable: true, editable: false
            },
             {
                 name: 'Description', index: 'Description', width: 80, sortable: true, editable: true, edittype: "textarea",
                 wrap: "on", formatter: 'iconFmatter'
             },
           {
               name: 'ReleaseDate', index: 'ReleaseDate', width: 40, sortable: true, editable: false
           },
            {
                name: 'Rating', index: 'Rating', width: 20, sortable: true, editable: false
            }
        ],
        prmNames: { sort: "orderProperty", order: "order" },

        jsonReader: {
            repeatitems: false,
            root: "Movies",
            page: "PaginationInfo.PageNumber",
            total: "PaginationInfo.PagesCount",
            records: "PaginationInfo.RowsCount"
        },
        rowNum: 10,
        rowList: [2, 3, 5, 10, 25, 50, 100],
        pager: '#moviesTablePager',
        viewrecords: true,
        sortorder: "asc",
        sortname: 'Name',
        caption: "Фильмы",
        sortable: true,
        loadError: function (xhr, st, err) {
            if (xhr.status == 500) {
                showErrorDialog('Загрузка списка фильмов. Внутренняя ошибка сервера. ');
                $('#moviesTable').trigger('reloadGrid');
            }
            if (xhr.status == 0) {
                showErrorDialog('Загрузка списка фильмов. Сервер не отвечает. ');
            }
        },
    });


    $("#moviesTable").jqGrid('navGrid', '#moviesTablePager', {
        search: false,
        refresh: true,
        add: false,
        del: false,
        edit: false,
        view: true,
        viewtext: "Подробно",
        viewicon: 'ui-icon-zoomin',
        refreshtext: 'Обновить',
    },
        null,
        null,
        null,
        null,
         { width: 600, recreateForm: true }
    );
}

$(function () {
    $.ajaxSetup({ cache: false });

    $("#errorDialog").dialog({ autoOpen: false });

    initializeMoviesGrid();
});