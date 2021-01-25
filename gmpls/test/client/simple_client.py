import socket
import msgpack

# creates socket object
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

HOST = socket.gethostname()
PORT = 3001

print(f'Connecting to {HOST}:{PORT}')
try:
    s.connect((HOST, PORT))
except ConnectionError:
    print('Could not connect. Is the server running?')
    exit(1)

SRC_PORT_ALIAS = '1234'
LABEL_LIST = [100, 200]

print('Ctrl-C to quit')
print('Format: <port_alias> [space] <message>')

while True:
    try:
        cmd = input('> ')
        [dest_port_alias, message] = cmd.split(' ')

        mpls = [SRC_PORT_ALIAS, dest_port_alias, LABEL_LIST, message]
        mpls_packed = msgpack.packb(mpls)

        s.send(mpls_packed)
        print('Sent')
    except KeyboardInterrupt:
        print()
        print('Closing client socket')
        s.close()
        exit(0)
