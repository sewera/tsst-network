# Get started with tsst-network project

1. Find a convenient shell + console / terminal for you
  - Powershell + Windows Terminal or any other modern terminal emulator for Windows
  - Bash or Zsh + any modern terminal emulator
    - Alacritty / Gnome terminal / VSCode terminal work great
2. Install [Git](https://git-scm.com) either with a package manager or installer
   if you do not already have it
3. Install Python 3
  - On Linux, simply use your package manager. If you are on Arch-based
    or Debian-based system like Ubuntu, you can use installation scripts from
    `scripts/linux/` directory
  - On Windows, use the official [Python 3](https://www.python.org)
    Windows installer or use a package manager like `scoop` or `chocolatey`
    - If you use the official installer, remember to check "Add Python to PATH"
  - On MacOS you can use [Homebrew](https://brew.sh) or official installers
4. Install .NET Core 3.1
  - On Linux, if you used one of the installation scripts, you are all set. If not,
    find a `dotnet` package for your distro
  - On Windows, use the [official installer](https://dotnet.microsoft.com/download/dotnet-core)
  - On MacOS you can use Homebrew or official installers
5. Set up development environment
  1. Add dotnet command completion in your shell (optional)
    - For Bash or Zsh (UNIX-like systems, so Linux, MacOS, etc.) use scripts from `scripts/unix`
    - For Powershell (Windows) use scripts from `scripts/windows`
  2. Configure git (highly recommended)
    - Use scripts from `scripts/unix` or `scripts/windows` where applicable
6. Install and set up your preferred code editor
  - We use VSCode, but it is not mandatory
  - For further info on VSCode configuration and recommended extensions, see [VSCode configuration](./vscode_configuration.md)
