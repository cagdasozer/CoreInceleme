using CoreInceleme.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace CoreInceleme.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly CustomerDbContext _customerDbContext;

		public EmployeeController(CustomerDbContext customerDbContext)
		{
			_customerDbContext = customerDbContext;
		}

		public async Task<IActionResult> Index()
		{
			var list = await _customerDbContext.Employees.ToListAsync();
			return View(list);
		}

		[HttpGet]
		public JsonResult GetDetailsById(int id)
		{
			var order = _customerDbContext.Employees.FirstOrDefault(x => x.ID.Equals(id));
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
		public JsonResult Add(Employee employee)
		{
			JsonResponseViewModel model = new JsonResponseViewModel();
			var result = _customerDbContext.Employees.Add(employee);

			if (employee.Name != null && employee.Department != null && employee.City != null && employee.City != null && employee.Salary != 0)
			{
				int successResponse = _customerDbContext.SaveChanges();
				if (successResponse != 0)
				{
					model.ResponseCode = 0;
					model.ResponseMessage = JsonConvert.SerializeObject(employee);
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
		public JsonResult Update(Employee employee, int id)
		{
			JsonResponseViewModel model = new JsonResponseViewModel();
			var result = _customerDbContext.Employees.FirstOrDefault(x => x.ID == id);
			if (result != null)
			{
				result.ID = employee.ID;
				result.Name = employee.Name;
				result.Department = employee.Department;
				result.City = employee.City;
				result.Salary = employee.Salary;
				var x = _customerDbContext.SaveChanges();

				if (x != 0)
				{
					model.ResponseCode = 0;
					model.ResponseMessage = JsonConvert.SerializeObject(employee);
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
			var result = _customerDbContext.Employees.FirstOrDefault(x => x.ID == id);
			if (result != null)
			{
				_customerDbContext.Employees.Remove(result);
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
