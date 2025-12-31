using Bunit;
using Draughts.Domain;
using Draughts.Domain.Models;
using Draughts.Domain.Rules;
using Draughts.Web.Pages;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Linq;

namespace Draughts.Web.Tests;

/// <summary>
/// bUnit tests for GameBoard Blazor component.
/// </summary>
public class GameBoardTests : TestContext
{
    public GameBoardTests()
    {
        // Register required services
        Services.AddSingleton<IRulesEngine, RulesEngineStub>();
        
        // Mock HttpClient for AI calls (we won't test actual AI calls in these unit tests)
        Services.AddScoped(_ => new HttpClient(new FakeHttpMessageHandler())
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public void GameBoard_RendersInitialSetup()
    {
        // Act
        var cut = RenderComponent<GameBoard>();

        // Assert: Board should be rendered with 64 cells (8x8)
        var cells = cut.FindAll(".cell");
        Assert.Equal(64, cells.Count);

        // Assert: Should have 24 pieces initially (12 per player)
        var pieces = cut.FindAll(".piece");
        Assert.Equal(24, pieces.Count);
    }

    [Fact]
    public void GameBoard_HasStatusBar()
    {
        // Act
        var cut = RenderComponent<GameBoard>();

        // Assert: Status bar component should be present
        var statusBar = cut.FindComponent<StatusBar>();
        Assert.NotNull(statusBar);
    }

    [Fact]
    public void GameBoard_InitialStatus_IsYourTurn()
    {
        // Act
        var cut = RenderComponent<GameBoard>();

        // Assert: Status should indicate it's the player's turn
        var statusBar = cut.FindComponent<StatusBar>();
        var statusText = statusBar.Find(".status-message");
        Assert.Contains("Your turn", statusText.TextContent);
    }

    [Fact]
    public void GameBoard_WhenClickingOwnPiece_SelectsPiece()
    {
        // Arrange
        var cut = RenderComponent<GameBoard>();

        // Spanish setup: white pieces on dark squares at rows 5-7
        // Dark squares are where (row + col) % 2 == 1
        // Row 5, col 0: (5+0) % 2 = 1 -> dark square with white piece
        var cells = cut.FindAll(".cell");
        var pieceCell = cells[5 * 8 + 0]; // 0-indexed: row 5, col 0

        // Act: Click to select
        pieceCell.Click();

        // Assert: Cell should be selected (have 'selected' class)
        var updatedCells = cut.FindAll(".cell");
        var selectedCell = updatedCells.FirstOrDefault(c => c.ClassList.Contains("selected"));
        Assert.NotNull(selectedCell);
    }

    [Fact]
    public void GameBoard_WhenPieceSelected_ShowsLegalMoveHighlights()
    {
        // Arrange
        var cut = RenderComponent<GameBoard>();

        // Find a white piece at row 5 that can move (col 2 has moves to row 4)
        // Row 5, col 2: (5+2) % 2 = 1 -> dark square with white piece
        var cells = cut.FindAll(".cell");
        var pieceCell = cells[5 * 8 + 2]; // Row 5, col 2

        // Act: Click to select
        pieceCell.Click();

        // Assert: At least one cell should be highlighted as legal destination
        var updatedCells = cut.FindAll(".cell");
        var highlightedCells = updatedCells.Where(c => c.ClassList.Contains("highlight")).ToList();
        Assert.NotEmpty(highlightedCells);
    }

    [Fact]
    public void GameBoard_WhenClickingElsewhere_ClearsSelection()
    {
        // Arrange
        var cut = RenderComponent<GameBoard>();

        // Select a white piece at row 5, col 2 (dark square)
        var cells = cut.FindAll(".cell");
        var pieceCell = cells[5 * 8 + 2];
        pieceCell.Click();

        // Verify selected
        var selectedBefore = cut.FindAll(".cell").Any(c => c.ClassList.Contains("selected"));
        Assert.True(selectedBefore);

        // Act: Click on an empty light square in the middle of the board
        // Must re-query cells after DOM update from first click
        var updatedCells = cut.FindAll(".cell");
        // Row 3, col 1: (3+1) % 2 = 0 -> light square (empty)
        var emptyCell = updatedCells[3 * 8 + 1];
        emptyCell.Click();

        // Assert: Selection should be cleared
        var selectedAfter = cut.FindAll(".cell").Any(c => c.ClassList.Contains("selected"));
        Assert.False(selectedAfter);
    }

    [Fact]
    public void GameBoard_BoardHasCorrectAccessibilityAttributes()
    {
        // Act
        var cut = RenderComponent<GameBoard>();

        // Assert: Board should have grid role
        var board = cut.Find(".board");
        Assert.Equal("grid", board.GetAttribute("role"));
        Assert.Equal("Draughts board", board.GetAttribute("aria-label"));

        // Assert: Cells should have gridcell role
        var cells = cut.FindAll(".cell");
        Assert.All(cells, cell =>
        {
            Assert.Equal("gridcell", cell.GetAttribute("role"));
            Assert.NotNull(cell.GetAttribute("aria-label"));
        });
    }

    [Fact]
    public void GameBoard_CellsAreFocusable()
    {
        // Act
        var cut = RenderComponent<GameBoard>();

        // Assert: All cells should have tabindex for keyboard navigation
        var cells = cut.FindAll(".cell");
        Assert.All(cells, cell =>
        {
            Assert.Equal("0", cell.GetAttribute("tabindex"));
        });
    }

    /// <summary>
    /// Fake HTTP message handler to prevent actual HTTP calls during testing.
    /// </summary>
    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Return an empty OK response for any request
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("{}")
            });
        }
    }
}
