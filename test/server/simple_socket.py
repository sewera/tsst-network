import socket
import msgpack

ADDRESS = 'localhost'
PORT = 7357

serversocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
serversocket.bind((ADDRESS, PORT))
serversocket.listen(5)  # become a server socket, maximum 5 connections

print(f'Listening on {ADDRESS}:{PORT}. Ctrl-C to quit')

while True:
    try:
        connection, address = serversocket.accept()
        buf = connection.recv(64)
        if len(buf) > 0:
            print(buf)
            print(msgpack.loads(buf))
            buf = b''
    except KeyboardInterrupt:
        print()
        print('Closing socket')
        serversocket.close()
        exit(0)
