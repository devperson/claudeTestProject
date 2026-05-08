namespace CommonServices
{
    public class DiscountService
    {
        private const decimal DiscountThreshold = 100m;
        private const decimal DiscountPercentage = 0.10m;

        /// <summary>
        /// Calculates a 10% discount for orders over $100.
        /// </summary>
        /// <param name="orderAmount">The order amount</param>
        /// <returns>The discount amount (0 if order is $100 or less)</returns>
        public decimal CalculateDiscount(decimal orderAmount)
        {
            if (orderAmount <= 0)
            {
                throw new ArgumentException("Order amount must be positive", nameof(orderAmount));
            }

            if (orderAmount > DiscountThreshold)
            {
                return orderAmount * DiscountPercentage;
            }
            return 0m;
        }

        /// <summary>
        /// Gets the final price after applying the discount.
        /// </summary>
        /// <param name="orderAmount">The order amount</param>
        /// <returns>The final price after discount</returns>
        public decimal GetFinalPrice(decimal orderAmount)
        {
            decimal discount = CalculateDiscount(orderAmount);
            return orderAmount - discount;
        }
    }
}
