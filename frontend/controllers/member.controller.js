app.controller('MemberController', function($scope, $routeParams, UserService, LoanService){
    $scope.user = {}

    const userId = $routeParams.userId

    const loanDetails = LoanService.getLoanDetails()

    function getUser(userId){
        UserService.getUserById(userId).then(function(response){
            $scope.user = response.data
            // console.log($scope.user)
        }).catch(function(error){
            console.error('Error loading user: ', error)
        })
    }

    if(userId){
        getUser(userId)
    }

    if(loanDetails){
        LoanService.getLoanDetailsByUserId(userId).then(function(response){
            $scope.loanDetails = response.data
            console.log($scope.loanDetails)
        }, function(error){
            console.log('Error fetching loan details', error)
        })
    }
})