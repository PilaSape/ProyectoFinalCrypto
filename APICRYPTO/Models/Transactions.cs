using System.ComponentModel.DataAnnotations.Schema;

namespace APICRYPTO.Models
{
    public class Transactions
    {
        public int Id { get; set; }
        public string CryptoCode { get; set; } 
        public string Action { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CryptoAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Money { get; set; }
        public DateTime DateTime { get; set; }
    }
}
