using System;
using System.Collections.Generic;

namespace TestAPI.Models
{
    public partial class Sepet
    {
        public Sepet()
        {
            SepetUrun = new HashSet<SepetUrun>();
        }

        public int Id { get; set; }
        public int MusteriId { get; set; }

        public virtual Musteri Musteri { get; set; }
        public virtual ICollection<SepetUrun> SepetUrun { get; set; }
    }
}
