using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HourglassServer
{
    public static class Utilities
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            IList<Claim> userClaimsWithId = user.Claims
                .Where(c => c.Type == ClaimTypes.Email).ToList();
            if (userClaimsWithId.Count < 1)
                throw new PermissionDeniedException();

            Claim userClaim = userClaimsWithId[0];

            if (userClaim == null)
                throw new PermissionDeniedException();

            return userClaim.Value;
        }

        public static bool HasRole(this ClaimsPrincipal user, Role role)
        {
            return user.IsInRole(((int)role).ToString());
        }

        public static async Task<int> InsertAsync<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            context.Set<TEntity>().Add(entity);
            return await context.SaveChangesAsync();
        }

        public static async Task<int> InsertAsync<TEntity>(this DbContext context, TEntity[] entities) where TEntity : class
        {
            context.Set<TEntity>().AddRange(entities);
            return await context.SaveChangesAsync();
        }

        public static async Task<int> UpdateAsync<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            context.Set<TEntity>().Update(entity);
            return await context.SaveChangesAsync();
        }

        public static async Task<int> DeleteAsync<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            context.Set<TEntity>().Remove(entity);
            return await context.SaveChangesAsync();
        }

        public static bool UserHasPermission(ClaimsPrincipal user, int id)
        {
            return user.HasRole(Role.Admin) || user.FindFirstValue(ClaimTypes.PrimarySid) == id.ToString();
        }
    }
}