using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICRYPTO.Data;
using APICRYPTO.Models;

namespace APICRYPTO.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public TransactionsController(ApplicationDbContext contexto, IHttpClientFactory fabricaHttpClient)
        {
            _context = contexto;
            _httpClient = fabricaHttpClient.CreateClient();
        }

        // traigo todas las transacciones ordenadas por fecha
        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var transacciones = await _context.transactions
                .OrderBy(t => t.DateTime)
                .ToListAsync();
            return Ok(transacciones);
        }

        // busco una transaccion por su id, si no existe devuelvo 404
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var transaccion = await _context.transactions.FindAsync(id);
            if (transaccion == null) return NotFound();
            return Ok(transaccion);
        }

        // creo una nueva transaccion de compra o venta
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Transactions request)
        {
            // valido que la cantidad sea mayor a 0
            if (request.CryptoAmount <= 0)
                return BadRequest("La cantidad debe ser mayor a 0");

            // si es una venta, verifico que el usuario tenga suficiente cripto antes de dejarla pasar
            if (request.Action == "sale")
            {
                var saldo = await ObtenerSaldo(request.CryptoCode);
                if (request.CryptoAmount > saldo)
                    return BadRequest($"No tenés suficiente {request.CryptoCode}. Saldo actual: {saldo}");
            }

            // consulto el precio actual a la API de criptoya y lo multiplico por la cantidad
            var precio = await ObtenerPrecioCrypto(request.CryptoCode);
            if (precio == null)
                return StatusCode(500, "No se pudo obtener el precio de la criptomoneda");

            request.Money = request.CryptoAmount * precio.Value;

            _context.transactions.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerPorId), new { id = request.Id }, request);
        }

        // edito una transaccion existente, solo piso los campos que me mandan
        [HttpPatch("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] Transactions updated)
        {
            var transaccion = await _context.transactions.FindAsync(id);
            if (transaccion == null) return NotFound();

            // solo actualizo el campo si viene con valor en el body
            if (updated.CryptoCode != null) transaccion.CryptoCode = updated.CryptoCode;
            if (updated.Action != null) transaccion.Action = updated.Action;
            if (updated.CryptoAmount > 0) transaccion.CryptoAmount = updated.CryptoAmount;
            if (updated.Money > 0) transaccion.Money = updated.Money;
            if (updated.DateTime != default) transaccion.DateTime = updated.DateTime;

            await _context.SaveChangesAsync();
            return Ok(transaccion);
        }

        // borro una transaccion por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Borrar(int id)
        {
            var transaccion = await _context.transactions.FindAsync(id);
            if (transaccion == null) return NotFound();

            _context.transactions.Remove(transaccion);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // devuelvo el estado actual de la cartera con el valor en ARS de cada cripto
        [HttpGet("portfolio")]
        public async Task<IActionResult> ObtenerPortafolio()
        {
            // paso 1: traigo todas las transacciones de la base de datos
            var transacciones = await _context.transactions.ToListAsync();

            // paso 2: calculo cuanta cripto tengo de cada tipo sumando compras y restando ventas
            var saldos = transacciones
                .GroupBy(t => t.CryptoCode)
                .Select(g => new
                {
                    CryptoCode = g.Key,
                    Cantidad = g.Sum(t => t.Action == "purchase" ? t.CryptoAmount : -t.CryptoAmount)
                })
                .Where(b => b.Cantidad > 0)
                .ToList();

            // paso 3: para cada cripto calculo cuanto vale en pesos hoy
            var portafolio = new List<object>();
            decimal total = 0;

            foreach (var item in saldos)
            {
                // consulto el precio actual a CriptoYa
                var precio = await ObtenerPrecioCrypto(item.CryptoCode);

                if (precio != null)
                {
                    // multiplico la cantidad que tengo por el precio actual
                    var valorARS = item.Cantidad * precio.Value;

                    // sumo al total general
                    total += valorARS;

                    // agrego la cripto a la lista con toda su info
                    portafolio.Add(new
                    {
                        item.CryptoCode,
                        item.Cantidad,
                        ValorARS = valorARS
                    });
                }
            }

            // paso 4: devuelvo la lista de criptos y el total
            return Ok(new { Tenencias = portafolio, TotalEnPesos = total });
        }

        // funcion para consultar el precio de una cripto en criptoya usando binance como exchange
        private async Task<decimal?> ObtenerPrecioCrypto(string cryptoCode)
        {
            try
            {
                var url = $"https://criptoya.com/api/binance/{cryptoCode}/ars/1";
                var respuesta = await _httpClient.GetFromJsonAsync<CriptoyaResponse>(url);
                return respuesta?.totalBid;
            }
            catch
            {
                return null;
            }
        }

        // funcion para calcular cuanta cripto tiene el usuario sumando compras y restando ventas
        private async Task<decimal> ObtenerSaldo(string cryptoCode)
        {
            // traigo todas las transacciones de esa cripto
            var transacciones = await _context.transactions
                .Where(t => t.CryptoCode == cryptoCode)
                .ToListAsync();

            // sumo todas las compras
            decimal totalComprado = 0;
            foreach (var transaccion in transacciones)
            {
                if (transaccion.Action == "purchase")
                {
                    totalComprado = totalComprado + transaccion.CryptoAmount;
                }
            }

            // sumo todas las ventas
            decimal totalVendido = 0;
            foreach (var transaccion in transacciones)
            {
                if (transaccion.Action == "sale")
                {
                    totalVendido = totalVendido + transaccion.CryptoAmount;
                }
            }

            // el saldo es lo que compré menos lo que vendí
            decimal saldo = totalComprado - totalVendido;
            return saldo;
        }
    }


}