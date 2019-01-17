(function (app) {
    app.controller('sizeEditController', sizeEditController);

    sizeEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function sizeEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.size = {};

        $scope.UpdateSize = UpdateSize;

        function loadSizeDetail() {
            apiService.get('api/size/getbyid/' + $stateParams.id, null, function (result) {
                console.log(result.data);
                $scope.size = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateSize() {
            apiService.put('api/size/update', $scope.size,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('sizes');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
        loadSizeDetail();
    }
})(angular.module('tedushop.sizes'));