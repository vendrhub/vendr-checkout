(function () {

    // Initialization
    function initSm() {

        $("body").on("click.sm", "#order-summary-toggle", function (e) {
            e.preventDefault();
            var toShow = $("#order-summary").hasClass("hidden");
            if (toShow) {
                showMobileOrderSummary();
            } else {
                hideMobileOrderSummary();
            }
        });

        hideMobileOrderSummary();

    }

    function initLg() {

        $("body").off(".sm");

    }

    function initCommon() {

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
            showHideShippingInfo(true);
        });

        var acceptTerms = $("#acceptTerms");
        if (acceptTerms.length > 0) {
            acceptTerms.on("click", enableDisableContinueButton);
            enableDisableContinueButton();
        }

        var errors = $(".validation-summary-errors");
        if (errors.length > 0) {
            Toastify({
                text: errors.find("ul li:first-child").text(),
                duration: 3000,
                gravity: "bottom", // `top` or `bottom`
                position: 'center', // `left`, `center` or `right`
                backgroundColor: "#f56565",
                className: "",
                stopOnFocus: true, // Prevents dismissing of toast on hover
            }).showToast();
        }

        showHideShippingInfo(false);
    }

    function showMobileOrderSummary() {
        $("#order-summary").removeClass("hidden");
        $("#order-summary-toggle__text-open").removeClass("hidden");
        $("#order-summary-toggle__text-closed").addClass("hidden");
    }

    function hideMobileOrderSummary() {
        $("#order-summary").addClass("hidden");
        $("#order-summary-toggle__text-open").addClass("hidden");
        $("#order-summary-toggle__text-closed").removeClass("hidden");
    }

    function showHideShippingInfo(clearValues) {
        var hideShippingInfo = $("input[name=shippingSameAsBilling]").is(":checked");
        if (hideShippingInfo) {
            $("#shipping-info").hide();
        } else {
            if (clearValues) {
                //$("input[type=text][name^=shipping]").val("");
            }
            $("#shipping-info").show();
        }
    }

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
            sl.removeClass("hidden")
                .addClass("block");
        } else {
            sl.removeClass("block")
                .addClass("hidden");
        }

    };

    function enableDisableContinueButton() {

        var enableContinueButton = $("#acceptTerms").is(":checked");
        if (enableContinueButton) {
            $("#continue").attr("disabled", false)
                .css("backgroundColor", "")
                .css("color", "")
                .css("cursor", "");
        } else {
            $("#continue").attr("disabled", true)
                .css("backgroundColor", "#f7fafc")
                .css("color", "#ddd")
                .css("cursor", "no-drop");
        }

    }

    // Setup responsive states
    ssm.addState({
        id: 'sm',
        query: '(max-width: 1023px)',
        onEnter: initSm
    });

    ssm.addState({
        id: 'lg',
        query: '(min-width: 1024px)',
        onEnter: initLg
    });

    initCommon();

})();


