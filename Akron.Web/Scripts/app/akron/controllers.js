angular.module('akron.controllers', [])
    .controller('MainController', [
        '$scope', 'dataService', function($scope, dataService) {
            //event handlers
            //dimension selected
            $scope.dimensionSelected = function (selection) {
                //only one dimension
                $scope.queryBuilder.selectedSlicers = [selection];
                //angular.forEach($scope.queryBuilder.availableSlicers, function(val) {
                //    if (val.isDefault)
                //        $scope.queryBuilder.selectedSlicers.push(val);
                //});
                //$scope.queryBuilder.selectedSlicers.push(selection);
              
                
            }
        
            //filterSelection changed
            $scope.filterSelectionChange = function (selection) {
                console.log(selection);
                var cascades = _.filter($scope.dependents, function(dependent) {
                    return _.filter(dependent.column.filterDependencyColumns, function (dependency) {
               
                        return dependency.columnName == selection.columnName;
                    });
                });

                _.forEach(cascades, function (cascade) {
                    console.log(cascade);
                    var pData = { parentColumnName: selection.column.columnName, columnName: cascade.column.columnName, parentColumnValue: selection.filterValue.value };

                    var ps = dataService.getAvaialableValuesAsync(pData);
                    ps.then(function(data) {
                        cascade.availableFilterValues = data;
                    });
                });

            }
            var d = dataService.getQueryBuilderAsync('incumbent');
            d.then(function (data) {
                $scope.dependents = _.filter(data.availableFilters, function (f) {
                    return f.column.filterDependencyColumns.length > 0;
                });
                $scope.queryBuilder = data;
                
            }, function () { });

            
            $scope.getData = function () {
    
                var getData = dataService.querySeriesAsync($scope.queryBuilder);
                getData.then(function(data) {
                    console.log(data);
                }, function() {});
            }
        
        }
    ]);