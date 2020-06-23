using Microsoft.EntityFrameworkCore;
using NerdStore.Enterprise.Pedido.Domain.Vouchers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NerdStore.Enterprise.Pedido.Infra.Data.Mappings
{
    public class VoucherMapping : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Codigo)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.ToTable("Vouchers");
        }
    }
}