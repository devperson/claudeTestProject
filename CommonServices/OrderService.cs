namespace CommonServices
{
    public class OrderService
    {
        /// <summary>
        /// Validates if an order amount is positive.
        /// </summary>
        /// <param name="orderAmount">The order amount to validate</param>
        /// <returns>True if the order amount is positive; otherwise false</returns>
        public bool IsValidOrderAmount(decimal orderAmount)
        {
            return orderAmount > 0;
        }

        /// <summary>
        /// Calculates the total cost including tax.
        /// </summary>
        /// <param name="baseAmount">The base order amount</param>
        /// <param name="taxRate">The tax rate as a decimal (e.g., 0.08 for 8%)</param>
        /// <returns>The total cost including tax</returns>
        public decimal CalculateTotalWithTax(decimal baseAmount, decimal taxRate)
        {
            if (!IsValidOrderAmount(baseAmount))
            {
                throw new ArgumentException("Base amount must be positive", nameof(baseAmount));
            }

            if (taxRate < 0 || taxRate > 1)
            {
                throw new ArgumentException("Tax rate must be between 0 and 1", nameof(taxRate));
            }

            return baseAmount + (baseAmount * taxRate);
        }

        /// <summary>
        /// Calculates shipping cost based on order amount.
        /// </summary>
        /// <param name="orderAmount">The order amount</param>
        /// <returns>The shipping cost (free for orders over $50)</returns>
        public decimal CalculateShippingCost(decimal orderAmount)
        {
            if (!IsValidOrderAmount(orderAmount))
            {
                throw new ArgumentException("Order amount must be positive", nameof(orderAmount));
            }

            if (orderAmount >= 50)
            {
                return 0m;
            }

            return 5.99m;
        }

        /// <summary>
        /// Gets the order status based on processing stage.
        /// </summary>
        /// <param name="stage">The processing stage (1=Pending, 2=Processing, 3=Shipped, 4=Delivered)</param>
        /// <returns>The status string</returns>
        public string GetOrderStatus(int stage)
        {
            return stage switch
            {
                1 => "Pending",
                2 => "Processing",
                3 => "Shipped",
                4 => "Delivered",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Estimates delivery days based on shipping method.
        /// </summary>
        /// <param name="shippingMethod">The shipping method ("standard", "express", "overnight")</param>
        /// <returns>Estimated delivery days</returns>
        public int GetEstimatedDeliveryDays(string shippingMethod)
        {
            if (string.IsNullOrWhiteSpace(shippingMethod))
            {
                throw new ArgumentException("Shipping method cannot be empty", nameof(shippingMethod));
            }

            return shippingMethod.Trim().ToLower() switch
            {
                "standard" => 5,
                "express" => 2,
                "overnight" => 1,
                _ => throw new ArgumentException($"Unknown shipping method: {shippingMethod}", nameof(shippingMethod))
            };
        }

        /// <summary>
        /// Generates an order summary with all costs.
        /// </summary>
        /// <param name="baseAmount">The base order amount</param>
        /// <param name="taxRate">The tax rate as a decimal</param>
        /// <param name="discountAmount">The discount amount (if any)</param>
        /// <returns>A dictionary containing itemized costs</returns>
        public Dictionary<string, decimal> GenerateOrderSummary(decimal baseAmount, decimal taxRate, decimal discountAmount = 0)
        {
            if (!IsValidOrderAmount(baseAmount))
            {
                throw new ArgumentException("Base amount must be positive", nameof(baseAmount));
            }

            if (discountAmount < 0 || discountAmount > baseAmount)
            {
                throw new ArgumentException("Discount amount must be between 0 and base amount", nameof(discountAmount));
            }

            decimal afterDiscount = baseAmount - discountAmount;
            decimal tax = afterDiscount * taxRate;
            decimal shipping = CalculateShippingCost(afterDiscount);
            decimal total = afterDiscount + tax + shipping;

            return new Dictionary<string, decimal>
            {
                { "Subtotal", baseAmount },
                { "Discount", discountAmount },
                { "Subtotal After Discount", afterDiscount },
                { "Tax", tax },
                { "Shipping", shipping },
                { "Total", total }
            };
        }
    }
}
