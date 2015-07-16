angular.module('akron.directives', []).
    directive('filterSelect', [function () {

        var result =  {
            templateUrl: '/Scripts/app/akron/partials/templates/filterSelect.html',
            scope: {
                filter: '=filter',
                filterSettings:'=settings'
                
                
            },
            link: function (scope, element, attr) {
                
                scope.filterSelection = [];
              
                scope.options = scope.filter.availableFilterValues;
                console.log(scope.filter.availableFilterValues);
                scope.events = {
                    onItemSelect: function(item) {
                        console.log('snarf');
                    }
                };
                scope.settings = {
                    externalIdProp: '',
                    displayProp:'key'
                };
                console.log(scope);
            }
        };

    return result;
}]);