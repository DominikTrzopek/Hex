public enum ResponseType {
    SUCCESS = 0,
    ENDOFMESSAGE = 1,       // End of server list 
    BADREQUEST = 2,         // No request type scpecified
    BADARGUMENTS = 3,       // No required arguments in request
    TCPSERVERFAIL = 4,      // TCP process fail to start
    WRONGPASSWORD = 5       // Wrong password
};
