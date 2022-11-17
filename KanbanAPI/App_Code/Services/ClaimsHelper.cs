using System;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using KanbanAPI.App_Code.Models;


    public static class ClaimsHelper
    {
        public static class ClaimTypes
        {
            public const string JWTID = "jti";
            public const string Department = "Department";
            public const string DepartmentID = "DepartmentID";
            public const string WorkingStatus = "WorkingStatus";
            public const string SubDepartment = "SubDepartment";
        }
        public static int GetUserIdentifier(ClaimsPrincipal user)
        {
            return Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        }
        public static Guid GetUserTokenID(ClaimsPrincipal user)
        {
            return new Guid(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.JWTID).Value);
        }
        public static IEnumerable<string> GetUserRoles(ClaimsPrincipal user)
        {
            return user.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value);
        }
        public static bool UserInRole(ClaimsPrincipal user, string role)
        {
            return user.Claims.Any(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == role);
        }
        public static bool IsUsername(string username)
        {
            string pattern;
            // start with a letter, allow letter or number, length between 3 to 256.
            pattern = "^[a-zA-Z][a-zA-Z0-9]{2,256}";

            Regex regex = new Regex(pattern);
            return regex.IsMatch(username);
        }
        #region ClaimsPrincipal extended properties
        public static Guid GetID(this ClaimsPrincipal user)
        {
            return new Guid(user.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        }
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.Identity.Name;
        }
        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email).Value;
        }
        public static string GetPersonName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.GivenName).Value;
        }
        public static string GetDepartment(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == ClaimsHelper.ClaimTypes.Department).Value;
        }
        public static int GetDepartmentID(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == ClaimsHelper.ClaimTypes.DepartmentID).Value);
        }
        public static string GetSubDepartment(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == ClaimsHelper.ClaimTypes.SubDepartment).Value;
        }
        public static int GetStatus(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == ClaimsHelper.ClaimTypes.WorkingStatus).Value);
        }
        public static User ToIdentityUser(this ClaimsPrincipal user)
        {
            User iuser = new User();
            foreach (Claim claim in user.Claims)
            {
                iuser.UserName = user.Identity.Name;
                if (claim.Type == System.Security.Claims.ClaimTypes.NameIdentifier) iuser.ID = Convert.ToInt32(claim.Value);
                if (claim.Type == System.Security.Claims.ClaimTypes.Email) iuser.Email = claim.Value;
                if (claim.Type == System.Security.Claims.ClaimTypes.GivenName) iuser.FirstName = claim.Value;
                if (claim.Type == System.Security.Claims.ClaimTypes.Surname) iuser.FirstName = claim.Value;

            }
            return iuser;
        }
        #endregion
    }
