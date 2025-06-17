app.controller('EditBookController', function($scope, $location, $routeParams, BookService){
    $scope.heading = "Edit Book"

    $scope.bookData = {}

    $scope.genres = []

    $scope.selectedGenres = []

    BookService.getGenres().then(function(response){
        $scope.genres = response.data
    })

    const bookId = $routeParams.bookId

    function getBook(bookId){
        BookService.getBookById(bookId).then(function(response){
            $scope.bookData = response.data
            if(typeof $scope.bookData.genres === 'string'){
                $scope.bookData.genres = $scope.bookData.genres.split(',').map(g => g.trim())
            }
            console.log($scope.bookData)
        }).catch(function(error){
            console.error('Error loading user: ', error)
        })
    }

    if(bookId){
        getBook(bookId)
    }

    $scope.editBook = function(){
        BookService.editBookDetails(bookId, $scope.bookData).then(function(response){
            alert(response.data.message)
            $location.path('/books')
        })

    }

    $scope.delete = function(bookId){
        if(confirm('Are you sure you want to delete this book details?')){
            BookService.deleteBook(bookId).then(function(response){
                alert("Book Deleted Successfully.");
                $location.path('/books');
            })
        }
    }
})