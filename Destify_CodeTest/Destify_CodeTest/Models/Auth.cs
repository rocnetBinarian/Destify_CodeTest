using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Serilog;

namespace Destify_CodeTest.Models
{
    public class DefaultAuthHandler : IAuthenticationHandler
    {
        public Task<AuthenticateResult> AuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            return Task.CompletedTask;
        }
    }

    public class APIAuthHandler : IAuthenticationHandler
    {
        private readonly Serilog.ILogger logger;
        private AuthenticationScheme _scheme { get; set; }
        private HttpContext _context { get; set; }
        private bool BadAuthNoCreds { get; set; }

        public APIAuthHandler()
        {
            logger = Log.ForContext<APIAuthHandler>();
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            logger.Debug("Beginning Authentication");
            AuthenticateResult rtn;

            string key = _context.Request.Headers["X-AUTH-KEY"].ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(key))
            {
                logger.Debug("Auth failed; key not provided");
                BadAuthNoCreds = true;
                // per spec, there is a lack of a result in this case.  NoResult indicates as such.
                rtn = AuthenticateResult.NoResult();
            }
            else if (key != Globals.Config.API_SECRET)
            {
                logger.Debug("Invalid credentials provided");
                BadAuthNoCreds = false;
                rtn = AuthenticateResult.Fail("Invalid Credentials");
            }
            else if (key == Globals.Config.API_SECRET) // manually specifiying in case there's some other scenario we missed.
            {
                // Create a generic principal.  This would be more complicated if we had a user database, etc.
                ClaimsPrincipal p = new ClaimsPrincipal();
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.Actor, "API User"),
                    new Claim(ClaimTypes.Role, "APIWriter")
                };
                ClaimsIdentity ci = new ClaimsIdentity(claims);
                p.AddIdentity(ci);

                AuthenticationTicket ticket = new AuthenticationTicket(p, _scheme.Name);
                rtn = AuthenticateResult.Success(ticket);
            }
            else
            {
                logger.Warning("Unknown authentication state.  Will return Fail.");
                logger.Debug("Provided key was " + key);
                rtn = AuthenticateResult.Fail("Unknown auth state.");
            }
            logger.Information("Authentication Complete: " + rtn.Succeeded);
            return Task.FromResult(rtn);
        }

        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            if (BadAuthNoCreds) {
                logger.Debug("No creds provided, returning 403");
                _context.Response.StatusCode = 403;
            } else {
                logger.Debug("Bad creds provided, returning 401");
                _context.Response.StatusCode = 401;
            }
            return Task.CompletedTask;
        }

        public async Task ForbidAsync(AuthenticationProperties? properties)
        {
            logger.Debug("Not authorized.  Returning 401");
            _context.Response.StatusCode = 401;
            await _context.Response.WriteAsJsonAsync(new { message = "Authorization failed.  Do you have the appropriate role?" });
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            _scheme = scheme;
            _context = context;
            return Task.CompletedTask;
        }
    }
}