const SpreadsheetModule = {

    get() {
        return ASPxClientControl.GetControlCollection().GetByName("spreadsheet");
    },

    guardar() {
        const state = this.get().GetSpreadsheetState();

        API.post('/Spreadsheet?handler=RibbonSaveToFile', {
            spreadsheetState: state,
            namefile: APP_CONFIG.documentName
        })
            .then(() => {
                Swal.fire(
                    `Se envió el documento "${APP_CONFIG.documentName}" al sistema PDNink.`,
                    "",
                    "success"
                );
            })
            .catch(console.error);
    }







};