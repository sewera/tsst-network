namespace NetworkCallController.Model
{
    public class Connection
    {
        public int Id { get; }
        public string SrcName { get; }
        public string SrcPortAlias { get; }
        public string DstName { get; }
        public string DstPortAlias { get; }
        public int SlotsNumber { get; }

        public Connection(int id, string srcName, string srcPortAlias, string dstName, string dstPortAlias, int slotsNumber)
        {
            Id = id;
            SrcName = srcName;
            SrcPortAlias = srcPortAlias;
            DstName = dstName;
            DstPortAlias = dstPortAlias;
            SlotsNumber = slotsNumber;
        }
        public override string ToString()
        {
            return $"[{Id}, {SrcName} : {SrcPortAlias}, {DstName} : {DstPortAlias}, sl={SlotsNumber}]";
        }
    }
}
