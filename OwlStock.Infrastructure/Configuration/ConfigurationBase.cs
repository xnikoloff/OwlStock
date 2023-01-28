using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlStock.Infrastructure.Configuration
{
    public abstract class ConfigurationBase<T> : IEntityTypeConfiguration<T> where T : class
    {
        public abstract void Configure(EntityTypeBuilder<T> builder);
    }
}
