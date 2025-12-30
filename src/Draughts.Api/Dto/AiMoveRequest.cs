namespace Draughts.Api.Dto;

public record AiMoveRequest(BoardStateDto Board, string Player);
