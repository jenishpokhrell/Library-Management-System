app.factory('LoanService', function($http){
    var baseUrl = 'https://localhost:44325/api/Loan'

    return{
        getMyLoanDetails: function(){
            return $http.get(`${baseUrl}/getmyloandetails`)
        },
        getLoanDetails: function(){
            return $http.get(`${baseUrl}/getloandetails`)
        },
        getLoanDetailsByUserId: function(userId){
            return $http.get(`${baseUrl}/GetLoanDetailsByuserId/${userId}`)
        },
    }
})