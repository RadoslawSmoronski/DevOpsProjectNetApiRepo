using Services;
using Models;
using FluentAssertions;

namespace Api.UnitTests.Services;

public class ProductServiceTest
{
    private readonly ProductService _productService = new ProductService();

    [Fact]
    public void CreateShouldReturnProductWhenArgumentsAreValid()
    {
        // Act
        var result = _productService.Create("test", 123);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(3);
        result.Name.Should().Be("test");
        result.Price.Should().Be(123);
    }

    [Fact]
    public void GetShouldReturnProductWhenArgumentIsValid()
    {
        // Act
        var result = _productService.Get(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Laptop");
        result.Price.Should().Be(999.99m);
    }

    [Fact]
    public void DeleteShouldNotThrowExceptionWhenArgumentIsValid()
    {
        // Act
        var action = () => _productService.Delete(1);

        // Assert
        action.Should().NotThrow();
        _productService.Get(1).Should().BeNull();
    }
}