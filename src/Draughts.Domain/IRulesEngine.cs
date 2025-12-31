using System.Collections.Generic;
using Draughts.Domain.Models;

namespace Draughts.Domain;

public interface IRulesEngine
{
    IEnumerable<Move> GetLegalMoves(Board board, Player player);
    Board ApplyMove(Board board, Move move);
    bool IsGameOver(Board board);
}
