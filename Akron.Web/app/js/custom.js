var myApp = angular.module('myAppName', ['angle']).factory('dataService', ['$http', '$q', function ($http, $q) {
    return {
        getQueryBuilderAsync: function (collection) {

            return $q(function (resolve, reject) {
                $http.get('/api/QueryBuilder/' + collection).
                success(function (data, status, headers, config) {
                    resolve(data);
                })
                .error(function (data, status, headers, config) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            });
        },
        getAvaialableValuesAsync: function (cascadeFilterModel) {
            return $q(function (resolve, reject) {
                $http.post('/api/FilterDataColumn/', cascadeFilterModel).
                success(function (data, status, headers, config) {
                    resolve(data);
                })
                .error(function (data, status, headers, config) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            });
        },
        queryDataAsync: function (queryBuilder) {
            return $q(function (resolve, reject) {
                $http.post('/api/QueryBuilder/', queryBuilder).
                success(function (data, status, headers, config) {
                    resolve(data);
                })
                .error(function (data, status, headers, config) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            });
        },
        querySeriesAsync: function (queryBuilder) {
            return $q(function (resolve, reject) {
                $http.post('/api/SeriesBuilder/', queryBuilder).
                success(function (data, status, headers, config) {
                    resolve(data);
                })
                .error(function (data, status, headers, config) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            });
        }

    };
}]);

myApp.run(["$log", function ($log) {

    $log.log('I\'m a line from custom.js');

}]);

myApp.config(["RouteHelpersProvider", function (RouteHelpersProvider) {

    // Custom Route definition

}]);

myApp.controller('oneOfMyOwnController', ["$scope", function ($scope) {
    /* controller code */
}]);

myApp.directive('oneOfMyOwnDirectives', function () {
    /*directive code*/
});

myApp.config([
    "$stateProvider", function ($stateProvider /* ... */) {
        /* specific routes here (see file config.js) */
    }
]).controller('MainController', [
    '$scope', '$rootScope', '$modal', 'dataService', 'ngDialog', function ($scope, $rootScope, $modal, dataService, ngDialog) {
        "use strict";
        //event handlers
        //dimension selected
        $scope.dimensionSelected = function (selection) {
            //only one dimension
            $scope.queryBuilder.selectedSlicers = [selection];


        }
        //filters modal
        $scope.generateBarChart=function() {
            AmCharts.makeChart("bardiv",
				{
				    "type": "serial",
				    "categoryField": "category",
				    "colors": myColors,
				    "startDuration": 1,
				    "categoryAxis": {
				        "gridPosition": "start"
				    },
				    "trendLines": [],
				    "graphs": [
						{
						    "balloonText": "[[title]] of [[category]]:[[value]]",
						    "fillAlphas": 1,
						    "id": "AmGraph-1",
						    "title": "graph 1",
						    "type": "column",
						    "valueField": "column-1"
						}
				    ],
				    "guides": [],
				    "valueAxes": [
						{
						    "id": "ValueAxis-1",
						    "stackType": "regular",
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
						    "text": "Chart Title"
						}
				    ],
				    "dataProvider": [
						{
						    "category": "2006",
						    "column-1": 8
						},
						{
						    "category": "2007",
						    "column-1": 6
						},
						{
						    "category": "2008",
						    "column-1": 2
						},
						{
						    "category": "2009",
						    "column-1": "6"
						},
						{
						    "category": "2010",
						    "column-1": "3"
						},
						{
						    "category": "2011",
						    "column-1": "2"
						}
				    ]
				}
			);
        }
        $scope.generateBubbleChart=function() {
            AmCharts.makeChart("bubblediv",
				{
				    "type": "xy",
				    "sequencedAnimation": false,
				    "startDuration": 1.5,
				    "startEffect": "easeInSine",
				    "trendLines": [],
				    "graphs": [
						{
						    "balloonText": "x:<b>[[x]]</b> y:<b>[[y]]</b><br>value:<b>[[value]]</b>",
						    "bullet": "round",
						    "id": "AmGraph-1",
						    "lineAlpha": 0,
						    "lineColor": "#b0de09",
						    "valueField": "value",
						    "xField": "x",
						    "yField": "y"
						},
						{
						    "balloonText": "x:<b>[[x]]</b> y:<b>[[y]]</b><br>value:<b>[[value]]</b>",
						    "bullet": "round",
						    "id": "AmGraph-2",
						    "lineAlpha": 0,
						    "lineColor": "#fcd202",
						    "valueField": "value2",
						    "xField": "x2",
						    "yField": "y2"
						}
				    ],
				    "guides": [],
				    "valueAxes": [
						{
						    "id": "ValueAxis-1",
						    "axisAlpha": 0
						},
						{
						    "id": "ValueAxis-2",
						    "position": "bottom",
						    "axisAlpha": 0
						}
				    ],
				    "allLabels": [],
				    "balloon": {},
				    "titles": [],
				    "dataProvider": [
						{
						    "y": 10,
						    "x": 14,
						    "value": 59,
						    "y2": -5,
						    "x2": -3,
						    "value2": 44
						},
						{
						    "y": 5,
						    "x": 3,
						    "value": 50,
						    "y2": -15,
						    "x2": -8,
						    "value2": 12
						},
						{
						    "y": -10,
						    "x": -3,
						    "value": 19,
						    "y2": -4,
						    "x2": 6,
						    "value2": 35
						},
						{
						    "y": -6,
						    "x": 5,
						    "value": 65,
						    "y2": -5,
						    "x2": -6,
						    "value2": 168
						},
						{
						    "y": 15,
						    "x": -4,
						    "value": 92,
						    "y2": -10,
						    "x2": -8,
						    "value2": 102
						},
						{
						    "y": 13,
						    "x": 1,
						    "value": 8,
						    "y2": -2,
						    "x2": -3,
						    "value2": 41
						},
						{
						    "y": 1,
						    "x": 6,
						    "value": 35,
						    "y2": 0,
						    "x2": -3,
						    "value2": 16
						}
				    ]
				}
			);
        }
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
            initializeDataSource: function () {


                angular.forEach($scope.seriesGrid.data, function (dataItem) {

                    var item = { key: dataItem[1].value }
                    var counter = 0;
                    angular.forEach(dataItem[0].value, function (dataItemValue) {
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
                        "colors":myColors,
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
                                "title": "Base Pay"
                            }
                        ],
                        "allLabels": [],
                        "balloon": {},
                        "legend": {
                            "bottom": -1,
                            "useGraphSettings": true,
                            "verticalGap": 2
                        },

                        "dataProvider": $scope.seriesGrid.dataSource
                    }
                );

            },
            getDefaultSeries: function (cb) {
                var getData = dataService.querySeriesAsync($scope.queryBuilder);
                getData.then(function (data) {

                    if (cb)
                        cb(data);
                }, function () { });
            }
        };

        $scope.filterClose = function (item) {
            $scope.filterSelectionChange(item);
        };
        $scope.filterSelection = [];

        //filterSelection changed
        $scope.filterSelectionChange = function (selection) {

            var cascades = [];
            angular.forEach($scope.dependents, function (dependent) {
                var dependentColumnName = dependent.column.columnName;
                angular.forEach(dependent.column.filterDependencyColumns, function (dependency) {

                    if (dependency.columnName == selection.column.columnName && dependentColumnName != selection.column.columnName)
                        cascades.push(dependent);
                });


            });
            var selectedFilters = _.filter(selection.availableFilterValues, function (val) {
                return val.active;
            });
            if (selectedFilters.length == 0)
                return;
            _.forEach(cascades, function (cascade) {

                var pData = { parentColumnName: selection.column.columnName, columnName: cascade.column.columnName, parentColumnValues: selectedFilters };

                var ps = dataService.getAvaialableValuesAsync(pData);
                ps.then(function (data) {
                    cascade.availableFilterValues = data;
                });
            });

        }
        $scope.refreshGrid = function () {
            $scope.$emit('gridStatusChange', { busy: true});
            $scope.seriesGrid.clear();
            $scope.seriesGrid.getDefaultSeries(function (seriesData) {

                $scope.seriesGrid.data = seriesData;
                $scope.seriesGrid.initializeDataSource();
                $scope.$emit('gridStatusChange', { busy: false });
                //temp for show
                $scope.generateBarChart();
                $scope.generateBubbleChart();
            });
        }
        $scope.getData = function () {

            var getData = dataService.querySeriesAsync($scope.queryBuilder);
            getData.then(function (data) {

                $scope.seriesGrid.getDefaultSeries(function (data) {
                    $scope.seriesGrid.data = data;
                    $scope.seriesGrid.initializeDataSource();
                });
            }, function () { });
        };
        $scope.showFilterSelection = function () {
            ngDialog.open({
                template: 'filterSelect',
                scope: $scope
            });
        };

        //filter modal

        //initialization
        function init() {

            var d = dataService.getQueryBuilderAsync('incumbent');
            d.then(function (data) {

                $scope.dependents = _.filter(data.availableFilters, function (f) {
                    return f.column.filterDependencyColumns.length > 0;
                });
                $scope.queryBuilder = data;

                $scope.refreshGrid();

            }, function () { });


        }

    
        init();

    }
]);
