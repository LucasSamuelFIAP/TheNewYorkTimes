using System.ComponentModel.DataAnnotations;

namespace TheNewYorkTimes.Models.InputModels
{
    public class NoticiaInputModel
    {
        [Required]
        public string Titulo { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public string Chapeu { get; set; }

        [Required]
        public string Autor { get; set; }
    }
}
