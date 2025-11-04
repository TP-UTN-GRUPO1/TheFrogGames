using Contracts.Platform.Request;
using Contracts.Platform.Response;

namespace Application.Service
{
    public interface IPlatformService
    {
        List<PlatformResponse> GetPlatform();
        PlatformResponse CreatePlatform(CreatePlatformRequest request);
        PlatformResponse UpdatePlatform(UpdatePlatformRequest request);
        bool DeletePlatform(int id);
    }
}
