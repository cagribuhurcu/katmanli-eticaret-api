using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokProject.Entities.Entities;
using StokProject.Services.Abstract;

namespace StokProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<User> _service;

        public UserController(IGenericService<User> service)
        {
            _service = service;
        }

        //GET: api/User/GetAllUsers
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _service.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            //return Ok(_service.GetByID(id));

            var user = _service.GetByID(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            _service.Add(newUser);
            return CreatedAtAction("GetUserById", new { id = newUser.ID }, newUser);

            //var user = _service.GetByDefault(x => x.FirstName == newUser.FirstName);
            //if (user is not null)
            //{
            //    return BadRequest("Kullanici zaten mevcut");
            //}
            //else
            //{
            //    _service.Add(newUser);
            //    return Ok("Kullanici başarılı bir şekilde eklendi");
            //}
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.ID)
                return BadRequest();

            try
            {
                _service.Update(user);
                return Ok(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_service.Any(x => x.ID == id))
                {
                    return NotFound();
                }
            }

            return NoContent();

            //var updateUser = _service.GetByID(user.ID);
            //if (updateUser != null)
            //    return Ok(_service.Update(user));
            //else
            //    return NotFound("Güncelleme başarısız!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _service.GetByID(id);
            if (user == null)
                return NotFound();

            try
            {
                _service.Remove(user);
                return Ok("Kullanici silindi");
            }
            catch (Exception)
            {
                return BadRequest();
            }

            //var user = _service.GetByID(id);
            //if (user != null)
            //{
            //    user.IsActive = false;
            //    _service.Update(user);
            //    return Ok($"{id} numaralı kategori silindi!");
            //}
            //else
            //    return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult KullaniciAktiflestir(int id)
        {
            var user = _service.GetByID(id);
            if (user == null)
                return NotFound();

            try
            {
                _service.Activate(id);
                //return Ok(user);
                return Ok(_service.GetByID(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetUserByName(string name)
        {
            //return Ok(_service.GetDefault(x => x.UserName.Contains(name)).ToList());

            var user = _service.GetDefault(x => x.FirstName == name).FirstOrDefault();
            if (user != null)
                return Ok(user);
            else
                return NotFound();
        }

        [HttpGet]
        public IActionResult GetActiveUsers()
        {
            //return Ok(_service.GetActive());

            var users = _service.GetDefault(x => x.IsActive == true);
            return Ok(users);
        }

        [HttpGet]
        public IActionResult Login(string email, string password)
        {
            if (_service.Any(user => user.Email == email))
            {
                User loggedUser = _service.GetByDefault(user => user.Email == email && user.Password == password);
                if (loggedUser != null)
                    return Ok(loggedUser);
                else
                    return BadRequest("Parola hatalı!");
            }

            //User loggedUser = _service.GetByDefault(user => user.Email == email && user.Password == password);
            //if (loggedUser != null)
            //    return Ok(loggedUser);

            return NotFound("KullanıcıBulunamadı!");
        }
    }
}
