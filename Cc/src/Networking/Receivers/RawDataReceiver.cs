using System;
using System.Net.Sockets;
using System.Text;
using NLog;

namespace Cc.Networking.Receivers
{
    public class RawDataReceiver : IDataReceiver
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private byte[] _buffer;
        private readonly Socket _receiveSocket;

        public RawDataReceiver(Socket receiveSocket)
        {
            _receiveSocket = receiveSocket;
        }

        public void StartReceiving()
        {
            LOG.Trace("StartReceiving");
            try
            {
                _buffer = new byte[4];
                _receiveSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                LOG.Error($"Error in method BeginReceive. _buffer: {_buffer}");
                LOG.Error(e);
            }
        }

        public void ReceiveCallback(IAsyncResult asyncResult)
        {
            LOG.Trace("ReceiveCallback");
            try
            {
                if (_receiveSocket.EndReceive(asyncResult) > 1)
                {
                    _buffer = new byte[BitConverter.ToInt32(_buffer, 0)];
                    LOG.Trace("Buffer created");
                    _receiveSocket.Receive(_buffer, _buffer.Length, SocketFlags.None);
                    LOG.Trace("_receiveSocket.Receive");
                    LOG.Debug($"Received buffer: {_buffer[0]}");
                    LOG.Info($"Received data: {Encoding.ASCII.GetString(_buffer)}");
                    StartReceiving();
                }
                else
                {
                    Disconnect();
                }
            }
            catch (Exception e)
            {
                LOG.Trace("Catch clause");
                LOG.Error(e);
                if (!_receiveSocket.Connected)
                {
                    Disconnect();
                }
                else
                {
                    StartReceiving();
                }
            }
        }

        public void Disconnect()
        {
            LOG.Trace("Disconnect");
            _receiveSocket.Disconnect(true);
        }
    }
}
