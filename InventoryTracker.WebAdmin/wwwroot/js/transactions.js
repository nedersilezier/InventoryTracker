document.addEventListener("DOMContentLoaded", function () {
    const dateFrom = document.getElementById("DateFrom");
    const dateTo = document.getElementById("DateTo");

    if (!dateFrom || !dateTo) return;

    function syncDateLimits() {
        if (dateFrom.value) {
            dateTo.min = dateFrom.value;
        } else {
            dateTo.removeAttribute("min");
        }

        if (dateTo.value) {
            dateFrom.max = dateTo.value;
        } else {
            dateFrom.removeAttribute("max");
        }
    }

    dateFrom.addEventListener("change", syncDateLimits);
    dateTo.addEventListener("change", syncDateLimits);

    syncDateLimits();
});