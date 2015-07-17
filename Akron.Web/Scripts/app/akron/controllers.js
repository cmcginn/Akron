angular.module('akron.controllers', [])
    .controller('MainController', [
        '$scope', 'dataService', function ($scope, dataService) {

            //event handlers
            //dimension selected
            $scope.dimensionSelected = function (selection) {
                //only one dimension
                $scope.queryBuilder.selectedSlicers = [selection];



            }
            //series grid
            $scope.seriesGrid = {
                categories:[],
                dataSource: [],
                series:[],
                initializeDataSource: function () {
                    $scope.seriesGrid.dataSource = [{ "key": 2008, "values": [{ "series": "Technology and Science", "value": 78173.4894416607 }, { "series": "Manufacturing and Construction", "value": 61764.073198198195 }, { "series": "Other", "value": 51043.816485507246 }, { "series": "Publishing and Broadcasting", "value": 66023.865921787714 }, { "series": "Association", "value": 76389.47843866171 }, { "series": "Telcom, ISP and Network Services", "value": 75921.052980132445 }, { "series": "Financial Services", "value": 52672.640260444925 }, { "series": "Professional Services", "value": 77484.892381332276 }, { "series": "Hospitality, Transportation, Services", "value": 61021.969740634006 }, { "series": "Health Care", "value": 56102.388880356317 }, { "series": "Government", "value": 61304.439819632331 }, { "series": "Non Profit", "value": 69297.5006254691 }, { "series": "Education", "value": 44504.961725723559 }] }, { "key": 2006, "values": [{ "series": "Government", "value": 47779.331039478449 }, { "series": "Education", "value": 40816.425956738771 }, { "series": "Financial Services", "value": 49586.447825580042 }, { "series": "Technology and Science", "value": 79738.124554274822 }, { "series": "Publishing and Broadcasting", "value": 61504.365269461079 }, { "series": "Health Care", "value": 51168.617042440317 }, { "series": "Manufacturing and Construction", "value": 44766.640532544377 }, { "series": "Hospitality, Transportation, Services", "value": 58667.278813559322 }, { "series": "Telcom, ISP and Network Services", "value": 72429.26274065685 }, { "series": "Association", "value": 71088.899451553923 }, { "series": "Non Profit", "value": 64631.936978683967 }, { "series": "Professional Services", "value": 65826.267557455911 }, { "series": "Other", "value": 67900.889599555056 }] }, { "key": 2010, "values": [{ "series": "Other", "value": 67557.159767248551 }, { "series": "Professional Services", "value": 81360.507943323319 }, { "series": "Financial Services", "value": 73300.230501647748 }, { "series": "Education", "value": 50871.980099502485 }, { "series": "Hospitality, Transportation, Services", "value": 56658.785813630042 }, { "series": "Government", "value": 71647.154102866116 }, { "series": "Publishing and Broadcasting", "value": 82279.970509383376 }, { "series": "Health Care", "value": 60340.830097367056 }, { "series": "Technology and Science", "value": 87593.831439062778 }, { "series": "Telcom, ISP and Network Services", "value": 88698.37447988904 }, { "series": "Manufacturing and Construction", "value": 76793.951388888891 }, { "series": "Non Profit", "value": 73385.986223862245 }, { "series": "Association", "value": 84219.21591307396 }] }, { "key": 2007, "values": [{ "series": "Manufacturing and Construction", "value": 54484.137549407118 }, { "series": "Education", "value": 41066.845464379541 }, { "series": "Health Care", "value": 53701.301032565527 }, { "series": "Telcom, ISP and Network Services", "value": 75406.253507951362 }, { "series": "Non Profit", "value": 66780.189748201432 }, { "series": "Financial Services", "value": 58401.301061609651 }, { "series": "Association", "value": 76551.809406952962 }, { "series": "Technology and Science", "value": 77858.531292770931 }, { "series": "Hospitality, Transportation, Services", "value": 61000.371636363634 }, { "series": "Professional Services", "value": 69869.192112091332 }, { "series": "Publishing and Broadcasting", "value": 66441.973043478254 }, { "series": "Other", "value": 62938.945353088318 }, { "series": "Government", "value": 69299.00128783 }] }, { "key": 2009, "values": [{ "series": "Manufacturing and Construction", "value": 58658.453987730063 }, { "series": "Association", "value": 80240.7931147541 }, { "series": "Telcom, ISP and Network Services", "value": 83814.907300115869 }, { "series": "Publishing and Broadcasting", "value": 72012.318840579712 }, { "series": "Health Care", "value": 60158.99019229284 }, { "series": "Government", "value": 62898.643571428569 }, { "series": "Other", "value": 73527.992197949177 }, { "series": "Non Profit", "value": 70071.918804623623 }, { "series": "Hospitality, Transportation, Services", "value": 58158.6875 }, { "series": "Technology and Science", "value": 81638.22856676727 }, { "series": "Professional Services", "value": 81101.7650419351 }, { "series": "Education", "value": 47427.6797149551 }, { "series": "Financial Services", "value": 57232.662986930591 }] }, { "key": 2011, "values": [{ "series": "Hospitality, Transportation, Services", "value": 99958.017467248908 }, { "series": "Education", "value": 53022.028867839421 }, { "series": "Publishing and Broadcasting", "value": 79496.058430717865 }, { "series": "Professional Services", "value": 84249.893926220349 }, { "series": "Manufacturing and Construction", "value": 85022.575675675675 }, { "series": "Technology and Science", "value": 89450.125371039991 }, { "series": "Non Profit", "value": 79416.894251054851 }, { "series": "Telcom, ISP and Network Services", "value": 91161.1919945726 }, { "series": "Other", "value": 60481.357778124511 }, { "series": "Health Care", "value": 59897.506642141838 }, { "series": "Government", "value": 80044.211432083583 }, { "series": "Financial Services", "value": 66161.454255658275 }, { "series": "Association", "value": 85784.2368045649 }] }, { "key": 2012, "values": [{ "series": "Hospitality, Transportation, Services", "value": 49735.168654173765 }, { "series": "Health Care", "value": 61459.941988950275 }, { "series": "Government", "value": 71968.017412935325 }, { "series": "Other", "value": 87405.797827398914 }, { "series": "Technology and Science", "value": 103755.95205152672 }, { "series": "Association", "value": 92159.550125944588 }, { "series": "Financial Services", "value": 112929.0014367816 }, { "series": "Publishing and Broadcasting", "value": 94242.382022471909 }, { "series": "Education", "value": 61948.578135048228 }, { "series": "Non Profit", "value": 80773.503845058382 }, { "series": "Professional Services", "value": 84891.168796490558 }, { "series": "Telcom, ISP and Network Services", "value": 117407.02325581395 }] }];
                    var counter = 0;
                    angular.forEach($scope.seriesGrid.dataSource, function (dataSourceItem) {
                        
                        var series = {
                            balloonText: "[[title]] - [[category]]",
                            bullet: 'round',
                            id: counter,
                            title: dataSourceItem.key,
                            valueField: dataSourceItem.values[counter]
                        };
                        $scope.seriesGrid.series.push(series);
                        counter++;
                    });
                    
                    AmCharts.makeChart("chartdiv",
                       {
                           "type": "serial",
                           "categoryField": "key",
                           "startDuration": 1,
                           "categoryAxis": {
                               "gridPosition": "start"
                           },
                           "trendLines": [],
                           "graphs": $scope.seriesGrid.series,
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
                                   "text": "Chart Title"
                               }
                           ],
                           "dataProvider": $scope.seriesGrid.dataSource
                       }
                   );
                    //angular.forEach($scope.seriesGrid.dataSource, function (dataItem) {
                        
                    //    var itemVal = { category: +dataItem.key };
                    //    angular.forEach(dataItem.values, function(dataItemValue) {
                    //        if ($scope.seriesGrid.categories.indexOf(dataItemValue.series) < 0)
                    //            $scope.seriesGrid.categories.push(dataItemValue.series);
                            
                    //    });
                    //});
              

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
                        console.log(dependency);
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
                        console.log('cascade is');
                        console.log(cascade);
                        cascade.availableFilterValues = data;
                    });
                });

            }

            $scope.getData = function () {

                var getData = dataService.querySeriesAsync($scope.queryBuilder);
                getData.then(function (data) {
                    console.log(data);
                }, function () { });
            };

            //initialization
            function init() {
                var d = dataService.getQueryBuilderAsync('incumbent');
                d.then(function (data) {
                    $scope.dependents = _.filter(data.availableFilters, function (f) {
                        return f.column.filterDependencyColumns.length > 0;
                    });
                    $scope.queryBuilder = data;
                    //load default series grid
                   // $scope.seriesGrid.initializeDataSource();
                    $scope.seriesGrid.getDefaultSeries(function(seriesData) {
                        // $scope.seriesGrid.dataSource = seriesData;
                        console.log(seriesData);
                        // $scope.seriesGrid.initializeDataSource();

                        //console.log($scope.seriesGrid.dataSource);
                        });
                    }, function () { });


            }

            init();

        }
    ]);