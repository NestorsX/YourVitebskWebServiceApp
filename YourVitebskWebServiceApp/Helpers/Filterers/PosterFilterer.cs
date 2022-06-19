using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Helpers.Filterers
{
    public class PosterFilterer
    {
        public PosterFilterer(List<PosterType> posterTypes, int? posterType, string searchLine)
        {
            posterTypes.Insert(0, new PosterType { PosterTypeId = 0, Name = "Все виды заведений" });
            PosterTypes = new SelectList(posterTypes, "PosterTypeId", "Name", posterType);
            SelectedRole = posterType;
            SearchLine = searchLine;
        }

        public SelectList PosterTypes { get; private set; }
        public int? SelectedRole { get; private set; }
        public string SearchLine { get; private set; }
    }
}
