public interface IUDPResponse
{
    public string getResponseCode();
    public static UDPResponse fromString(string json) => new UDPResponse();
}
