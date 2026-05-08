using Xunit;
using CommonServices;

namespace claudeTestProject.Tests
{
    public class OrderServiceTests
    {
        private readonly OrderService _orderService = new();

        #region IsValidOrderAmount Tests

        [Fact]
        public void IsValidOrderAmount_PositiveAmount_ReturnsTrue()
        {
            // Arrange
            decimal orderAmount = 50m;

            // Act
            bool result = _orderService.IsValidOrderAmount(orderAmount);

            // Assert
            Assert.True(result); 
        }

        [Fact]
        public void IsValidOrderAmount_ZeroAmount_ReturnsFalse()
        {
            // Arrange
            decimal orderAmount = 0m;

            // Act
            bool result = _orderService.IsValidOrderAmount(orderAmount);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidOrderAmount_NegativeAmount_ReturnsFalse()
        {
            // Arrange
            decimal orderAmount = -10m;

            // Act
            bool result = _orderService.IsValidOrderAmount(orderAmount);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region CalculateTotalWithTax Tests

        [Fact]
        public void CalculateTotalWithTax_ValidAmountAndRate_CalculatesCorrectly()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 0.08m;
            decimal expected = 108m;

            // Act
            decimal result = _orderService.CalculateTotalWithTax(baseAmount, taxRate);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateTotalWithTax_NoTax_ReturnsBaseAmount()
        {
            // Arrange
            decimal baseAmount = 50m;
            decimal taxRate = 0m;
            decimal expected = 50m;

            // Act
            decimal result = _orderService.CalculateTotalWithTax(baseAmount, taxRate);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateTotalWithTax_MaxTaxRate_CalculatesCorrectly()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 1m;
            decimal expected = 200m;

            // Act
            decimal result = _orderService.CalculateTotalWithTax(baseAmount, taxRate);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateTotalWithTax_InvalidBaseAmount_ThrowsException()
        {
            // Arrange
            decimal baseAmount = 0m;
            decimal taxRate = 0.08m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.CalculateTotalWithTax(baseAmount, taxRate));
        }

        [Fact]
        public void CalculateTotalWithTax_NegativeBaseAmount_ThrowsException()
        {
            // Arrange
            decimal baseAmount = -50m;
            decimal taxRate = 0.08m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.CalculateTotalWithTax(baseAmount, taxRate));
        }

        [Fact]
        public void CalculateTotalWithTax_NegativeTaxRate_ThrowsException()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = -0.08m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.CalculateTotalWithTax(baseAmount, taxRate));
        }

        [Fact]
        public void CalculateTotalWithTax_TaxRateGreaterThanOne_ThrowsException()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 1.5m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.CalculateTotalWithTax(baseAmount, taxRate));
        }

        #endregion

        #region CalculateShippingCost Tests

        [Fact]
        public void CalculateShippingCost_SmallOrder_ReturnsShippingCost()
        {
            // Arrange
            decimal orderAmount = 25m;
            decimal expected = 5.99m;

            // Act
            decimal result = _orderService.CalculateShippingCost(orderAmount);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateShippingCost_LargeOrder_ReturnsFreeShipping()
        {
            // Arrange
            decimal orderAmount = 75m;
            decimal expected = 0m;

            // Act
            decimal result = _orderService.CalculateShippingCost(orderAmount);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateShippingCost_ExactThreshold_ReturnsFreeShipping()
        {
            // Arrange
            decimal orderAmount = 50m;
            decimal expected = 0m;

            // Act
            decimal result = _orderService.CalculateShippingCost(orderAmount);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateShippingCost_JustBelowThreshold_ReturnsShippingCost()
        {
            // Arrange
            decimal orderAmount = 49.99m;
            decimal expected = 5.99m;

            // Act
            decimal result = _orderService.CalculateShippingCost(orderAmount);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateShippingCost_InvalidAmount_ThrowsException()
        {
            // Arrange
            decimal orderAmount = 0m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.CalculateShippingCost(orderAmount));
        }

        #endregion

        #region GetOrderStatus Tests

        [Theory]
        [InlineData(1, "Pending")]
        [InlineData(2, "Processing")]
        [InlineData(3, "Shipped")]
        [InlineData(4, "Delivered")]
        public void GetOrderStatus_ValidStage_ReturnsCorrectStatus(int stage, string expected)
        {
            // Act
            string result = _orderService.GetOrderStatus(stage);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetOrderStatus_InvalidStage_ReturnsUnknown()
        {
            // Arrange
            int stage = 99;
            string expected = "Unknown";

            // Act
            string result = _orderService.GetOrderStatus(stage);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetOrderStatus_NegativeStage_ReturnsUnknown()
        {
            // Arrange
            int stage = -1;
            string expected = "Unknown";

            // Act
            string result = _orderService.GetOrderStatus(stage);

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region GetEstimatedDeliveryDays Tests

        [Theory]
        [InlineData("standard", 5)]
        [InlineData("express", 2)]
        [InlineData("overnight", 1)]
        public void GetEstimatedDeliveryDays_ValidMethod_ReturnsCorrectDays(string method, int expected)
        {
            // Act
            int result = _orderService.GetEstimatedDeliveryDays(method);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetEstimatedDeliveryDays_CaseInsensitive_ReturnsCorrectDays()
        {
            // Arrange
            string method = "STANDARD";

            // Act
            int result = _orderService.GetEstimatedDeliveryDays(method);

            // Assert
            Assert.Equal(5, result);
        }

        [Theory]
        [InlineData(" standard", 5)]
        [InlineData("express ", 2)]
        [InlineData(" overnight ", 1)]
        public void GetEstimatedDeliveryDays_WhitespacePadded_ReturnsCorrectDays(string method, int expected)
        {
            // Act
            int result = _orderService.GetEstimatedDeliveryDays(method);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetEstimatedDeliveryDays_InvalidMethod_ThrowsException()
        {
            // Arrange
            string method = "invalid";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.GetEstimatedDeliveryDays(method));
        }

        [Fact]
        public void GetEstimatedDeliveryDays_EmptyMethod_ThrowsException()
        {
            // Arrange
            string method = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.GetEstimatedDeliveryDays(method));
        }

        [Fact]
        public void GetEstimatedDeliveryDays_NullMethod_ThrowsException()
        {
            // Arrange
            string? method = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.GetEstimatedDeliveryDays(method!));
        }

        #endregion

        #region GenerateOrderSummary Tests

        [Fact]
        public void GenerateOrderSummary_ValidInput_ReturnsCompleteOrderSummary()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 0.10m;
            decimal discountAmount = 10m;

            // Act
            var summary = _orderService.GenerateOrderSummary(baseAmount, taxRate, discountAmount);

            // Assert
            Assert.NotNull(summary);
            Assert.Equal(8, summary.Count);
            Assert.Equal(100m, summary["Subtotal"]);
            Assert.Equal(10m, summary["Discount"]);
            Assert.Equal(90m, summary["Subtotal After Discount"]);
            Assert.Equal(9m, summary["Tax"]);
            // Note: Shipping is free because $90 > $50 threshold (before any other deductions)
            Assert.Equal(0m, summary["Shipping"]);
            Assert.Equal(99m, summary["Total"]);
        }

        [Fact]
        public void GenerateOrderSummary_NoDiscount_ReturnsCorrectSummary()
        {
            // Arrange
            decimal baseAmount = 60m;
            decimal taxRate = 0.08m;

            // Act
            var summary = _orderService.GenerateOrderSummary(baseAmount, taxRate);

            // Assert
            Assert.Equal(60m, summary["Subtotal"]);
            Assert.Equal(0m, summary["Discount"]);
            Assert.Equal(60m, summary["Subtotal After Discount"]);
            Assert.Equal(4.8m, summary["Tax"]);
            Assert.Equal(0m, summary["Shipping"]);
            Assert.Equal(64.8m, summary["Total"]);
        }

        [Fact]
        public void GenerateOrderSummary_LargeOrderFreeShipping_ReturnsCorrectSummary()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 0.05m;

            // Act
            var summary = _orderService.GenerateOrderSummary(baseAmount, taxRate);

            // Assert
            Assert.Equal(0m, summary["Shipping"]);
            Assert.Equal(105m, summary["Total"]);
        }

        [Fact]
        public void GenerateOrderSummary_InvalidBaseAmount_ThrowsException()
        {
            // Arrange
            decimal baseAmount = 0m;
            decimal taxRate = 0.08m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.GenerateOrderSummary(baseAmount, taxRate));
        }

        [Fact]
        public void GenerateOrderSummary_NegativeDiscount_ThrowsException()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 0.08m;
            decimal discountAmount = -10m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.GenerateOrderSummary(baseAmount, taxRate, discountAmount));
        }

        [Fact]
        public void GenerateOrderSummary_DiscountGreaterThanBase_ThrowsException()
        {
            // Arrange
            decimal baseAmount = 50m;
            decimal taxRate = 0.08m;
            decimal discountAmount = 100m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.GenerateOrderSummary(baseAmount, taxRate, discountAmount));
        }

        [Fact]
        public void GenerateOrderSummary_NegativeTaxRate_ThrowsException()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = -0.08m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.GenerateOrderSummary(baseAmount, taxRate));
        }

        [Fact]
        public void GenerateOrderSummary_TaxRateGreaterThanOne_ThrowsException()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 1.5m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.GenerateOrderSummary(baseAmount, taxRate));
        }

        [Fact]
        public void GenerateOrderSummary_AllKeysPresentInDictionary_VerifyKeys()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 0.08m;

            // Act
            var summary = _orderService.GenerateOrderSummary(baseAmount, taxRate);

            // Assert
            Assert.Contains("Subtotal", summary.Keys);
            Assert.Contains("Discount", summary.Keys);
            Assert.Contains("Subtotal After Discount", summary.Keys);
            Assert.Contains("Bulk Discount", summary.Keys);
            Assert.Contains("Priority Fee", summary.Keys);
            Assert.Contains("Tax", summary.Keys);
            Assert.Contains("Shipping", summary.Keys);
            Assert.Contains("Total", summary.Keys);
        }

        #endregion

        #region CalculateBulkDiscount Tests

        [Fact]
        public void CalculateBulkDiscount_QuantityBelow3_ReturnsZero()
        {
            // Arrange
            int quantity = 2;
            decimal unitPrice = 50m;

            // Act
            decimal result = _orderService.CalculateBulkDiscount(quantity, unitPrice);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void CalculateBulkDiscount_QuantityExactly3_ReturnsDiscount()
        {
            // Arrange
            int quantity = 3;
            decimal unitPrice = 50m;
            decimal expected = 15m; // 3 * 50 * 0.10

            // Act
            decimal result = _orderService.CalculateBulkDiscount(quantity, unitPrice);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateBulkDiscount_QuantityAbove3_ReturnsDiscount()
        {
            // Arrange
            int quantity = 5;
            decimal unitPrice = 40m;
            decimal expected = 20m; // 5 * 40 * 0.10

            // Act
            decimal result = _orderService.CalculateBulkDiscount(quantity, unitPrice);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateBulkDiscount_ZeroQuantity_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.CalculateBulkDiscount(0, 50m));
        }

        [Fact]
        public void CalculateBulkDiscount_NegativeQuantity_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.CalculateBulkDiscount(-1, 50m));
        }

        [Fact]
        public void CalculateBulkDiscount_ZeroUnitPrice_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderService.CalculateBulkDiscount(3, 0m));
        }

        #endregion

        #region GetOrderStatus Priority Tests

        [Fact]
        public void GetOrderStatus_PriorityStage2_ReturnsUrgentProcessing()
        {
            // Act
            string result = _orderService.GetOrderStatus(2, isPriority: true);

            // Assert
            Assert.Equal("Urgent Processing", result);
        }

        [Fact]
        public void GetOrderStatus_NonPriorityStage2_ReturnsProcessing()
        {
            // Act
            string result = _orderService.GetOrderStatus(2, isPriority: false);

            // Assert
            Assert.Equal("Processing", result);
        }

        [Fact]
        public void GetOrderStatus_PriorityStage1_ReturnsPending()
        {
            // Priority only affects stage 2
            string result = _orderService.GetOrderStatus(1, isPriority: true);

            Assert.Equal("Pending", result);
        }

        #endregion

        #region GenerateOrderSummary with Bulk Discount Tests

        [Fact]
        public void GenerateOrderSummary_WithBulkDiscount_AppliesCorrectly()
        {
            // Arrange — 5 units @ $30 = $150 base; bulk discount = 5 * 30 * 0.10 = $15
            decimal baseAmount = 150m;
            decimal taxRate = 0.10m;
            int quantity = 5;
            decimal unitPrice = 30m;

            // Act
            var summary = _orderService.GenerateOrderSummary(baseAmount, taxRate, 0, quantity, unitPrice);

            // Assert
            Assert.Equal(150m, summary["Subtotal"]);
            Assert.Equal(0m,   summary["Discount"]);
            Assert.Equal(150m, summary["Subtotal After Discount"]);
            Assert.Equal(15m,  summary["Bulk Discount"]);   // 5 * 30 * 0.10
            Assert.Equal(13.5m, summary["Tax"]);            // 135 * 0.10
            Assert.Equal(0m,   summary["Shipping"]);        // 135 >= 50
            Assert.Equal(148.5m, summary["Total"]);         // 135 + 13.5
        }

        [Fact]
        public void GenerateOrderSummary_NoBulkDiscount_WhenQuantityBelow3()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 0.10m;
            int quantity = 2;
            decimal unitPrice = 30m;

            // Act
            var summary = _orderService.GenerateOrderSummary(baseAmount, taxRate, 0, quantity, unitPrice);

            // Assert
            Assert.Equal(0m, summary["Bulk Discount"]);
            Assert.Equal(110m, summary["Total"]); // 100 + 10 tax, free shipping
        }

        [Fact]
        public void GenerateOrderSummary_QuantityWithoutUnitPrice_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _orderService.GenerateOrderSummary(100m, 0.10m, quantity: 5));
        }

        [Fact]
        public void GenerateOrderSummary_UnitPriceWithoutQuantity_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _orderService.GenerateOrderSummary(100m, 0.10m, unitPrice: 30m));
        }

        [Fact]
        public void GenerateOrderSummary_BulkDiscountExceedsSubtotal_ThrowsException()
        {
            // Arrange — bulk discount (3 * 100 * 0.10 = $30) > baseAmount ($20)
            decimal baseAmount = 20m;
            decimal taxRate = 0.10m;
            int quantity = 3;
            decimal unitPrice = 100m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _orderService.GenerateOrderSummary(baseAmount, taxRate, 0, quantity, unitPrice));
        }

        #endregion

        #region GenerateOrderSummary Priority Processing Tests

        [Fact]
        public void GenerateOrderSummary_PriorityOrder_AddsPriorityFee()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 0.10m;

            // Act
            var summary = _orderService.GenerateOrderSummary(baseAmount, taxRate, isPriority: true);

            // Assert
            Assert.Equal(9.99m, summary["Priority Fee"]);
            Assert.Equal(119.99m, summary["Total"]); // 100 + 10 tax + 0 shipping + 9.99
        }

        [Fact]
        public void GenerateOrderSummary_NonPriorityOrder_NoPriorityFee()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 0.10m;

            // Act
            var summary = _orderService.GenerateOrderSummary(baseAmount, taxRate);

            // Assert
            Assert.Equal(0m, summary["Priority Fee"]);
            Assert.Equal(110m, summary["Total"]);
        }

        [Fact]
        public void GenerateOrderSummary_PriorityOrder_BelowMinAmount_ThrowsException()
        {
            // Arrange — baseAmount $15 is below the $20 minimum for priority
            decimal baseAmount = 15m;
            decimal taxRate = 0.10m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _orderService.GenerateOrderSummary(baseAmount, taxRate, isPriority: true));
        }

        [Fact]
        public void GenerateOrderSummary_PriorityOrder_ExactMinAmount_AppliesFee()
        {
            // Arrange — exactly $20 should be allowed
            decimal baseAmount = 20m;
            decimal taxRate = 0m;

            // Act
            var summary = _orderService.GenerateOrderSummary(baseAmount, taxRate, isPriority: true);

            // Assert
            Assert.Equal(9.99m, summary["Priority Fee"]);
            Assert.Equal(35.98m, summary["Total"]); // 20 - 0 discount + 0 tax + 5.99 shipping + 9.99
        }

        #endregion
    }
}
