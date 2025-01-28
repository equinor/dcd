using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.Videos.Get;

public class GetVideoController(GetVideoService getVideoService) : ControllerBase
{
    [HttpGet("videos/{videoName}")]
    [Authorize]
    public async Task<VideoDto> GetVideo(string videoName)
    {
        return await getVideoService.GetVideoStream(videoName);
    }
}
