using Customers.Model;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Customers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomerController(IConfiguration config)
        {
            this._config = config;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var connection = new SqlConnection(_config.GetConnectionString("CustomerConnection"));

            var customers = await connection.QueryAsync<Customer>("SELECT * FROM Customer");

            return Ok(customers);
        }
    }
}
