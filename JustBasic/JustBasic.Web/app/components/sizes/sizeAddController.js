(function (app) {
    app.controller('sizeAddController', sizeAddController);

    sizeAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function sizeAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.size = {
            Status: true
        }
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }
        $scope.AddSize = AddSize;

        function AddSize() {
            apiService.post('api/size/create', $scope.size,
                function (result) {
                    notificationService.displaySuccess('Thêm mới thành công.');
                    $state.go('sizes');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
    }
})(angular.module('tedushop.colors'));