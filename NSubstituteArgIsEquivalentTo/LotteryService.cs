namespace NSubstituteArgIsEquivalentTo;

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Copyrighted { get; set; }
    public Author[] Authors { get; set; }
}

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Description { get; set; }
}


public interface ILotteryService
{
    bool CheckIfUserWon(IEnumerable<Book> books);
}

public class LotteryService : ILotteryService
{
    private readonly IBookClient _bookClient;

    public LotteryService(IBookClient bookClient)
    {
        _bookClient = bookClient;
    }

    public bool CheckIfUserWon(IEnumerable<Book> books)
    {
        var lotterNumber = _bookClient.ProcessBooks(books.Where(x => x.Copyrighted == true));
        return lotterNumber == 123456789;
    }
}

public interface IBookClient
{
    int ProcessBooks(IEnumerable<Book> books);
}

public class BookClient : IBookClient
{
    public int ProcessBooks(IEnumerable<Book> books)
    {
        throw new NotImplementedException();
    }
}