angular.module('akron.controllers', [])
    .controller('QueryBuilderController', [
        '$scope', 'dataService', function ($scope, dataService) {
        
            var d = dataService.getQueryBuilderAsync('incumbent');
        d.then(function(data) {
            console.log(data);
        }, function() {});
    }
    ]).controller('MainController', ['$scope','$state', function ($scope,$state) {

        $state.transitionTo('main.queryBuilder');

}]);