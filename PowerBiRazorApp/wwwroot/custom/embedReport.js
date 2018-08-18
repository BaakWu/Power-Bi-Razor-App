function embedReport(reportGuid) {
    var models = window['powerbi-client'].models;

    $.ajax({
        type: "get",
        url: "/Index?handler=ReportForId",
        contentType: "application/json",
        dataType: "json",
        data: { "reportId": reportGuid },
        success: function (report) {
            var config = {
                type: "report",
                id: report.id,
                accessToken: report.accessToken,
                embedUrl: report.embedUrl,
                tokenType: 1,
                settings: {
                    filterPaneEnabled: false
                }
            };

            var reportContainer = document.getElementById("desktopReport");
            powerbi.embed(reportContainer, config);
        }
    });
}