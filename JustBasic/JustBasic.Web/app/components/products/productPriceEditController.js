(function (app) {
    app.controller('productPriceEditController', productPriceEditController);

    productPriceEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function productPriceEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.productPrice = {};
        $scope.UpdateProductPrice = UpdateProductPrice;

        function loadProductPriceDetail() {
            apiService.get('api/price/getbyid/' + $stateParams.id, null, function (result) {
                console.log(result.data);
                $scope.productPrice = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateProductPrice() {
            apiService.put('api/price/update', $scope.productPrice,
                function (result) {
                    notificationService.displaySuccess('Cập nhật thành công.');
                    $state.go('product_prices');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
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
        loadProductPriceDetail();
    }
})(angular.module('tedushop.products'));