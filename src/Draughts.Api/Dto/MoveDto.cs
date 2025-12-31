using System.Collections.Generic;

namespace Draughts.Api.Dto;

/// <summary>
/// Represents a position that was captured during a move.
/// </summary>
public record CapturedPositionDto(int Row, int Col);

/// <summary>
/// Represents a move from one position to another, optionally with captured pieces.
/// </summary>
public record MoveDto(
    int FromRow,
    int FromCol,
    int ToRow,
    int ToCol,
    List<CapturedPositionDto>? CapturedPositions = null);
