angular.module('akron.directives', []).
    directive('lineChart', [function () {
        return function (scope, elm, attrs) {
        console.log(scope);
    };
}]);