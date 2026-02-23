using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Models.Identity;
using IdentityCore.Models;

namespace ServiceLayer.Services.IdentityService;

public interface IIdentityService
{
    public Task<string> CreateJwtTokenAsync(string email, CancellationToken cancellationToken);
    public Task<bool> IsCredentialsValidAsync(AuthModel authModel, CancellationToken cancellationToken);
    public Task<Guid> RegisterUserAsync(RegisterUserDto userDto, CancellationToken cancellationToken);
    public Task<User> GetUserAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<User>> GetUsersAsync(CancellationToken cancellationToken);
    public Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken);
    public Task<bool> UpdateEmailAsync(Guid id, string email, CancellationToken cancellationToken);

}