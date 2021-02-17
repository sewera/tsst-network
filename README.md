# TSST Network Project

## Development setup
See [Development setup](docs/development_setup.md) and [Contributing](docs/contributing.md).

## First stage
See [First stage](docs/stages/1st.md).

## Launch it
Simply go to one of the directories: `mpls` for MPLS and Transport Plane
(first stage) or `eon` for EON and Control Plane (second stage).

Then, using python3 launch `start.py` script. You will have to build the
project first, so `python3 start.py -b` to build. If it fails the first time,
give it a second try, it usually happens that way.

Then simply launch `python3 start.py` and the project will start.

You can kill all the processes with `python3 start.py -k`.

I recommend using PowerShell (or PS in Windows Terminal) when on Windows or
Alacritty when everywhere else.

## Our experience after finishing the project

### Tools
Please DO NOT USE VS Code with the tools from Microsoft. They are a piece of
garbage, as of early 2021. JetBrains' Rider was our IDE of choice. You can't
go wrong with any of their products. Being cross-platform is a huge advantage.

### Scripts
Especially for cross-platform development, choose some modern scripting
language like Python or Ruby. I chose Python, because I was already familiar
with it. There were no problems with it on Windows, Linux and MacOS, which was
tested. Our startup scripts were a bit quick 'n' dirty, so maybe don't rely on
them as they are, but from my experience Python gives you a bit more
flexibility with variables, uniform path handling and so on over shell or
batch scripts.

### Code reuse
The code in `eon/Common` dotnet core project can be reused for future TSST
projects. Most of the classes there are generic, the only thing you have to
provide are some model classes (like RequestPacket for instance).

Such code reuse wouldn't be possible without Dependency Injection programming
principle that is one of the SOLID principles of, well, solid Object Oriented
Programming. Read more about it on [Wikipedia – dependency injection](https://en.wikipedia.org/wiki/Dependency_injection)
and [Wikipedia – SOLID](https://en.wikipedia.org/wiki/SOLID).

#### Data models
I recommend using [MessagePack](https://msgpack.org/) as a serializing library.
Mainly because it's binary, which seemed to be a requirement, but it's also
easy to implement and provides useful methods for (de)serialization.

If you use the code from the Common library, you have to write a model that
implements `ISerializablePacket` interface. A good example on how to do it would
be `Common.Models.GenericDataPacket`, or, without multiple levels of
inheritance, `Common.Models.ManagementPacket`.

Notice the Builder design pattern. From our experience it did an amazing job
at constructing different RequestPackets for different components. I
**strongly** recommend it. For more details on this pattern, I recommend
reading the Gang of Four's book on Design Patterns.

#### Networking
Our networking components, that are basically raw TCP ports and clients are
sometimes a bit rough, but mostly stable.

**Persistently connected ports**: Used between nodes and CableCloud mainly.
They let you send data in whichever direction you want, whenever you want.
They don't normally disconnect, unless you shut down one of the nodes.  
Examples: server: `CableCloud.CableCloudManager._serverPort`, client:
`ClientNode.ClientNodeManager._clientPort`.

**Request-response ports**: Used between Control Plane components. They let
you send a request and get a response. They immediately disconnect after that.
You can, however receive a request, send from that method another request,
making sort of a chain of requests and then the responses will *trace back*
this chain. Consult any CP component, like
`NetworkCallController.NccState.OnCallRequestReceived()` for example. It is
worth noting, that this method, as well as many others were injected ([DI
principle](https://en.wikipedia.org/wiki/Dependency_inversion_principle)) to
the `OneShotServerPort<>` object.

For a full picture on how to use it, study the NetworkCallController
component, as it is the simplest. Oh, and those components are async, so you
need to "pause" the application while the ports are listening. That's why
there's `_idle.WaitOne()`. It just pauses the main thread without putting a
load on a CPU (an empty `while (true)` loop puts an enormous load, so don't do
that).

I recommend wiring up your dependencies as high up in the execution hierarchy as
possible (in our case in a Main() method, which is the highest). This way you
can provide some mock implementations for testing, like
`MockConfigurationParser` that you can find in any component.

### Configuration
For your time's sake, please, PLEASE DO NOT USE XML. It's stupid,
time-consuming and pointless. Use JSON and some library for deserialization.
We thought it was a requirement to use XMLs, but nobody gave a damn, so you
shouldn't too. Whatever you do, don't use xml.

### Startup boilerplate
Your application has to start somehow, some things like loggers need to be
initialized, so you need to write some boilerplate code. Offload it to a
common library, like `Common.Startup`. This piece of code is a bit dirty,
so don't use exactly that, but you get the idea.

### Unit tests
Some bugs could be spotted upfront if we wrote some unit tests earlier for our
util functions. You don't have to test everything, but generally, when you
have a function that checks for the gateway port, it is a good idea to have it
tested. At least it was our main bug factory. You can see an example in
`Common.test.Utils` or `RoutingController.test`.

### Additional libraries
We used and recommend [NLog](https://nlog-project.org/) for logging,
[NUnit](https://nunit.org/) for unit testing and, of course,
[MessagePack](https://msgpack.org/) for serialization.

### Project organization
You can see our tools here on Github. We used built-in Issues, Projects and
Pull requests, as well as [Create Issue Branch plugin](https://github.com/marketplace/actions/create-issue-branch)
which I highly recommend.

------------------------------------------------------------
© 2020-2021 Blazej Sewera, Mateusz Winnicki, Andrzej Gawor
