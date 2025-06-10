app.factory('AuthInterceptor', function($injector){
    return{
        request: function(config){
            const AuthService = $injector.get('AuthService');
            var token = AuthService.getToken();
            if(token){
                config.headers.Authorization = 'Bearer ' + token
            }
            return config;
        }
    }
})