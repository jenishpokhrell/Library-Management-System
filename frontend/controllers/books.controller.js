app.controller('BooksController', function($scope, BookService, AuthService){
    $scope.message = "This is books page."

    $scope.books = {}

    $scope.user = {}

    function loadBooks(){
        BookService.getBooks().then(function(response){
            $scope.books = response.data
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

    
})

