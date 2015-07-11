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
            getQueryBuilder: function (collection) {


            $http.get('/api/QueryBuilder/' + collection).
                success(function(data, status, headers, config) {
                    console.log(data);
                })
                .error(function(data, status, headers, config) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });

        }
    };
}]);