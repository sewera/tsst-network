using System;
using System.Collections.Generic;
using MessagePack;

namespace Common.Models
{
    [MessagePackObject]
    public class EonPacket : ISerializablePacket
    {
        [Key(1)] public (int, int) Slots  { get; set; }
        [Key(2)] public string SrcPort { get; set; }
        [Key(3)] public string Message { get; set; }

        public byte[] ToBytes()
        {
            return MessagePackSerializer.Serialize(this);
        }

        public static EonPacket FromBytes(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<EonPacket>(bytes);
        }

        public override string ToString()
        {
            return $"[{Slots}, {Message}]";
        }

        private EonPacket((int, int) slots, string srcPort, string message)
        {
            Slots = slots;
            SrcPort = srcPort;
            Message = message;
        }

        public string GetKey()
        {
            return SrcPort;
        }
        
        /// <summary>
        /// Constructor only for MessagePack deserialization
        /// </summary>
        /// To manually create MplsPacket object, use <see cref="Builder"/>
        public EonPacket() {}

        /// <summary>
        /// Builder for EonPacket
        /// </summary>
        /// Sample usage
        /// <code>
        /// EonPacket packet = new EonPacket.Builder()
        ///     .SetSlots((69,420))
        ///     .SetSrcPort("1234")
        ///     .SetMessage("Hello")
        ///     .Build();
        /// </code>
        public class Builder
        {
            private (int, int) _slots = (0, 0);
            private string _srcPort = String.Empty;
            private string _message = String.Empty;
            
            public Builder SetSlots((int, int) slots)
            {
                _slots = slots;
                return this;
            }
            
            public Builder SetSrcPort(string srcPort)
            {
                _srcPort = srcPort;
                return this;
            }
            
            public Builder SetMessage(string message)
            {
                _message = message;
                return this;
            }
            
            public EonPacket Build()
            {
                return new EonPacket(_slots, _srcPort, _message);
            }
        }
    }
}
