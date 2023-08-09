namespace TheNewYorkTimes.Models.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string LoginUser { get; set; }
        public string Senha { get; set; }
        public string Role { get; set; }
        public bool Ativo { get; set; }
    }
}
