using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokProject.Entities.Entities;
using StokProject.Services.Abstract;

namespace StokProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IGenericService<Supplier> _service;

        public SupplierController(IGenericService<Supplier> service)
        {
            _service = service;
        }

        //GET: api/Supplier/GetAllSuppliers
        [HttpGet]
        public IActionResult GetAllSuppliers()
        {
            var suppliers = _service.GetAll();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public IActionResult GetSupplierById(int id)
        {
            //return Ok(_service.GetByID(id));

            var supplier = _service.GetByID(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }

        [HttpPost]
        public IActionResult CreateSupplier([FromBody] Supplier newSupplier)
        {
            //_service.Add(newSupplier);
            //return CreatedAtAction("IdyeGoreTedarikçiGetir", new { id = newSupplier.ID }, newSupplier);

            var supplier = _service.GetByDefault(x => x.SupplierName == newSupplier.SupplierName);
            if (supplier is not null)
            {
                return BadRequest("Tedarikçi zaten mevcut");
            }
            else
            {
                _service.Add(newSupplier);
                return Ok("Tedarikçi başarılı bir şekilde eklendi");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSupplier(int id, [FromBody] Supplier supplier)
        {
            if (id != supplier.ID)
                return BadRequest();

            try
            {
                _service.Update(supplier);
                return Ok(supplier);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_service.Any(x => x.ID == id))
                {
                    return NotFound();
                }
            }

            return NoContent();

            //var updateSupplier = _service.GetByID(supplier.ID);
            //if (updateSupplier != null)
            //    return Ok(_service.Update(supplier));
            //else
            //    return NotFound("Güncelleme başarısız!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(int id)
        {
            var supplier = _service.GetByID(id);
            if (supplier == null)
                return NotFound();

            try
            {
                _service.Remove(supplier);
                return Ok("Tedarikçi silindi");
            }
            catch (Exception)
            {
                return BadRequest();
            }

            //var supplier = _service.GetByID(id);
            //if (supplier != null)
            //{
            //    supplier.IsActive = false;
            //    _service.Update(supplier);
            //    return Ok($"{id} numaralı kategori silindi!");
            //}
            //else
            //    return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult TedarikciAktiflestir(int id)
        {
            var supplier = _service.GetByID(id);
            if (supplier == null)
                return NotFound();

            try
            {
                _service.Activate(id);
                //return Ok(supplier);
                return Ok(_service.GetByID(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetSupplierByName(string name)
        {
            //return Ok(_service.GetDefault(x => x.SupplierName.Contains(name)).ToList());

            var supplier = _service.GetDefault(x => x.SupplierName == name).FirstOrDefault();
            if (supplier != null)
                return Ok(supplier);
            else
                return NotFound();
        }

        [HttpGet]
        public IActionResult GetActiveSuppliers()
        {
            //return Ok(_service.GetActive());

            var suppliers = _service.GetDefault(x => x.IsActive == true);
            return Ok(suppliers);
        }
    }
}
