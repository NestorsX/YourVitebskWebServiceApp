using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Helpers.Filterers
{
    public class CafeFilterer
    {
        public CafeFilterer(List<CafeType> cafeTypes, int? cafeType, string searchLine)
        {
            cafeTypes.Insert(0, new CafeType { CafeTypeId = 0, Name = "Все виды заведений" });
            CafeTypes = new SelectList(cafeTypes, "CafeTypeId", "Name", cafeType);
            SelectedRole = cafeType;
            SearchLine = searchLine;
        }

        public SelectList CafeTypes { get; private set; }
        public int? SelectedRole { get; private set; }
        public string SearchLine { get; private set; }
    }
}
