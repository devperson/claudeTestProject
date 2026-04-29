using Xunit;
using CommonServices;

namespace claudeTestProject.Tests
{
    public class DiscountServiceTests
    {
        private readonly DiscountService _discountService = new();

        [Fact]
        public void CalculateDiscount_OrderUnder100_ReturnsZero()
        {
            // Arrange
            decimal orderAmount = 50m;

            // Act
            decimal discount = _discountService.CalculateDiscount(orderAmount);

            // Assert
            Assert.Equal(0m, discount);
        }

        [Fact]
        public void CalculateDiscount_OrderExactly100_ReturnsZero()
        {
            // Arrange
            decimal orderAmount = 100m;

            // Act
            decimal discount = _discountService.CalculateDiscount(orderAmount);

            // Assert
            Assert.Equal(0m, discount);
        }

        [Fact]
        public void CalculateDiscount_OrderOver100_Returns10Percent()
        {
            // Arrange
            decimal orderAmount = 150m;
            decimal expected = 15m;

            // Act
            decimal discount = _discountService.CalculateDiscount(orderAmount);

            // Assert
            Assert.Equal(expected, discount);
        }

        [Fact]
        public void CalculateDiscount_OrderAt101_Returns10Percent()
        {
            // Act
            decimal discount = _discountService.CalculateDiscount(101m);

            // Assert
            Assert.Equal(10.10m, discount);
        }

        [Fact]
        public void CalculateDiscount_OrderAt200_Returns20Percent()
        {
            // Act
            decimal discount = _discountService.CalculateDiscount(200m);

            // Assert
            Assert.Equal(20m, discount);
        }

        [Fact]
        public void CalculateDiscount_OrderAt500_Returns50Percent()
        {
            // Act
            decimal discount = _discountService.CalculateDiscount(500m);

            // Assert
            Assert.Equal(50m, discount);
        }

        [Fact]
        public void GetFinalPrice_OrderUnder100_ReturnsSameAmount()
        {
            // Arrange
            decimal orderAmount = 50m;

            // Act
            decimal finalPrice = _discountService.GetFinalPrice(orderAmount);

            // Assert
            Assert.Equal(orderAmount, finalPrice);
        }

        [Fact]
        public void GetFinalPrice_OrderOver100_ReturnsDiscountedPrice()
        {
            // Arrange
            decimal orderAmount = 150m;
            decimal expected = 135m;

            // Act
            decimal finalPrice = _discountService.GetFinalPrice(orderAmount);

            // Assert
            Assert.Equal(expected, finalPrice);
        }
    }
}
