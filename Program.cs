using CommonServices;

namespace claudeTestProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var discountService = new DiscountService();

            decimal orderAmount = 150m;
            decimal discount = discountService.CalculateDiscount(orderAmount);
            decimal finalPrice = discountService.GetFinalPrice(orderAmount);

            Console.WriteLine($"Order Amount: ${orderAmount:F2}");
            Console.WriteLine($"Discount (10%): ${discount:F2}");
            Console.WriteLine($"Final Price: ${finalPrice:F2}");
        }
    }
}
