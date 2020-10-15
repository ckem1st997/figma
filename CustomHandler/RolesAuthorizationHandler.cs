using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using figma.Data;
using figma.Models;

namespace figma.CustomHandler
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
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
            if (requirement.AllowedRoles == null ||
                requirement.AllowedRoles.Any() == false)
            {

                validRole = true;
            }
            else
            {
                var claims = context.User.Claims;
                var userName = claims.FirstOrDefault(c => c.Type == "UserName").Value;
                var userRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                var roles = requirement.AllowedRoles;
                validRole = _context.Members.ToList().Where(p => roles.Contains(p.Role) && p.Email == userName).Any();

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
