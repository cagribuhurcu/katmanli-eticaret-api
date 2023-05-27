using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokProject.Entities.Entities;
using StokProject.Services.Abstract;

namespace StokProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericService<Category> _service;

        public CategoryController(IGenericService<Category> service)
        {
            _service = service;
        }

        //GET: api/Category/GetAllCategories
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _service.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            //return Ok(_service.GetByID(id));

            var category = _service.GetByID(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category newCategory)
        {
            //_service.Add(newCategory);
            //return CreatedAtAction("IdyeGoreKategoriGetir", new { id = newCategory.ID }, newCategory);

            var category = _service.GetByDefault(x => x.CategoryName == newCategory.CategoryName);
            if (category is not null)
            {
                return BadRequest("Kategori zaten mevcut");
            }
            else
            {
                _service.Add(newCategory);
                return Ok("Kategori başarılı bir şekilde eklendi");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.ID)
                return BadRequest();

            try
            {
                _service.Update(category);
                return Ok(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!_service.Any(x=>x.ID == id))
                {
                    return NotFound();
                }
            }

            return NoContent();

            //var updateCategory = _service.GetByID(category.ID);
            //if (updateCategory != null)
            //    return Ok(_service.Update(category));
            //else
            //    return NotFound("Güncelleme başarısız!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _service.GetByID(id);
            if (category == null)
                return NotFound();

            try
            {
                _service.Remove(category);
                return Ok("Kategori silindi");
            }
            catch (Exception)
            {
                return BadRequest();
            }

            //var category = _service.GetByID(id);
            //if (category != null)
            //{
            //    category.IsActive = false;
            //    _service.Update(category);
            //    return Ok($"{id} numaralı kategori silindi!");
            //}
            //else
            //    return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult KategoriAktiflestir(int id)
        {
            var category = _service.GetByID(id);
            if (category == null)
                return NotFound();

            try
            {
                _service.Activate(id);
                //return Ok(category);
                return Ok(_service.GetByID(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetCategoryByName(string name)
        {
            //return Ok(_service.GetDefault(x => x.CategoryName.Contains(name)).ToList());

            var category = _service.GetDefault(x => x.CategoryName == name).FirstOrDefault();
            if (category != null)
                return Ok(category);
            else
                return NotFound();
        }

        [HttpGet]
        public IActionResult GetActiveCategories()
        {
            //return Ok(_service.GetActive());

            var categories = _service.GetDefault(x => x.IsActive == true);
            return Ok(categories);
        }
    }
}
