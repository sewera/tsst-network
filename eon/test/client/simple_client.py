import argparse
import json
import socket
import msgpack

parser = argparse.ArgumentParser()
parser.add_argument("-p", "--port", type=int, required=True)
args = parser.parse_args()

HOST = "localhost"
PORT = args.port

print("Ctrl-C to quit")
print("Format: json")

while True:
    # creates socket object
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    print(f"Connecting to {HOST}:{PORT}")
    try:
        s.connect((HOST, PORT))
    except ConnectionError:
        print("Could not connect. Is the server running?")
        exit(1)

    try:
        cmd = input("> ")
        data = json.loads(cmd)

        data_bytes = msgpack.packb(data)
        print(f"Sending {data_bytes}")
        s.send(data_bytes)
        print("Sent")

        response_bytes = s.recv(4096)
        print(f"Response bytes: {response_bytes}")
        print(f"Response: {msgpack.unpackb(response_bytes)}")
        print("Closing client socket")
        s.close()
        input("Enter to send another packet, Ctrl-C to quit")
    except KeyboardInterrupt:
        print()
        exit(0)
