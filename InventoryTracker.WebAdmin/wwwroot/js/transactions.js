document.addEventListener("DOMContentLoaded", function () {
    const dateFrom = document.getElementById("DateFrom");
    const dateTo = document.getElementById("DateTo");

    // Sets up the Manage dropdown menus in the transactions table.
    initializeTransactionActions();

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

function initializeTransactionActions() {
    // Find every Manage dropdown wrapper on the page.
    const dropdowns = Array.from(document.querySelectorAll("[data-transaction-actions]"));

    // Stop here if this page has no transaction dropdowns.
    if (!dropdowns.length) return;

    // Keep the floating menu at least this many pixels away from the edge of the screen.
    const viewportPadding = 8;

    // Leave this many pixels between the Manage button and the opened menu.
    const dropdownGap = 8;

    function resetDropdown(dropdown) {
        // Find the menu inside this specific dropdown.
        const menu = dropdown.querySelector("[data-transaction-actions-menu]");

        // Stop if the menu is missing, so the rest of the function does not crash.
        if (!menu) return;

        // Remove the class that tells CSS the menu has been positioned and can be shown.
        menu.classList.remove("is-floating");

        // Clear the custom horizontal position that was added when the menu opened.
        menu.style.left = "";

        // Clear the custom vertical position that was added when the menu opened.
        menu.style.top = "";
    }

    function closeDropdown(dropdown) {
        // Remove the native <details> open state, which closes the dropdown.
        dropdown.removeAttribute("open");

        // Clean up the menu position and visibility state after closing it.
        resetDropdown(dropdown);
    }

    function positionDropdown(dropdown) {
        // Find the clickable Manage button for this dropdown.
        const toggle = dropdown.querySelector("[data-transaction-actions-toggle]");

        // Find the menu that should appear next to the Manage button.
        const menu = dropdown.querySelector("[data-transaction-actions-menu]");

        // Stop if required elements are missing, or if this dropdown is not open.
        if (!toggle || !menu || !dropdown.open) return;

        // Get the Manage button's current size and position in the browser window.
        const toggleRect = toggle.getBoundingClientRect();

        // Read the menu width so it can be aligned to the right side of the button.
        const menuWidth = menu.offsetWidth;

        // Read the menu height so the code can decide whether to open above or below.
        const menuHeight = menu.offsetHeight;

        // Calculate how much space exists under the Manage button.
        const availableBelow = window.innerHeight - toggleRect.bottom - dropdownGap - viewportPadding;

        // Open upward if there is not enough room below and enough room above.
        const opensAbove = availableBelow < menuHeight && toggleRect.top > menuHeight + dropdownGap;

        // Choose the menu's vertical position: above the button or below it.
        const top = opensAbove
            ? toggleRect.top - menuHeight - dropdownGap
            : toggleRect.bottom + dropdownGap;

        // Calculat the menu's horizontal position
        const left = toggleRect.right - menuWidth;

        // Apply the calculated horizontal position to the menu.
        menu.style.left = `${left}px`;

        // Apply the calculated vertical position to the menu.
        menu.style.top = `${Math.max(viewportPadding, top)}px`;

        // Mark the menu as positioned, so CSS can make it visible.
        menu.classList.add("is-floating");
    }

    function closeOtherDropdowns(activeDropdown) {
        // Check every dropdown on the page.
        dropdowns.forEach((dropdown) => {
            // Skip the dropdown the user just opened.
            if (dropdown !== activeDropdown) {
                // Close any other open dropdown.
                closeDropdown(dropdown);
            }
        });
    }

    // Attach behavior to every Manage dropdown.
    dropdowns.forEach((dropdown) => {
        // Run this whenever the native <details> element opens or closes.
        dropdown.addEventListener("toggle", function () {
            // If this dropdown was opened, close the others and position this menu.
            if (dropdown.open) {
                // Make sure only one Manage menu is open at a time.
                closeOtherDropdowns(dropdown);

                // Move this menu beside the clicked Manage button.
                positionDropdown(dropdown);
            } else {
                // If this dropdown closed, remove its floating menu state.
                resetDropdown(dropdown);
            }
        });
    });

    // Listen for clicks anywhere on the page.
    document.addEventListener("click", function (event) {
        // Do nothing if the click happened inside one of the Manage dropdowns.
        if (event.target.closest("[data-transaction-actions]")) return;

        // Close all Manage dropdowns when the user clicks somewhere else.
        dropdowns.forEach(closeDropdown);
    });

    // Recalculate open menu positions if the browser window size changes.
    window.addEventListener("resize", function () {
        // Reposition each dropdown that is currently open.
        dropdowns.forEach(positionDropdown);
    });

    // Recalculate open menu positions if the page or table scrolls.
    window.addEventListener("scroll", function () {
        // Reposition each dropdown that is currently open.
        dropdowns.forEach(positionDropdown);
    }, true);
}
