import argparse
import socket
import msgpack

parser = argparse.ArgumentParser()
parser.add_argument("-p", "--port", type=int, required=True)
args = parser.parse_args()

ADDRESS = "localhost"
PORT = args.port

try:
    serversocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    serversocket.bind((ADDRESS, PORT))
    serversocket.listen(5)  # become a server socket, maximum 5 connections

    print(f"Listening on {ADDRESS}:{PORT}. Ctrl-C to quit")

    connection, address = serversocket.accept()
    print(f"Connection from {address} accepted")

    while True:
        buf = connection.recv(512)
        if len(buf) > 0:
            print(f"Raw buffer: {buf}")
            print(f"Msgpack unpacked: {msgpack.loads(buf)}")
            buf = b""
except KeyboardInterrupt:
    print()
    print("Closing socket")
    serversocket.close()
    exit(0)
