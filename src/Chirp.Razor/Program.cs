using Chirp.Razor;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepService, CheepService>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();



/*
string pathtodb;
Console.WriteLine("here");
Console.WriteLine(Environment.GetEnvironmentVariable("CHIRPDBPATH"));
if (Environment.GetEnvironmentVariable("CHIRPDBPATH") != null)
{
    pathtodb = Environment.GetEnvironmentVariable("CHIRPDBPATH");
}
else
{
    pathtodb = Path.Combine(Path.GetTempPath(), "chirp.db");
}


if (!File.Exists(pathtodb))
{
    makeDB(pathtodb);
}
builder.Services.AddSingleton(new DBFacade(pathtodb));
static void makeDB(string pathtodb)
{
    using var connection = new SqliteConnection($"Data Source={pathtodb};");
    connection.Open();
    var command = connection.CreateCommand();
    
    command.CommandText = @"
        create table if not exists user (
          user_id integer primary key autoincrement,
          username string not null,
          email string not null,
          pw_hash string not null
        );

        create table if not exists message (
          message_id integer primary key autoincrement,
          author_id integer not null,
          text string not null,
          pub_date integer
        );
    ";
    
    command.ExecuteNonQuery();
}*/



var app = builder.Build();
// Create a disposable service scope
using (var scope = app.Services.CreateScope())
{
    // From the scope, get an instance of our database context.
    // Through the `using` keyword, we make sure to dispose it after we are done.
    using var context = scope.ServiceProvider.GetService<ChirpDBContext>();

    // Execute the migration from code.
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();


