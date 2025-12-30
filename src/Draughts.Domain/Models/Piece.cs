namespace Draughts.Domain.Models;

public enum Player
{
    White,
    Black
}

public enum PieceType
{
    Man,
    King
}

public record Piece(Player Owner, PieceType Type);
