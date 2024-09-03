using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customers.Models
{
    public class CustomerProduct
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CustomerFK")]
        public int CustomerId { get; set; }
        public Customer CustomerFK { get; set; }
        [ForeignKey("ProductFK")]
        public int ProductId { get; set; }
        public Product ProductFK { get; set; }
    }
}
