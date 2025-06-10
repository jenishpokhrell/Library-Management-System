app.controller('LoginController', function($scope, $location, AuthService){
    $scope.user = {}
    $scope.role = null

    $scope.login = function(){
        AuthService.login($scope.user).then(function(response){
            AuthService.saveToken(response.data.token);
            //$scope.role = response.data.users.role
            $location.path('/profile')
        }, function(){
            alert('Login failed');
        })
    }
})