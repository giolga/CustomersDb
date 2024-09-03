using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Customers.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Customer> CustomersList { get; set; }
    }
}
