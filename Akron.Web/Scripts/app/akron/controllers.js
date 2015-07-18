angular.module('akron.controllers', [])
    .controller('MainController', [
        '$scope','$modal', 'dataService', function ($scope, $modal, dataService) {

            //event handlers
            //dimension selected
            $scope.dimensionSelected = function(selection) {
                //only one dimension
                $scope.queryBuilder.selectedSlicers = [selection];


            }
            //filters modal

            //series grid
            $scope.seriesGrid = {
                data: [],
                dataSource: [],
                series: [],
                graphs: [],
                chart: null,
                clear: function () {
                    if ($scope.seriesGrid.chart)
                    $scope.seriesGrid.chart.clear();
                    $scope.seriesGrid.dataSource = [];
                    $scope.seriesGrid.series = [];
                    $scope.seriesGrid.graphs = [];
                },
                initializeDataSource: function() {
                    
                        
      
                    angular.forEach($scope.seriesGrid.data, function(dataItem) {

                        var item = { key: dataItem[1].value }
                        var counter = 0;
                        angular.forEach(dataItem[0].value, function(dataItemValue) {
                            if ($scope.seriesGrid.series.indexOf(dataItemValue[0].value) < 0) {
                                $scope.seriesGrid.series.push(dataItemValue[0].value);
                                var graph = {
                                    balloonText: "[[title]] - [[category]] $[[value]]",
                                    bullet: 'round',
                                    id: counter,
                                    title: dataItemValue[0].value,
                                    valueField: dataItemValue[0].value
                                };
                                counter++;
                                $scope.seriesGrid.graphs.push(graph);
                            }
                            item[dataItemValue[0].value] = Math.floor(dataItemValue[1].value);
                        });
                        $scope.seriesGrid.dataSource.push(item);

                    });

                    $scope.seriesGrid.chart = AmCharts.makeChart("chartdiv",
                        {
                            "type": "serial",
                            "categoryField": "key",
                            "startDuration": 1,
                            "sequencedAnimation": false,
                            "startEffect": "easeInSine",
                            "categoryAxis": {
                                "gridPosition": "start"
                            },
                            "trendLines": [],
                            "graphs": $scope.seriesGrid.graphs,
                            "guides": [],
                            "valueAxes": [
                                {
                                    "id": "ValueAxis-1",
                                    "title": "Axis title"
                                }
                            ],
                            "allLabels": [],
                            "balloon": {},
                            "legend": {
                                "useGraphSettings": true
                            },
                            "titles": [
                                {
                                    "id": "Title-1",
                                    "size": 15,
                                    "text": "Base Pay By Job Family"
                                }
                            ],
                            "dataProvider": $scope.seriesGrid.dataSource
                        }
                    );

                },
                getDefaultSeries: function(cb) {
                    var getData = dataService.querySeriesAsync($scope.queryBuilder);
                    getData.then(function(data) {

                        if (cb)
                            cb(data);
                    }, function() {});
                }
            };

            $scope.filterClose = function(item) {
                $scope.filterSelectionChange(item);
            };
            $scope.filterSelection = [];

            //filterSelection changed
            $scope.filterSelectionChange = function(selection) {

                var cascades = [];
                angular.forEach($scope.dependents, function(dependent) {
                    var dependentColumnName = dependent.column.columnName;
                    angular.forEach(dependent.column.filterDependencyColumns, function(dependency) {
                    
                        if (dependency.columnName == selection.column.columnName && dependentColumnName != selection.column.columnName)
                            cascades.push(dependent);
                    });


                });
                var selectedFilters = _.filter(selection.availableFilterValues, function(val) {
                    return val.active;
                });
                if (selectedFilters.length == 0)
                    return;
                _.forEach(cascades, function(cascade) {

                    var pData = { parentColumnName: selection.column.columnName, columnName: cascade.column.columnName, parentColumnValues: selectedFilters };

                    var ps = dataService.getAvaialableValuesAsync(pData);
                    ps.then(function(data) {
                        cascade.availableFilterValues = data;
                    });
                });

            }
            $scope.refreshGrid = function () {
                $scope.seriesGrid.clear();
                $scope.seriesGrid.getDefaultSeries(function (seriesData) {
                    $scope.seriesGrid.data = seriesData;
                    $scope.seriesGrid.initializeDataSource();
                });
            }
            $scope.getData = function() {

                var getData = dataService.querySeriesAsync($scope.queryBuilder);
                getData.then(function(data) {
                    $scope.seriesGrid.getDefaultSeries(function(data) {
                        $scope.seriesGrid.data = data;
                        $scope.seriesGrid.initializeDataSource();
                    });
                }, function() {});
            };

            //filter modal

            //initialization
            function init() {
                var d = dataService.getQueryBuilderAsync('incumbent');
                d.then(function(data) {
                    $scope.dependents = _.filter(data.availableFilters, function(f) {
                        return f.column.filterDependencyColumns.length > 0;
                    });
                    $scope.queryBuilder = data;
                    $scope.refreshGrid();

                }, function() {});


            }


            init();

        }
    ])