'use strict';
angular.module('akron', [
    'ui.router',
    'akron.services',
    'akron.controllers'
    
]).config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise("/");
    $stateProvider
        .state('main', {
            url: "/",
            templateUrl: 'scripts/app/akron/partials/main.html', controller: 'MainController' 
        })
        .state('main.queryBuilder', {

            templateUrl: 'scripts/app/akron/partials/main.queryBuilder.html', controller: 'QueryBuilderController'
        });
});