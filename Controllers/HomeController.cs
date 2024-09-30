using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using TesteWebCanToWebAPI.Models;

namespace TesteWebCanToWebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveImage(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    // Define o caminho onde a imagem será salva
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageFile.FileName);

                    // Cria o diretório se ele não existir
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    // Salva a imagem no diretório wwwroot/images
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    return Json(new { message = "Imagem recebida e salva com sucesso!" });
                }
                catch (Exception ex)
                {
                    return Json(new { message = "Erro ao salvar a imagem: " + ex.Message });
                }
            }

            return Json(new { message = "Nenhum arquivo recebido." });
        }

        public class ImageModel
        {
            public string ImageBase64 { get; set; }
        }

        public IActionResult ExampleViewColetor()
        {
            return View();
        }
    }
}