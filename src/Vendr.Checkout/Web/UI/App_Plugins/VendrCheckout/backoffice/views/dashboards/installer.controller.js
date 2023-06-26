(function() {

    'use strict';

    function InstallerDashboardController($scope, $routeParams, editorService) {

        var vm = this;

        var dialogOptions = {
            view: '/App_Plugins/VendrCheckout/backoffice/views/dialogs/installer.html',
            size: 'small',
            config: { },
            submit: function (model) {
                editorService.close();
            },
            close: function () {
                editorService.close();
            }
        };

        vm.openInstaller = function() {
            editorService.open(dialogOptions);
        };

    }

    angular.module('vendr').controller('Vendr.Checkout.Controllers.InstallerDashboardController', InstallerDashboardController);

}());