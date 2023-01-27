using CoreInceleme.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreInceleme.Controllers
{
	public class ProductController : Controller
	{
		private readonly CustomerDbContext _customerDbContext;

		public ProductController(CustomerDbContext customerDbContext)
		{
			_customerDbContext = customerDbContext;
		}

		public async Task<IActionResult> Index()
		{
			var list = await _customerDbContext.Products.ToListAsync();
			return View(list);
		}

		[HttpGet]
		public JsonResult GetDetailsById(int id)
		{
			var product = _customerDbContext.Products.FirstOrDefault(x => x.ID.Equals(id));
			JsonResponseViewModel model = new JsonResponseViewModel();
			if (product != null)
			{
				model.ResponseCode = 0;
				model.ResponseMessage = JsonConvert.SerializeObject(product);
			}
			else
			{
				model.ResponseCode = 1;
				model.ResponseMessage = "Bir Hata Oluştu";
			}
			return Json(model);
		}

		[HttpPost]
		public JsonResult Add(Product product)
		{
			JsonResponseViewModel model = new JsonResponseViewModel();
			var result = _customerDbContext.Products.Add(product);

			if (product.ProductName != null && product.Price != 0 && product.Stock != 0)
			{
				int successResponse = _customerDbContext.SaveChanges();
				if (successResponse != 0)
				{
					model.ResponseCode = 0;
					model.ResponseMessage = JsonConvert.SerializeObject(product);
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
		public JsonResult Update(Product product, int id)
		{
			JsonResponseViewModel model = new JsonResponseViewModel();
			var result = _customerDbContext.Products.FirstOrDefault(x => x.ID == id);
			if (result != null)
			{
				result.ID = product.ID;
				result.ProductName = product.ProductName;
				result.Price = product.Price;
				result.Stock = product.Stock;
				var x = _customerDbContext.SaveChanges();

				if (x != 0)
				{
					model.ResponseCode = 0;
					model.ResponseMessage = JsonConvert.SerializeObject(product);
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
			var result = _customerDbContext.Products.FirstOrDefault(x => x.ID == id);
			if (result != null)
			{
				_customerDbContext.Products.Remove(result);
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
