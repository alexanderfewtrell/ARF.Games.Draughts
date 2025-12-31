using Bunit;
using Draughts.Domain.Models;
using Draughts.Web.Pages;
using Xunit;

namespace Draughts.Web.Tests;

/// <summary>
/// bUnit tests for RestartDialog Blazor component.
/// </summary>
public class RestartDialogTests : TestContext
{
    [Fact]
    public void RestartDialog_RendersGameOverTitle()
    {
        // Arrange & Act
        var cut = RenderComponent<RestartDialog>(parameters => parameters
            .Add(p => p.Winner, Player.White));

        // Assert
        var title = cut.Find("#restart-title");
        Assert.Equal("Game Over", title.TextContent);
    }

    [Fact]
    public void RestartDialog_ShowsWinMessageForWhite()
    {
        // Arrange & Act
        var cut = RenderComponent<RestartDialog>(parameters => parameters
            .Add(p => p.Winner, Player.White));

        // Assert
        var message = cut.Find("p");
        Assert.Contains("You win", message.TextContent);
    }

    [Fact]
    public void RestartDialog_ShowsLossMessageForBlack()
    {
        // Arrange & Act
        var cut = RenderComponent<RestartDialog>(parameters => parameters
            .Add(p => p.Winner, Player.Black));

        // Assert
        var message = cut.Find("p");
        Assert.Contains("AI wins", message.TextContent);
    }

    [Fact]
    public void RestartDialog_ShowsDrawMessage()
    {
        // Arrange & Act
        var cut = RenderComponent<RestartDialog>(parameters => parameters
            .Add(p => p.Winner, (Player?)null));

        // Assert
        var message = cut.Find("p");
        Assert.Contains("draw", message.TextContent);
    }

    [Fact]
    public void RestartDialog_HasNewGameButton()
    {
        // Arrange & Act
        var cut = RenderComponent<RestartDialog>(parameters => parameters
            .Add(p => p.Winner, Player.White));

        // Assert
        var button = cut.Find("button");
        Assert.Equal("New Game", button.TextContent);
    }

    [Fact]
    public void RestartDialog_ButtonInvokesCallback()
    {
        // Arrange
        var callbackInvoked = false;
        var cut = RenderComponent<RestartDialog>(parameters => parameters
            .Add(p => p.Winner, Player.White)
            .Add(p => p.OnRestart, () => { callbackInvoked = true; }));

        // Act
        var button = cut.Find("button");
        button.Click();

        // Assert
        Assert.True(callbackInvoked);
    }

    [Fact]
    public void RestartDialog_HasAccessibilityAttributes()
    {
        // Arrange & Act
        var cut = RenderComponent<RestartDialog>(parameters => parameters
            .Add(p => p.Winner, Player.Black));

        // Assert
        var dialog = cut.Find(".restart-dialog");
        Assert.Equal("dialog", dialog.GetAttribute("role"));
        Assert.Equal("true", dialog.GetAttribute("aria-modal"));
        Assert.Equal("restart-title", dialog.GetAttribute("aria-labelledby"));
    }
}
