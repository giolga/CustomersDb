using Customers.Models;
using Microsoft.EntityFrameworkCore;

namespace Customers.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Customer> CustomersTb {  get; set; }  
        public DbSet<Product> ProductTb { get; set; }
        public DbSet<CustomerProduct> CustomerProductsTb { get; set; }  
    }
}
