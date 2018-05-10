
app.controller("ProtocolCtrl", ['$scope', function ($scope, $http) {
    $scope.selectedProtocol = null;
    $scope.count = 0;
    $http({
        method: 'GET',
        url: 'http://localhost:49648/api/protocols'
         }).success(function (data) {
                 $scope.protocols = data;});

    $scope.count = 0;
    //$scope.myFunc = function () {
    //    $scope.count++;
    //    alert('hello');
    //};
}]);


    
