// Models/Book.cs
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }

    public Book(int id, string title, string author, int year)
    {
        Id = id;
        Title = title;
        Author = author;
        Year = year;
    }

    public void Save()
    {
        //luu book vao database
    }

    public List<Book> GetAllBooks()
    {
        //lay book tu database
        return new List<Book>();
    }

    public Book GetBook(int id)
    {
        return new Book(1,"title","author",1);
    }

    public void AddBook(Book book)
    {
        //add book
    }

    public void DeleteBook(int id)
    {
        //
    }

    public void EditBook()
    {
        //
    }
}
