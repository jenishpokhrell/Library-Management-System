app.controller('BooksController', function($scope, BookService, LoanService, AuthService){
    $scope.message = "This is books page."

    $scope.searchedBook = {}

    $scope.user = {}

    $scope.title = ''

    $scope.loanId = {}

    function loadBooks(){
        BookService.getBooks().then(function(response){
            $scope.books = response.data
            console.log($scope.books)
        }).catch(function(error){
            console.error('Error fetching users: ', error)
        })
    }

    loadBooks()

    const myDetails = AuthService.getMyDetails()
    
    if(myDetails){
        AuthService.getMyDetails().then(function(response){
            $scope.user = response.data.users
        }, function(error){
            console.log('Error fetching data ', error)
        })
    }else{
        console.error('Token Not Found')
    }

    $scope.search = function(){
        BookService.getBookByTitle($scope.title).then(function(response){
            $scope.searchedBook = response.data
        })
    }

    
    
})

