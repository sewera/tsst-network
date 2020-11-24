using System;
using System.Collections.Generic;
using cc.Config;
using cc.Models;
using cc.Networking.Client;
using NLog;

namespace cc.Networking.Forwarding
{
    public class PacketForwarder : IPacketForwarder
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private List<(string, string, bool)> _connectionTable;
        private Dictionary<string, IClientWorker> _clientWorkers;

        public PacketForwarder(Configuration configuration)
        {
            _connectionTable = configuration.ConnectionTable;
        }

        public void ForwardPacket((string, MplsPacket) forwardPacketTuple)
        {
            (string portAlias, MplsPacket packet) = forwardPacketTuple;
            try
            {
                (string portAlias1, string portAlias2, bool isAlive) = _connectionTable.Find(connection =>
                {
                    (string port1, string port2, _) = connection;
                    return port1 == portAlias || port2 == portAlias;
                });

                if (!isAlive)
                {
                    LOG.Warn($"The connection between {portAlias1} and {portAlias2} is dead");
                    return;
                }

                if (portAlias1 == portAlias)
                {
                    LOG.Debug($"Sending {packet} to {portAlias2}");
                    _clientWorkers[portAlias2].Send(packet);
                }
                else
                {
                    LOG.Debug($"Sending {packet} to {portAlias1}");
                    _clientWorkers[portAlias1].Send(packet);
                }
            }
            catch (ArgumentNullException e)
            {
                LOG.Error("SourcePortAlias in incoming MplsPacket did not match any entries in connection table");
            }
        }

        public void SetClientPorts(Dictionary<string, IClientWorker> clientWorkers)
        {
            _clientWorkers = clientWorkers;
        }

        public void SetConnectionAlive((string, string, bool) requestedConnection)
        {
            (string portAlias1, string portAlias2, bool isAlive) = requestedConnection;
            int index = _connectionTable.FindIndex(connection =>
            {
                (string port1, string port2, _) = connection;
                return port1 == portAlias1 && port2 == portAlias2 || port1 == portAlias2 && port2 == portAlias1;
            });
            _connectionTable.RemoveAt(index);
            _connectionTable.Add(requestedConnection);
        }
    }
}
