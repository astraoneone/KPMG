using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiShared.DomainClasses
{
    public class TaxReturn
    {
        public int Id { get; set; }

        public DateTime ImportedOn { get; set; }

        public string Account { get; set; }

        public string Description { get; set; }

        public string CurrencyCode { get; set; }

        public decimal TaxValue { get; set; }
    }
}
