using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Exception;
using Common.Models.Media;
using Media.Persistence;
using MediaCore.Enums;
using MediaCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.Services.MediaService;

public class MediaService
{
    private readonly MediaContext _mediaContext;

    public MediaService(MediaContext mediaContext)
    {
        _mediaContext = mediaContext;
    }

    public async Task<ContentItemDto> GetPublishedMediaByIdAsync(Guid mediaId, Guid? userId,
        CancellationToken cancellationToken)
    {
        var media = await _mediaContext.Medias
            .Where(m => m.Id == mediaId && m.Status == MediaStatus.Published && !m.IsDeleted)
            .Select(m => new ContentItemDto()
            {
                Id = m.Id,
                AuthorId = m.AuthorId,
                Title = m.Title,
                Description = m.Description,
                CommentsCount = m.CommentsCount,
                LikesCount = m.LikesCount,
                CreatedAt = m.CreatedAt,
                MediaType = m.MediaType,
                Size = m.Size,
                ViewCount = m.ViewCount,
                IsLikedByCurrentUser = userId != null &&
                                       _mediaContext.UserMediaLike.Any(uml =>
                                           uml.MediaId == mediaId && uml.UserId == userId),
                IsSavedByCurrentUser = userId != null &&
                                       _mediaContext.UserSavedMedia.Any(usm =>
                                           usm.UserId == userId && usm.MediaId == mediaId),
                Tags = m.MediaTags.Select(mt => mt.Tag.Name).ToList()
            }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (media == null)
            throw new NotFoundException<ContentItem>("Media not found");

        return media;
    }
}