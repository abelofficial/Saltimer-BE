using Saltimer.Api.Config;
using Saltimer.Api.Hubs;
using Saltimer.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServicesFromAssembly(builder.Configuration);

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi3();
app.UseReDoc(c =>
{
    c.DocumentTitle = "Saltimer API";
    c.SpecUrl = "/swagger/v1/swagger.json";
});

app.UseRouting();

app.UseCors();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<AuthUserMiddleware>();

app.MapControllers();
app.MapHub<MobTimerHub>("/timer");


app.Run();
