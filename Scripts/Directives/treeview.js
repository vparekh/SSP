app.directive('ngTree', function () {
    return {
        restrict: 'E',
        transclude: true,

        controller: function ($scope) {
            $scope.showStates = function (item) {
                item.active = !item.active;
            };

            $scope.items = [
                {
                    country: "INDIA",
                    states: [
                        { state: "Assam" },
                        { state: "Chhattisgarh" },
                        { state: "Sikkim" },
                        { state: "Maharastra" },
                        { state: "Madhya Pradesh" }
                    ]
                },
                {
                    country: "UNITED STATES",
                    states: [
                        { state: "Alabama" },
                        { state: "Georgia" },
                        { state: "California" },
                        { state: "Maine" }
                    ]
                }
            ];
        },
        templateUrl: '../../Templates/TreeView.html'
    };
})

