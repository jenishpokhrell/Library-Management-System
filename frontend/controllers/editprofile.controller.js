app.controller('EditProfileController', function($scope, $routeParams, $location, UserService){
    $scope.formData = {}

    const userId = $routeParams.userId;

    function getUser(userId){
        UserService.getUserById(userId).then(function(response){
            $scope.formData = response.data
        }).catch(function(error){
            console.error('Error loading user: ', error)
        })
    }

    if(userId){
        getUser(userId)
    }

    $scope.submitForm = function(){
        UserService.editUser($scope.formData, userId).then(function(){
            $location.path('/profile')
        })

    }
})