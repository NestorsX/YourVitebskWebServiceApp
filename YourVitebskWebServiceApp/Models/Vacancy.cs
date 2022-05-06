using System;
using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class Vacancy : IViewModel
    {
        [Key]
        public int? VacancyId { get; set; }

        [Required(ErrorMessage = "Необходимо указать заголовок вакансии")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Необходимо указать описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Необходимо указать предполагаемую з/п")]
        public string Salary { get; set; }

        [Required(ErrorMessage = "Необходимо указать компанию-нанимателя")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Необходимо указать контактные данные")]
        public string Contacts { get; set; }

        [Required(ErrorMessage = "Необходимо указать адрес")]
        public string Address { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
