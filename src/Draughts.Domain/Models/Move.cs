namespace Draughts.Domain.Models;

/// <summary>
/// Represents a move from one position to another, optionally capturing pieces.
/// </summary>
/// <param name="FromRow">Starting row.</param>
/// <param name="FromCol">Starting column.</param>
/// <param name="ToRow">Destination row.</param>
/// <param name="ToCol">Destination column.</param>
/// <param name="CapturedPositions">Positions of captured pieces (row, col) in order of capture.</param>
public record Move(int FromRow, int FromCol, int ToRow, int ToCol, IReadOnlyList<(int Row, int Col)>? CapturedPositions = null)
{
    /// <summary>
    /// Indicates whether this move captures any pieces.
    /// </summary>
    public bool IsCapture => CapturedPositions is { Count: > 0 };

    /// <summary>
    /// Creates a simple non-capture move.
    /// </summary>
    public static Move Simple(int fromRow, int fromCol, int toRow, int toCol)
        => new(fromRow, fromCol, toRow, toCol);

    /// <summary>
    /// Creates a capture move with one or more captured positions.
    /// </summary>
    public static Move Capture(int fromRow, int fromCol, int toRow, int toCol, params (int Row, int Col)[] captured)
        => new(fromRow, fromCol, toRow, toCol, captured);
}
