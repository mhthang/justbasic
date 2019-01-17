(function (app) {
    app.controller('colorAddController', colorAddController);

    colorAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function colorAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.color = {
            Status: true
        }
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }
        $scope.AddColor = AddColor;

        function AddColor() {
            apiService.post('api/color/create', $scope.color,
                function (result) {
                    notificationService.displaySuccess('Thêm mới thành công.');
                    $state.go('colors');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.color.Image = fileUrl;
                })
            }
            finder.popup();
        }
    }
})(angular.module('tedushop.colors'));