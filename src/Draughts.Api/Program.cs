using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Draughts.Api.Dto;
using Draughts.Api.Services;
using Draughts.Domain.Models;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Development CORS policy for local testing
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowDev", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Register domain rules engine implementation
builder.Services.AddSingleton<Draughts.Domain.IRulesEngine, Draughts.Domain.Rules.RulesEngineStub>();
// AI service
builder.Services.AddScoped<IAiService, AiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Enable CORS for development
app.UseCors("AllowDev");

app.MapGet("/api/health", () => Results.Ok(new { status = "Healthy" }));

app.MapPost("/api/ai/move", (AiMoveRequest request, IAiService ai, ILogger<Program> logger) =>
{
    if (request is null)
        return Results.BadRequest();

    if (!System.Enum.TryParse<Player>(request.Player, true, out var player))
        return Results.BadRequest("Invalid player");

    // Validate board coordinates
    if (request.Board?.Pieces != null)
    {
        foreach (var p in request.Board.Pieces)
        {
            if (p.Row < 0 || p.Row >= Draughts.Domain.Models.Board.Size || p.Col < 0 || p.Col >= Draughts.Domain.Models.Board.Size)
                return Results.BadRequest("Invalid piece coordinates in board state");
            if (string.IsNullOrWhiteSpace(p.Owner) || string.IsNullOrWhiteSpace(p.Type))
                return Results.BadRequest("Invalid piece data in board state");
        }
    }

    var sw = Stopwatch.StartNew();
    var move = ai.GetMove(request.Board ?? new BoardStateDto(System.Array.Empty<Draughts.Api.Dto.PieceDto>()), player);
    sw.Stop();
    logger.LogInformation("AI move computed in {ElapsedMs} ms for player {Player}", sw.ElapsedMilliseconds, player);

    return move is null ? Results.NoContent() : Results.Json(move);
});

app.Run();
