using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Exception;
using Common.Models.FileDTO;
using Common.Models.Media;
using Common.Models.ResponseModels;
using Media.Persistence;
using MediaCore.Enums;
using MediaCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLayer.Services.MediaManagmentService;
using ServiceLayer.Services.TagService;


namespace ServiceLayer.Services.MediaService;

public class MediaService : IMediaService
{
    private readonly MediaContext _mediaContext;
    private readonly ILogger<MediaService> _logger;
    private readonly IMediaManagementService _mediaManagementService;
    private readonly ITagService _tagService;

    public MediaService(MediaContext mediaContext, ILogger<MediaService> logger,
        IMediaManagementService mediaManagementService, ITagService tagService)
    {
        _mediaContext = mediaContext;
        _logger = logger;
        _mediaManagementService = mediaManagementService;
        _tagService = tagService;
    }

    public async Task<Guid> CreateAsync(CreateMediaDTO mediaDTO, Guid userId, CancellationToken cancellationToken)
    {
        await using var fileDto = new FileDTO(mediaDTO.File.FileName, mediaDTO.File.ContentType,
            mediaDTO.File.OpenReadStream());
        var fileMetaInfo =
            await _mediaManagementService.UploadFileAsync(fileDto, cancellationToken).ConfigureAwait(false);
        if (string.Equals(fileMetaInfo.State, "FAILED", StringComparison.OrdinalIgnoreCase))
            throw new FIleNotUploadException("Failed to upload file");

        var tagIds = await _tagService.GetOrCreateAsync(mediaDTO.Tags, cancellationToken).ConfigureAwait(false);

        var media = new ContentItem()
        {
            Id = Guid.NewGuid(),
            AuthorId = userId,
            StorageFileId = fileMetaInfo.Id,
            Title = mediaDTO.Title,
            Description = mediaDTO.Description,
            MediaType = ContentItem.ResolveMediaType(fileMetaInfo.MimeType),
            Status = MediaStatus.Published,
            Size = mediaDTO.File.Length,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        media.MediaTags = tagIds.Select(t => new MediaTag
        {
            MediaId = media.Id,
            TagId = t
        }).ToList();

        await _mediaContext.Medias.AddAsync(media, cancellationToken).ConfigureAwait(false);

        return await _mediaContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0
            ? media.Id
            : throw new DatabaseSaveChangesException("Unable to save media");
    }

    public async Task<bool> UpdateAsync(UpdateMediaDTO mediaDTO, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(mediaDTO);

        var media = await _mediaContext.Medias
            .Where(m => m.Id == mediaDTO.MediaId && !m.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (media == null) throw new NotFoundException<ContentItem>("Media not found");

        if (mediaDTO.UserId != media.AuthorId) return false;

        if (string.IsNullOrWhiteSpace(mediaDTO.Title)) media.Title = mediaDTO.Title;
        if (string.IsNullOrWhiteSpace(mediaDTO.Description)) media.Description = mediaDTO.Description;

        if (mediaDTO.Tags != null)
        {
            var newTagIds = await _tagService.GetOrCreateAsync(mediaDTO.Tags, cancellationToken)
                .ConfigureAwait(false);
            var currentTagIds = media.MediaTags.Select(mt => mt.TagId).ToList();

            var tagsToAdd = newTagIds.Except(currentTagIds).ToList();
            var tagsToRemove = currentTagIds.Except(newTagIds).ToList();

            foreach (var tagId in tagsToAdd)
            {
                media.MediaTags.Add(new MediaTag
                {
                    MediaId = media.Id,
                    TagId = tagId
                });
            }

            media.MediaTags = media.MediaTags.Where(mt => !tagsToRemove.Contains(mt.TagId)).ToList();
        }

        return await _mediaContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }

    public async Task<MediaMetaInfoResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var media = await _mediaContext.Medias.FindAsync(id, cancellationToken)
            .ConfigureAwait(false);
        if (media == null) throw new NotFoundException<ContentItem>("Media not found");

        var response = new MediaMetaInfoResponse
        {
            Id = media.Id,
            AuthorId = media.AuthorId,
            Title = media.Title,
            Description = media.Description,
            MediaType = media.MediaType,
            Status = media.Status,
            ViewCount = media.ViewCount,
            LikesCount = media.LikesCount,
            CommentsCount = media.CommentsCount,
            CreatedAt = media.CreatedAt
        };

        var tagIds = media.MediaTags.Select(mt => mt.TagId).ToList();
        response.MediaTags = await _mediaContext.Tags
            .Where(t => tagIds.Contains(t.Id))
            .Select(t => t.Name)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        return response;
    }

    public async Task<bool> HideMediaAsync(Guid id, CancellationToken cancellationToken)
    {
        var media = await _mediaContext.Medias.FindAsync(id, cancellationToken)
            .ConfigureAwait(false);
        if(media == null) throw new NotFoundException<ContentItem>("Media not found");
        
        media.Status = MediaStatus.Hidden;
        return await _mediaContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }
    
    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var media = await _mediaContext.Medias.FindAsync(id, cancellationToken)
            .ConfigureAwait(false);
        if(media == null) throw new NotFoundException<ContentItem>("Media not found");

        media.Status = MediaStatus.Deleted;
        media.IsDeleted = true;
        
        return await _mediaContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }
}