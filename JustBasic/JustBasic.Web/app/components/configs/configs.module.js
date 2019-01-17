/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('tedushop.configs', ['tedushop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('configs', {
                url: "/configs",
                parent: 'base',
                templateUrl: "/app/components/configs/configListView.html",
                controller: "configListController"
            }).state('config_add', {
                url: "/config_add",
                parent: 'base',
                templateUrl: "/app/components/configs/configAddView.html",
                controller: "configAddController"
            })
            .state('config_edit', {
                url: "/config_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/configs/configEditView.html",
                controller: "configEditController"
            });
    }
})();