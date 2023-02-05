using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
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

		public ProductController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
			return View(objProductList);
		}

		// GET
		public IActionResult Upsert(int? id)
		{
			Product product = new();

			IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			});

			IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			});

			if (id == null || id == 0)
			{
				// Create product
				return View(product);
			}
			else
			{
				// Update product
			}

			return View();
		}

		// POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(Product obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "Product updated successfully";
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
	}
}
