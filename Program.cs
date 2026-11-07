using AutoWeg.API.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// NAYA BADLAV: Ports ko force karne ke liye Kestrel configuration add kiya
// =========================================================================
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5203); // Normal HTTP Port
    options.ListenAnyIP(7208, listenOptions => listenOptions.UseHttps()); // Secure HTTPS Port
});
// =========================================================================

// 1. CORS Policy Update (Bina kisi error ke Android aur WinForm sabhi ko allow karega)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true) // Sabhi origins (like 10.0.2.2 ya localhost) ko allow karega
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // SignalR ke liye zaroori hai
    });
});

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Android local testing ke liye HTTPS redirection ko temporary comment out kiya
// app.UseHttpsRedirection();

// 2. CORS Middleware active kiya
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.MapHub<GarageHub>("/garageHub");

app.Run();


