using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APICRYPTO.Models;
namespace APICRYPTO.Controllers
{
    [Route("prices")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public PricesController(IHttpClientFactory fabricaHttpClient)
        {
            _httpClient = fabricaHttpClient.CreateClient();
        }


        [HttpGet("{cryptoCode}")]
        public async Task<IActionResult> ObtenerPrecio(string cryptoCode)
        {
            try
            {
                var url = $"https://criptoya.com/api/binance/{cryptoCode}/EUR/1";
                var respuesta = await _httpClient.GetFromJsonAsync<CriptoyaResponse>(url);
                if (respuesta == null) return StatusCode(500, "No se pudo obtener el precio");

                var resultado = new Prices
                {
                    CryptoCode = cryptoCode,
                    Price = respuesta.totalBid
                };
                return Ok(resultado);
            }
            catch
            {
                return StatusCode(500, "No se pudo obtener el precio");
            }
        }
    }
}
