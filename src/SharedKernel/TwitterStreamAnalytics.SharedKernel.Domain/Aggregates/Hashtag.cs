using TwitterStreamAnalytics.SharedKernel.Domain.SeedWork;

namespace TwitterStreamAnalytics.SharedKernel.Domain.Aggregates;

// TODO: do not share entity models between Api and Consumers (blocked by https://github.com/dotnet/efcore/issues/28778#issuecomment-1221324545)
public class Hashtag : IAggregateRoot
{
    public Hashtag(string tag)
    {
        Tag = tag;
    }

    public string Tag { get; init; }
    public int Count { get; private set; } = 1;

    public void IncrementCount()
    {
        Count++;
    }
}