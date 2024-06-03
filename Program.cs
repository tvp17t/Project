var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//instance
var books = new List<Book>
{
    new Book(1, "The Great Gatsby", "F. Scott Fitzgerald", 1925),
    new Book(2, "To Kill a Mockingbird", "Harper Lee", 1960),
    new Book(3, "To Kill a Mockingbird 2", "Harper Lee", 1962),
};
//get 
app.MapGet("/books", () =>
{
    return books;//res
})
.WithName("GetBooks")
.WithOpenApi();
//post
app.MapPost("/books", (HttpContext context) =>
{
    // Read book data from form data
    var request = context.Request;
    var form = request.Form;

    // Extract book data from form fields
    var id = int.Parse(form["id"]);
    var title = form["title"];
    var author = form["author"];
    var year = int.Parse(form["year"]);

    // Create a new book instance
    var newBook = new Book(id, title, author, year);

    // Assign a new ID and add to the list of books
    newBook.Id = books.Count + 1;
    books.Add(newBook);

    // Return the newly added book
    return newBook;
})
.WithName("AddBook")
.WithOpenApi();
//delete
app.MapDelete("/books/{id}", (HttpContext context) =>
{
    // Extract the book ID from the query string
    var idString = context.Request.RouteValues["id"].ToString();
    
    // Parse the ID to an integer
    if (!int.TryParse(idString, out int id))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return "Invalid book ID.";
    }

    // Find the book with the specified ID
    var bookToRemove = books.FirstOrDefault(b => b.Id == id);
    if (bookToRemove == null)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return "Book not found.";
    }

    // Remove the book from the list
    books.Remove(bookToRemove);

    // Return a success message
    return $"Book with ID {id} has been deleted.";
})
.WithName("DeleteBook")
.WithOpenApi();
//patch
app.MapPatch("/books/{id}", async (int id, HttpRequest request) =>
{
    // Find the book with the specified ID
    var bookToUpdate = books.FirstOrDefault(b => b.Id == id);
    if (bookToUpdate == null)
    {
        return Results.NotFound("Book not found.");
    }

    // Read book data from request body
    var form = await request.ReadFormAsync();

    // Extract book data from form fields
    if (form.ContainsKey("title"))
    {
        bookToUpdate.Title = form["title"];
    }
    if (form.ContainsKey("author"))
    {
        bookToUpdate.Author = form["author"];
    }
    if (form.ContainsKey("year"))
    {
        if (int.TryParse(form["year"], out int year))
        {
            bookToUpdate.Year = year;
        }
        else
        {
            return Results.BadRequest("Invalid year value.");
        }
    }

    // Return the updated book
    return Results.Ok(bookToUpdate);
})
.WithName("UpdateBook")
.WithOpenApi();

app.Run();
