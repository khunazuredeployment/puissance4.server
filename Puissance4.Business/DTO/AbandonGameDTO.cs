namespace Puissance4.Business.DTO
{
    public class AbandonGameDTO
    {
        public AbandonGameDTO()
        {

        }

        public AbandonGameDTO(Guid gameId)
        {
            GameId = gameId;
        }

        public Guid GameId { get; set; }
    }
}
