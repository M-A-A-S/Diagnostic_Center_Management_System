using Azure.Core;
using BusinessLayer;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController(JwtOptions jwtOptions) : ControllerBase
    {
        [HttpPost("login")]
        //[Route("auth")]
        //[Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login(UserDTO UserDTO)
        {
            if (!clsUser.Login(UserDTO))
            {
                return Unauthorized("Invalid username or password");
            }

            var accessToken = clsUtil.GenerateAccessToken(UserDTO, jwtOptions);
            var refreshToken = clsUtil.GenerateRefreshToken(clsUser.GetUserByUsername(UserDTO.Username).Id);
            clsRefreshToken.SaveRefreshToken(refreshToken);

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("refresh")]
        public IActionResult Refresh(string refreshToken)
        {
            var storedToken = clsRefreshToken.GetRefreshToken(refreshToken);
            if (storedToken == null || storedToken.IsRevoked || storedToken.IsUsed || storedToken.Expires < DateTime.UtcNow)
            {
                return Unauthorized("Invalid refresh token");
            }

            storedToken.IsUsed = true;
            clsRefreshToken.SaveRefreshToken(storedToken);

            var user = clsUser.GetUserById(storedToken.UserId);
            var newAccessToken = clsUtil.GenerateAccessToken(user, jwtOptions);

            return Ok(new { AccessToken = newAccessToken });
        }
    }
}
