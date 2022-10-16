public enum ResponseType {
    SUCCESS = 0,
    BADREQUEST = 1,      // No request type scpecified
    BADARGUMENTS = 2,     // No required arguments in request 
    ENDOFMESSAGE = 3     // End of server list 
};
