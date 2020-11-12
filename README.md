# TSST Network Project

## Development setup
See [Development setup](docs/development_setup.md) and [Contributing](docs/contributing.md).

## First stage
See [First stage](docs/stages/1st.md).

<<<<<<< HEAD
## First Stage
### Objective
**IP/MPLS network emulator**

IP is not necessary. We can assume that the packet coming out of a client is
an MPLS packet equipped with one label.

We focus on data and administration aspects.

### What management actually is: FCAPS
- F — fault — recognize, isolate, correct and log faults that occur in the network
- C — configuration — make and store configs, plan future scaling
- A — accounting — billing management
- P — performance — ensuring that network performs at acceptable levels
- S — security — access to network assets control

### Execution
4 programs:
- Network node
- Client node
- Management system
- Cable cloud

#### Example idea of Network node
At least 3 classes for following responsibilities:
- Commutation field
- Input/output port
- Management agent
- Others/helpers

### Network configuration
The program has to be written so that any configuration should be possible.
For presentation, the configuration should be not so complex, but complex
enough to present the full feature set.

#### Example idea of Client node
- Client node
- Interface connector 
- Input/output port
- MPLS package
- Others/helpers

### Client node functionality 
Client node should be able to accept user's input via UI and direct package
to the next Network nodes and ultimately to another Client node through the Cable cloud.
The above should work both ways, so the Client node has to accept packages 
that are coming from remote node.
=======
------------------------------------------------------------
© 2020-2021 Blazej Sewera, Mateusz Winnicki, Andrzej Gawor
>>>>>>> master
