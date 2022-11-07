using Contracts.Examples;
using FluentAssertions;
using Services.Examples;
using Xunit;

namespace Services.UnitTests.Examples;

public class PostExampleValidatorTests
{
    private readonly PostExampleValidator validator = new();
    
    [Fact]
    public async Task ValidateTest_WhenEverythingIsValid_ReturnsTrue()
    {
        var request = new PostExample { Name = "valid name" };
        var validationResult = await validator.ValidateAsync(request);

        validationResult.IsValid.Should().Be(true);
    }
    
    [Fact]
    public async Task ValidateTest_WhenNameIsTooLong_ReturnsFalseWithValidationError()
    {
        var request = new PostExample { Name = new string('a', 21) };
        var validationResult = await validator.ValidateAsync(request);

        validationResult.IsValid.Should().Be(false);
        validationResult.Errors.Select(e => e.ErrorCode).Should().ContainEquivalentOf(PostExample.ErrorCodes.NameIsTooLong.ToString());
    }
    
    [Fact]
    public async Task ValidateTest_WhenNameIsEmpty_ReturnsFalseWithValidationError()
    {
        var request = new PostExample { Name = "" };
        var validationResult = await validator.ValidateAsync(request);

        validationResult.IsValid.Should().Be(false);
        validationResult.Errors.Select(e => e.ErrorCode).Should().ContainEquivalentOf(PostExample.ErrorCodes.NameIsEmpty.ToString());
    }
}