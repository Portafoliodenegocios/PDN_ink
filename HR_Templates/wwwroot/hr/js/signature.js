const SignatureModule = {

    async open(documentName) {
        const res = await fetch('_Signature?documentName=' + encodeURIComponent(documentName));
        const html = await res.text();

        document.getElementById("signatureContainer").innerHTML = html;

        const modal = new bootstrap.Modal(document.getElementById('signatureModal'));
        modal.show();

        setTimeout(() => this.initCanvas(), 200);
    },

    initCanvas() {
        const canvas = document.getElementById('Lienzo');
        const ctx = canvas.getContext('2d');
        let drawing = false;

        canvas.onmousedown = e => {
            drawing = true;
            ctx.beginPath();
            ctx.moveTo(e.offsetX, e.offsetY);
        };

        canvas.onmousemove = e => {
            if (!drawing) return;
            ctx.lineTo(e.offsetX, e.offsetY);
            ctx.stroke();
        };

        canvas.onmouseup = () => drawing = false;
    },

    async save() {
        const canvas = document.getElementById("Lienzo");
        const imageData = canvas.toDataURL("image/png");

        const res = await API.post("/Spreadsheet?handler=SaveSignature", {
            imageData,
            documentName: APP_CONFIG.documentName
        });

        if (res.success) {
            SpreadsheetModule.guardar();
            this.close();
        }
    },

    close() {
        bootstrap.Modal.getInstance(
            document.getElementById('signatureModal')
        )?.hide();
    }
};