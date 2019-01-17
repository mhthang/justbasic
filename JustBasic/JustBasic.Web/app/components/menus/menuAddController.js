(function (app) {
    app.controller('menuAddController', menuAddController);

    menuAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function menuAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.menu = {
            Status: true
        }
        $scope.AddMenu = AddMenu;

        function AddMenu() {
            apiService.post('api/menu/create', $scope.menu,
                function (result) {
                    notificationService.displaySuccess('Thêm mới thành công.');
                    $state.go('menus');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
    }
})(angular.module('tedushop.menus'));