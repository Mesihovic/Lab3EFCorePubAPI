using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

namespace PubDBAPI;

public static class BookEndpoints
{
    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Book").WithTags(nameof(Book));

       
        group.MapGet("/", async (PubContext db) =>
            await db.Books
                .Include(b => b.Author)
                .Include(b => b.Cover)
                .AsNoTracking()
                .ToListAsync()
        )
        .WithName("GetAllBooks");

   
        group.MapGet("/{bookid:int}", async Task<Results<Ok<Book>, NotFound>> (int bookid, PubContext db) =>
        {
            var book = await db.Books
                .Include(b => b.Author)
                .Include(b => b.Cover)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookId == bookid);

            return book is not null ? TypedResults.Ok(book) : TypedResults.NotFound();
        })
        .WithName("GetBookById");

     
        group.MapPost("/", async (Book book, PubContext db) =>
        {
       
            db.Books.Add(book);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Book/{book.BookId}", book);
        })
        .WithName("CreateBook");

    
        group.MapPut("/{bookid:int}", async Task<Results<Ok, NotFound>> (int bookid, Book book, PubContext db) =>
        {
            var affected = await db.Books
                .Where(b => b.BookId == bookid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(b => b.Title, book.Title)
                    .SetProperty(b => b.PublishDate, book.PublishDate)
                    .SetProperty(b => b.BasePrice, book.BasePrice)
                    .SetProperty(b => b.AuthorId, book.AuthorId)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateBook");

  
        group.MapDelete("/{bookid:int}", async Task<Results<Ok, NotFound>> (int bookid, PubContext db) =>
        {
            var affected = await db.Books
                .Where(b => b.BookId == bookid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBook");

        return routes;
    }
}
