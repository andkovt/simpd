namespace SimpD.Exceptions;

public class ImagePullException : System.Exception
{
    public ImagePullException(string latestErrorMessage) : base(latestErrorMessage)
    {
    }
}
