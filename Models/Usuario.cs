namespace TheNewYorkTimes.Models
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string LoginUser { get; set; }
        public string Senha { get; set; }
        public string Role { get; set; }
        public bool Ativo { get; set; }

        public Usuario(string nome, string loginUser, string senha, string role, bool ativo)
        {
            Id = new Guid();
            Nome = nome;
            LoginUser = loginUser;
            Senha = senha;
            Role = role;
            Ativo = ativo;
        }
    }
}
