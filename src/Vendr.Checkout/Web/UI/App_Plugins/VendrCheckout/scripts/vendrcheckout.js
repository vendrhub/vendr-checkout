(function () {

    // Initialization
    function init() {

        // Setup order summary toggle
        $("#order-summary-toggle").on("click", function (e) {
            e.preventDefault();
            var toShow = $("#order-summary").hasClass("hidden");
            if (toShow) {
                $("#order-summary").removeClass("hidden");
                $("#order-summary-toggle__text-open").removeClass("hidden");
                $("#order-summary-toggle__text-closed").addClass("hidden");
            } else {
                $("#order-summary").addClass("hidden");
                $("#order-summary-toggle__text-open").addClass("hidden");
                $("#order-summary-toggle__text-closed").removeClass("hidden");
            }
        });

        // Display billing address regions if any
        $("select[name='billingAddress.Country']").on("change", function () {
            showRegions("billing", $(this).children("option:selected").data("regions"));
        }).triggerHandler("change");

        // Display shipping address regions if any
        $("select[name='shippingAddress.Country']").on("change", function () {
            showRegions("shipping", $(this).children("option:selected").data("regions"));
        }).triggerHandler("change");

        // Toggle shipping address display
        $("input[name=shippingSameAsBilling]").on("click", function () {
            var hideShippingInfo = $(this).is(":checked");
            if (hideShippingInfo) {
                $("#shipping-info").addClass("hidden");
            } else {
                $("#shipping-info").removeClass("hidden");
            }
        }).triggerHandler("click");

        // Enable / disable continue button when accepting terms
        $("#acceptTerms").on("click", function () {
            var enableContinueButton = $("#acceptTerms").is(":checked");
            if (enableContinueButton) {
                $("#continue").attr("disabled", false);
            } else {
                $("#continue").attr("disabled", true);
            }
        }).triggerHandler("click");
    }

    // Helper functions
    function showRegions(addressType, regions) {

        var sl = $("select[name='" + addressType + "Address.Region']");
        var slVal = sl.data("value");

        sl.empty();

        var containsValue = false;

        regions.forEach(function (itm, idx) {
            sl.append($('<option>', {
                value: itm.id,
                text: itm.name
            }));
            if (slVal && itm.id === slVal)
                containsValue = true;
        });

        if (containsValue) {
            sl.val(slVal);
        }

        if (regions.length > 0) {
            sl.removeClass("hidden").addClass("block");
        } else {
            sl.removeClass("block").addClass("hidden");
        }

    };

    // Trigger init
    init();

})();


