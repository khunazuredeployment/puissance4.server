using Microsoft.AspNetCore.SignalR;
using Puissance4.API.Extensions;
using Puissance4.Business.DTO;
using Puissance4.Business.Services;
using Puissance4.Domain.Enums;

namespace Puissance4.API.Hubs
{
    public class GameHub: Hub
    {
        private readonly GameService _gameService;

        public GameHub(GameService gameService)
        {
            _gameService = gameService;
        }

        public void CreateGame(Color color)
        {
            GameDTO g = _gameService.Create(Context.GetId(), color);
            Groups.AddToGroupAsync(Context.ConnectionId, g.Id.ToString());
            Clients.Caller.SendAsync("joinTable", g);
            Clients.All.SendAsync("allFreeGames", _gameService.Games
                .Where(g => g.Value.YellowUserId == null || g.Value.RedUserId == null)
                .Select(g => new GameDTO(g.Value)));
        }
    }
}
