using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestGST.Models
{
    public class ExpenseClaim
    {

        public string CostCentre { get; set; }

        public double TotalExpense { get; set; }

        public string PaymentMethod { get; set; }

        public double TotalExcludingGST { get; set; }

        public double GSTApplied { get; set; }

    }
}