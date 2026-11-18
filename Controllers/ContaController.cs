using Microsoft.AspNetCore.Mvc;

namespace projetoAvaliativoCidade.Controllers
{
    public class ContaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
