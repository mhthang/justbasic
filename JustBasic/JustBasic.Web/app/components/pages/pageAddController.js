(function (app) {
    app.controller('pageAddController', pageAddController);

    pageAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function pageAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.page = {
            Status: true
        }
        $scope.ckeditorOptions = {
            languague: 'vi',
            allowedContent: true,
            height: '200px'
        }
        function GetSeoTitle() {
            $scope.page.Alias = commonService.getSeoTitle($scope.page.Name);
        }
        $scope.AddPage = AddPage;

        function AddPage() {
            apiService.post('api/page/create', $scope.page,
                function (result) {
                    notificationService.displaySuccess('Thêm mới thành công.');
                    $state.go('pages');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
    }
})(angular.module('tedushop.colors'));