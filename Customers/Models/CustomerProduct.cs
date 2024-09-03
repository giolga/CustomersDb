using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Customers.Models
{
    public class CustomerProduct
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CustomerFK")]
        public int CustomerId { get; set; }
        [JsonIgnore]
        public Customer CustomerFK { get; set; }
        [ForeignKey("ProductFK")]
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product ProductFK { get; set; }
    }
}
