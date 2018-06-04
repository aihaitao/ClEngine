namespace ClEngine.CoreLibrary.IO
{
    public class FtpManager
    {
        public static bool IsFtp(string url)
        {
            return url.StartsWith("ftp://");
        }
    }
}