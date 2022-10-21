namespace Puissance4.Business.DTO
{
    public class LeaveGameDTO
    {
        public LeaveGameDTO()
        {

        }

        public LeaveGameDTO(Guid gameId, bool definitive)
        {
            GameId = gameId;
            Definitive = definitive;
        }

        public Guid GameId { get; set; }
        public bool Definitive { get; set; }
    }
}
