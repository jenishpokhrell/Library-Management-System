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

    function loadLoanDetails(){
        if(loanDetails){
            LoanService.getLoanDetailsByUserId(userId).then(function(response){
                $scope.loanDetails = response.data
            }, function(error){
                console.log('Error fetching loan details', error)
            })
        }
    }

    loadLoanDetails()

    $scope.updateLoanDetail = function(loan){
        const loanId = loan.loanId
        const payload = {
            isReturned: true
        }
        LoanService.updateLoanDetails(loanId, payload).then(function(response){
            alert("Loan Details updated successfully.")
            loadLoanDetails()
        })
    }
})