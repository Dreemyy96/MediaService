using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Models.Media;

namespace ServiceLayer.Services.TagService;

public interface ITagService
{
    public Task<List<Guid>> GetOrCreateAsync(List<string> tagsNames, CancellationToken cancellationToken);
    public Task<List<Guid>> CreateAsync(List<string> tagsNames, CancellationToken cancellationToken);
    public Task<List<TagDto>> GetAllTagsAsync(CancellationToken cancellationToken);
    public Task<Guid> DeleteTagAsync(Guid tagId, CancellationToken cancellationToken);
}