(function (app) {
    app.controller('rootController', rootController);

    rootController.$inject = ['$state', 'authData', 'loginService', '$scope', 'authenticationService', '$ngBootbox'];

    function rootController($state, authData, loginService, $scope, authenticationService, $ngBootbox) {
        $scope.logOut = function () {
            $ngBootbox.confirm('Bạn có chắc muốn thoát?').then(function () {
                loginService.logOut();
                $state.go('login');
            });
        }
        $scope.authentication = authData.authenticationData;
        $scope.sideBar = "/app/shared/views/sideBar.html";
        //authenticationService.validateRequest();
    }
})(angular.module('tedushop'));