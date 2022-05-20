using System;
using System.ComponentModel.DataAnnotations;

namespace YourVitebskWebServiceApp.Models
{
    public class Poster
    {
        [Key]
        public int? PosterId { get; set; }

        [Required(ErrorMessage = "Необходимо указать тип события")]
        public int PosterTypeId { get; set; }

        [Required(ErrorMessage = "Необходимо указать название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Необходимо указать описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Необходимо время события")]
        public string DateTime { get; set; }

        [Required(ErrorMessage = "Необходимо указать адрес")]
        public string Address { get; set; }

        public string ExternalLink { get; set; }
    }
}
