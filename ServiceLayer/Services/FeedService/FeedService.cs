using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Exception;
using Common.Models.Media;
using Media.Persistence;
using MediaCore.Enums;
using MediaCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ServiceLayer.Services.FeedService;

public class FeedService : IFeedService
{
    private readonly MediaContext _mediaContext;
    private readonly ILogger<FeedService> _logger;

    public FeedService(MediaContext mediaContext, ILogger<FeedService> logger)
    {
        _logger = logger;
        _mediaContext = mediaContext;
    }

    public async Task<List<FeedItemDto>> GetFeedAsync(Guid? userId, CancellationToken cancellationToken)
    {
        var feed = _mediaContext.Medias
            .Where(m => !m.IsDeleted && m.Status == MediaStatus.Published)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new FeedItemDto()
            {
                Id = m.Id,
                AuthorId = m.Id,
                Title = m.Title,
                Description = m.Description,
                CommentsCount = m.CommentsCount,
                LikesCount = m.LikesCount,
                ViewCount = m.ViewCount,
                MediaType = m.MediaType,
                CreatedAt = m.CreatedAt,
                FileUrl = $"api/files/{m.StorageFileId}"
            });

        foreach (var feedItem in feed)
        {
            var media = await _mediaContext.Medias.FindAsync(feedItem.Id, cancellationToken)
                .ConfigureAwait(false);
            if (media is null) throw new NotFoundException<ContentItem>("Media not found");

            var tagIds = media.MediaTags.Select(mt => mt.TagId).ToList();

            feedItem.Tags = await _mediaContext.Tags
                .Where(t => tagIds.Contains(t.Id))
                .Select(t => t.Name)
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            if (userId is null) continue;

            if (await _mediaContext.UserMediaLike
                    .AnyAsync(uml => uml.MediaId == media.Id && uml.UserId == userId, cancellationToken)
                    .ConfigureAwait(false)) feedItem.IsLikedByCurrentUser = true;

            if (await _mediaContext.UserSavedMedia
                    .AnyAsync(usm => usm.MediaId == media.Id && usm.UserId == userId, cancellationToken)
                    .ConfigureAwait(false)) feedItem.IsSavedByCurrentUser = true;
        }

        return await feed.ToListAsync(cancellationToken).ConfigureAwait(false);
    }
}