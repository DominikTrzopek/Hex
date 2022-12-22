public enum ResponseType {
    SUCCESS = 0,
    ENDOFMESSAGE = 1,       // End of server list 
    BADREQUEST = 2,         // No request type scpecified
    BADARGUMENTS = 3,       // No required arguments in request
    TCPSERVERFAIL = 4,      // TCP process fail to start
    WRONGPASSWORD = 5,      // Wrong password
    BADPLAYERDATA = 6,      // Missing player information
    BADCONNECTION = 7,      // Broken connect msg json
    DISCONNECT = 8,         // Disconnected from server
    UDPSERVERDOWN = 9,      // Disconnected from server
    BADADDRESS = 10,        // Bad arguments for socket init
    FILENOTFOUND = 11,      // File not found when loading custom map
    MAPSIZETOLARGE = 12     // Map array to large to send
};
