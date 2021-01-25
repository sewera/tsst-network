namespace NetworkNode.Models
{
    public interface ISerializablePacket
    {
        byte[] ToBytes();
    }
}
