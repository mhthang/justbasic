/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('tedushop.sizes', ['tedushop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('sizes', {
                url: "/sizes",
                parent: 'base',
                templateUrl: "/app/components/sizes/sizeListView.html",
                controller: "sizeListController"
            }).state('size_add', {
                url: "/size_add",
                parent: 'base',
                templateUrl: "/app/components/sizes/sizeAddView.html",
                controller: "sizeAddController"
            })
            .state('size_edit', {
                url: "/size_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/sizes/sizeEditView.html",
                controller: "sizeEditController"
            });
    }
})();