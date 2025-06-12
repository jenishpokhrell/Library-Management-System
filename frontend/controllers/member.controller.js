app.controller('MemberController', function($scope, $routeParams, UserService, LoanService){
    $scope.user = {}

    const userId = $routeParams.userId

    const loanDetails = LoanService.getLoanDetails()

    function getUser(userId){
        UserService.getUserById(userId).then(function(response){
            $scope.user = response.data
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
        }, function(error){
            console.log('Error fetching loan details', error)
        })
    }

    $scope.updateLoanDetail = function(loan){
        const loanId = loan.loanId
        const payload = {
            isReturned: true
        }
        LoanService.updateLoanDetails(payload, loanId).then(function(response){
            alert(response.data.message)
            loadBooks()
        })
    }
})