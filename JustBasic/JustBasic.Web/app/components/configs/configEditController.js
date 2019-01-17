(function (app) {
    app.controller('configEditController', configEditController);

    configEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function configEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.config = {};

        $scope.UpdateConfig = UpdateConfig;

        $scope.ckeditorOptions = {
            language: 'vi',
            allowedContent: true,
            toolbar: 'Full',
            enterMode: CKEDITOR.ENTER_BR,
            shiftEnterMode: CKEDITOR.ENTER_P,
            height: '400px'
        }
        function loadConfigDetail() {
            apiService.get('api/footer/getbyid/' + $stateParams.id, null, function (result) {
                console.log(result.data);
                $scope.config = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateConfig() {
            apiService.put('api/footer/update', $scope.config,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('configs');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
        loadConfigDetail();
    }
})(angular.module('tedushop.configs'));