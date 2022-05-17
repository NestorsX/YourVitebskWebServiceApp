using System;
using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.ViewModels
{
    public class Poster : IViewModel
    {
        public int? PosterId { get; set; }

        [Required(ErrorMessage = "Необходимо указать тип искусства")]
        public int PosterTypeId { get; set; }
        public string PosterType { get; set; }

        [Required(ErrorMessage = "Необходимо название события")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Необходимо указать описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Необходимо указать дату и время")]
        public string DateTime { get; set; }
        public DateTime DateTimeObj { get; set; }

        [Required(ErrorMessage = "Необходимо указать адрес")]
        public string Address { get; set; }
        public string ExternalLink { get; set; }
    }
}
