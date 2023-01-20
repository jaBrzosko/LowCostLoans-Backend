using System.Net;
using Contracts.Common;
using Contracts.Inquires;
using Contracts.Offers;
using Contracts.Users;
using FastEndpoints;
using FluentAssertions;
using IntegrationTests.MockedServices;
using Services.Endpoints.Inquires;
using Services.Endpoints.Offers;
using Xunit;

namespace IntegrationTests.Offers;

public class GetOfferContractTests : TestBase
{
    public GetOfferContractTests(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }
    
    [Fact]
    public async Task Get_offer_contract()
    {
        var inquireId = await CreateInquireAndEnsureSuccessAsync();
        var offers = await GetOffersAsync(inquireId);
        foreach (var offer in offers)
        {
            if (offer.SourceBank == OfferSourceBankDto.OurBank)
            {
                var contract = await Client.GETAsync<GetOfferContractEndpoint, GetOfferContract, ContractDto>(new() { OfferId = offer.Id });
                contract.result.ContractUrl.Should().BeEquivalentTo(MockedOurApiClient.UrlToContract);
            }
        }
    }

    private async Task<List<OfferDto>> GetOffersAsync(Guid inquireId)
    {
        var getInquireDetails = new GetInquireDetailsById()
        {
            Id = inquireId,
        };

        var getResult = await Client
            .GETAsync<GetInquireDetailsByIdEndpoint, GetInquireDetailsById, InquireDetailsDto>(getInquireDetails);

        return getResult.result.Offers;
    }

    private async Task<Guid> CreateInquireAndEnsureSuccessAsync()
    {
        var createInquire = new PostCreateInquireAsAnonymous
        {
            MoneyInSmallestUnit = 10000,
            NumberOfInstallments = 12,
            PersonalData = new PersonalDataDto
            {
                FirstName = "Name",
                GovernmentId = "00000000000",
                GovernmentIdType = GovernmentIdTypeDto.Pesel,
                JobType = JobTypeDto.SomeJobType,
                LastName = "last name",
            },
        };
        var postResult = await Client
            .POSTAsync<PostCreateInquireAsAnonymousEndpoint, PostCreateInquireAsAnonymous, PostResponseWithIdDto>(createInquire);

        postResult.response.StatusCode.Should().Be(HttpStatusCode.OK);
        postResult.result.Should().NotBeNull();

        return postResult.result.Id;
    }
}
