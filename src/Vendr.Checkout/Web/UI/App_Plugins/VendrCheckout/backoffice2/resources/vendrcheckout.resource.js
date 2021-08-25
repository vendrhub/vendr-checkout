(function() {

    'use strict';

    function vendrCheckoutResource($http, umbRequestHelper) {

        return {

            installVendrCheckout: function(siteRootNodeId) {
                return umbRequestHelper.resourcePromise(
                    $http.get("/umbraco/backoffice/VendrCheckout/VendrCheckoutApi/InstallVendrCheckout?siteRootNodeId=" + siteRootNodeId),
                    "Failed to install Vendr.Checkout");
            }

        };

    };

    angular.module('vendr.resources').factory('vendrCheckoutResource', vendrCheckoutResource);

}());