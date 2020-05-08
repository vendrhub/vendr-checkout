(function() {

    'use strict';

    function InstallerDashboardController($scope, $routeParams, editorService) {

        var vm = this;

        var dialogOptions = {
            view: '/app_plugins/vendrcheckout/backoffice/views/dialogs/installer.html',
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