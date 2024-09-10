using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TwoFactorAuthNet;
using TwoFactorAuthNet.Providers.Qr;
using Utils.Responses;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TwoFactorController : ControllerBase
    {
        private readonly ITwoFactorService _twoFactorService;
        public TwoFactorController(ITwoFactorService twoFactorService)
        {
            _twoFactorService = twoFactorService;
        }
        [HttpGet, Route("GetQRCode")]
        public async Task<IActionResult> GetQRCode(string userName)
        {
            var tfa = new TwoFactorAuth("prueba_konecta", 6, 30, Algorithm.SHA256, new ImageChartsQrCodeProvider());
            var secret = tfa.CreateSecret(160);

            await _twoFactorService.SetSecret(userName, secret);

            string imgQR = tfa.GetQrCodeImageAsDataUri(userName, secret);
            string imgHTML = imgQR;
            PetitionResponse res = new PetitionResponse()
            {
                message = "Código Qr",
                result = imgQR,
                success = true
            };
            return Ok(res);
        }

        [HttpGet, Route("ValidateCode")]
        public async Task<bool> ValidateCode(string userName, string code) 
        {
            string secret = await _twoFactorService.GetSecret(userName);
            var tfa = new TwoFactorAuth("prueba_konecta", 6, 30, Algorithm.SHA256);
            return tfa.VerifyCode(secret, code);
        }

        [HttpGet, Route("HaveTFA")]
        public async Task<bool> HaveTFA(string userName)
        {
            return await _twoFactorService.HaveTFA(userName);
        }
    }
}
