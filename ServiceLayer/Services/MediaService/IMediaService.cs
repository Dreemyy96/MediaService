using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Models.Media;

namespace ServiceLayer.Services.MediaService;

public interface IMediaService
{
    public Task<ContentItemDto> GetPublishedMediaByIdAsync(Guid mediaId, Guid? userId,
        CancellationToken cancellationToken);
}