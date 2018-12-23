using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonelsApp.Models;

namespace PersonelsApp.ViewModel
{
    public class PersonelViewModel
    {
        public Personel Personel { get; set; }

        public IEnumerable<Departman> Departmanlar { get; set; }
    }
}