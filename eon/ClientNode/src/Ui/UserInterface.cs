using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private readonly ConcurrentDictionary<string, (int, int)> _currentSlots = new ConcurrentDictionary<string, (int, int)>();
        
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
                        if (_currentSlots.IsEmpty)
                            Console.WriteLine("Not connected");
                        else
                        {
                            Console.WriteLine($"Connected with ID(s): [{string.Join(", ", _currentSlots.Keys)}]");
                            Console.WriteLine("Add another connection");
                        }

                        Console.WriteLine("Input format: <<dst_name>> [space] <<required slots number>>");
                        Console.Write("> ");
                        string input = Console.ReadLine();

                        try
                        {
                            (string mplsOutLabel, int slotsNumber) = _commandParser.ParseCpccCommand(input);
                            ResponsePacket responsePacket = _cpccState.AskForConnection(_localName, mplsOutLabel, slotsNumber);
                            if (responsePacket.Res == ResponsePacket.ResponseType.Ok)
                            {
                                _currentSlots[responsePacket.Id.ToString()] = responsePacket.Slots;
                                _connected = true;
                                LOG.Info($"Received NCC::CallRequest_res(res = OK, id = {responsePacket.Id}, slots = {responsePacket.Slots})");

                            }
                            else
                                LOG.Debug($"Received NCC::CallRequest_res(res = {ResponsePacket.ResponseTypeToString(responsePacket.Res)})");
                        }
                        catch (ParserException e)
                        {
                            LOG.Warn(e.ExceptionMessage);
                        }
                        break;

                    case true:
                        Console.WriteLine("Enter message in the following format 'send <connection_id> <message>',");
                        Console.WriteLine("'add' to add another connection");
                        Console.WriteLine("or 'dc <connection_id>' to disconnect");
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
                            string[] message = inputConn.Split(' ', 2);
                            try
                            {
                                if (!_currentSlots.Keys.Contains(message[1]))
                                {
                                    LOG.Error($"There is no such Connection ID. Available options: [{string.Join(", ", _currentSlots.Keys)}]");
                                    break;
                                }
                                int connId = int.Parse(message[1]);
                                ResponsePacket nccTeardownResponse = _cpccState.Teardown(connId);
                                if (nccTeardownResponse.Res == ResponsePacket.ResponseType.Ok)
                                {
                                    LOG.Info("Successfully disconnected");
                                    _currentSlots.Remove(message[1], out (int, int) removedValue);
                                    LOG.Info($"Removed Connection with ID: {message[1]} that had slots: {removedValue}");
                                    _connected = false;
                                }
                            }
                            catch (FormatException)
                            {
                                LOG.Error("Connection ID could not be parsed: integer parse error");
                            }
                        }

                        if (Regex.IsMatch(inputConn, "^add"))
                        {
                            LOG.Info("Keeping current connection");
                            _connected = false;
                        }

                        if (Regex.IsMatch(inputConn, "^send"))
                        {
                            string[] message = inputConn.Split(' ', 3);
                            try
                            {
                                _clientNodeManager.Send(message[2], message[1], _currentSlots[message[1]]);
                            }
                            catch (KeyNotFoundException)
                            {
                                LOG.Error("Connection id not found");
                            }
                        }

                        break;
                }
            }
        }

        private static void MessageReceived((string portAlias, EonPacket mplsPacket) portAliasAndPacketTuple)
        {
            (_, EonPacket mplsPacket) = portAliasAndPacketTuple;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Received EonPacket: {mplsPacket}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("> ");
        }
    }
}
