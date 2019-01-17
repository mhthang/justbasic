(function (app) {
    app.controller('slideAddController', slideAddController);

    slideAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function slideAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.slide = {
            Status: true
        }
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }
        $scope.AddSlide = AddSlide;

        function AddSlide() {
            apiService.post('api/slide/create', $scope.slide,
                function (result) {
                    notificationService.displaySuccess('Thêm mới thành công.');
                    $state.go('slides');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.slide.Image = fileUrl;
                })
            }
            finder.popup();
        }
    }
})(angular.module('tedushop.slides'));