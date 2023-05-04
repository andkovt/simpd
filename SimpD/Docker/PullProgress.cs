using Docker.DotNet.Models;

namespace SimpD.Docker;

public class PullProgress : IProgress<JSONMessage>
{
    public string? LatestError { get; private set; } = null;
    
    public void Report(JSONMessage value)
    {
        if (!string.IsNullOrEmpty(value.ErrorMessage)) {
            LatestError = value.ErrorMessage;
        }
    }
}
