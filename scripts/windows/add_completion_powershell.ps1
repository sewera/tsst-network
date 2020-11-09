if (!(Test-Path -Path $PROFILE)) {
  New-Item -ItemType File -Path $PROFILE -Force
}

Add-Content -Path $PROFILE -Value '# PowerShell parameter completion shim for the dotnet CLI'
Add-Content -Path $PROFILE -Value 'Register-ArgumentCompleter -Native -CommandName dotnet -ScriptBlock {'
Add-Content -Path $PROFILE -Value '     param($commandName, $wordToComplete, $cursorPosition)'
Add-Content -Path $PROFILE -Value '         dotnet complete --position $cursorPosition "$wordToComplete" | ForEach-Object {'
Add-Content -Path $PROFILE -Value '            [System.Management.Automation.CompletionResult]::new($_, $_, "ParameterValue", $_)'
Add-Content -Path $PROFILE -Value '         }'
Add-Content -Path $PROFILE -Value '}'
