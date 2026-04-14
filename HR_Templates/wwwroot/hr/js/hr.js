const items = document.querySelectorAll(".list-item");
const hidden = document.getElementById("selectedValue");
const search = document.getElementById("search");

items.forEach(item => {
    item.addEventListener("click", () => {

        items.forEach(i => i.classList.remove("active"));
        item.classList.add("active");
        hidden.value = item.dataset.value;

    });
});

search.addEventListener("keyup", () => {
    const text = search.value.toLowerCase();

    items.forEach(item => {
        const visible = item.innerText.toLowerCase().includes(text);
        item.style.display = visible ? "block" : "none";
    });
});

function enviar() {
    if (!hidden.value) {
        alert("Selecciona un formato");
        return;
    }
    document.forms[0].submit();
}