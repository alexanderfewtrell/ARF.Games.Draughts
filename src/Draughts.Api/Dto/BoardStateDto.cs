using System.Collections.Generic;

namespace Draughts.Api.Dto;

public record BoardStateDto(IEnumerable<PieceDto>? Pieces);
