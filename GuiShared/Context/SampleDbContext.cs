using GuiShared.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices;

namespace GuiShared.Context
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext() : base("name=SampleContext")
        {

        }

        public DbSet<TaxReturn> TaxReturns { get; set; }

        public DbSet<CurrencyCode> CurrencyCodes { get; set; }

    }
}
