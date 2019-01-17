(function (app) {
    app.controller('configAddController', configAddController);

    configAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function configAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.config = {
            Status: true
        }
        $scope.ckeditorOptions = {
            language: 'vi',
            allowedContent: true,
            toolbar: 'Full',
            enterMode: CKEDITOR.ENTER_BR,
            shiftEnterMode: CKEDITOR.ENTER_P,
            height: '400px'
        }
        function GetSeoTitle() {
            $scope.config.Alias = commonService.getSeoTitle($scope.config.Name);
        }
        $scope.AddConfig = AddConfig;

        function AddConfig() {
            apiService.post('api/footer/create', $scope.config,
                function (result) {
                    notificationService.displaySuccess('Thêm mới thành công.');
                    $state.go('configs');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
    }
})(angular.module('tedushop.colors'));