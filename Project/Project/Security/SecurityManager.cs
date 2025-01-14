using Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Project.Security
{
    public class SecurityManager
    {
        private IEnumerable<Claim> GetUserClaims(Account account)
        {
            //đối tượng Claim sẽ lưu thông tin User qua các quyền (account - role)
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name,account.Username));

            //add role tương ứng với user này
            claims.Add(new Claim(ClaimTypes.Role, account.Role.Name));
            return claims;
        }
        public async Task SignIn(HttpContext context,Account account)
        {
            //cho phép dùng cookie để đăng nhập
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(GetUserClaims(account),CookieAuthenticationDefaults.AuthenticationScheme);
            //quản lí thông tin user thông qua định danh claimsIdentity,không lưu password
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            await context.SignInAsync
                (CookieAuthenticationDefaults.AuthenticationScheme,principal);
        }
        public async Task SignOut(HttpContext context)
        {
            await context.SignOutAsync();
        }
    }
}
