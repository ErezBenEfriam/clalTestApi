using ClalTEST.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//i did it in controller too
app.MapGet("/minmalapiGetWeather", async() =>
{
    //אין באמת סיבה להעברת עיר מכיוון שיש רק עיר אחת
    HttpClient client = new HttpClient();
    string url = @"https://api.weatherapi.com/v1/forecast.json?key=39f8ecaf506c4f76b3f55139222906&q=london&days=3&aqi=yes&alerts=yes";
    var response = await client.GetAsync(url);
    string responseBody = await response.Content.ReadAsStringAsync();
    var articles = JsonConvert.DeserializeObject<Root>(responseBody);
    var londonWeather = articles.current.temp_c;
    var conditiontext = articles.current.condition.text;

    return Results.Ok($"the wather in london is: {londonWeather}  {conditiontext}");
})
.WithName("minmalapiGetWeather");

app.Run();
