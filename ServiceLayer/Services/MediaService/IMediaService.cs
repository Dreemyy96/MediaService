using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Models.Media;
using Common.Models.ResponseModels;

namespace ServiceLayer.Services.MediaService;

public interface IMediaService
{
    public Task<Guid> CreateAsync(CreateMediaDTO mediaDTO, Guid userId, CancellationToken cancellationToken);
    public Task<bool> UpdateAsync(UpdateMediaDTO mediaDTO, CancellationToken cancellationToken);
    public Task<MediaMetaInfoResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<bool> HideMediaAsync(Guid id, CancellationToken cancellationToken);
    public Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
}