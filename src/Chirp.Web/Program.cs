using Chirp.Web;
using Microsoft.EntityFrameworkCore;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);   

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string was not found");
builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));


builder.Services.AddDefaultIdentity<Author>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<ChirpDBContext>();

//adding Authentication with github
builder.Services.AddAuthentication(options =>
    {
        /*options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "GitHub";*/
    })
    //.AddCookie()
    .AddGitHub(o =>
    {
        o.ClientId = builder.Configuration["authentication_github_clientId"]!;
        o.ClientSecret = builder.Configuration["authentication_github_clientSecret"]!;
        o.CallbackPath = "/signin-github";
        
    });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<SignInManager<Author>, CustomSignInManager<Author>>();


var app = builder.Build();

// Create a disposable service scope
using (var scope = app.Services.CreateScope())
{
    // From the scope, get an instance of our database context.
    // Through the `using` keyword, we make sure to dispose it after we are done.
    using var context = scope.ServiceProvider.GetService<ChirpDBContext>();

    // Execute the migration from code.
    context!.Database.EnsureCreated();
    DbInitializer.SeedDatabase(context, scope.ServiceProvider);
    
    Console.WriteLine("seeding done");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if(app.Environment.IsProduction())
{
    app.UseHsts(); // Send HSTS headers, but only in production
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
//app.UseSession();


app.MapRazorPages();

app.Run();