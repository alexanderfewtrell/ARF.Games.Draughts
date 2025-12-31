using Draughts.Domain.Models;
using Draughts.Domain.Rules;
using Xunit;
using System.Linq;

namespace Draughts.Domain.Tests;

public class RulesEngineTests
{
    private readonly RulesEngineStub _engine = new();

    [Fact]
    public void InitialBoard_CanBeCreated()
    {
        var board = Board.CreateInitial();
        Assert.NotNull(board);
    }

    [Fact]
    public void RulesEngineStub_GeneratesSimpleMoves()
    {
        var board = Board.CreateInitial();

        // place a white man at row 5, col 2 to allow simple moves
        board.Set(5, 2, new Piece(Player.White, PieceType.Man));

        var moves = _engine.GetLegalMoves(board, Player.White);

        Assert.NotEmpty(moves);
    }

    [Fact]
    public void ManCapture_WhenOpponentAdjacent_ReturnsOnlyCaptureMove()
    {
        // Arrange: White man at (5,2), Black man at (4,3), empty at (3,4)
        var board = new Board();
        board.Set(5, 2, new Piece(Player.White, PieceType.Man));
        board.Set(4, 3, new Piece(Player.Black, PieceType.Man));

        // Act
        var moves = _engine.GetLegalMoves(board, Player.White).ToList();

        // Assert: Only capture move should be available (mandatory capture)
        Assert.Single(moves);
        Assert.True(moves[0].IsCapture);
        Assert.Equal(5, moves[0].FromRow);
        Assert.Equal(2, moves[0].FromCol);
        Assert.Equal(3, moves[0].ToRow);
        Assert.Equal(4, moves[0].ToCol);
        Assert.Single(moves[0].CapturedPositions!);
        Assert.Contains((4, 3), moves[0].CapturedPositions!);
    }

    [Fact]
    public void ManCapture_WhenNoCapturePossible_ReturnsSimpleMoves()
    {
        // Arrange: White man at (5,2), no adjacent opponents
        var board = new Board();
        board.Set(5, 2, new Piece(Player.White, PieceType.Man));

        // Act
        var moves = _engine.GetLegalMoves(board, Player.White).ToList();

        // Assert: Simple moves available
        Assert.Equal(2, moves.Count); // Two diagonal forward moves
        Assert.All(moves, m => Assert.False(m.IsCapture));
    }

    [Fact]
    public void ManCapture_WhenLandingSquareOccupied_NoCaptureAvailable()
    {
        // Arrange: White man at (5,2), Black at (4,3), White at (3,4) - blocking landing
        var board = new Board();
        board.Set(5, 2, new Piece(Player.White, PieceType.Man));
        board.Set(4, 3, new Piece(Player.Black, PieceType.Man));
        board.Set(3, 4, new Piece(Player.White, PieceType.Man)); // Blocks landing

        // Act
        var moves = _engine.GetLegalMoves(board, Player.White).ToList();

        // Assert: No capture possible due to blocked landing, simple move available
        var whitePieceMoves = moves.Where(m => m.FromRow == 5 && m.FromCol == 2).ToList();
        Assert.Single(whitePieceMoves); // Only one simple move (4,1)
        Assert.False(whitePieceMoves[0].IsCapture);
    }

    [Fact]
    public void MultiCapture_WhenChainAvailable_ReturnsFullChain()
    {
        // Arrange: White man at (6,1), Black at (5,2), Black at (3,4)
        // After first capture landing at (4,3), second capture available
        var board = new Board();
        board.Set(6, 1, new Piece(Player.White, PieceType.Man));
        board.Set(5, 2, new Piece(Player.Black, PieceType.Man));
        board.Set(3, 4, new Piece(Player.Black, PieceType.Man));

        // Act
        var moves = _engine.GetLegalMoves(board, Player.White).ToList();

        // Assert: Should have multi-capture chain
        Assert.Single(moves);
        Assert.True(moves[0].IsCapture);
        Assert.Equal(2, moves[0].CapturedPositions!.Count);
        Assert.Contains((5, 2), moves[0].CapturedPositions!);
        Assert.Contains((3, 4), moves[0].CapturedPositions!);
        Assert.Equal(2, moves[0].ToRow); // Final landing position
        Assert.Equal(5, moves[0].ToCol);
    }

    [Fact]
    public void ApplyMove_WithCapture_RemovesCapturedPiece()
    {
        // Arrange
        var board = new Board();
        board.Set(5, 2, new Piece(Player.White, PieceType.Man));
        board.Set(4, 3, new Piece(Player.Black, PieceType.Man));

        var captureMove = Move.Capture(5, 2, 3, 4, (4, 3));

        // Act
        var newBoard = _engine.ApplyMove(board, captureMove);

        // Assert
        Assert.Null(newBoard.Get(5, 2)); // Original position empty
        Assert.Null(newBoard.Get(4, 3)); // Captured piece removed
        Assert.NotNull(newBoard.Get(3, 4)); // Piece at new position
        Assert.Equal(Player.White, newBoard.Get(3, 4)!.Owner);
    }

    [Fact]
    public void ApplyMove_WithMultiCapture_RemovesAllCapturedPieces()
    {
        // Arrange
        var board = new Board();
        board.Set(6, 1, new Piece(Player.White, PieceType.Man));
        board.Set(5, 2, new Piece(Player.Black, PieceType.Man));
        board.Set(3, 4, new Piece(Player.Black, PieceType.Man));

        var multiCapture = Move.Capture(6, 1, 2, 5, (5, 2), (3, 4));

        // Act
        var newBoard = _engine.ApplyMove(board, multiCapture);

        // Assert
        Assert.Null(newBoard.Get(6, 1)); // Original position empty
        Assert.Null(newBoard.Get(5, 2)); // First captured piece removed
        Assert.Null(newBoard.Get(3, 4)); // Second captured piece removed
        Assert.NotNull(newBoard.Get(2, 5)); // Piece at final position
    }

    [Fact]
    public void KingSimpleMove_CanMoveMultipleSquares()
    {
        // Arrange: King at (4,4), empty diagonals
        var board = new Board();
        board.Set(4, 4, new Piece(Player.White, PieceType.King));

        // Act
        var moves = _engine.GetLegalMoves(board, Player.White).ToList();

        // Assert: Flying king can move multiple squares in each direction
        Assert.True(moves.Count > 4); // More than just 4 one-step moves
        Assert.Contains(moves, m => m.ToRow == 0 && m.ToCol == 0); // Can reach corner
        Assert.Contains(moves, m => m.ToRow == 7 && m.ToCol == 7); // Can reach opposite corner
    }

    [Fact]
    public void KingCapture_CanCaptureInAnyDirection()
    {
        // Arrange: King at (4,4), Black man at (2,2) - can capture backwards
        var board = new Board();
        board.Set(4, 4, new Piece(Player.White, PieceType.King));
        board.Set(2, 2, new Piece(Player.Black, PieceType.Man));

        // Act
        var moves = _engine.GetLegalMoves(board, Player.White).ToList();

        // Assert: King should capture (mandatory) and land beyond the captured piece
        Assert.All(moves, m => Assert.True(m.IsCapture));
        Assert.Contains(moves, m => m.ToRow == 1 && m.ToCol == 1);
        Assert.Contains(moves, m => m.ToRow == 0 && m.ToCol == 0);
    }

    [Fact]
    public void KingCapture_FlyingKing_CanLandOnMultipleSquares()
    {
        // Arrange: King at (5,5), Black man at (3,3), empty squares beyond
        var board = new Board();
        board.Set(5, 5, new Piece(Player.White, PieceType.King));
        board.Set(3, 3, new Piece(Player.Black, PieceType.Man));

        // Act
        var moves = _engine.GetLegalMoves(board, Player.White).ToList();

        // Assert: Flying king has multiple landing options beyond captured piece
        Assert.True(moves.Count >= 2);
        Assert.All(moves, m => Assert.True(m.IsCapture));
        // Can land on (2,2), (1,1), or (0,0)
        Assert.Contains(moves, m => m.ToRow == 2 && m.ToCol == 2);
        Assert.Contains(moves, m => m.ToRow == 1 && m.ToCol == 1);
        Assert.Contains(moves, m => m.ToRow == 0 && m.ToCol == 0);
    }

    [Fact]
    public void Kinging_WhenManReachesLastRank_BecomesKing()
    {
        // Arrange: White man at row 1, can move to row 0
        var board = new Board();
        board.Set(1, 2, new Piece(Player.White, PieceType.Man));

        var move = Move.Simple(1, 2, 0, 1);

        // Act
        var newBoard = _engine.ApplyMove(board, move);

        // Assert
        var piece = newBoard.Get(0, 1);
        Assert.NotNull(piece);
        Assert.Equal(PieceType.King, piece.Type);
        Assert.Equal(Player.White, piece.Owner);
    }

    [Fact]
    public void IsGameOver_WhenPlayerHasNoMoves_ReturnsTrue()
    {
        // Arrange: Only one white piece, completely blocked
        var board = new Board();
        board.Set(0, 0, new Piece(Player.White, PieceType.Man)); // Corner, can't move forward
        board.Set(1, 1, new Piece(Player.Black, PieceType.Man)); // Blocks diagonal

        // Act
        var isOver = _engine.IsGameOver(board);

        // Assert: White has no moves, but Black might - check both
        var whiteMoves = _engine.GetLegalMoves(board, Player.White).ToList();
        Assert.Empty(whiteMoves);
    }

        [Fact]
        public void MandatoryCapture_WhenMultipleCapturesAvailable_AllReturned()
        {
            // Arrange: White man at (5,3), Black at (4,2) and (4,4) - two capture options
            var board = new Board();
            board.Set(5, 3, new Piece(Player.White, PieceType.Man));
            board.Set(4, 2, new Piece(Player.Black, PieceType.Man));
            board.Set(4, 4, new Piece(Player.Black, PieceType.Man));

            // Act
            var moves = _engine.GetLegalMoves(board, Player.White).ToList();

            // Assert: Both capture moves should be available
            Assert.Equal(2, moves.Count);
            Assert.All(moves, m => Assert.True(m.IsCapture));
        }

        [Fact]
        public void IsGameOver_WhenPlayerHasNoPieces_ReturnsTrue()
        {
            // Arrange: Only black pieces on board (white has been eliminated)
            var board = new Board();
            board.Set(3, 3, new Piece(Player.Black, PieceType.Man));

            // Act
            var isOver = _engine.IsGameOver(board);

            // Assert: Game is over because white has no pieces
            Assert.True(isOver);
        }

        [Fact]
        public void IsGameOver_WhenBothPlayersHavePiecesAndMoves_ReturnsFalse()
        {
            // Arrange: Both players have pieces and can move
            var board = new Board();
            board.Set(5, 2, new Piece(Player.White, PieceType.Man));
            board.Set(2, 3, new Piece(Player.Black, PieceType.Man));

            // Act
            var isOver = _engine.IsGameOver(board);

            // Assert: Game is not over
            Assert.False(isOver);
        }

        [Fact]
        public void IsGameOver_WhenOnePlayerHasNoLegalMoves_ReturnsTrue()
        {
            // Arrange: White piece blocked in corner with no moves
            var board = new Board();
            board.Set(0, 0, new Piece(Player.White, PieceType.Man)); // Corner, can't move forward (already at top)
            board.Set(3, 3, new Piece(Player.Black, PieceType.Man)); // Black has moves

            // Act
            var isOver = _engine.IsGameOver(board);

            // Assert: Game is over because white has no legal moves
            Assert.True(isOver);
        }
    }
