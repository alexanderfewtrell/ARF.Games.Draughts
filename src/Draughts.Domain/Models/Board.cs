namespace Draughts.Domain.Models;

public class Board
{
    public const int Size = 8;
    private readonly Piece?[,] _cells = new Piece?[Size, Size];

    public Piece? Get(int row, int col)
    {
        ValidateCoordinates(row, col);
        return _cells[row, col];
    }

    public void Set(int row, int col, Piece? piece)
    {
        ValidateCoordinates(row, col);
        _cells[row, col] = piece;
    }

    private static void ValidateCoordinates(int row, int col)
    {
        if (row < 0 || row >= Size) throw new System.ArgumentOutOfRangeException(nameof(row));
        if (col < 0 || col >= Size) throw new System.ArgumentOutOfRangeException(nameof(col));
    }

    public static Board CreateInitial()
    {
        var b = new Board();
        return b;
    }
}
