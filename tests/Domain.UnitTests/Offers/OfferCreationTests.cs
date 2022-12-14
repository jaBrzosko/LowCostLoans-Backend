using Domain.Offers;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.Offers;

public class OfferCreationTests
{
    [Fact]
    public void Create_with_normal_values()
    {
        var expectedInquireId = Guid.NewGuid();
        int expectedInterestRate = 100;
        int expectedMoney = 10_000;
        int expectedNumberOfInstallments = 12;
        
        var actualOffer = new Offer(expectedInquireId, expectedInterestRate, expectedMoney, expectedNumberOfInstallments, OfferSourceBank.OurBank);

        actualOffer.InquireId.Should().Be(expectedInquireId);
        actualOffer.InterestRateInPromiles.Should().Be(expectedInterestRate);
        actualOffer.MoneyInSmallestUnit.Should().Be(expectedMoney);
        actualOffer.NumberOfInstallments.Should().Be(expectedNumberOfInstallments);
    }

    [Fact]
    public void Money_is_zero()
    {
        TestThrowingArgumentException(Guid.NewGuid(), 100, 0, 12);
    }
    
    [Fact]
    public void Money_is_negative()
    {
        TestThrowingArgumentException(Guid.NewGuid(), 100, -12, 12);
    }
    
    [Fact]
    public void NumberOfInstallments_is_zero()
    {
        TestThrowingArgumentException(Guid.NewGuid(), 100, 1000, 0);
    }
    
    [Fact]
    public void NumberOfInstallments_is_negative()
    {
        TestThrowingArgumentException(Guid.NewGuid(), 100, 1000, -1);
    }
    
    private void TestThrowingArgumentException(Guid inquireId, int interestRate, int money, int numberOfInstallments)
    {
        var action = () => { new Offer(inquireId, interestRate, money, numberOfInstallments, OfferSourceBank.OurBank); };
        action.Should().Throw<ArgumentException>();
    }
}
