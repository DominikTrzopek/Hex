public interface IUDPResponse
{
    public string GetResponseCode();
    public static UDPResponse FromString(string json) => new UDPResponse();
}
