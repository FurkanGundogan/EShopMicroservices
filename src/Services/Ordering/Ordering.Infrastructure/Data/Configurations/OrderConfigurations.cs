using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(x => x.Id).HasConversion(
            orderId => orderId.Value,
            dbId => OrderId.Of(dbId));


        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .IsRequired();


        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(o => o.OrderId);

        builder.ComplexProperty(
            o => o.OrderName, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                           .HasColumnName(nameof(Order.OrderName))
                           .HasMaxLength(100)
                           .IsRequired();
            });

        builder.ComplexProperty(
            o => o.ShippingAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.FirstName)
                                   .HasMaxLength(50)
                                   .IsRequired();
                addressBuilder.Property(a => a.LastName)
                   .HasMaxLength(50)
                   .IsRequired();
                addressBuilder.Property(a => a.EmailAddress)
                   .HasMaxLength(50);
                addressBuilder.Property(a => a.AddressLine)
                   .HasMaxLength(180)
                   .IsRequired();
                addressBuilder.Property(a => a.Country)
                    .HasMaxLength(50);
                addressBuilder.Property(a => a.State)
                    .HasMaxLength(50);
                addressBuilder.Property(a => a.ZipCode)
                    .HasMaxLength(5);
            });

        builder.ComplexProperty(
           o => o.BillingAddress, addressBuilder =>
           {
               addressBuilder.Property(a => a.FirstName)
                                  .HasMaxLength(50)
                                  .IsRequired();
               addressBuilder.Property(a => a.LastName)
                  .HasMaxLength(50)
                  .IsRequired();
               addressBuilder.Property(a => a.EmailAddress)
                  .HasMaxLength(50);
               addressBuilder.Property(a => a.AddressLine)
                  .HasMaxLength(180)
                  .IsRequired();
               addressBuilder.Property(a => a.Country)
                   .HasMaxLength(50);
               addressBuilder.Property(a => a.State)
                   .HasMaxLength(50);
               addressBuilder.Property(a => a.ZipCode)
                   .HasMaxLength(5);
           });

        builder.ComplexProperty(
           o => o.Payment, paymentBuilder =>
           {
               paymentBuilder.Property(a => a.CardName)
                                  .HasMaxLength(50);
               paymentBuilder.Property(a => a.CardNumber)
                  .HasMaxLength(24)
                  .IsRequired();
               paymentBuilder.Property(a => a.Expiration)
                  .HasMaxLength(10);
               paymentBuilder.Property(a => a.CVV)
                  .HasMaxLength(3);
               paymentBuilder.Property(a => a.PaymentMethod);

           });

        builder.Property(a => a.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
                s => s.ToString(),
                dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

        builder.Property(o => o.TotalPrice);
    }
}
