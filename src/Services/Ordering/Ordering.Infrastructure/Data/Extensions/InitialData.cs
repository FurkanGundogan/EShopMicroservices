namespace Ordering.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa815d0")),"testName","test@gmail.com"),
            Customer.Create(CustomerId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa814d0")),"testSecondName","testSecond@gmail.com")
        };
    }
}
