namespace client.dto
{
    public class EndRoundDto : IDto
    {
        public PlayerEndRoundDto Player1EndRound { get; private set; }
        public PlayerEndRoundDto Player2EndRound { get; private set; }
        public string PlayerIdOnBottom { get; private set; }
            
        public void WriteToStream(CustomMemoryStream ms)
        {
            
        }

        public void ReadFromStream(CustomMemoryStream ms)
        {
            Player1EndRound = new PlayerEndRoundDto(ms);
            Player2EndRound = new PlayerEndRoundDto(ms);
            PlayerIdOnBottom = ms.ReadString();
        }
    }
}