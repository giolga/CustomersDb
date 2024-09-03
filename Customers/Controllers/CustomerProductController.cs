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
    public class CustomerProductController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomerProductController(IConfiguration config)
        {
            this._config = config;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerProduct>>> Get()
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            try
            {
                var customerProduct = await connection.QueryAsync<CustomerProduct>(@"SELECT * FROM CustomerProductsTb");
                return Ok(customerProduct);
            }
            catch (Exception)
            {
                return BadRequest($"Error!");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerProduct>> Get(int id)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            try
            {
                var customerProduct = await connection.QueryFirstOrDefaultAsync<CustomerProduct>(@"SELECT * FROM CustomerProductsTb WHERE Id = @Id", new {Id = id});

                if(customerProduct == null)
                {
                    return NotFound($"CustomerProduct with the Id:{id} not found! Try Again!");
                }

                return Ok(customerProduct);
            }
            catch (Exception)
            {
                return BadRequest($"Error! Not Found CustomerProduct with the Id of {id}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerProduct>> Post(CustomerProductDTO customerProductDto)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            var customer = await connection.QueryFirstOrDefaultAsync<Customer>(@"SELECT * FROM CustomersTb WHERE Id = @Id", new {Id = customerProductDto.CustomerId});
            if(customer == null)
            {
                return BadRequest($"Error! Customer with the Id:{customerProductDto.CustomerId} not found!");
            }

            var product = await connection.QueryFirstOrDefaultAsync<Product>(@"SELECT * FROM ProductTb WHERE Id = @Id", new {Id = customerProductDto.ProductId});
            if(product == null)
            {
                return BadRequest($"Error! Product with the Id:{customerProductDto.ProductId} not found!");
            }

            try
            {
                await connection.ExecuteAsync(@"INSERT INTO CustomerProductsTb (ProductId, CustomerId) VALUES (@ProductId, @CustomerId)", customerProductDto);
                return Ok($"Success!\n {customerProductDto}");
            }
            catch (Exception)
            {
                return BadRequest($"Error! Post Failed!");
            }
        }

        [HttpPut]
        public async Task<ActionResult<CustomerProduct>> Update(int id, CustomerProductDTO customerProductDto)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
            var getCustomerProduct = await connection.QueryFirstOrDefaultAsync<CustomerProduct>(@"SELECT * FROM CustomerProductsTb WHERE Id = @Id", new {Id = id});

            if(getCustomerProduct == null)
            {
                return NotFound($"Error! CustomerProduct with the id:{id} Not Found! Try Again!");
            }

            try
            {
                await connection.ExecuteAsync(@"UPDATE CustomerProductsTb SET CustomerId = @CustomerId, ProductId = @ProductId WHERE Id = @Id", new {Id = id, CustomerId = customerProductDto.CustomerId, ProductId = customerProductDto.ProductId});
                return Ok($"Updated Successfully!");
            }
            catch (Exception)
            {
                return BadRequest($"Update Failed! Please Try Again!");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTable()
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            try
            {
                await connection.ExecuteAsync("DELETE CustomerProductsTb");
                await connection.ExecuteAsync(@"DBCC CHECKIDENT('CustomerProductsTb', RESEED, 0);");
                return Ok("Table Refreshed Successfully!");
            }
            catch (Exception)
            {
                return BadRequest("Error! Couldn't Refresh Table!");
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerProduct>> Delete(int id)
        {
            SqlConnection connection = new SqlConnection( _config.GetConnectionString("Default"));

            var check = await connection.QueryFirstOrDefaultAsync<CustomerProduct>(@"SELECT * FROM CustomerProductsTb WHERE Id = @Id", new {Id = id});

            if(check == null)
            {
                return NotFound($"Error! CustomerProduct with the id of {id} Not Found!");
            }

            try
            {
                await connection.ExecuteAsync(@"DELETE FROM CustomerProductsTb WHERE Id = @Id", new {Id = id});
                return Ok($"Deletion executed successfully!");
            }
            catch
            {
                return BadRequest($"Deletion failed! Try Again!");
            }
        }


    }
}
