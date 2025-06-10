app.factory('AuthService', function($window, $http){

    const baseUrl = 'https://localhost:44325/api/Users'

    const auth = {};

    const currentUser = {}

    auth.saveToken = function(token){
        $window.localStorage['jwt-token'] = token
    }

    auth.getToken = function(){
        return $window.localStorage['jwt-token']
    }

    auth.isLoggedIn = function(){
        var token = auth.getToken();

        return !!token
    }

    auth.login = function(user){
        return $http.post(`${baseUrl}/login`, user)
    }

    auth.getMyDetails = function(){
        const token = auth.getToken();
        if(!token) return null;

        return $http.post(`${baseUrl}/me`, { token: token })
    }

    auth.signup = function(user){
        return $http.post(`${baseUrl}/registeruser`, user)
    }

    auth.logout = function(){
        $window.localStorage.removeItem('jwt-token')
    }

    auth.getCurrentUser = function(){
        return currentUser;
    }

    auth.getRole = function(){
        // return currentUser ? currentUser.role : null
        const token = auth.getToken();
        if (!token) return null;

        const payload = JSON.parse(atob(token.split('.')[1]));
        return payload.role;
    }
    
    return auth
})