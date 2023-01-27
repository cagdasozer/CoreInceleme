using CoreInceleme.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace CoreInceleme.Controllers
{
	public class OrderController : Controller
	{
		private readonly CustomerDbContext _customerDbContext;

		public OrderController(CustomerDbContext customerDbContext)
		{
			_customerDbContext = customerDbContext;
		}

		public async Task<IActionResult> Index()
		{
			var list = await _customerDbContext.Orders.ToListAsync();
			return View(list);
		}

		[HttpGet]
		public JsonResult GetDetailsById(int id)
		{
			var order = _customerDbContext.Orders.FirstOrDefault(x => x.ID.Equals(id));
			JsonResponseViewModel model = new JsonResponseViewModel();
			if (order != null)
			{
				model.ResponseCode = 0;
				model.ResponseMessage = JsonConvert.SerializeObject(order);
			}
			else
			{
				model.ResponseCode = 1;
				model.ResponseMessage = "Bir Hata Oluştu";
			}
			return Json(model);
		}

		[HttpPost]
		public JsonResult Add(Order order)
		{
			JsonResponseViewModel model = new JsonResponseViewModel();
			var result = _customerDbContext.Orders.Add(order);

			if (order.CustomerName != null && order.CustomerSurname != null && order.CustomerAddress != null)
			{
				int successResponse = _customerDbContext.SaveChanges();
				if (successResponse != 0)
				{
					model.ResponseCode = 0;
					model.ResponseMessage = JsonConvert.SerializeObject(order);
				}
				else
				{
					model.ResponseCode = 1;
					model.ResponseMessage = "Bir Hata Oluştu";
				}
			}
			return Json(model);
		}

		[HttpPut] //Güncelle olarak kullanılır
		public JsonResult Update(Order order, int id)
		{
			JsonResponseViewModel model = new JsonResponseViewModel();
			var result = _customerDbContext.Orders.FirstOrDefault(x => x.ID == id);
			if (result != null)
			{
				result.ID = order.ID;
				result.CustomerName = order.CustomerName;
				result.CustomerSurname = order.CustomerSurname;
				result.CustomerAddress = order.CustomerAddress;
				var x = _customerDbContext.SaveChanges();

				if (x != 0)
				{
					model.ResponseCode = 0;
					model.ResponseMessage = JsonConvert.SerializeObject(order);
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
			var result = _customerDbContext.Orders.FirstOrDefault(x => x.ID == id);
			if (result != null)
			{
				_customerDbContext.Orders.Remove(result);
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
