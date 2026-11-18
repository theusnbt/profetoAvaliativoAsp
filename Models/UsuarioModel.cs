using System.ComponentModel.DataAnnotations;

namespace projetoAvaliativoCidade.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public string Nome { get; set; }
    }
}
