app.controller('ContactController', function($scope, ContactService){
    $scope.formData = {}

    $scope.send = function(){
        ContactService.sendMessage($scope.formData).then(function(){
            alert('Message sent successfully!')
            $scope.resetForm();
        })
    }

    $scope.resetForm = function(){
        $scope.formData = {}
    }
})