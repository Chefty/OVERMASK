namespace client.dto
{
    public class ConnectToRoomWithCodeDto : IDto
    {
        public int RoomCode { get; private set; }
        public string Reason { get; private set; }

        public ConnectToRoomWithCodeDto(CustomMemoryStream ms)
        {
            ReadFromStream(ms);
        }
        
        public void WriteToStream(CustomMemoryStream ms)
        {
        }

        public void ReadFromStream(CustomMemoryStream ms)
        {
            RoomCode = ms.ReadInt();
            Reason = ms.ReadString();
        }
    }
}