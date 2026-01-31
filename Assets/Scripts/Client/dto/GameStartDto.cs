namespace client.dto
{
    public class GameStartDto : IDto
    {
        public PlayerDto Player { get; private set; }
        public PlayerDto Opponent { get; private set; }
        
        public CardsDataDto CardsData { get; private set; }
        
        public GameStartDto()
        {
        }

        public GameStartDto(PlayerDto player, PlayerDto opponent)
        {
            Player = player;
            Opponent = opponent;
        }

        public void WriteToStream(CustomMemoryStream ms)
        {
        }

        public void ReadFromStream(CustomMemoryStream ms)
        {
            Player = new PlayerDto(ms);
            Opponent = new PlayerDto(ms);
            CardsData = new CardsDataDto(ms);
        }
    }
}