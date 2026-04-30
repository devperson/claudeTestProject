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
            Assert.Equal(6, summary.Count);
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
            Assert.Contains("Tax", summary.Keys);
            Assert.Contains("Shipping", summary.Keys);
            Assert.Contains("Total", summary.Keys);
        }

        #endregion
    }
}
