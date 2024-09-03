using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Customers.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Product> ProductsList { get; set; }
    }
}
