using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokProject.Entities.Entities;
using StokProject.Services.Abstract;
using StokProject.Entities.Enums;

namespace StokProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<OrderDetails> _odService;
        private readonly IGenericService<Product> _productService;
        private readonly IGenericService<User> _userService;

        public OrderController(IGenericService<Order> orderService, IGenericService<OrderDetails> odService, IGenericService<Product> productService, IGenericService<User> userService)
        {
            _orderService = orderService;
            _odService = odService;
            _productService = productService;
            _userService = userService;
        }

        //GET: api/Order/GetAllOrders
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok(_orderService.GetAll(t0 => t0.SiparisDetaylari, t1 => t1.Kullanici));
        }

        [HttpGet("{id}")]
        public IActionResult SiparisDetayiGetir(int id)
        {
            return Ok(_odService.GetAll(x => x.OrderID == id, t0 => t0.Urun));
        }

        //[HttpGet("{id}")]
        //public IActionResult GetOrderById(int id)
        //{
        //    return Ok(_orderService.GetByID(id, t0 => t0.ID, t1 => t1.SiparisDetaylari));
        //}

        [HttpPost]
        public IActionResult SiparisEkle([FromQuery] int userID, [FromQuery] int[] productIDs, [FromQuery] short[] quantites)
        {
            Order yeniSiparis = new Order();
            yeniSiparis.UserID = userID;
            yeniSiparis.Status = Status.Pending;
            yeniSiparis.IsActive = true;

            _orderService.Add(yeniSiparis); // DB'ye 1 satır Order eklendi.

            //int[] productIDs = new int { 1, 5, 12, 20 };
            //short[] quantities = new short { 10, 2, 25, 120 };

            for (int i = 0; i < productIDs.Length; i++)
            {
                OrderDetails yeniDetay = new OrderDetails();
                yeniDetay.OrderID = yeniSiparis.ID;
                yeniDetay.ProductID = productIDs[i];
                yeniDetay.Quantity = quantites[i];
                yeniDetay.UnitPrice = _productService.GetByID(productIDs[i]).UnitPrice;
                yeniDetay.IsActive = true;

                _odService.Add(yeniDetay);
            }

            return CreatedAtAction("SiparisDetayiGetir", new { id = yeniSiparis.ID }, yeniSiparis);
        }

        [HttpGet("{id}")]
        public IActionResult SiparisOnayla(int id)
        {
            Order onaylanacakSiparis = _orderService.GetByID(id);

            if (onaylanacakSiparis == null)
                return NotFound("Sipariş bulunamadı!");
            else
            {
                List<OrderDetails> detaylar = _odService.GetDefault(x => x.OrderID == onaylanacakSiparis.ID).ToList();

                foreach (OrderDetails item in detaylar)
                {
                    Product detaydakiUrun = _productService.GetByID(item.ProductID);
                    detaydakiUrun.Stock -= (int)item.Quantity;
                    _productService.Update(detaydakiUrun);

                    item.IsActive = false;
                    _odService.Update(item);
                }

                onaylanacakSiparis.Status = Status.Confirmed;
                onaylanacakSiparis.IsActive = false;
                _orderService.Update(onaylanacakSiparis);

                return Ok(onaylanacakSiparis);
            }
        }

        [HttpGet("{id}")]
        public IActionResult SiparisReddet(int id)
        {
            Order reddedilecekSiparis = _orderService.GetByID(id);

            if (reddedilecekSiparis == null)
                return NotFound("Sipariş bulunamadı!");
            else
            {
                List<OrderDetails> detaylar = _odService.GetDefault(x => x.OrderID == id).ToList();

                foreach (OrderDetails item in detaylar)
                {
                    item.IsActive = false;
                    _odService.Update(item);
                }

                reddedilecekSiparis.Status = Status.Canceled;
                reddedilecekSiparis.IsActive = false;
                _orderService.Update(reddedilecekSiparis);

                return Ok(reddedilecekSiparis);
            }
        }

        [HttpGet("{id}")]
        public IActionResult UrunAktiflestir(int id)
        {
            var order = _orderService.GetByID(id);
            if (order == null)
                return NotFound();

            try
            {
                _orderService.Activate(id);
                //return Ok(order);
                return Ok(_orderService.GetByID(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult BekleyenSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Pending).ToList());
        }

        [HttpGet]
        public IActionResult OnaylananSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Confirmed).ToList());
        }

        [HttpGet]
        public IActionResult ReddedilenSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Canceled).ToList());
        }

        [HttpGet]
        public IActionResult GetActiveOrders()
        {
            return Ok(_orderService.GetActive(t0 => t0.SiparisDetaylari, t1 => t1.Kullanici));
        }
    }
}
