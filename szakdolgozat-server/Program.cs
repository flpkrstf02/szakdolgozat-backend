using szakdolgozat_server.Data;
using szakdolgozat_server.Logic;
using szakdolgozat_server.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<IFlowerLogic, FlowerLogic>();
builder.Services.AddTransient<ICroppedImageLogic, CroppedImageLogic>();
builder.Services.AddTransient<IFlowerRepository, FlowerRepository>();
builder.Services.AddTransient<ICroppedImageRepository, CroppedImageRepository>();
builder.Services.AddSingleton<DataContext, DataContext>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Error");
}

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.UseStaticFiles();

app.MapRazorPages();

app.Run();
