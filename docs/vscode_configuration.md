# Visual Studio Code configuration
## Extensions
- C# language support is optional in VS Code, so we need to install it from marketplace.

  [C# for Visual Studio Code (powered by OmniSharp)](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)

- For drawing graphs, UMLs class diagrams and so on, use [Draw.io integration](https://marketplace.visualstudio.com/items?itemName=hediet.vscode-drawio)

## Running programs
There are several ways to run a program while developing it in Visual Studio Code.

- Simply select the program from the list to run in **Run** view (`Ctrl+Shift+D`), which gives you also the option to debug.

- Navigate to the program's project directory in VS Code terminal and run command

  `dotnet run`

  e.g. if you want to run cc.exe navigate to `~/tsst-network/cc` directory and run `dotnet run`.

- Or you can run (following the example above)

  `dotnet run -p cc/cc.csproj`

  in the `~/tsst-network` directory

## Configuration
Insert:
```json
  "files.insertFinalNewline": true
```
in your settings.json. You can access it by going into Command Palette (Ctrl+Shift+P)
and searching "Preferences: Open Settings (JSON)".
