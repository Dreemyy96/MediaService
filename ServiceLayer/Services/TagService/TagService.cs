using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Exception;
using Common.Models.Media;
using Media.Persistence;
using MediaCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.Services.TagService;

public class TagService : ITagService
{
    private readonly MediaContext _mediaContext;

    public TagService(MediaContext mediaContext)
    {
        _mediaContext = mediaContext;
    }

    public async Task<List<Guid>> GetOrCreateAsync(List<string> tagsNames, CancellationToken cancellationToken)
    {
        if (tagsNames == null)
            return [];

        var normalizedTags = tagsNames
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.Trim().ToLower())
            .Distinct()
            .ToList();

        var existingTags = await _mediaContext.Tags.Where(t => normalizedTags.Contains(t.Name))
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        var existingNames = existingTags.Select(t => t.Name).ToHashSet();

        var newTags = normalizedTags
            .Where(n => !existingNames.Contains(n))
            .Select(n => new Tag() { Id = Guid.NewGuid(), Name = n })
            .ToList();

        if (newTags.Any())
            await _mediaContext.Tags.AddRangeAsync(newTags, cancellationToken).ConfigureAwait(false);

        return existingTags.Concat(newTags).Select(t => t.Id).ToList();
    }

    public async Task<List<Guid>> CreateAsync(List<string> tagsNames, CancellationToken cancellationToken)
    {
        var tagsIds = await GetOrCreateAsync(tagsNames, cancellationToken).ConfigureAwait(false);
        return await _mediaContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0
            ? tagsIds
            : throw new DatabaseSaveChangesException("Unable to add tags");
    }

    public async Task<List<TagDto>> GetAllTagsAsync(CancellationToken cancellationToken) => await _mediaContext.Tags
        .Select(t => new TagDto() { Name = t.Name }).ToListAsync(cancellationToken);

    public async Task<Guid> DeleteTagAsync(Guid tagId, CancellationToken cancellationToken)
    {
        var removeTag = await _mediaContext.Tags.Where(t => t.Id == tagId).FirstOrDefaultAsync(cancellationToken) ??
                        throw new NotFoundException<Tag>("Tag not found");

        _mediaContext.Tags.Remove(removeTag);
        return await _mediaContext.SaveChangesAsync(cancellationToken) > 0
            ? tagId
            : throw new DatabaseSaveChangesException("Unable to remove tag");
    }
}