using Customers.DTOs;
using Customers.Models;
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
            var connection = new SqlConnection(_config.GetConnectionString("Default"));

            var customers = await connection.QueryAsync<Customer>("SELECT * FROM CustomersTb");

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            var customer = await connection.QueryFirstOrDefaultAsync<Customer>("SELECT * FROM CustomersTb WHERE Id = @id", new { id = id });

            if (customer == null)
            {
                return NotFound($"CustomersTb with the id {id} not found. Try Again!");
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> InsertCustomer(CustomerDTO customerDto)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
            var insertCustomer = await connection.ExecuteAsync("INSERT INTO CustomersTb (FirstName, LastName) VALUES(@FirstName, @LastName)", customerDto);

            return Ok(insertCustomer);
        }

        [HttpPut]
        public async Task<ActionResult<Customer>> updateCustomer(CustomerDTO customerDto, int id)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            var customerExists = await connection.QueryFirstOrDefaultAsync<Customer>("SELECT * FROM CustomersTb WHERE Id = @id", new { id = id });

            if (customerExists == null)
            {
                return BadRequest("No such customer exists in my precious Db");
            }

            await connection.ExecuteAsync("UPDATE CustomersTb SET FirstName = @FirstName, LastName = @LastName WHERE Id = @id", new { FirstName = customerDto.FirstName, LastName = customerDto.LastName, id = id });

            return Ok($"CustomersTb with Id {id} was successfully updated! (CHAMA!)");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTable()
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            try
            {
                await connection.ExecuteAsync("DELETE CustomersTb");
                await connection.ExecuteAsync(@"DBCC CHECKIDENT('CustomersTb', RESEED, 0);");
                return Ok("Table Refreshed Successfully!");
            }
            catch (Exception)
            {
                return BadRequest("Error! Couldn't Refresh Table!");
            }
        }

        [HttpDelete("{customerId}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int customerId)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            var customerExists = await connection.QueryFirstOrDefaultAsync<Customer>("SELECT * FROM CustomersTb WHERE Id = @id", new { id = customerId });

            if (customerExists == null)
            {
                return BadRequest("No such customer exists in my precious Db");
            }

            await connection.ExecuteAsync("DELETE FROM CustomersTb WHERE Id = @id", new { id = customerId });

            return Ok($"Customer with the Id {customerId} was deleted successfully! (CHAMA!)");
        }
    }
}
