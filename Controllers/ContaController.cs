using Microsoft.AspNetCore.Mvc;
using projetoAvaliativoCidade.Models;
using projetoAvaliativoCidade.Repositories;

namespace projetoAvaliativoCidade.Controllers
{
    public class ContaController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public ContaController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [HttpGet]
        public IActionResult Regsiter() => View();

        public async Task<IActionResult> Register(string nome, string email, string senha)
        {
            if(string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                ViewBag.Erro("Preencha todos os campos");
                return View();
            }

            var usuarioExistente = await _usuarioRepositorio.BuscarEmail(email);

            if(email != null)
            {
                ViewBag.Erro("Esse email ja existe");
            }

            var novoUsuario = new UsuarioModel { Nome = nome, Email = email };

            var sucesso = await _usuarioRepositorio.Cadastrar(novoUsuario, senha);

            if (sucesso)
            {
                TempData["sucesso"] = "Cadastro realizado com sucesso";
               // return RedirectToAction(nameof());
            }

            ViewBag.Erro = "Erro ao cadastrar usuário.";
            return View();
        }



        public IActionResult Index()
        { 
            return View();
        }
    }
}
