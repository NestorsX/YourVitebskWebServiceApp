using System;
using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.Models
{
    public class Vacancy : IViewModel
    {
        [Key]
        public int? VacancyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Conditions { get; set; }
        public string Salary { get; set; }
        public string CompanyName { get; set; }
        public string Contacts { get; set; }
        public string Address { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
