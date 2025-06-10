app.controller('AddBookController', function($scope, $location, BookService){
    $scope.heading = "Add Book"

    $scope.book = {}
    $scope.genres = []

    $scope.selectedGenres = []

    BookService.getGenres().then(function(response){
        $scope.genres = response.data
        console.log($scope.genres)
    })

    $scope.submitBook = function(){
        BookService.addBook($scope.book).then(function(response){
            alert(response.data.message)
            $location.path('/books')
        }, function(error){
            console.error("Error adding books: ", error)
        })
    }

})