using System.ComponentModel.DataAnnotations;

namespace TheNewYorkTimes.Models.InputModels
{
    public class LoginInputModel
    {
        [Required]
        public string LoginUser { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
