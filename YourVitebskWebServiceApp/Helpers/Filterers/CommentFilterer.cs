using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Helpers.Filterers
{
    public class CommentFilterer
    {
        public CommentFilterer(string service, string searchLine)
        {
            var services = new List<Service>();
            services.Insert(0, new Service { ServiceId = 0, Name = "Все сервисы" });
            services.Insert(1, new Service { ServiceId = 1, Name = "Заведения" });
            services.Insert(2, new Service { ServiceId = 2, Name = "Афиша" });
            Services = new SelectList(services, "Name", "Name", service);
            SelectedService = service;
            SearchLine = searchLine;
        }

        public SelectList Services { get; private set; }
        public string SelectedService { get; private set; }
        public string SearchLine { get; private set; }
    }
}
