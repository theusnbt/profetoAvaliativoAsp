using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using projetoAvaliativoCidade.Models;

namespace projetoAvaliativoCidade.Repositories
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly string _connectionString;
        private readonly PasswordHasher<UsuarioModel> _passwordHasher;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection") ?? string.Empty;
            _passwordHasher = new PasswordHasher<UsuarioModel>();
        }
        public async Task<bool> Cadastrar(UsuarioModel usuario, string senhaPura)
        {
            try
            {
                usuario.SenhaHash = _passwordHasher.HashPassword(usuario, senhaPura);

                using var conexao = new MySqlConnection(_connectionString);

                await conexao.OpenAsync();

                var query = "INSERTO INTO usuarios (nome, email, senhaHash) VALUES (@nome, @email, @senhaHash)";

                using var comando = new MySqlCommand(query, conexao);

                comando.Parameters.AddWithValue("@Nome", usuario.Nome);
                comando.Parameters.AddWithValue("@Email", usuario.Email);
                comando.Parameters.AddWithValue("@Senha", usuario.SenhaHash);

                var linhasAfetadas = await comando.ExecuteNonQueryAsync();

                return linhasAfetadas > 0;
            }
            catch (Exception ex) 
            {

            }
        }
    }
}
