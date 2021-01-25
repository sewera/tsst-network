using MessagePack;

namespace Common.Models
{
    [MessagePackObject]
    public class ManagementPacket : ISerializablePacket
    {
        [Key(0)] public string CommandType { get; set; }

        [Key(1)] public string CommandData { get; set; }

        public byte[] ToBytes()
        {
            return MessagePackSerializer.Serialize(this);
        }

        public static ManagementPacket FromBytes(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<ManagementPacket>(bytes);
        }

        public override string ToString()
        {
            return $"[{CommandType}, {CommandData}]";
        }

        private ManagementPacket(string commandType, string commandData)
        {
            CommandType = commandType;
            CommandData = commandData;
        }

        /// <summary>
        /// Constructor only for MessagePack deserialization
        /// </summary>
        /// To manually create ManagementPacket object, use <see cref="Builder"/>
        public ManagementPacket()
        {
        }

        /// <summary>
        /// Builder for ManagementPacket
        /// </summary>
        /// Sample usage
        /// <code>
        /// ManagementPacket packet = new ManagementPacket.Builder()
        ///     .SetCommandType(commandType)
        ///     .SetCommandData(commandData)
        ///     .Build();
        /// </code>
        public class Builder
        {
            private string _commandType = string.Empty;
            private string _commandData = string.Empty;

            public Builder SetCommandType(string commandType)
            {
                _commandType = commandType;
                return this;
            }

            public Builder SetCommandData(string commandData)
            {
                _commandData = commandData;
                return this;
            }

            public ManagementPacket Build()
            {
                return new ManagementPacket(_commandType, _commandData);
            }
        }
    }
}
