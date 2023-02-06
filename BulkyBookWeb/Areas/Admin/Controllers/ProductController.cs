using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Linq;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostEnvironment;

		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}

		// GET
		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new()
			{
				Product = new(),
				CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
				CoverTypeList = _unitOfWork.CoverType.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				})
			};

			if (id == null || id == 0)
			{
				// Create product
				// ViewBag.CategoryList = CategoryList;
				// ViewData["CoverTypeList"] = CoverTypeList;
				return View(productVM);
			}
			else
			{
				// Update product
			}

			return View(productVM);
		}

		// POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(ProductVM obj, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				string wwwRootPath = _hostEnvironment.WebRootPath;

				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString();
					var uploads = Path.Combine(wwwRootPath, @"images\products");
					var extension = Path.GetExtension(file.FileName);

					using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
					{
						file.CopyTo(fileStreams);
					}

					obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
				}

				_unitOfWork.Product.Add(obj.Product);
				_unitOfWork.Save();
				TempData["success"] = "Product created successfully";
				return RedirectToAction("Index");
			}

			return View(obj);
		}

		// GET
		public IActionResult Delete(int? id)
		{
			Product product = new();

			if (id == null || id == 0)
			{
				// Create product
				return View(product);
			}
			else
			{
				// Delete product
			}

			return View();
		}

		// POST
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePOST(int? id)
		{
			var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

			if (obj == null)
			{
				return NotFound();
			}

			_unitOfWork.Product.Remove(obj);
			_unitOfWork.Save();
			TempData["success"] = "Product deleted successfully";
			return RedirectToAction("Index");
		}

		#region API CALLS

		[HttpGet]
		public IActionResult GetAll()
		{
			var productList = _unitOfWork.Product.GetAll();
			return Json(new { data = productList });
		}

		#endregion
	}
}
