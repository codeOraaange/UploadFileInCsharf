using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using uploadfile.Models;

//tambah 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace uploadfile.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //tambah ini
        
        IHostingEnvironment _env;

        
        public HomeController(ILogger<HomeController> logger, IHostingEnvironment env)
        {
            _logger = logger;
            //tambah, juga di atas + env tanpa spasi
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        //untuk ng post file-nya. Save.
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file)
        {
            if(file==null)
            {
                return NotFound();
            }

            string filePath = Path.Combine(_env.WebRootPath, "uploads");
            //buat folder di atas "uploads" jika tdk ada
            if(!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            //ngambil nama  file dan ekstensi
            string fileName = file.FileName;
            string fullPath = Path.Combine(filePath, fileName);
            using(var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            //such as header di php
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
