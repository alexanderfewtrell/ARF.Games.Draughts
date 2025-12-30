using Draughts.Api.Dto;
using Draughts.Domain.Models;

namespace Draughts.Api.Mappers;

public static class BoardMapper
{
    public static Board ToBoard(BoardStateDto dto)
    {
        var board = new Board();
        if (dto?.Pieces is null)
            return board;

        foreach (var p in dto.Pieces)
        {
            if (!System.Enum.TryParse<Player>(p.Owner, true, out var owner))
                continue;
            if (!System.Enum.TryParse<PieceType>(p.Type, true, out var type))
                continue;

            board.Set(p.Row, p.Col, new Piece(owner, type));
        }

        return board;
    }

    public static BoardStateDto ToDto(Board board)
    {
        var pieces = new List<PieceDto>();
        for (var r = 0; r < Board.Size; r++)
        {
            for (var c = 0; c < Board.Size; c++)
            {
                var piece = board.Get(r, c);
                if (piece is not null)
                    pieces.Add(new PieceDto(r, c, piece.Owner.ToString(), piece.Type.ToString()));
            }
        }
        return new BoardStateDto(pieces);
    }
}
