﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
                throw new PermissionDeniedException("No email claim type found for user.", "badValue");

            Claim userClaim = userClaimsWithId[0];

            if (userClaim == null)
                throw new PermissionDeniedException("User claim is null.", "badValue");

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

        public static (string salt, string hash) HashPassword(string password, int saltLength = 16)
        {
            using var rngCsp = new RNGCryptoServiceProvider();
            byte[] salt = new byte[saltLength];
            rngCsp.GetBytes(salt);
            string hash = Hash(password, salt);
            string saltString = Convert.ToBase64String(salt);
            return (saltString, hash);
        }

        public static bool PasswordMatches(string enteredPassword, string saltString, string realHash)
        {
            byte[] salt = Convert.FromBase64String(saltString);
            string hash = Hash(enteredPassword, salt);
            return hash == realHash;
        }

        private static string Hash(string password, byte[] salt, int hashLength = 128, int iterations = 10000)
        {
            int hashByteCount = hashLength / 8;
            byte[] bytes = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, iterations, hashByteCount);
            return Convert.ToBase64String(bytes);
        }
    }
}
