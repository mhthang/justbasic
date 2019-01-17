(function (app) {
    app.controller('configListController', configListController);

    configListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function configListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.configs = [];
        $scope.config = 0;
        $scope.configsCount = 0;
        $scope.getPages = getPages;
        $scope.keyword = '';

        $scope.search = search;

        $scope.deleteConfig = deleteConfig;

        $scope.selectAll = selectAll;

        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedconfigs: JSON.stringify(listId)
                }
            }
            apiService.del('api/footer/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.configs, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.configs, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("configs", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteConfig(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/footer/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }

        function search() {
            getPages();
        }

        function getPages(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('/api/footer/getpaging', config, function (result) {
                $scope.configs = result.data.Items;
                $scope.size = result.data.size;
                $scope.PagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load config failed.');
            });
        }

        $scope.getPages();
    }
})(angular.module('tedushop.configs'));