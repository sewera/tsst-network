import argparse
import os
from pathlib import Path
import time


parser = argparse.ArgumentParser(description="Launch tsst-network")
parser.add_argument("-k", "--kill", action="store_true", help="Kill all CMDs")
parser.add_argument(
    "-b",
    "--build",
    action="store_true",
    help="Build solution with Release configuration",
)
parser.add_argument(
    "--forcewindowsimpl",
    action="store_true",
    help="Force the Windows implementation of the script",
)
args = parser.parse_args()

project_root = Path(__file__).parent
config_dir = Path("resources/config")
exec_dir = Path("bin/Release/netcoreapp3.1")
log_dir = Path("logs")

CableCloud = Path("CableCloud")
CableCloud_csproj = Path("CableCloud.csproj")
CableCloud_config = Path("CableCloud.xml")
CableCloud_exe = Path("CableCloud.exe")

ClientNode = Path("ClientNode")
ClientNode_csproj = Path("ClientNode.csproj")
ClientNode_configs = [
    Path("ClientNode1.xml"),
    Path("ClientNode2.xml"),
    Path("ClientNode3.xml"),
    Path("ClientNode4.xml"),
]
ClientNode_exe = Path("ClientNode.exe")

NetworkNode = Path("NetworkNode")
NetworkNode_csproj = Path("NetworkNode.csproj")
NetworkNode_configs = [
    Path("NetworkNode01.xml"),
    Path("NetworkNode02.xml"),
    Path("NetworkNode03.xml"),
    Path("NetworkNode11.xml"),
    Path("NetworkNode12.xml"),
    Path("NetworkNode13.xml"),
    Path("NetworkNode14.xml"),
    Path("NetworkNode21.xml"),
    Path("NetworkNode22.xml"),
    Path("NetworkNode23.xml"),
    Path("NetworkNode31.xml"),
    Path("NetworkNode32.xml"),
]
NetworkNode_exe = Path("NetworkNode.exe")

ConnectionController = Path("ConnectionController")
ConnectionController_csproj = Path("ConnectionController.csproj")
ConnectionController_configs = [
    Path("ConnectionControllerR01.xml"),
    Path("ConnectionControllerR02.xml"),
    Path("ConnectionControllerR03.xml"),
    Path("ConnectionControllerR11.xml"),
    Path("ConnectionControllerR12.xml"),
    Path("ConnectionControllerR13.xml"),
    Path("ConnectionControllerR14.xml"),
    Path("ConnectionControllerR21.xml"),
    Path("ConnectionControllerR22.xml"),
    Path("ConnectionControllerR23.xml"),
    Path("ConnectionControllerR31.xml"),
    Path("ConnectionControllerR32.xml"),
    Path("ConnectionControllerS1.xml"),
    Path("ConnectionControllerS2.xml"),
    Path("ConnectionControllerSN0.xml"),
    Path("ConnectionControllerSN1.xml"),
    Path("ConnectionControllerSN2.xml"),
    Path("ConnectionControllerSN3.xml"),
]
ConnectionController_exe = Path("ConnectionController.exe")

NetworkCallController = Path("NetworkCallController")
NetworkCallController_csproj = Path("NetworkCallController.csproj")
NetworkCallController_configs = [
    Path("NetworkCallControllerS1.xml"),
    Path("NetworkCallControllerS2.xml"),
]
NetworkCallController_exe = Path("NetworkCallController.exe")

RoutingController = Path("RoutingController")
RoutingController_csproj = Path("RoutingController.csproj")
RoutingController_configs = [
    Path("RoutingControllerS1.xml"),
    Path("RoutingControllerS2.xml"),
    Path("RoutingControllerSN0.xml"),
    Path("RoutingControllerSN1.xml"),
    Path("RoutingControllerSN2.xml"),
    Path("RoutingControllerSN3.xml"),
]
RoutingController_exe = Path("RoutingController.exe")

if os.name == "nt" or args.forcewindowsimpl:
    if args.build:
        os.system(
            f'start cmd.exe /k "dotnet build -c Release {(project_root/CableCloud/CableCloud_csproj).absolute()}"'
        )
        os.system(
            f'start cmd.exe /k "dotnet build -c Release {(project_root/NetworkNode/NetworkNode_csproj).absolute()}"'
        )
        os.system(
            f'start cmd.exe /k "dotnet build -c Release {(project_root/ClientNode/ClientNode_csproj).absolute()}"'
        )
        os.system(
            f'start cmd.exe /k "dotnet build -c Release {(project_root/ConnectionController/ConnectionController_csproj).absolute()}"'
        )
        os.system(
            f'start cmd.exe /k "dotnet build -c Release {(project_root/NetworkCallController/NetworkCallController_csproj).absolute()}"'
        )
        os.system(
            f'start cmd.exe /k "dotnet build -c Release {(project_root/RoutingController/RoutingController_csproj).absolute()}"'
        )
        exit(0)

    if args.kill:
        print("Killing all CMDs")
        os.system("taskkill /F /IM cmd.exe /T")
        exit(0)

    os.system(
        f'start cmd.exe /k "{(project_root/CableCloud/exec_dir/CableCloud_exe).absolute()} -c {(project_root/CableCloud/config_dir/CableCloud_config).absolute()} -l {(project_root/log_dir).absolute()}"'
    )

    for rc_config in RoutingController_configs:
        os.system(
            f'start cmd.exe /k "{(project_root/RoutingController/exec_dir/RoutingController_exe).absolute()} -c {(project_root/RoutingController/config_dir/rc_config).absolute()} -l {(project_root/log_dir).absolute()}"'
        )

    time.sleep(2)

    for ncc_config in NetworkCallController_configs:
        os.system(
            f'start cmd.exe /k "{(project_root/NetworkCallController/exec_dir/NetworkCallController_exe).absolute()} -c {(project_root/NetworkCallController/config_dir/ncc_config).absolute()} -l {(project_root/log_dir).absolute()}"'
        )

    for cc_config in ConnectionController_configs:
        os.system(
            f'start cmd.exe /k "{(project_root/ConnectionController/exec_dir/ConnectionController_exe).absolute()} -c {(project_root/ConnectionController/config_dir/cc_config).absolute()} -l {(project_root/log_dir).absolute()}"'
        )

    for nn_config in NetworkNode_configs:
        os.system(
            f'start cmd.exe /k "{(project_root/NetworkNode/exec_dir/NetworkNode_exe).absolute()} -c {(project_root/NetworkNode/config_dir/nn_config).absolute()} -l {(project_root/log_dir).absolute()}"'
        )

    for cn_config in ClientNode_configs:
        os.system(
            f'start cmd.exe /k "{(project_root/ClientNode/exec_dir/ClientNode_exe).absolute()} -c {(project_root/ClientNode/config_dir/cn_config).absolute()} -l {(project_root/log_dir).absolute()}"'
        )
else:
    if args.build:
        os.system(
            f'{os.getenv("TERM")} -e dotnet build -c Release {(project_root/CableCloud/CableCloud_csproj).absolute()}'
        )
        os.system(
            f'{os.getenv("TERM")} -e dotnet build -c Release {(project_root/NetworkNode/NetworkNode_csproj).absolute()}'
        )
        os.system(
            f'{os.getenv("TERM")} -e dotnet build -c Release {(project_root/ClientNode/ClientNode_csproj).absolute()}'
        )
        os.system(
            f'{os.getenv("TERM")} -e dotnet build -c Release {(project_root/ConnectionController/ConnectionController_csproj).absolute()}'
        )
        os.system(
            f'{os.getenv("TERM")} -e dotnet build -c Release {(project_root/NetworkCallController/NetworkCallController_csproj).absolute()}'
        )
        os.system(
            f'{os.getenv("TERM")} -e dotnet build -c Release {(project_root/RoutingController/RoutingController_csproj).absolute()}'
        )
        exit(0)

    if args.kill:
        os.system("killall ClientNode")
        os.system("killall NetworkNode")
        os.system("killall CableCloud")
        os.system("killall ConnectionController")
        os.system("killall NetworkCallController")
        os.system("killall RoutingController")
        exit(0)

    os.popen(
        f'{os.getenv("TERM")} -e {(project_root/CableCloud/exec_dir/CableCloud).absolute()} -c {(project_root/CableCloud/config_dir/CableCloud_config).absolute()} -l {(project_root/log_dir).absolute()}'
    )

    for rc_config in RoutingController_configs:
        os.popen(
            f'{os.getenv("TERM")} -e {(project_root/RoutingController/exec_dir/RoutingController).absolute()} -c {(project_root/RoutingController/config_dir/rc_config).absolute()} -l {(project_root/log_dir).absolute()}'
        )

    time.sleep(2)

    for ncc_config in NetworkCallController_configs:
        os.popen(
            f'{os.getenv("TERM")} -e {(project_root/NetworkCallController/exec_dir/NetworkCallController).absolute()} -c {(project_root/NetworkCallController/config_dir/ncc_config).absolute()} -l {(project_root/log_dir).absolute()}'
        )

    for cc_config in ConnectionController_configs:
        os.popen(
            f'{os.getenv("TERM")} -e {(project_root/ConnectionController/exec_dir/ConnectionController).absolute()} -c {(project_root/ConnectionController/config_dir/cc_config).absolute()} -l {(project_root/log_dir).absolute()}'
        )

    for nn_config in NetworkNode_configs:
        os.popen(
            f'{os.getenv("TERM")} -e {(project_root/NetworkNode/exec_dir/NetworkNode).absolute()} -c {(project_root/NetworkNode/config_dir/nn_config).absolute()} -l {(project_root/log_dir).absolute()}'
        )

    for cn_config in ClientNode_configs:
        os.popen(
            f'{os.getenv("TERM")} -e {(project_root/ClientNode/exec_dir/ClientNode).absolute()} -c {(project_root/ClientNode/config_dir/cn_config).absolute()} -l {(project_root/log_dir).absolute()}'
        )
