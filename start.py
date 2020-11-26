import os
import time
from pathlib import Path


project_root = Path(__file__).parent
config_dir = Path('resources/config')

cc = Path('cc')
cc_csproj = Path('cc.csproj')
cc_config = Path('CableCloud.xml')

cn = Path('cn')
cn_csproj = Path('cn.csproj')
cn_configs = [
    Path('ClientNode1.xml'),
    Path('ClientNode2.xml'),
    Path('ClientNode3.xml'),
    Path('ClientNode4.xml')
]

ms = Path('ms')
ms_csproj = Path('ms.csproj')
ms_config = Path('ManagementSystem.xml')

nn = Path('nn')
nn_csproj = Path('nn.csproj')
nn_configs = [
    Path('NetworkNode1.xml'),
    Path('NetworkNode2.xml'),
    Path('NetworkNode3.xml'),
    Path('NetworkNode4.xml'),
    Path('NetworkNode5.xml'),
    Path('NetworkNode6.xml'),
    Path('NetworkNode7.xml')
]

if os.name != 'nt':
    print('Currently, batch program opening only for Windows')
    exit(1)

os.system(f'start cmd.exe /k "dotnet run -p {(project_root/cc/cc_csproj).absolute()} -- -c {(project_root/cc/config_dir/cc_config).absolute()}"')

os.system(f'start cmd.exe /k "dotnet run -p {(project_root/ms/ms_csproj).absolute()} -- -c {(project_root/ms/config_dir/ms_config).absolute()}"')
time.sleep(2)

for nn_config in nn_configs:
    os.system(f'start cmd.exe /k "dotnet run -p {(project_root/nn/nn_csproj).absolute()} -- -c {(project_root/nn/config_dir/nn_config).absolute()}"')

for cn_config in cn_configs:
    os.system(f'start cmd.exe /k "dotnet run -p {(project_root/cn/cn_csproj).absolute()} -- -c {(project_root/cn/config_dir/cn_config).absolute()}"')

print("Launched all programs. Write 'exit' to close them")

while True:
    cmd = input()
    if cmd.lower() == 'exit':
        print('Killing all CMDs')
        os.system('taskkill /F /IM cmd.exe /T')
        exit(0)
