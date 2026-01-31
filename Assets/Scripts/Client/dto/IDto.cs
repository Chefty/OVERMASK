namespace client.dto
{
    public interface IDto
    {
        public void WriteToStream(CustomMemoryStream ms);

        public void ReadFromStream(CustomMemoryStream ms);
    }
}