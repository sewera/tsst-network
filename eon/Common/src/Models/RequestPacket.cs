using MessagePack;
using NLog;

namespace Common.Models
{
    [MessagePackObject]
    public class RequestPacket : GenericPacket
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        [Key(1)] public int Id;

        [Key(2)] public int SlotsNumber;

        [Key(3)] public string SrcName;

        [Key(4)] public string DstName;

        [Key(5)] public string SrcPort;

        [Key(6)] public string DstPort;

        [Key(7)] public Who WhoRequests;

        [Key(8)] public string End;

        /// <summary>
        /// Constructor only for MessagePack deserialization
        /// </summary>
        /// To manually create an object, use <see cref="Builder"/>
        public RequestPacket(){}

        protected RequestPacket(int id,
                                int slotsNumber,
                                string srcName,
                                string dstName,
                                string srcPort,
                                string dstPort,
                                Who whoRequests,
                                string end) : base(PacketType.Request)
        {
            Id = id;
            SlotsNumber = slotsNumber;
            SrcName = srcName;
            DstName = dstName;
            SrcPort = srcPort;
            DstPort = dstPort;
            WhoRequests = whoRequests;
            End = end;
        }

        public override string ToString()
        {
            return $"[{PacketTypeToString(Type)}, id: {Id}, slotsNumber: {SlotsNumber}, srcName: {SrcName}, dstName: {DstName},\n" +
                   $" srcPort: {SrcPort}, dstPort: {DstPort}, who: {WhoRequestsToString(WhoRequests)}, end: {End}]";
        }

        public class Builder
        {
            private int _id;
            private int _slotsNumber;
            private string _srcName = string.Empty;
            private string _dstName = string.Empty;
            private string _srcPort = string.Empty;
            private string _dstPort = string.Empty;
            private Who _whoRequests = Who.NotSet;
            private string _end = string.Empty;

            public Builder SetId(int id)
            {
                _id = id;
                return this;
            }

            public Builder SetSlotsNumber(int slotsNumber)
            {
                _slotsNumber = slotsNumber;
                return this;
            }

            public Builder SetSrcName(string srcName)
            {
                _srcName = srcName;
                return this;
            }

            public Builder SetDstName(string dstName)
            {
                _dstName = dstName;
                return this;
            }

            public Builder SetSrcPort(string srcPort)
            {
                _srcPort = srcPort;
                return this;
            }

            public Builder SetDstPort(string dstPort)
            {
                _dstPort = dstPort;
                return this;
            }

            public Builder SetWhoRequests(Who whoRequests)
            {
                _whoRequests = whoRequests;
                return this;
            }

            public Builder SetEnd(string end)
            {
                _end = end;
                return this;
            }

            public RequestPacket Build()
            {
                return new RequestPacket(_id, _slotsNumber, _srcName, _dstName, _srcPort, _dstPort, _whoRequests, _end);
            }
        }

        public static string WhoRequestsToString(Who whoRequests)
        {
            return whoRequests switch
            {
                Who.Cc => "CC Requests",
                Who.Lrm => "LRM Requests",
                _ => "_"
            };
        }

        public enum Who
        {
            NotSet,
            Lrm,
            Cc
        }
    }
}
