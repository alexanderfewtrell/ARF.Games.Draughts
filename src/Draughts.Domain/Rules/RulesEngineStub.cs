using System.Collections.Generic;
using System.Linq;
using Draughts.Domain.Models;

namespace Draughts.Domain.Rules;

/// <summary>
/// Spanish draughts rules engine with mandatory capture enforcement.
/// </summary>
public class RulesEngineStub : IRulesEngine
{
    public IEnumerable<Move> GetLegalMoves(Board board, Player player)
    {
        var captures = GetAllCaptures(board, player).ToList();

        // Mandatory capture: if captures exist, only capture moves are legal
        if (captures.Count > 0)
            return captures;

        return GetSimpleMoves(board, player);
    }

    /// <summary>
    /// Gets simple (non-capture) moves for all pieces of a player.
    /// </summary>
    private static IEnumerable<Move> GetSimpleMoves(Board board, Player player)
    {
        for (var r = 0; r < Board.Size; r++)
        for (var c = 0; c < Board.Size; c++)
        {
            var piece = board.Get(r, c);
            if (piece is null || piece.Owner != player)
                continue;

            foreach (var move in GetSimpleMovesForPiece(board, r, c, piece))
                yield return move;
        }
    }

    private static IEnumerable<Move> GetSimpleMovesForPiece(Board board, int r, int c, Piece piece)
    {
        if (piece.Type == PieceType.Man)
        {
            // Men move forward diagonally only
            var dir = piece.Owner == Player.White ? -1 : 1;
            var tr = r + dir;
            if (tr >= 0 && tr < Board.Size)
            {
                if (c - 1 >= 0 && board.Get(tr, c - 1) is null)
                    yield return Move.Simple(r, c, tr, c - 1);
                if (c + 1 < Board.Size && board.Get(tr, c + 1) is null)
                    yield return Move.Simple(r, c, tr, c + 1);
            }
        }
        else
        {
            // Flying King: can move any number of squares diagonally until blocked
            var deltas = new[] { (-1, -1), (-1, 1), (1, -1), (1, 1) };
            foreach (var (dr, dc) in deltas)
            {
                var tr = r + dr;
                var tc = c + dc;
                while (tr >= 0 && tr < Board.Size && tc >= 0 && tc < Board.Size && board.Get(tr, tc) is null)
                {
                    yield return Move.Simple(r, c, tr, tc);
                    tr += dr;
                    tc += dc;
                }
            }
        }
    }

    /// <summary>
    /// Gets all capture moves for all pieces of a player, including multi-capture chains.
    /// </summary>
    private static IEnumerable<Move> GetAllCaptures(Board board, Player player)
    {
        for (var r = 0; r < Board.Size; r++)
        for (var c = 0; c < Board.Size; c++)
        {
            var piece = board.Get(r, c);
            if (piece is null || piece.Owner != player)
                continue;

            // Pass original position (r, c) through the recursion
            foreach (var capture in GetCapturesForPiece(board, r, c, r, c, piece, []))
                yield return capture;
        }
    }

    /// <summary>
    /// Recursively finds all capture chains for a single piece.
    /// </summary>
    /// <param name="board">Current board state.</param>
    /// <param name="origRow">Original starting row of the piece.</param>
    /// <param name="origCol">Original starting column of the piece.</param>
    /// <param name="r">Current row position.</param>
    /// <param name="c">Current column position.</param>
    /// <param name="piece">The piece being moved.</param>
    /// <param name="capturedSoFar">Positions already captured in this chain.</param>
    private static IEnumerable<Move> GetCapturesForPiece(
        Board board,
        int origRow,
        int origCol,
        int r,
        int c,
        Piece piece,
        List<(int Row, int Col)> capturedSoFar)
    {
        var foundCapture = false;
        var directions = GetCaptureDirections(piece);

        foreach (var (dr, dc) in directions)
        {
            foreach (var (midR, midC, landR, landC) in TryCapturesInDirection(board, r, c, dr, dc, piece, capturedSoFar))
            {
                foundCapture = true;

                // Create a temporary board with this capture applied
                var tempBoard = ApplyCaptureToBoard(board, r, c, midR, midC, landR, landC, piece);
                var newCaptured = new List<(int, int)>(capturedSoFar) { (midR, midC) };

                // Check for further captures (multi-capture chain)
                var furtherCaptures = GetCapturesForPiece(tempBoard, origRow, origCol, landR, landC, piece, newCaptured).ToList();

                if (furtherCaptures.Count > 0)
                {
                    // Continue the chain - yield all further captures
                    foreach (var further in furtherCaptures)
                        yield return further;
                }
                else
                {
                    // End of chain, return this capture sequence with original from position
                    yield return Move.Capture(origRow, origCol, landR, landC, newCaptured.ToArray());
                }
            }
        }
    }

    private static (int, int)[] GetCaptureDirections(Piece piece)
    {
        // Men capture diagonally forward only in Spanish draughts
        // Kings can capture in all directions
        if (piece.Type == PieceType.Man)
        {
            var dir = piece.Owner == Player.White ? -1 : 1;
            return [(dir, -1), (dir, 1)];
        }

        return [(-1, -1), (-1, 1), (1, -1), (1, 1)];
    }

    /// <summary>
    /// Attempts captures in the given direction. For men, returns single capture.
    /// For flying kings, scans along diagonal and returns all possible landing squares.
    /// </summary>
    private static IEnumerable<(int MidR, int MidC, int LandR, int LandC)> TryCapturesInDirection(
        Board board,
        int r,
        int c,
        int dr,
        int dc,
        Piece piece,
        List<(int Row, int Col)> alreadyCaptured)
    {
        if (piece.Type == PieceType.Man)
        {
            // Men: single step capture
            var result = TryManCapture(board, r, c, dr, dc, piece, alreadyCaptured);
            if (result.HasValue)
                yield return result.Value;
        }
        else
        {
            // Flying King: scan along diagonal, find opponent, land on any empty square beyond
            foreach (var capture in TryKingCaptures(board, r, c, dr, dc, piece, alreadyCaptured))
                yield return capture;
        }
    }

    private static (int MidR, int MidC, int LandR, int LandC)? TryManCapture(
        Board board,
        int r,
        int c,
        int dr,
        int dc,
        Piece piece,
        List<(int Row, int Col)> alreadyCaptured)
    {
        var midR = r + dr;
        var midC = c + dc;
        var landR = r + 2 * dr;
        var landC = c + 2 * dc;

        // Check bounds
        if (landR < 0 || landR >= Board.Size || landC < 0 || landC >= Board.Size)
            return null;

        // Check if there's an opponent piece to capture
        var midPiece = board.Get(midR, midC);
        if (midPiece is null || midPiece.Owner == piece.Owner)
            return null;

        // Check if this piece was already captured in this chain
        if (alreadyCaptured.Contains((midR, midC)))
            return null;

        // Check if landing square is empty
        if (board.Get(landR, landC) is not null)
            return null;

        return (midR, midC, landR, landC);
    }

    private static IEnumerable<(int MidR, int MidC, int LandR, int LandC)> TryKingCaptures(
        Board board,
        int r,
        int c,
        int dr,
        int dc,
        Piece piece,
        List<(int Row, int Col)> alreadyCaptured)
    {
        // Flying king: scan along diagonal to find first piece
        var scanR = r + dr;
        var scanC = c + dc;

        // Skip empty squares until we find a piece or edge
        while (scanR >= 0 && scanR < Board.Size && scanC >= 0 && scanC < Board.Size)
        {
            var scannedPiece = board.Get(scanR, scanC);
            if (scannedPiece is not null)
            {
                // Found a piece - check if it's an opponent we can capture
                if (scannedPiece.Owner == piece.Owner)
                    yield break; // Own piece blocks

                if (alreadyCaptured.Contains((scanR, scanC)))
                    yield break; // Already captured in this chain

                // It's an opponent piece - check landing squares beyond it
                var landR = scanR + dr;
                var landC = scanC + dc;

                while (landR >= 0 && landR < Board.Size && landC >= 0 && landC < Board.Size)
                {
                    if (board.Get(landR, landC) is not null)
                        yield break; // Blocked by another piece

                    yield return (scanR, scanC, landR, landC);
                    landR += dr;
                    landC += dc;
                }

                yield break; // Only one capture per direction
            }

            scanR += dr;
            scanC += dc;
        }
    }

    private static Board ApplyCaptureToBoard(Board board, int fromR, int fromC, int midR, int midC, int toR, int toC, Piece piece)
    {
        var newBoard = CloneBoard(board);
        newBoard.Set(fromR, fromC, null);
        newBoard.Set(midR, midC, null); // Remove captured piece
        newBoard.Set(toR, toC, piece);
        return newBoard;
    }

    private static Board CloneBoard(Board board)
    {
        var newBoard = new Board();
        for (var r = 0; r < Board.Size; r++)
            for (var c = 0; c < Board.Size; c++)
                newBoard.Set(r, c, board.Get(r, c));
        return newBoard;
    }

    public Board ApplyMove(Board board, Move move)
    {
        var newBoard = CloneBoard(board);

        var piece = newBoard.Get(move.FromRow, move.FromCol);
        if (piece is null)
            return newBoard;

        // Remove captured pieces
        if (move.CapturedPositions is not null)
        {
            foreach (var (capturedRow, capturedCol) in move.CapturedPositions)
                newBoard.Set(capturedRow, capturedCol, null);
        }

        // Kinging: if a man reaches opponent's last rank, promote
        var promoted = piece;
        if (piece.Type == PieceType.Man)
        {
            if (piece.Owner == Player.White && move.ToRow == 0)
                promoted = new Piece(piece.Owner, PieceType.King);
            else if (piece.Owner == Player.Black && move.ToRow == Board.Size - 1)
                promoted = new Piece(piece.Owner, PieceType.King);
        }

        newBoard.Set(move.ToRow, move.ToCol, promoted);
        newBoard.Set(move.FromRow, move.FromCol, null);

        return newBoard;
    }

    public bool IsGameOver(Board board)
    {
        // Game over when no legal moves for either player
        var whiteMoves = GetLegalMoves(board, Player.White);
        if (whiteMoves.Any())
            return false;
        var blackMoves = GetLegalMoves(board, Player.Black);
        return !blackMoves.Any();
    }
}
