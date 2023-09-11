using AutoFixture;
using FluentAssertions;
using NSubstitute;

namespace NSubstituteArgIsEquivalentTo.UnitTests;

public class LotteryServiceTests
{
    private readonly Fixture _fixture = new();
    private readonly LotteryService _service;
    private readonly IBookClient _client = Substitute.For<IBookClient>();

    public LotteryServiceTests()
    {
        _service = new LotteryService(_client);
    }

    [Fact]
    public void When_BookClient_Returns_Winning_Number_Then_User_Should_Win()
    {
        var userWon = SetupCheckIfUserWon(123456789);
        userWon.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(435345)]
    public void When_BookClient_Returns_Non_Winning_Number_Then_User_Should_Lose(int lotteryNumber)
    {
        var userWon = SetupCheckIfUserWon(lotteryNumber);
        userWon.Should().BeFalse();
    }

    private bool SetupCheckIfUserWon(int lotteryNumber)
    {
        var copyrightedBooks = _fixture.Build<Book>()
            .With(x => x.Copyrighted, true)
            .CreateMany();
        /*
         * Be careful - if you ToList() the above, your EquivalentTo<T> will now be looking for a List instead of an IEnumerable for its T
         *      since we are checking against it on line 53
         * In this case, that would make the test fail, since the LotteryService passes in an IEnumerable to ProcessBooks()
         * You can still do this successsfully, but you need to specify the type like so: ArgIs.EquivalentTo<List<Book>>
         * You could also cast the object inside of the ArgIs.EquivalentTo method to T. Here, I have opted to not do this to
         *      instead just be careful with my test values
         */
        var nonCopyrightedBook = _fixture.Build<Book>()
            .With(x => x.Copyrighted, false)
            .Create();
        var allBooks = new List<Book>(copyrightedBooks) { nonCopyrightedBook };

        _client.ProcessBooks(ArgIs.EquivalentTo(copyrightedBooks)).Returns(lotteryNumber);
        var userWon = _service.CheckIfUserWon(allBooks);
        _client.Received().ProcessBooks(ArgIs.EquivalentTo(allBooks));

        return userWon;
    }
}