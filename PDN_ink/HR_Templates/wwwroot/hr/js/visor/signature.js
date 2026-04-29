window.Signature = {


    init() {
        if (this.initialized) return;

        this.canvas = document.getElementById("Lienzo");
        this.ctx = this.canvas.getContext("2d");

        let drawing = false;

        this.canvas.onmousedown = e => {
            drawing = true;
            this.ctx.beginPath();
            this.ctx.moveTo(e.offsetX, e.offsetY);
        };

        this.canvas.onmousemove = e => {
            if (!drawing) return;
            this.ctx.lineTo(e.offsetX, e.offsetY);
            this.ctx.stroke();
        };

        this.canvas.onmouseup = () => drawing = false;
        this.canvas.onmouseleave = () => drawing = false;

        this.initialized = true;
    },
    //init() {
    //    this.canvas = document.getElementById("Lienzo");
    //    this.ctx = this.canvas.getContext("2d");

    //    let drawing = false;

    //    this.canvas.onmousedown = e => {
    //        drawing = true;
    //        this.ctx.beginPath();
    //        this.ctx.moveTo(e.offsetX, e.offsetY);
    //    };

    //    this.canvas.onmousemove = e => {
    //        if (!drawing) return;
    //        this.ctx.lineTo(e.offsetX, e.offsetY);
    //        this.ctx.stroke();
    //    };

    //    this.canvas.onmouseup = () => drawing = false;
    //},

    open() {
        //document.getElementById("modalSign").classList.remove("hidden");
        //setTimeout(() => this.init(), 100);
        document.getElementById("modalSign").classList.remove("hidden");
        this.init();
        this.clear();
    },

    close() {
        document.getElementById("modalSign").classList.add("hidden");
    },

    clear() {
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    },

    isEmpty() {
        const blank = document.createElement("canvas");
        blank.width = this.canvas.width;
        blank.height = this.canvas.height;
        return this.canvas.toDataURL() === blank.toDataURL();
    },

    async save() {
        if (this.isEmpty()) {
            UI.warn("Debe firmar primero");
            return;
        }

        try {
            await API.post("/VisorReport?handler=SaveSignature", {
                imageData: this.canvas.toDataURL(),
                documentName: APP_CONFIG.name
            });

            UI.success("Documento firmado correctamente");
            this.close();

        } catch {
            UI.error("Error al guardar firma");
        }
    }
};