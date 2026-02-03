namespace client.dto
{
    public class JoinRoomWithCodeDto : IDto
    {
        public int RoomCode { get; private set; }

        public JoinRoomWithCodeDto(int roomCode)
        {
            RoomCode = roomCode;
        }
        
        public void WriteToStream(CustomMemoryStream ms)
        {
            ms.WriteInt(RoomCode);
        }

        public void ReadFromStream(CustomMemoryStream ms) { }
    }
}