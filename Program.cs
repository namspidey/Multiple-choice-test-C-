var builder = WebApplication.CreateBuilder(args);
//add start
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
//builder.Services.AddScoped<ISession, ISession>();
//add-end

var app = builder.Build();

//update-start
//app.MapGet("/", () => "Hello World!");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
    );

app.UseSession();
//update-end

app.Run();
