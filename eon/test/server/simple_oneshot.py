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

    while True:
        connection, address = serversocket.accept()
        print(f"Connection from {address} accepted")

        buf = connection.recv(4096)
        if len(buf) > 0:
            print(f"Raw buffer: {buf}")
            print(f"Msgpack unpacked: {msgpack.unpackb(buf)}")
            buf = b""

        res = [1, 0, 2137, "nextZonePort", "gateway", (3, 2), "dstZone", [], "end"]
        print()
        print(f"Sending response: {res}")
        msgpack_res = msgpack.packb(res)
        connection.sendall(msgpack_res)
        print()
        connection.shutdown(socket.SHUT_RDWR)
        connection.close()
except KeyboardInterrupt:
    print()
    print("Closing socket")
    serversocket.close()
    exit(0)
