using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Puissance4.API.DTO;
using Puissance4.API.Extensions;
using Puissance4.Business.DTO;
using Puissance4.Business.Exceptions;
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

        [Authorize]
        public async Task CreateGame(CreateGameDTO dto)
        {
            GameDetailsDTO? game = _gameService.GetCurrentGame(Context.GetId());
            if(game is not null)
            {
                await AbandonGame(new AbandonGameDTO(game.Id));
            }
            game = await _gameService.Create(Context.GetId(), dto.Color);
            await AddToGroupAsync(game.Id);
            await BroadCastAll(game);
        }

        [Authorize]
        public async Task JoinGame(JoinGameDTO dto)
        {
            try
            {
                GameDetailsDTO? game = _gameService.GetCurrentGame(Context.GetId());
                if (game is not null && game.Id != dto.GameId)
                {
                    await AbandonGame(new AbandonGameDTO(game.Id));
                }
                game = await _gameService.Join(Context.GetId(), dto.GameId);
                await AddToGroupAsync(game.Id);
                await BroadCastAll(game);
            } 
            catch(GameException ex)
            {
                await Clients.Caller.SendAsync("message", new MessageDTO(ex.Message, Enums.Severity.Error));
            }
        }

        [Authorize]
        public async Task DisconnectGame(DisconnectGameDTO dto)
        {
            await Clients.Caller.SendAsync("currentGame", null);
            await RemoveFromGroupAsync(dto.GameId);
            GameDetailsDTO? game = _gameService.GetCurrentGame(Context.GetId());
            if(game is not null)
            {
                game = _gameService.Disconnect(Context.GetId(), dto.GameId);       
                await BroadCastAll(game);
            }
        }

        [Authorize]
        public async Task AbandonGame(AbandonGameDTO dto)
        {
            await Clients.Caller.SendAsync("currentGame", null);
            await RemoveFromGroupAsync(dto.GameId);
            GameDetailsDTO? game = _gameService.GetCurrentGame(Context.GetId());
            if (game is not null)
            {
                string? leaver = game.RedUserId == Context.GetId() ? game.RedUsername : game.YellowUsername;
                game = _gameService.Abandon(Context.GetId(), dto.GameId);
                await Clients.Group(game.Id.ToString()).SendAsync("message", new MessageDTO($"{leaver} a quitté la partie", Enums.Severity.Info, true));
                await BroadCastAll(game);
            }
        }

        [Authorize]
        public async Task Play(PlayDTO dto)
        {
            try
            {
                (CoinDTO play, GameDetailsDTO game) = _gameService.Play(Context.GetId(), dto.GameId, dto.Column);
                await Clients.Group(play.GameId.ToString()).SendAsync("newCoin", play);
                if(game.Status == GameStatus.Closed)
                {
                    await Clients.Group(play.GameId.ToString()).SendAsync("currentGameClosed", game);
                }
            }
            catch (GameException ex)
            {
                await Clients.Caller.SendAsync("message", new MessageDTO(ex.Message, Enums.Severity.Error));
            }
        }

        [Authorize]
        public async Task WatchGame(WatchGameDTO dto)
        {
            GameDetailsDTO? game = _gameService.GetById(dto.GameId);
            if (game is not null)
            {
                await AddToGroupAsync(game.Id);
                await Clients.Caller.SendAsync("currentGame", game);
            }
        }


        public async override Task OnConnectedAsync()
        {
            GameDetailsDTO? game = _gameService.GetCurrentGame(Context.GetId());
            if(game is not null)
            {
                game = _gameService.Reconnect(Context.GetId(), game.Id);
                await AddToGroupAsync(game.Id);
                await Clients.Group(game.Id.ToString()).SendAsync("currentGame", game);
            }
            await Clients.Caller.SendAsync("allGames", _gameService.GetAll());
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            GameDetailsDTO? game = _gameService.GetCurrentGame(Context.GetId());
            if(game is not null)
            {
                await DisconnectGame(new DisconnectGameDTO(game.Id));
            }
        }

        private async Task AddToGroupAsync(Guid gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }

        private async Task RemoveFromGroupAsync(Guid gameId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId.ToString());
        }

        private async Task BroadCastAll(GameDetailsDTO game)
        {
            await Clients.All.SendAsync("allGames", _gameService.GetAll());
            await Clients.Group(game.Id.ToString()).SendAsync("currentGame", game);
        }
    }
}
