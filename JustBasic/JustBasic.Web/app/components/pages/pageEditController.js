(function (app) {
    app.controller('pageEditController', pageEditController);

    pageEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function pageEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.page = {};

        $scope.UpdatePage = UpdatePage;

        $scope.ckeditorOptions = {
            languague: 'vi',
            allowedContent: true,
            height: '200px'
        }
        function GetSeoTitle() {
            $scope.page.Alias = commonService.getSeoTitle($scope.page.Name);
        }
        function loadPageDetail() {
            apiService.get('api/page/getbyid/' + $stateParams.id, null, function (result) {
                console.log(result.data);
                $scope.page = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdatePage() {
            apiService.put('api/page/update', $scope.page,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('pages');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
        loadPageDetail();
    }
})(angular.module('tedushop.pages'));