using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace EMS.Controllers
{

    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<ImageController> _logger;

        public ImageController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, ILogger<ImageController> logger)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        // GET: Image/Upload
        public IActionResult Upload(Guid? employeeId)
        {
            ViewBag.EmployeeId = employeeId;
            return View();
        }

        // POST: Image/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(Guid? employeeId, Image image)
        {
            if (employeeId == null)
            {
                ModelState.AddModelError("", "Employee ID is required.");
            }
            if (image == null)
            {
                ModelState.AddModelError("", "Image file is required.");
            }

            if (ModelState.IsValid)
            {


                var existingImage = _context.Images.FirstOrDefault(i => i.EmployeeId == employeeId);
                if(existingImage != null)
                {

                    var existingImagePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", existingImage.ImageName);
                    _logger.LogInformation(existingImage.ImageName);
                    if (System.IO.File.Exists(existingImagePath))
                    {
                        System.IO.File.Delete(existingImagePath);
                    }

                    // Update existing image properties
                    existingImage.ImageName = GetUniqueFileName(image.ImageFile.FileName);
                    existingImage.LastUpdatedAt = DateTime.Now;
                    existingImage.ImageFile = image.ImageFile;
                    _context.Images.Update(existingImage);
                    _context.SaveChanges();
                    _logger.LogInformation("Image updated and saved to the database.");
                    return RedirectToAction("ShowEmployeeList", "Employee");


                }

                // Save the image to wwwroot folder
                var uniqueFileName = GetUniqueFileName(image.ImageFile.FileName);
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                Directory.CreateDirectory(uploadsFolder);
                image.ImageFile.CopyTo(new FileStream(filePath, FileMode.Create));

                // Update image properties
                image.ImageName = uniqueFileName;
                image.CreatedAt = DateTime.Now;
                image.LastUpdatedAt = DateTime.Now;

                _context.Images.Add(image);
                _context.SaveChanges();
                _logger.LogInformation("Image uploaded and saved to the database.");

                return RedirectToAction("ShowEmployeeList", "Employee");
            }
            else
            {
                ModelState.AddModelError("", "Model error occured.");

                // If ModelState is invalid, you can inspect the errors
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        _logger.LogError(error.ErrorMessage);

                    }
                }
            }



            return View(image);
        }

        // Helper method to generate unique file names
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                + "_" + Guid.NewGuid().ToString().Substring(0, 8)
                + Path.GetExtension(fileName);
        }
    }
}
