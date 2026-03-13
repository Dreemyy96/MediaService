using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Exception;
using Common.Models.Media;
using Identity.Persistence;
using IdentityCore.Enums;
using IdentityCore.Models;
using Media.Persistence;
using MediaCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.Services.CommentService;

public class CommentService : ICommentService
{
    private readonly MediaContext _mediaContext;
    private readonly IdentityContext _identityContext;

    public CommentService(MediaContext mediaContext, IdentityContext identityContext)
    {
        _mediaContext = mediaContext;
        _identityContext = identityContext;
    }

    public async Task<List<CommentDto>> GetCommentsAsync(Guid mediaId, CancellationToken cancellationToken)
    {
        var comments = await _mediaContext.Comments.AsNoTracking()
            .Where(c => c.MediaId == mediaId && !c.IsDeleted)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentDto()
            {
                Id = c.Id,
                CreatedAt = c.CreatedAt,
                MediaId = c.MediaId,
                Text = c.Text,
                UserName = _identityContext.Users.Where(u => u.Id == c.UserId).Select(u => u.Name).FirstOrDefault()
            }).ToListAsync(cancellationToken).ConfigureAwait(false);

        return comments;
    }

    public async Task<Guid> AddCommentAsync(CreateCommentDto createCommentDto, CancellationToken cancellationToken)
    {
        var comment = new Comment()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
            MediaId = createCommentDto.MediaId,
            UserId = createCommentDto.UserId,
            Text = createCommentDto.Text
        };

        if (!await _mediaContext.Medias.AnyAsync(m => m.Id == comment.MediaId && !m.IsDeleted))
            throw new NotFoundException<ContentItem>("Media not found");

        await _mediaContext.Comments.AddAsync(comment, cancellationToken);
        return await _mediaContext.SaveChangesAsync(cancellationToken) > 0
            ? comment.Id
            : throw new DatabaseSaveChangesException("Unable to save comment");
    }

    public async Task<bool> UpdateCommentTextAsync(UpdateCommentTextDto modelDto, CancellationToken cancellationToken)
    {
        var comment = await _mediaContext.Comments
            .Where(c => c.Id == modelDto.CommentId && c.UserId == modelDto.UserId && !c.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        
        comment.Text = modelDto.Text;
        
        return await _mediaContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteCommentAsync(Guid commentId, Guid userId, CancellationToken cancellationToken)
    {
        var comment = await _mediaContext.Comments.Where(c => c.Id == commentId).FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false) ?? throw new NotFoundException<Comment>("Comment not found");
        var isAdmin = await _identityContext.Users.Where(u => u.Id == userId && !u.IsDeleted).Select(u => u.Role)
            .FirstOrDefaultAsync(cancellationToken) == Role.Admin;

        if (comment.UserId != userId && !isAdmin)
            return false;

        comment.IsDeleted = true;
        return await _mediaContext.SaveChangesAsync(cancellationToken) > 0;
    }
}