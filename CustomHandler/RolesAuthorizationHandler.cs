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

        //     private readonly IHttpContextAccessor _httpContextAccessor;
        public RolesAuthorizationHandler(ShopProductContext context/*, IHttpContextAccessor httpContextAccessor*/)
        {
            _context = context;
            //   _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RolesAuthorizationRequirement requirement)
        {

            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            // requirement.AllowedRoles =(Roles = "Admin"
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
                // Console.WriteLine(11111);
                // Console.WriteLine(_httpContextAccessor.HttpContext.Session.GetString(ClaimTypes.Role));
                // validRole = context1.Users.Where(p => roles.Contains(p.Role) && p.UserName == userName).ToList().Any();
                validRole = _context.Admins.ToList().Where(p => roles.Contains(p.Role) && p.Username == userName).Any();

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
