using System.Net;
using Contracts.Common;
using Contracts.Inquires;
using Contracts.Users;
using FastEndpoints;
using FluentAssertions;
using Services.Endpoints.Inquires;
using Xunit;

namespace IntegrationTests.Inquiries;

public class CreatingInquireTests : TestBase
{
    public CreatingInquireTests(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }

    [Fact]
    public async Task Request_is_invalid()
    {
        var createInquire = new PostCreateInquireAsAnonymous
        {
            MoneyInSmallestUnit = 0,
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

        postResult.response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Mocked_Offers_are_added()
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

        postResult.response!.StatusCode.Should().Be(HttpStatusCode.OK);
        postResult.result.Should().NotBeNull();

        var getInquireDetails = new GetInquireDetailsById()
        {
            Id = postResult.result!.Id,
        };

        var getResult = await Client
            .GETAsync<GetInquireDetailsByIdEndpoint, GetInquireDetailsById, InquireDetailsDto>(getInquireDetails);
        
        getResult.response!.StatusCode.Should().Be(HttpStatusCode.OK);
        getResult.result.Should().NotBeNull();
        getResult.result!.Offers.Count.Should().BePositive();
        getResult.result.Id.Should().Be(postResult.result.Id);
    }
}
