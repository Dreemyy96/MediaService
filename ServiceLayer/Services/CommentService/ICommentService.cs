using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Models.Media;

namespace ServiceLayer.Services.CommentService;

public interface ICommentService
{
    public Task<List<CommentDto>> GetCommentsAsync(Guid mediaId, CancellationToken cancellationToken);
    public Task<Guid> AddCommentAsync(CreateCommentDto createCommentDto, CancellationToken cancellationToken);
    public Task<bool> UpdateCommentTextAsync(UpdateCommentTextDto modelDto, CancellationToken cancellationToken);
    public Task<bool> DeleteCommentAsync(Guid commentId, Guid userId, CancellationToken cancellationToken);
}