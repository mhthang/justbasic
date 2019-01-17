(function (app) {
    app.controller('colorEditController', colorEditController);

    colorEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function colorEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.color = {};

        $scope.UpdateColor = UpdateColor;

        function loadColorDetail() {
            apiService.get('api/color/getbyid/' + $stateParams.id, null, function (result) {
                console.log(result.data);
                $scope.color = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateColor() {
            apiService.put('api/color/update', $scope.color,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('colors');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
        loadColorDetail();
    }
})(angular.module('tedushop.colors'));