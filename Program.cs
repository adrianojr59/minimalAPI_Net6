using minimalAPI_Net6;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<appDbContext>(c => c.UseSqlServer(ConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("view", async (appDbContext db) => await db.PersonList.ToListAsync());

app.MapGet("View Filter {id}", async (int id, appDbContext db) =>  await db.PersonList.FindAsync(id) is Person person ? Results.Ok(person) :Results.NotFound());

app.MapPost("add", async (Person person, appDbContext db) => 
{
    db.PersonList.Add(person);
    await db.SaveChangesAsync();
    Results.Created($"Nome: {person.Name} {person.Age}",person);
    
    });

app.MapPut("updat", async (int id, Person PersonUpdat, appDbContext db) => 
{

    var person = await db.PersonList.FindAsync(id);

    if (person is null) return Results.NotFound();

    person.Name = PersonUpdat.Name;
    person.Age = PersonUpdat.Age;
    await db.SaveChangesAsync();

    return Results.Ok(person);

});

app.MapDelete("delet",async (int id, appDbContext db) => {

     if(await db.PersonList.FindAsync(id) is Person person)
    {
        db.PersonList.Remove(person);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

     return Results.NotFound();

 });


app.Run();
