window.UI = {
    showLoader() {
        document.getElementById("loader").classList.remove("hidden");
    },
    hideLoader() {
        document.getElementById("loader").classList.add("hidden");
    },
    success(msg) {
        Swal.fire("OK", msg, "success");
    },
    //error(msg) {
    //    Swal.fire("Error", msg, "error");
    //},
    warn(msg) {
        Swal.fire("Atención", msg, "warning");
    }
};

document.addEventListener("DOMContentLoaded", () => {


    const token = document.querySelector('[name="__RequestVerificationToken"]').value;
    
    APP_CONFIG.token = token;
    console.log(APP_CONFIG.token);
    Report.load();
});