using System;
using System.Collections.Generic;
using NLog;
using nn.Config;
using nn.Models;
using nn.Networking.Forwarding.FIB;

namespace nn.Networking.Forwarding
{
    public class MplsPacketForwarder : IPacketForwarder
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly Configuration _configuration;
        private Dictionary<string, IPort<MplsPacket>> _clientPorts;

        private ForwardingInformationBase FIB;

        public MplsPacketForwarder(Configuration configuration)
        {
            _configuration = configuration;
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

        public void SetClientPorts(Dictionary<string, IPort<MplsPacket>> clientPorts)
        {
            _clientPorts = clientPorts;
        }
    }
}
