angular.module('akron.controllers', [])
    .controller('MainController', [
        '$scope', 'dataService', function($scope, dataService) {
           

            var d = dataService.getQueryBuilderAsync('incumbent');
            d.then(function (data) {
                $scope.queryBuilder = data;
                console.log(data);
            }, function () { });

        
        }
    ]);