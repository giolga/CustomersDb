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


            var products = await connection.QueryAsync("SELECT * FROM Product");

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

            var product = await connection.QueryFirstAsync<Product>("SELECT * FROM Product WHERE Id = @id", new {id = id});

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

            await connection.ExecuteAsync("INSERT INTO Product (Name) VALUES(@Name)", productDto);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(int id,  Product product)
        {
            if(id != product.Id)
            {
                return BadRequest($"Product Id: {product.Id} and input id {id} Doesn't Match!");
            }

            SqlConnection connection = new SqlConnection( _config.GetConnectionString("Default"));

            await connection.ExecuteAsync("UPDATE Product SET Name = @Name WHERE Id = @id", new {Name = product.Name, id = id});

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            var product = await connection.QueryFirstAsync<Product>("SELECT * FROM Product WHERE Id = @Id", new {Id = id});
            if(product == null)
            {
                return NotFound("No such customer exists in my precious Db");
            }

            await connection.ExecuteAsync("DELETE FROM Product WHERE Id = @Id", new {Id = id});

            return Ok(); // CHAMA!!!
        }
    }
}