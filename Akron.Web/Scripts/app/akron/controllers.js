angular.module('akron.controllers', [])
    .controller('MainController', [
        '$scope', 'dataService', function($scope, dataService) {
            //event handlers
            //filterSelection changed
            $scope.filterSelectionChange = function (selection) {
                console.log(selection);
                var cascades = _.filter($scope.dependents, function(dependent) {
                    return _.filter(dependent.column.filterDependencyColumns, function (dependency) {
               
                        return dependency.columnName == selection.columnName;
                    });
                });

                _.forEach(cascades, function(cascade) {
                    var pData = { parentColumnName: selection.column.columnName, columnName: cascade.column.columnName, parentColumnValue: selection.selectedValue.value };
                    var ps = dataService.getAvaialableValuesAsync(pData);
                    ps.then(function(data) {
                        console.log(data);
                    });
                });

            }
            var d = dataService.getQueryBuilderAsync('incumbent');
            d.then(function (data) {
                $scope.dependents = _.filter(data.availableQueryFields, function (f) {
                    return f.column.filterDependencyColumns.length > 0;
                });
                $scope.queryBuilder = data;
                
            }, function () { });

        
        }
    ]);