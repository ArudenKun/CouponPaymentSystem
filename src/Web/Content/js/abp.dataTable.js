var abp = abp || {};
(function () {
    if (!$.fn.dataTable) {
        return;
    }

    abp.libs = abp.libs || {};
    var l = abp.localization.getSource("Sentinel");

    var language = {
        emptyTable: "No data available in table",
        info: "_START_-_END_ of _TOTAL_ items",
        infoEmpty: "No records",
        infoFiltered: "(filtered from _MAX_ total entries)",
        infoPostFix: "",
        infoThousands: ",",
        lengthMenu: "Show _MENU_ entries",
        loadingRecords: "Loading...",
        processing: '<i class="bi bi-arrow-repeat spin"></i>',
        search: "Search:",
        zeroRecords: "No matching records found",
        paginate: {
            first: '<i class="bi bi-chevron-double-left"></i>',
            last: '<i class="bi bi-chevron-double-right"></i>',
            next: '<i class="bi bi-chevron-right"></i>',
            previous: '<i class="bi bi-chevron-left"></i>',
        },
        aria: {
            sortAscending: ": activate to sort column ascending",
            sortDescending: ": activate to sort column descending",
        },
    };

    $.extend(true, $.fn.dataTable.defaults, {
        searching: false,
        ordering: true,
        language: language,
        processing: true,
        autoWidth: false,
        responsive: true,
        dom: [
            "<'row'<'col-md-12'f>>",
            "<'row'<'col-md-12't>>",
            "<'row mt-2'",
            "<'col-lg-1 col-xs-12'<'float-left text-center data-tables-refresh'B>>",
            "<'col-lg-3 col-xs-12'<'float-left text-center'i>>",
            "<'col-lg-3 col-xs-12'<'text-center'l>>",
            "<'col-lg-5 col-xs-12'<'float-right'p>>",
            ">",
        ].join(""),
    });
})();
