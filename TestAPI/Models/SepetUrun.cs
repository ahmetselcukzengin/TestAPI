using System;
using System.Collections.Generic;

namespace TestAPI.Models
{
    public partial class SepetUrun
    {
        public int Id { get; set; }
        public int SepetId { get; set; }
        public decimal Tutar { get; set; }
        public string Aciklama { get; set; }

        public virtual Sepet Sepet { get; set; }
    }
}
