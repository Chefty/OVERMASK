namespace client.dto
{
    public class GameOverDto : IDto
    {
        public byte RedScore { get; private set; }
        public byte BlueScore { get; private set; }

        public GameOverDto(CustomMemoryStream ms)
        {
            ReadFromStream(ms);
        }

        public void WriteToStream(CustomMemoryStream ms) { }

        public void ReadFromStream(CustomMemoryStream ms)
        {
            RedScore = ms.ReadByte();
            BlueScore = ms.ReadByte();
        }

        public byte GetScoreByFaction(PlayerFaction faction)
        {
            if(faction == PlayerFaction.Red)
                return RedScore;
            return BlueScore;
        }
    }
}