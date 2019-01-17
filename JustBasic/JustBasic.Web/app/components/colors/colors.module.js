/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('tedushop.colors', ['tedushop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('colors', {
                url: "/colors",
                parent: 'base',
                templateUrl: "/app/components/colors/colorListView.html",
                controller: "colorListController"
            }).state('color_add', {
                url: "/color_add",
                parent: 'base',
                templateUrl: "/app/components/colors/colorAddView.html",
                controller: "colorAddController"
            })
            .state('color_edit', {
                url: "/color_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/colors/colorEditView.html",
                controller: "colorEditController"
            });
    }
})();