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
        /// <param name="isPriority">Whether the order has priority processing enabled</param>
        /// <returns>The status string</returns>
        public string GetOrderStatus(int stage, bool isPriority = false)
        {
            return stage switch
            {
                1 => "Pending",
                2 => isPriority ? "Urgent Processing" : "Processing",
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
        /// Calculates a 10% bulk discount when quantity is 3 or more.
        /// </summary>
        /// <param name="quantity">The number of units ordered</param>
        /// <param name="unitPrice">The price per unit</param>
        /// <returns>The bulk discount amount (0 if quantity is less than 3)</returns>
        public decimal CalculateBulkDiscount(int quantity, decimal unitPrice)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be positive", nameof(quantity));
            }

            if (unitPrice <= 0)
            {
                throw new ArgumentException("Unit price must be positive", nameof(unitPrice));
            }

            if (quantity >= 3)
            {
                return quantity * unitPrice * 0.10m;
            }

            return 0m;
        }

        /// <summary>
        /// Generates an order summary with all costs.
        /// </summary>
        /// <param name="baseAmount">The base order amount</param>
        /// <param name="taxRate">The tax rate as a decimal</param>
        /// <param name="discountAmount">The discount amount (if any)</param>
        /// <param name="quantity">The number of units ordered (used to calculate bulk discount)</param>
        /// <param name="unitPrice">The price per unit (used to calculate bulk discount)</param>
        /// <param name="isPriority">Whether to apply a $9.99 priority processing fee</param>
        /// <returns>A dictionary containing itemized costs</returns>
        public Dictionary<string, decimal> GenerateOrderSummary(decimal baseAmount, decimal taxRate, decimal discountAmount = 0, int quantity = 0, decimal unitPrice = 0m, bool isPriority = false)
        {
            if (!IsValidOrderAmount(baseAmount))
            {
                throw new ArgumentException("Base amount must be positive", nameof(baseAmount));
            }

            if (taxRate < 0 || taxRate > 1)
            {
                throw new ArgumentException("Tax rate must be between 0 and 1", nameof(taxRate));
            }

            if (discountAmount < 0 || discountAmount > baseAmount)
            {
                throw new ArgumentException("Discount amount must be between 0 and base amount", nameof(discountAmount));
            }

            if (isPriority && baseAmount < 20m)
            {
                throw new ArgumentException("Priority processing requires a minimum order amount of $20", nameof(isPriority));
            }

            decimal afterDiscount = baseAmount - discountAmount;

            decimal bulkDiscount = 0m;
            if (quantity > 0 && unitPrice > 0)
            {
                bulkDiscount = CalculateBulkDiscount(quantity, unitPrice);
                if (bulkDiscount > afterDiscount)
                {
                    throw new ArgumentException("Bulk discount cannot exceed the subtotal after discount", nameof(quantity));
                }
            }

            decimal afterAllDiscounts = afterDiscount - bulkDiscount;
            decimal tax = afterAllDiscounts * taxRate;
            decimal shipping = CalculateShippingCost(afterAllDiscounts);
            decimal priorityFee = isPriority ? 9.99m : 0m;
            decimal total = afterAllDiscounts + tax + shipping + priorityFee;

            return new Dictionary<string, decimal>
            {
                { "Subtotal", baseAmount },
                { "Discount", discountAmount },
                { "Subtotal After Discount", afterDiscount },
                { "Bulk Discount", bulkDiscount },
                { "Tax", tax },
                { "Shipping", shipping },
                { "Priority Fee", priorityFee },
                { "Total", total }
            };
        }
    }
}
