'use strict';
angular.module('akron', [
    'ngRoute',
    'ui.bootstrap',
    'akron.services',
    'akron.directives',
    'akron.controllers'
]).config(['$routeProvider', function ($routeProvider) {
    $routeProvider.otherwise({ templateUrl: 'Scripts/app/akron/partials/main.html', controller: 'MainController' });
}]);
