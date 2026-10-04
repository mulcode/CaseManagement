using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDataAccess.Managers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IDefendant, DefendantManager>();
builder.Services.AddScoped<IUser, UserManager>();
builder.Services.AddScoped<IComplaint, ComplaintManager>();
builder.Services.AddScoped<IDocument, DocumentManager>();

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddAuthentication("MyCookie")
    .AddCookie("MyCookie", options =>
    {
        options.LoginPath = "/Login";
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthorizedUsers", policy =>
    {
        policy.RequireClaim("UserType", "CIPD", "Magistrate");
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/", context =>
{
    context.Response.Redirect("/Login");
    return Task.CompletedTask;
});

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();



app.Run();
