using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokProject.Entities.Entities;
using StokProject.Services.Abstract;

namespace StokProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericService<Product> _service;

        public ProductController(IGenericService<Product> service)
        {
            _service = service;
        }

        //GET: api/Product/GetAllProducts
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(_service.GetAll(t0 => t0.Kategori, t1 => t1.Tedarikci));
        }

        [HttpGet("{id}")]
        public IActionResult TedarikcininTumUrunleriGetir(int id)
        {
            return Ok(_service.GetAll(x => x.SupplierID == id, t0 => t0.Kategori, t1 => t1.Tedarikci));
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            return Ok(_service.GetByID(id, t0 => t0.Kategori, t1 => t1.Tedarikci));
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product newProduct)
        {
            //_service.Add(newProduct);
            //return CreatedAtAction("IdyeGoreUrunGetir", new { id = newProduct.ID }, newProduct);

            var product = _service.GetByDefault(x => x.ProductName == newProduct.ProductName);
            if (product is not null)
            {
                return BadRequest("Urun zaten mevcut");
            }
            else
            {
                _service.Add(newProduct);
                return Ok("Urun başarılı bir şekilde eklendi");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.ID)
                return BadRequest();

            try
            {
                _service.Update(product);
                return Ok(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_service.Any(x => x.ID == id))
                {
                    return NotFound();
                }
            }

            return NoContent();

            //var updateProduct = _service.GetByID(product.ID);
            //if (updateProduct != null)
            //    return Ok(_service.Update(product));
            //else
            //    return NotFound("Güncelleme başarısız!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _service.GetByID(id);
            if (product == null)
                return NotFound();

            try
            {
                _service.Remove(product);
                return Ok("Urun silindi");
            }
            catch (Exception)
            {
                return BadRequest();
            }

            //var product = _service.GetByID(id);
            //if (product != null)
            //{
            //    product.IsActive = false;
            //    _service.Update(product);
            //    return Ok($"{id} numaralı kategori silindi!");
            //}
            //else
            //    return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult UrunAktiflestir(int id)
        {
            var product = _service.GetByID(id);
            if (product == null)
                return NotFound();

            try
            {
                _service.Activate(id);
                //return Ok(product);
                return Ok(_service.GetByID(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetProductByName(string name)
        {
            return Ok(_service.GetDefault(x => x.ProductName.Contains(name)).ToList());

            //var product = _service.GetDefault(x => x.ProductName == name).FirstOrDefault();
            //if (product != null)
            //    return Ok(product);
            //else
            //    return NotFound();
        }

        [HttpGet]
        public IActionResult GetActiveProducts()
        {
            return Ok(_service.GetActive(t0 => t0.Kategori, t1 => t1.Tedarikci));
        }
    }
}
