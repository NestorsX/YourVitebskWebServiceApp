using System;
using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class Poster
    {
        [Key]
        public int? PosterId { get; set; }

        [Required(ErrorMessage = "Необходимо указать тип искусства")]
        public int PosterTypeId { get; set; }

        [Required(ErrorMessage = "Необходимо название события")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Необходимо указать описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Необходимо указать дату и время")]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Необходимо указать адрес")]
        public string Address { get; set; }

        public string ExternalLink { get; set; }
    }
}
