window.Report = {

    async load() {
        try {
            UI.showLoader();

            const res = await API.post('/VisorReport?handler=GenerateReport', {
                CreditId: "97812",
                ReportId: "1",
                ReportName: APP_CONFIG.name
            });
            console.log("Respuesta:", res);
            if (!res.success) throw new Error();

            //document.getElementById("pdfViewer").src =
            //    `/pdfjs/web/viewer.html?file=${encodeURIComponent(res.url)}`;
            document.getElementById("pdfViewer").src =
                `/pdfjs/web/viewer.html?file=${encodeURIComponent(window.location.origin + res.url)}`;

        } catch {
           /* UI.error("No se pudo generar el reporte");*/
        } finally {
            UI.hideLoader();
        }
    }
};