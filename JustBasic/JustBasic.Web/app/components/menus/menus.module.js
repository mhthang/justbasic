/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('tedushop.menus', ['tedushop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('menus', {
                url: "/menus",
                parent: 'base',
                templateUrl: "/app/components/menus/menuListView.html",
                controller: "menuListController"
            }).state('menu_add', {
                url: "/menu_add",
                parent: 'base',
                templateUrl: "/app/components/menus/menuAddView.html",
                controller: "menuAddController"
            })
            .state('menu_edit', {
                url: "/menu_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/menus/menuEditView.html",
                controller: "menuEditController"
            });
    }
})();