using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;

namespace NSubstituteArgIsEquivalentTo.UnitTests;

public static class ArgIs
{
    public static bool MatchingAssertion(Action assertion)
    {
        using var assertionScope = new AssertionScope();
        assertion();
        return !assertionScope.Discard().Any();
    }

    public static T EquivalentTo<T>(T value) where T : class
    {
        return Arg.Is<T>(arg => MatchingAssertion(() => arg.Should().BeEquivalentTo(value, string.Empty)));
    }
}
