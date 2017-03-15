using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Models;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace HIS.WebApi.Auth
{
    public class IdentityWithAdditionalClaimsProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;

        public IdentityWithAdditionalClaimsProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();

            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));
            claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email ?? user.UserName));

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any(x => x.Equals("Administrator") || x.Equals("Owner")))
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "admin"));
            }
            else
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "user"));
            }

            /*
             Add additional Claims like 
             if (user.DataEventRecordsRole == "dataEventRecords.admin")
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "dataEventRecords.admin"));
                claims.Add(new Claim(JwtClaimTypes.Role, "dataEventRecords.user"));
                claims.Add(new Claim(JwtClaimTypes.Role, "dataEventRecords"));
                claims.Add(new Claim(JwtClaimTypes.Scope, "dataEventRecords"));
            }
            else
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "dataEventRecords.user"));
                claims.Add(new Claim(JwtClaimTypes.Role, "dataEventRecords"));
                claims.Add(new Claim(JwtClaimTypes.Scope, "dataEventRecords"));
            }
             
             */

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
