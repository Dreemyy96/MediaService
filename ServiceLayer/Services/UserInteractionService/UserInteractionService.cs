using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Exception;
using Media.Persistence;
using MediaCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ServiceLayer.Services.UserInteractionService;

public class UserInteractionService
{
    private readonly MediaContext _mediaContext;
    private readonly ILogger<UserInteractionService> _logger;

    public UserInteractionService(MediaContext mediaContext, ILogger<UserInteractionService> logger)
    {
        _mediaContext = mediaContext;
        _logger = logger;
    }

    public async Task<bool> ToggleLikeAsync(Guid mediaId, Guid userId, CancellationToken cancellationToken)
    {
        var media = await _mediaContext.Medias.FindAsync(mediaId, cancellationToken)
            .ConfigureAwait(false);
        if (media is null) throw new NotFoundException<ContentItem>("Media not found");
        
        if (await _mediaContext.UserMediaLike
                .AnyAsync(uml => uml.MediaId == mediaId && uml.UserId == userId, cancellationToken)
                .ConfigureAwait(false))
        {
            var userMediaLike = await _mediaContext.UserMediaLike
                .Where(uml => uml.MediaId == mediaId && uml.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            
            media.LikesCount--;
            _mediaContext.UserMediaLike.Remove(userMediaLike);

            return await _mediaContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
        }

        var newUserMediaLike = new UserMediaLike
        {
            Id = Guid.NewGuid(),
            MediaId = mediaId,
            UserId = userId,
            CreatedAt = DateTime.Now
        };
        await _mediaContext.UserMediaLike.AddAsync(newUserMediaLike, cancellationToken).ConfigureAwait(false);
        media.LikesCount++;
        
        return await _mediaContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }
    
    
}