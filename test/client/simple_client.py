import socket

# creates socket object
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

host = socket.gethostname()
port = 3001

s.connect((host, port))
s.send(b'Hello World!')
