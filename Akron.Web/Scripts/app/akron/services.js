'use strict';
angular.module('akron.services', []).
    factory('dataService', ['$http','$q',function($http,$q) {
        return {
            getQueryBuilderAsync: function(collection) {

                return $q(function(resolve, reject) {
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
            getAvaialableValuesAsync:function(cascadeFilterModel) {
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
            queryDataAsync:function(queryBuilder) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Incumbent/', queryBuilder).
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