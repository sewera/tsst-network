using System.Collections.Generic;
using MessagePack;
using NLog;

namespace Common.Models
{
    [MessagePackObject]
    public class RequestPacket : GenericPacket
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        [Key(1)] public int Id;

        [Key(2)] public (int, int) Slots;

        [Key(3)] public List<(int, int)> SlotsArray;

        [Key(4)] public bool ShouldAllocate;

        [Key(5)] public string SrcName;

        [Key(6)] public string DstName;

        [Key(7)] public string SrcPort;

        [Key(8)] public string DstPort;

        [Key(9)] public Who WhoRequests;

        [Key(10)] public string End;

        /// <summary>
        /// Constructor only for MessagePack deserialization
        /// </summary>
        /// To manually create an object, use <see cref="Builder"/>
        public RequestPacket(){}

        protected RequestPacket(int id,
                                (int, int) slots,
                                List<(int, int)> slotsArray,
                                bool shouldAllocate,
                                string srcName,
                                string dstName,
                                string srcPort,
                                string dstPort,
                                Who whoRequests,
                                string end) : base(PacketType.Request)
        {
            Id = id;
            Slots = slots;
            SlotsArray = slotsArray;
            ShouldAllocate = shouldAllocate;
            SrcName = srcName;
            DstName = dstName;
            SrcPort = srcPort;
            DstPort = dstPort;
            WhoRequests = whoRequests;
            End = end;
        }

        public override string ToString()
        {
            return $"[{PacketTypeToString(Type)}, id: {Id}, slots: {Slots}, slotsArray: [{string.Join(", ", SlotsArray)}],\n" +
                   $" shouldAllocate: {ShouldAllocate}, srcName: {SrcName}, dstName: {DstName},\n" +
                   $" srcPort: {SrcPort}, dstPort: {DstPort}, who: {WhoRequestsToString(WhoRequests)}, end: {End}]";
        }

        public class Builder
        {
            private int _id;
            private (int, int) _slots;
            private List<(int, int)> _slotsArray;
            private bool _shouldAllocate;
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

            public Builder SetSlots((int, int) slots)
            {
                _slots = slots;
                return this;
            }

            public Builder SetSlotsArray(List<(int, int)> slotsArray)
            {
                _slotsArray = slotsArray;
                return this;
            }

            public Builder AddSlotsToSlotsArray((int, int) slots)
            {
                _slotsArray ??= new List<(int, int)>();
                _slotsArray.Add(slots);
                return this;
            }

            public Builder SetShouldAllocate(bool shouldAllocate)
            {
                _shouldAllocate = shouldAllocate;
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
                _slotsArray ??= new List<(int, int)>();
                return new RequestPacket(_id,
                    _slots,
                    _slotsArray,
                    _shouldAllocate,
                    _srcName,
                    _dstName,
                    _srcPort,
                    _dstPort,
                    _whoRequests,
                    _end);
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
