app.factory('UserService', function($http){
    var baseUrl = 'https://localhost:44325/api/Users'

    return{
        getUsers: function(){
            return $http.get(`${baseUrl}/getusers`)
        },

        getUserById: function(userId){
            return $http.get(`${baseUrl}/getusersby/${userId}`)
        },
        
        editUser: function(user, userId){
            return $http.put(`${baseUrl}/updateuser/${userId}`, user).then(response => {
                const newToken = response.headers('X-New-JWT');
                if(newToken){
                    localStorage.setItem('token', newToken);
                }else{
                    alert(response.data.message)
                }
            })
        }
    }
})