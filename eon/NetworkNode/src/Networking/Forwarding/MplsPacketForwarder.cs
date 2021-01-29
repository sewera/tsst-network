using System;
using System.Collections.Generic;
using Common.Models;
using Common.Networking.Client.Persistent;
using NetworkNode.Networking.Forwarding.FIB;
using NLog;

namespace NetworkNode.Networking.Forwarding
{
    public class MplsPacketForwarder : IPacketForwarder
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private Dictionary<string, IPersistentClientPort<EonPacket>> _clientPorts;

        private ForwardingInformationBase FIB;

        public MplsPacketForwarder()
        {
            FIB = new ForwardingInformationBase();
        }

        public void ForwardPacket((string portAlias, EonPacket packet) forwardPacketTuple)
        {
            
            if (_clientPorts == null)
            {
                LOG.Warn("Dictionary with clientPorts was not initialized yet");
                return;
            }
            try
            {
                LOG.Debug($"Received packet {forwardPacketTuple.packet} on port {forwardPacketTuple.portAlias}");
                (string outPort, EonPacket outPacket) = FIB.Commutate(forwardPacketTuple);
               
                LOG.Debug($"Forwarding packet {outPacket} to port {outPort}");
                _clientPorts[outPort].Send(outPacket);
                LOG.Debug($"Forwarded packet {outPacket} on port {outPort}");
            }
            catch(Exception e)
            {
                LOG.Debug(e.Message);
            }
        }

        public void ConfigureFromManagementSystem((string portAlias, ManagementPacket packet) managementTuple)
        {
            (string portAlias, ManagementPacket packet) = managementTuple;
            if(packet.CommandType == "add")
            {
                FIB.AddRow(packet.CommandData);
            }
            else if (packet.CommandType == "delete")
            {
                FIB.DeleteRow(packet.CommandData);
            }
            else
            {
                ;;
            }
        }

        public void SetClientPorts(Dictionary<string, IPersistentClientPort<EonPacket>> clientPorts)
        {
            _clientPorts = clientPorts;
        }
    }
}
