app.factory('ContactService', function($http){
    var baseUrl = 'https://localhost:44325/api/Conversation'

    return{
        sendMessage: function(message){
            return $http.post(`${baseUrl}/createconversation`, message)
        },

        getConversation: function(conversationId){
            return $http.get(`${baseUrl}/${conversationId}`)
        }
    }
})