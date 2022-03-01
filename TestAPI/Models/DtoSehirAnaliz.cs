using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI.Models
{
    public class DtoSehirAnaliz
    {
        public string SehirAdi { get; set; }
        public int SepetAdet { get; set; }
        public decimal ToplamTutar { get; set; }
    }
}
