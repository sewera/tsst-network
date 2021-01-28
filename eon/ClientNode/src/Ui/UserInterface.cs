using System;
using System.Text.RegularExpressions;
using System.Threading;
using ClientNode.Ui.Parsers;
using ClientNode.Ui.Parsers.Exceptions;
using Common.Models;
using Common.Ui;
using NLog;

namespace ClientNode.Ui
{
    public class UserInterface : IUserInterface
    {
        private readonly ICommandParser _commandParser;
        private readonly IClientNodeManager _clientNodeManager;
        private readonly CpccState _cpccState;
        private readonly string _localName;
        private bool _connected = false;
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        public UserInterface(ICommandParser commandParser, IClientNodeManager clientNodeManager, CpccState cpccState, string localName)
        {
            _commandParser = commandParser;
            _clientNodeManager = clientNodeManager;
            _cpccState = cpccState;
            _localName = localName;
        }

        public void Start()
        {
            _clientNodeManager.Start();
            _clientNodeManager.RegisterReceiveMessageEvent(MessageReceived);
            ThreadStart startCommandParsing = StartCommandParsing;
            LOG.Trace("Creating child thread to handle command parsing");
            Thread commandParsingThread = new Thread(startCommandParsing);
            LOG.Trace("Starting command parsing thread");
            commandParsingThread.Start();
            LOG.Trace("Started thread");
        }

        private void StartCommandParsing()
        {
            Console.WriteLine("Enter destination name and slot range to request from NCC.");
            while (true)
            {
                switch (_connected)
                {
                    case false:
                        Console.WriteLine("Not connected");
                        Console.WriteLine("Input format: <<dst_name>> [space] <<slots_lower>> [space] <<slots_upper>>");
                        Console.Write("> ");
                        string input = Console.ReadLine();

                        try
                        {
                            (string mplsOutLabel, int slotsNumber) = _commandParser.ParseCpccCommand(input);
                            ResponsePacket responsePacket = _cpccState.AskForConnection(_localName, mplsOutLabel, slotsNumber);
                            if (responsePacket.Res == ResponsePacket.ResponseType.Ok)
                            {
                                LOG.Info($"Successfully connected with id: {responsePacket.Id}");
                                _connected = true;
                            }
                            else
                                LOG.Debug($"Connection refused with description: {ResponsePacket.ResponseTypeToString(responsePacket.Res)}");
                        }
                        catch (ParserException e)
                        {
                            LOG.Warn(e.ExceptionMessage);
                        }
                        break;

                    case true:
                        Console.WriteLine("Enter message or 'dc' to disconnect");
                        Console.Write("> ");
                        string inputConn = Console.ReadLine();

                        if (inputConn == null)
                        {
                            LOG.Debug("Input cannot be null");
                            break;
                        }

                        if (Regex.IsMatch(inputConn, "^dc"))
                        {
                            LOG.Info("Disconnecting");
                            ResponsePacket nccTeardownResponse = _cpccState.Teardown();
                            if (nccTeardownResponse.Res == ResponsePacket.ResponseType.Ok)
                            {
                                LOG.Info("Successfully disconnected");
                                _connected = false;
                            }
                        }
                        break;
                }
            }
        }

        private static void MessageReceived((string portAlias, MplsPacket mplsPacket) portAliasAndPacketTuple)
        {
            (_, MplsPacket mplsPacket) = portAliasAndPacketTuple;
            Console.WriteLine($"Received: {mplsPacket}");
            Console.Write("> ");
        }
    }
}
