using CoreInceleme.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CoreInceleme.Controllers
{
	public class CustomerController : Controller
	{
		private readonly CustomerDbContext _customerDbContext;

		public CustomerController(CustomerDbContext customerDbContext)
		{
			_customerDbContext = customerDbContext;
		}

		public async Task<IActionResult> Index()
		{
			var list = await _customerDbContext.Customers.ToListAsync();
			return View(list);
		}

		[HttpGet]
		public JsonResult GetDetailsByID(int id)
		{
			var customer = _customerDbContext.Customers.FirstOrDefault(x => x.ID.Equals(id));
			JsonResponseViewModel model = new JsonResponseViewModel();
			if (customer != null)
			{
				model.ResponseCode = 0;
				model.ResponseMessage = JsonConvert.SerializeObject(customer); //gelen datayı gönderiyoruz ancak öncesinde json tipine dönüştürüyoruz.
			}
			else
			{
				model.ResponseCode = 1;
				model.ResponseMessage = "Bir Hata Oluştu";
			}
			return Json(model);
		}

		[HttpPost]
		public JsonResult Add(Customer customer)
		{
			JsonResponseViewModel model = new JsonResponseViewModel();
			var result = _customerDbContext.Customers.Add(customer);
			int successResponse = _customerDbContext.SaveChanges();
			if (successResponse != 0)
			{
				model.ResponseCode = 0;
				model.ResponseMessage = JsonConvert.SerializeObject(customer);
			}
			else
			{
				model.ResponseCode = 1;
				model.ResponseMessage = "Bir Hata Oluştu";
			}
			return Json(model);

		}

		[HttpPut] //Güncelle olarak kullanılır
		public JsonResult Update(Customer customer, int id)
		{
			JsonResponseViewModel model = new JsonResponseViewModel();
			var result = _customerDbContext.Customers.FirstOrDefault(x => x.ID == id);
			if (result != null)
			{
				result.Name = customer.Name;
				result.Surname = customer.Surname;
				result.Country = customer.Country;
				result.PhoneNumber = customer.PhoneNumber;
				var x = _customerDbContext.SaveChanges();

				if (x != 0)
				{
					model.ResponseCode = 0;
					model.ResponseMessage = JsonConvert.SerializeObject(customer);
				}
				else
				{
					model.ResponseCode = 1;
					model.ResponseMessage = "Bir Hata Oluştu";
				}
			}
			else
			{
				model.ResponseCode = 1;
				model.ResponseMessage = "Bir Hata Oluştu.";
			}
			return Json(model);
		}

		[HttpDelete]
		public JsonResult Delete(int id)
		{
			JsonResponseViewModel model = new JsonResponseViewModel();
			var result = _customerDbContext.Customers.FirstOrDefault(x => x.ID == id);
			if (result != null)
			{
				_customerDbContext.Customers.Remove(result);
				int x = _customerDbContext.SaveChanges();
				if (x != 0)
				{
					model.ResponseCode = 0;
					model.ResponseMessage = JsonConvert.SerializeObject(result);
				}
				else
				{
					model.ResponseCode = 1;
					model.ResponseMessage = "Bir Hata Oluştu";
				}
			}
			else
			{
				model.ResponseCode = 1;
				model.ResponseMessage = "Bir Hata Oluştu.";
			}
			return Json(model);
		}

		[HttpGet]
		public JsonResult Reflesh()
		{
			var order = _customerDbContext.Orders.FirstOrDefault();
			JsonResponseViewModel model = new JsonResponseViewModel();
			model.ResponseCode = 0;
			model.ResponseMessage = "Sayfa Yenilendi";
			return Json(model);
		}
	}
}
