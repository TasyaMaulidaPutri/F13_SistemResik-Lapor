using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEM_RESIK_LAPOR
{
    internal class ClassSetoran
    {
        public int IdSetoran { get; set; }
        public string NamaPelapor { get; set; }
        public string JenisSampah { get; set; }
        public int BeratKg { get; set; }
        public int PoinPerKg { get; set; }
        public string StatusVerifikasi { get; set; }
        public DateTime TanggalSetor { get; set; }
    }
}
