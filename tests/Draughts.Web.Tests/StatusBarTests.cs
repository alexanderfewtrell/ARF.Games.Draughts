using Bunit;
using Draughts.Domain.Models;
using Draughts.Web.Pages;
using Xunit;

namespace Draughts.Web.Tests;

/// <summary>
/// bUnit tests for StatusBar Blazor component.
/// </summary>
public class StatusBarTests : TestContext
{
    [Fact]
    public void StatusBar_RendersCurrentPlayer()
    {
        // Arrange & Act
        var cut = RenderComponent<StatusBar>(parameters => parameters
            .Add(p => p.CurrentPlayer, Player.White)
            .Add(p => p.Status, "Your turn"));

        // Assert
        var playerSpan = cut.Find(".current-player");
        Assert.Contains("White", playerSpan.TextContent);
    }

    [Fact]
    public void StatusBar_RendersStatusMessage()
    {
        // Arrange & Act
        var cut = RenderComponent<StatusBar>(parameters => parameters
            .Add(p => p.CurrentPlayer, Player.White)
            .Add(p => p.Status, "Your turn"));

        // Assert
        var statusMessage = cut.Find(".status-message");
        Assert.Equal("Your turn", statusMessage.TextContent);
    }

    [Fact]
    public void StatusBar_HasAccessibilityRole()
    {
        // Arrange & Act
        var cut = RenderComponent<StatusBar>(parameters => parameters
            .Add(p => p.CurrentPlayer, Player.Black)
            .Add(p => p.Status, "AI is thinking..."));

        // Assert
        var statusBar = cut.Find(".status-bar");
        Assert.Equal("status", statusBar.GetAttribute("role"));
        Assert.Equal("polite", statusBar.GetAttribute("aria-live"));
    }

    [Fact]
    public void StatusBar_UpdatesWhenPlayerChanges()
    {
        // Arrange
        var cut = RenderComponent<StatusBar>(parameters => parameters
            .Add(p => p.CurrentPlayer, Player.White)
            .Add(p => p.Status, "Your turn"));

        // Act - update to Black player
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.CurrentPlayer, Player.Black)
            .Add(p => p.Status, "AI is thinking..."));

        // Assert
        var playerSpan = cut.Find(".current-player");
        Assert.Contains("Black", playerSpan.TextContent);

        var statusMessage = cut.Find(".status-message");
        Assert.Equal("AI is thinking...", statusMessage.TextContent);
    }
}
