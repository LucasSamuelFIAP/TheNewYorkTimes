namespace TheNewYorkTimes.Models.ViewModels
{
    public class NoticiaViewModel
    {
        public NoticiaViewModel()
        {
            DataPublicacao = DateTime.Now;
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Chapeu { get; set; }
        public DateTime DataPublicacao { get; set; }
        public string Autor { get; set; }
    }
}
