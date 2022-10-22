namespace Puissance4.Business.DTO
{
    public class DisconnectGameDTO
    {
        public DisconnectGameDTO()
        {

        }

        public DisconnectGameDTO(Guid gameId)
        {
            GameId = gameId;
        }

        public Guid GameId { get; set; }
    }
}
