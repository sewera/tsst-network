using System;
using System.Collections.Generic;
using Common.Models;
using Common.Networking.Client.Persistent;
using NLog;
using NetworkNode.Networking.Forwarding.FIB;

namespace NetworkNode.Networking.Forwarding
{
    public class MplsPacketForwarder : IPacketForwarder
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private Dictionary<string, IPersistentClientPort<MplsPacket>> _clientPorts;

        private ForwardingInformationBase FIB;

        public MplsPacketForwarder()
        {
            FIB = new ForwardingInformationBase();
        }

        public void ForwardPacket((string portAlias, MplsPacket packet) forwardPacketTuple)
        {
            
            if (_clientPorts == null)
            {
                LOG.Warn("Dictionary with clientPorts was not initialized yet");
                return;
            }
            try
            {
                LOG.Info($"Received packet {forwardPacketTuple.packet} on port {forwardPacketTuple.portAlias}");
                (string outPort, MplsPacket outPacket) = FIB.Commutate(forwardPacketTuple);
                for (int i = 0; i < outPacket.MplsLabels.Count; i++)
                {
                    outPacket.MplsLabels[i] = Math.Abs(outPacket.MplsLabels[i]);
                }
                LOG.Info($"Forwarding packet {outPacket} to port {outPort}");
                _clientPorts[outPort].Send(outPacket);
                LOG.Info($"Forwarded packet {outPacket} on port {outPort}");
            }
            catch(Exception e)
            {
                LOG.Info(e.Message);
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

        public void SetClientPorts(Dictionary<string, IPersistentClientPort<MplsPacket>> clientPorts)
        {
            _clientPorts = clientPorts;
        }
    }
}
