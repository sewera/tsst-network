using System;
using System.Collections.Generic;
using MessagePack;
using NLog;

namespace Common.Models
{
    [MessagePackObject]
    public class ResponsePacket : GenericPacket
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        [Key(1)] public ResponseType Res;

        [Key(2)] public int Id;

        [Key(3)] public string NextZonePort;

        [Key(4)] public string Gateway;

        [Key(5)] public (int, int) Slots;

        [Key(6)] public string DstZone;

        [Key(7)] public List<int> SlotsArray;

        /// <summary>
        /// Constructor only for MessagePack deserialization
        /// </summary>
        /// To manually create an object, use <see cref="Builder"/>
        public ResponsePacket()
        {
        }

        protected ResponsePacket(ResponseType res,
                                 int id,
                                 string nextZonePort,
                                 string gateway,
                                 (int, int) slots,
                                 string dstZone,
                                 List<int> slotsArray) : base(PacketType.Response)
        {
            Res = res;
            Id = id;
            NextZonePort = nextZonePort;
            Gateway = gateway;
            Slots = slots;
            DstZone = dstZone;
            SlotsArray = slotsArray;
        }

        public override string ToString()
        {
            return $"[{PacketTypeToString(Type)}, {ResponseTypeToString(Res)}, id: {Id}, nextZonePort: {NextZonePort},\n" +
                   $" gateway: {Gateway}, slots: {Slots}, dstZone: {DstZone}, slotsArray: [{string.Join(", ", SlotsArray)}]]";
        }


        public enum ResponseType
        {
            Ok,
            Refused,
            AuthProblem,
            NoClient,
            NetworkProblem
        }

        public static string ResponseTypeToString(ResponseType res)
        {
            return res switch
            {
                ResponseType.Ok => "OK",
                ResponseType.Refused => "Refused",
                ResponseType.AuthProblem => "AuthProblem",
                ResponseType.NoClient => "NoClient",
                ResponseType.NetworkProblem => "NetworkProblem",
                _ => "Other"
            };
        }

        public class Builder
        {
            private ResponseType _res = ResponseType.Ok;
            private int _id;
            private string _nextZonePort;
            private string _gateway;
            private (int, int) _slots;
            private string _dstZone;
            private List<int> _slotsArray;

            public Builder SetRes(ResponseType res)
            {
                _res = res;
                return this;
            }

            public Builder SetId(int id)
            {
                _id = id;
                return this;
            }

            public Builder SetNextZonePort(string nextZonePort)
            {
                _nextZonePort = nextZonePort;
                return this;
            }

            public Builder SetGateway(string gateway)
            {
                _gateway = gateway;
                return this;
            }

            public Builder SetSlots((int, int) slots)
            {
                _slots = slots;
                return this;
            }

            public Builder SetDstZone(string dstZone)
            {
                _dstZone = dstZone;
                return this;
            }

            public Builder SetSlotsArray(List<int> slotsArray)
            {
                _slotsArray = slotsArray;
                return this;
            }

            public Builder AddSlot(int slot)
            {
                _slotsArray ??= new List<int>();
                _slotsArray.Add(slot);
                return this;
            }

            public ResponsePacket Build()
            {
                _slotsArray ??= new List<int>();
                return new ResponsePacket(_res, _id, _nextZonePort, _gateway, _slots, _dstZone, _slotsArray);
            }
        }
    }
}
