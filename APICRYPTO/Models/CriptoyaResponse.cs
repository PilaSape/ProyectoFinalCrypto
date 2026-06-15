namespace APICRYPTO.Models
{
    public class CriptoyaResponse
    {
        // clase para traducir la respuesta que devuelve la API de criptoya para que c#
        // lo pueda leer
        public decimal ask { get; set; }
        public decimal totalAsk { get; set; }
        public decimal bid { get; set; }
        public decimal totalBid { get; set; }
    }
}
