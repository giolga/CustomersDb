using Customers.DTOs;
using Customers.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data.SqlClient;

namespace Customers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductController(IConfiguration config)
        {
            this._config = config;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));


            var products = await connection.QueryAsync("SELECT * FROM ProductTb");

            try
            {
                return Ok(products);
            }
            catch (DbException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            var product = await connection.QueryFirstAsync<Product>("SELECT * FROM ProductTb WHERE Id = @id", new { id = id });

            try
            {
                return Ok(product);
            }
            catch (DbException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> InsertProduct(ProductDTO productDto)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            await connection.ExecuteAsync("INSERT INTO ProductTb (Name) VALUES(@Name)", productDto);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(int id, ProductDTO product)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            var productExists = await connection.QueryFirstOrDefaultAsync<Product>("SELECT * FROM ProductTb WHERE Id = @id", new { id = id });

            if (productExists == null)
            {
                return BadRequest("No such product exists in my precious Db");
            }

            await connection.ExecuteAsync("UPDATE ProductTb SET Name = @name WHERE Id = @id", new { name = product.Name, id = id });

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTable()
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            try
            {
                await connection.ExecuteAsync("DELETE ProductTb");
                await connection.ExecuteAsync(@"DBCC CHECKIDENT('ProductTb', RESEED, 0);");
                return Ok("Table Refreshed Successfully!");
            }
            catch (Exception)
            {
                return BadRequest("Error! Couldn't Refresh Table!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            var product = await connection.QueryFirstAsync<Product>("SELECT * FROM ProductTb WHERE Id = @Id", new { Id = id });
            if (product == null)
            {
                return NotFound("No such product exists in my precious Db");
            }

            await connection.ExecuteAsync("DELETE FROM ProductTb WHERE Id = @Id", new { Id = id });

            return Ok(); // CHAMA!!!
        }
    }
}