using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Draughts.Api;
using Draughts.Api.Dto;
using Xunit;

namespace Draughts.Api.Tests;

public class AiMoveEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AiMoveEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AiMove_WithValidBoard_ReturnsMove()
    {
        // Arrange: board with one white man at row 5 col 2
        var request = new AiMoveRequest(
            new BoardStateDto(new[]
            {
                new PieceDto(5, 2, "White", "Man")
            }),
            "White");

        // Act
        var response = await _client.PostAsJsonAsync("/api/ai/move", request);

        // Assert: either a move or no content if no legal moves
        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task AiMove_InvalidPlayer_ReturnsBadRequest()
    {
        var request = new AiMoveRequest(
            new BoardStateDto(System.Array.Empty<PieceDto>()),
            "InvalidPlayer");

        var response = await _client.PostAsJsonAsync("/api/ai/move", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
