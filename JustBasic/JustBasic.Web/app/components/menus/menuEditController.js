(function (app) {
    app.controller('menuEditController', menuEditController);

    menuEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function menuEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.menu = {};

        $scope.UpdateMenu = UpdateMenu;

        function loadMenuDetail() {
            apiService.get('api/menu/getbyid/' + $stateParams.id, null, function (result) {
                console.log(result.data);
                $scope.menu = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateMenu() {
            apiService.put('api/menu/update', $scope.menu,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('menus');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
        loadMenuDetail();
    }
})(angular.module('tedushop.menus'));