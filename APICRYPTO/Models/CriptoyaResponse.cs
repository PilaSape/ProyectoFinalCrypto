namespace APICRYPTO.Models
{
    public class CriptoyaResponse
    {
        // clase para traducir la respuesta que devuelve la API de criptoya para que c#
        // lo pueda leer
        public decimal ask { get; set; } //el precio al que el exchange vende la cripto
        public decimal totalAsk { get; set; } //igual que ask pero incluyendo las comisiones del exchange
        public decimal bid { get; set; } //l precio al que el exchange compra la cripto (lo que recibirías si vendés).
        public decimal totalBid { get; set; }//igual que bid pero incluyendo comisiones.
    }
}
