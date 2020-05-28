using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class IncomeInequalitySlo
    {
        public DateTime Year { get; set; }
        public decimal IncomeInequality { get; set; }
    }
}
