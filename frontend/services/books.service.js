app.factory('BookService', function($http){
    var baseUrl = 'https://localhost:44325/api/books'

    return{
        getBooks: function(){
            return $http.get(`${baseUrl}/getallbookdetails`)
        },
        getGenres: function(){
            return $http.get('genres.json')
        },
        addBook: function(book){
            return $http.post(`${baseUrl}/addbook`, book)
        },
        getBookByTitle: function(title){
            return $http.get(`${baseUrl}/getbookbytitle/${title}`)
        },

        getBookById: function(bookId){
            return $http.get(`${baseUrl}/getbookbyid/${bookId}`)
        },  

        editBookDetails: function(bookId, book){
            return $http.put(`${baseUrl}/updatebookdetails/${bookId}`, book)
        },
        
        deleteBook: function(bookId){
            return $http.delete(`${baseUrl}/deletebook/${bookId}`)
        }
    }
})