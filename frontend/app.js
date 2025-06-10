var app = angular.module('LibraryManagementSystem', ['ngRoute'])

app.config(function($routeProvider, $httpProvider){
    $routeProvider
    .when('/', {
        templateUrl: 'views/home.html',
        controller: 'HomeController'
    })
    .when('/books', {
        templateUrl: 'views/books.html',
        controller: 'BooksController',
        requiresAuth: true
    })
    .when('/about-us', {
        templateUrl: 'views/about-us.html',
        controller: 'AboutUsController'
    })
    .when('/contact-us', {
        templateUrl: 'views/contact.html',
        controller: 'ContactController',
        requiresAuth: true
    })
    .when('/conversation', {
        templateUrl: 'views/conversation.html',
        controller: 'ConversationController',
        requiresAuth: true
    })
    .when('/login', {
        templateUrl: 'views/login.html',
        controller: 'LoginController'
    })
    .when('/signup', {
        templateUrl: 'views/signup.html',
        controller: 'SignupController'
    })
    .when('/profile', {
        templateUrl: 'views/profile.html',
        controller: 'ProfileController',
        requiresAuth: true
    })
    .when('/editprofile/:userId', {
        templateUrl: 'views/editprofile.html',
        controller: 'EditProfileController',
        requiresAuth: true
    })
    .when('/members', {
        templateUrl: 'views/members.html',
        controller: 'ProfileController',
        requiresAuth: true,
        resolve: {
            auth: function($q, $location, AuthService){
                return AuthService.getMyDetails().then(function(response){
                    if(response.data.users.role === "Admin"){
                        return true;
                    } else {
                        $location.path('/unauthorized');
                        return $q.reject('Not Authorized');
                    }
                })
                
            }
        }
    })
    .when('/member/:userId', {
        templateUrl: 'views/member.html',
        controller: 'MemberController',
        requiresAuth: true
    })
    .when('/addbook', {
        templateUrl: 'views/addbook.html',
        controller: 'AddBookController',
        requiresAuth: true,
        resolve: {
            auth: function($q, $location, AuthService){
                return AuthService.getMyDetails().then(function(response){
                    if(response.data.users.role === "Admin"){
                        return true;
                    } else {
                        $location.path('/unauthorized');
                        return $q.reject('Not Authorized');
                    }
                })
                
            }
        }
    })
    // .when('/admin', {
    //     templateUrl: 'views/adminpanel.html',
    //     controller: 'AdminController',
    //     resolve: {
    //         auth: function($q, $location, AuthService){
    //             const role = AuthService.getRole()
    //             if(role == "Admin"){
    //                 return true
    //             }else{
    //                 $location.path('/unauthorized');
    //                 return $q.reject('Not Authorized')
    //             }
    //         }
    //     }
    // })

    .when('/unauthorized', {
        templateUrl: 'views/unauthorized.html'
    })
    .otherwise({
        redirectTo: '/login'
    })

    $httpProvider.interceptors.push('AuthInterceptor')
})

app.run(function($rootScope, $location, AuthService){
    $rootScope.$on('$routeChangeStart', function(event, next){
        if(next.requiresAuth && !AuthService.isLoggedIn()){
            event.preventDefault();
            $location.path('/login')
        }
    })
})