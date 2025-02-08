namespace Ordering.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa815d0")),"john doe","test@gmail.com"),
            Customer.Create(CustomerId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa814d0")),"mark luther","testSecond@gmail.com")
        };


        public static IEnumerable<Product> Products =>
        new List<Product>
        {
                    Product.Create(ProductId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa815d9")),"IPhone X",500),
                    Product.Create(ProductId.Of(new Guid("01921491-0714-46f8-9f84-123e2f5414d7")),"Samsung X",400),
                    Product.Create(ProductId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa81407")),"Ipad X",650),
                    Product.Create(ProductId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa81467")),"Apple Watch",450)
        };

        public static IEnumerable<Order> OrdersWithItems
        {
            get
            {
                var address1 = Address.Of("john","doe","jd@gmail.com","address text","test","test","test");
                var address2 = Address.Of("mark","luther","ml@gmail.com","address text","test", "test", "test");

                var payment1 = Payment.Of("testCard1","5555555555555555","12/28","355",1);
                var payment2 = Payment.Of("testCard1","4444455555555544","06/30","222",2);

                var order1 = Order.Create(
                    OrderId.Of(Guid.NewGuid()),
                    CustomerId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa815d0")),
                    OrderName.Of("ORD_1"),
                    shippingAddress: address1,
                    billingAddress: address1,
                    payment1
                    );
                order1.Add(ProductId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa815d9")),1,500);
                order1.Add(ProductId.Of(new Guid("01921491-0714-46f8-9f84-123e2f5414d7")),1,400);

                var order2 = Order.Create(
                    OrderId.Of(Guid.NewGuid()),
                    CustomerId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa814d0")),
                    OrderName.Of("ORD_2"),
                    shippingAddress: address2,
                    billingAddress: address2,
                    payment1
                    );
                order2.Add(ProductId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa81407")), 1, 650);
                order2.Add(ProductId.Of(new Guid("01921491-0714-46f8-9f84-123e2fa81467")), 1, 450);

                return new List<Order> { order1, order2 };
            }
        }

    }
}
