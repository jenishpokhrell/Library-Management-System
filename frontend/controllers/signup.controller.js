app.controller('SignupController', function($scope, $location, AuthService){
    $scope.user = {}

    $scope.createUser = function(){
        if($scope.createUserForm.$invalid){
            $scope.createUserForm.$setSubmitted();
            return
        }
        AuthService.signup($scope.user).then(function(){
            alert('User Created Successfully')
            $location.path('/login')
        })
    }
})