(function (app) {
    app.controller('productPriceAddController', productPriceAddController);

    productPriceAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function productPriceAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.productPrice = {};
        $scope.AddProductPrice = AddProductPrice;

        function AddProductPrice() {
            apiService.post('api/price/create', $scope.productPrice,
                function (result) {
                    notificationService.displaySuccess('Thêm mới thành công.');
                    $scope.productPrice = {};
                }, function (error) {
                    notificationService.displayError(error.data.ErrorMesage);
                });
        }
        function loadProduct() {
            apiService.get('/api/product/getallparents', null, function (result) {
                $scope.products = result.data;
            }, function () {
                console.log('Load product failed.');
            });
        }
        function loadSize() {
            apiService.get('/api/size/getall', null, function (result) {
                $scope.sizes = result.data;
            }, function () {
                console.log('Load size failed.');
            });
        }
        function loadColor() {
            apiService.get('/api/color/getall', null, function (result) {
                $scope.colors = result.data;
            }, function () {
                console.log('Load color failed.');
            });
        }
        loadProduct();
        loadSize();
        loadColor();
    }
})(angular.module('tedushop.products'));