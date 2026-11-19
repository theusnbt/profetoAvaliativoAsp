using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projetoAvaliativoCidade.Models;
using projetoAvaliativoCidade.Repositories;
using System.Security.Claims;

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
                return RedirectToAction(nameof(Login));
            }

            ViewBag.Erro = "Erro ao cadastrar usuário.";
            return View();
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _usuarioRepositorio.BuscarEmail(model.Email);
            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "E-mail inválido");
            }

            var passwordHasher = new PasswordHasher<UsuarioModel>();
            var resultado = passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, model.Senha);

            if(resultado == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Senha inválida");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Index()
        { 
            return View();
        }
    }
}
