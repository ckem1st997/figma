using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using figma.Data;

namespace figma.CustomHandler
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>
    {
        public readonly ShopProductContext _context;
        public RolesAuthorizationHandler(ShopProductContext context)
        {
            _context = context;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RolesAuthorizationRequirement requirement)
        {

            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var validRole = false;
            if (requirement.AllowedRoles == null || !requirement.AllowedRoles.Any())
            {

                validRole = true;
            }
            else
            {
                var claims = context.User.Claims;
                var userName = claims.FirstOrDefault(c => c.Type == "UserName").Value;
                var roles = requirement.AllowedRoles;
                validRole = _context.Members.AsEnumerable().Any(p => roles.Contains(p.Role) && p.Email == userName);
            }

            if (validRole)
            {
                context.Succeed(requirement);

            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
