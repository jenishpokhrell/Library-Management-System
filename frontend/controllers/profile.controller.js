app.controller('ProfileController', function($scope, $location, AuthService, LoanService, UserService){
    $scope.message = "This is profile page."
    $scope.user = {}
    $scope.myLoanDetails = {}
    $scope.users = {}

    $scope.email = ''
    $scope.searchedUser = {}

    const myDetails = AuthService.getMyDetails()
    const myLoanDetails = LoanService.getMyLoanDetails();
    const loanDetails = LoanService.getLoanDetails()
    const users = UserService.getUsers();
    //const user = UserService.getUserByEmail()

    $scope.isLoggedIn = function(){
        return AuthService.isLoggedIn()
    }

    if(myDetails){
        AuthService.getMyDetails().then(function(response){
            $scope.user = response.data.users
        }, function(error){
            console.log('Error fetching data ', error)
        })
    }else{
        console.error('Token Not Found')
    }

    if(myLoanDetails){
        LoanService.getMyLoanDetails().then(function(response){
            $scope.myLoanDetails = response.data
        }, function(error){
            console.log('Error fetching loan details', error)
        })
    }

    if(loanDetails){
        LoanService.getLoanDetails().then(function(response){
            $scope.loanDetails = response.data
        }, function(error){
            console.log('Error fetching loan details', error)
        })
    }

    if(users){
        UserService.getUsers().then(function(response){
            $scope.users = response.data
        }, function(error){
            console.error('Error fetching user data ', error)
        })
    }

    $scope.searchUser = function(){
        UserService.getUserByEmail($scope.email).then(function(response){
            $scope.searchedUser = response.data
        })
    }
 
    $scope.logout = function(){
        AuthService.logout();
        $location.path('/login')
    }
})