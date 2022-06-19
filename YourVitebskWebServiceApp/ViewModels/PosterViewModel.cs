﻿using System.ComponentModel.DataAnnotations;
using YourVitebskWebServiceApp.Interfaces;

namespace YourVitebskWebServiceApp.ViewModels
{
    public class PosterViewModel : IViewModel
    {
        [Key]
        public int? PosterId { get; set; }
        public int PosterTypeId { get; set; }
        public string PosterType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DateTime { get; set; }
        public string Address { get; set; }
        public string ExternalLink { get; set; }
    }
}
