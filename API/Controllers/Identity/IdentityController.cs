using System.Threading;
using System.Threading.Tasks;
using Common.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer.Services.IdentityService;

namespace API.Controllers.Identity;

[ApiController]
[Route("/api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(IIdentityService identityService, ILogger<IdentityController> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> LoginAsync(AuthModel model, CancellationToken cancellationToken)
    {
        _logger.LogWarning($"Try to login {model.Email}");
        if (await _identityService.IsCredentialsValidAsync(model, cancellationToken).ConfigureAwait(false))
        {
            return Ok(await _identityService.CreateJwtTokenAsync(model.Email, cancellationToken).ConfigureAwait(false));
        }

        _logger.LogError($"Login attempt with invalid creds for user {model.Email}");
        return Unauthorized("Invalid credentials");
    }
    
    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync(RegisterUserDto model, CancellationToken cancellationToken) =>
        Ok(await _identityService.RegisterUserAsync(model, cancellationToken).ConfigureAwait(false));
}