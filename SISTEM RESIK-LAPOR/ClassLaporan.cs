using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEM_RESIK_LAPOR
{
    internal class ClassLaporan
    {
        public int IdLaporan { get; set; }
        public string NamaPelapor { get; set; }
        public string Deskripsi { get; set; }
        public string Lokasi { get; set; }
        public string Status { get; set; }
        public DateTime TanggalLapor { get; set; }
    }
}
