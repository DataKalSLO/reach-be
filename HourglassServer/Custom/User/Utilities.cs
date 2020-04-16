using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HourglassServer
{
    public static class Utilities
    {
        public static bool HasRole(this ClaimsPrincipal user, Role role)
        {
            return user.IsInRole(role.ToString());
        }

        public static async Task<int> InsertAsync<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            context.Set<TEntity>().Add(entity);
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