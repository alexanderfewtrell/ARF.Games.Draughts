using System;
using System.Collections.Generic;
using System.Linq;
using Draughts.Api.Dto;
using Draughts.Domain;
using Draughts.Domain.Models;

namespace Draughts.Api.Services;

public interface IAiService
{
    MoveDto? GetMove(BoardStateDto boardState, Player player);
}

/// <summary>
/// Simple AI that prefers captures and selects randomly among equal options.
/// </summary>
public class AiService : IAiService
{
    private readonly IRulesEngine _rules;
    private readonly Random _random = new();

    public AiService(IRulesEngine rules)
    {
        _rules = rules;
    }

    public MoveDto? GetMove(BoardStateDto boardState, Player player)
    {
        var board = MapToBoard(boardState);
        var moves = _rules.GetLegalMoves(board, player).ToList();

        if (moves.Count == 0)
            return null;

        // Prefer captures (the rules engine already enforces mandatory capture,
        // but we explicitly prioritize for clarity)
        var captures = moves.Where(m => m.IsCapture).ToList();
        var selectedMove = captures.Count > 0
            ? SelectBestCapture(captures)
            : SelectRandomMove(moves);

        return MapToDto(selectedMove);
    }

    private Move SelectBestCapture(List<Move> captures)
    {
        // Prefer captures that take more pieces (longer chains)
        var maxCaptures = captures.Max(m => m.CapturedPositions?.Count ?? 0);
        var bestCaptures = captures.Where(m => (m.CapturedPositions?.Count ?? 0) == maxCaptures).ToList();

        return bestCaptures[_random.Next(bestCaptures.Count)];
    }

    private Move SelectRandomMove(List<Move> moves)
    {
        return moves[_random.Next(moves.Count)];
    }

    private static Board MapToBoard(BoardStateDto? boardState)
    {
        var board = new Board();

        if (boardState?.Pieces is null)
            return board;

        foreach (var p in boardState.Pieces)
        {
            if (!Enum.TryParse<Player>(p.Owner, true, out var owner))
                continue;
            if (!Enum.TryParse<PieceType>(p.Type, true, out var type))
                continue;

            board.Set(p.Row, p.Col, new Piece(owner, type));
        }

        return board;
    }

    private static MoveDto MapToDto(Move move)
    {
        var captured = move.CapturedPositions?
            .Select(c => new CapturedPositionDto(c.Row, c.Col))
            .ToList();

        return new MoveDto(move.FromRow, move.FromCol, move.ToRow, move.ToCol, captured);
    }
}
