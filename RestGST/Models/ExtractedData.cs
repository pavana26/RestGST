using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestGST.Models
{
    public class ExtractedData
    {

        public List<ExpenseClaim> ExpenseClaims { get; set; }

        public List<Reservation> Reservations { get; set; }

        public List<string> ErrorsInProcesing { get; set; }
    }
}