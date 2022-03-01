(function () {

    // Initialization
    function init() {

        // Setup order summary toggle
        document.getElementById("order-summary-toggle").addEventListener("click", function (e) {
            e.preventDefault();
            var osEl = document.getElementById("order-summary");
            var showOrderSummary = osEl.classList.contains("hidden");
            osEl.classList.toggle("hidden", !showOrderSummary);
            document.getElementById("order-summary-toggle__text-open").classList.toggle("hidden", !showOrderSummary);
            document.getElementById("order-summary-toggle__text-closed").classList.toggle("hidden", showOrderSummary);
        });

        // Display billing address regions if any
        document.querySelectorAll("select[name='billingAddress.Country']").forEach(el => {
            var h = () => { toggleShippingRequiredInputValidation(); showRegions("billing", JSON.parse(el.selectedOptions[0].dataset.regions)); }
            el.addEventListener("change", h);
            h();
        });

        // Display shipping address regions if any
        document.querySelectorAll("select[name='shippingAddress.Country']").forEach(el => {
            var h = () => { toggleShippingRequiredInputValidation(); showRegions("shipping", JSON.parse(el.selectedOptions[0].dataset.regions)); }
            el.addEventListener("change", h);
            h();
        });

        // Toggle shipping address display
        document.querySelectorAll("input[name=shippingSameAsBilling]").forEach(el => {
            var h = () => { document.getElementById("shipping-info").classList.toggle("hidden", el.checked); toggleShippingRequiredInputValidation(); }
            el.addEventListener("change", h);
            h();
        });

        // Enable / disable continue button when accepting terms
        var acceptTermsEl = document.getElementById("accept-terms");
        if (acceptTermsEl) {
            var h = () => document.getElementById("continue").disabled = !acceptTermsEl.checked;
            acceptTermsEl.addEventListener("click", h);
            h();
        }
    }

    // Helper functions
    function toggleShippingRequiredInputValidation() {
        var shippingSameAsBillingEl = document.querySelector("input[name=shippingSameAsBilling]");
        var shippingSameAsBilling = shippingSameAsBillingEl && shippingSameAsBillingEl.checked;
        document.querySelectorAll("#shipping-info [required]").forEach(el => {
            el.disabled = shippingSameAsBilling; // Disable any shipping required fields (this overrides the required validation)
        });
    }

    function showRegions(addressType, regions) {
        var sl = document.querySelector("select[name='" + addressType + "Address.Region']");
        var slVal = sl.dataset.value;

        sl.innerHTML = "";

        var hasRegions = regions.length > 0;
        if (hasRegions) {
            var containsValue = false;

            var opt = document.createElement('option');
            opt.value = "";
            opt.text = sl.dataset.placeholder;
            opt.disabled = true;
            opt.selected = true;
            sl.appendChild(opt);

            regions.forEach(function (itm, idx) {
                var opt = document.createElement('option');
                opt.value = itm.id;
                opt.text = itm.name;
                sl.appendChild(opt);
                if (slVal && itm.id === slVal)
                    containsValue = true;
            });

            if (containsValue) {
                sl.value = slVal;
            }
        }

        sl.required = hasRegions;
        sl.disabled = !hasRegions;
    };

    // Trigger init
    init();

})();


