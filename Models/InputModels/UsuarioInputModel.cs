using System.ComponentModel.DataAnnotations;

namespace TheNewYorkTimes.Models.InputModels
{
    public class UsuarioInputModel
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string LoginUser { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
