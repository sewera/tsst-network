# Contributing to tsst-network project
1. Use English.
2. Place helper scripts in `scripts` directory.
3. Place documentation in `docs` directory if it is project-wide doc
   or in `<program-name>/docs`, e.g. `cc/docs` if it is program-wide one.
4. Write documentation in Markdown where possible.
5. Write code compliant with [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions).
6. Make issues for every task that needs to be done in a project.
7. Remember to take care of an issue on a board (moving issues between To do, In progress, etc.).
8. Name your branches after issue names with kebab-case, e.g. issue named
   "Network node program specification and architecture" will have a branch
   "`network-node-program-specification-and-architecture`".
9. All commit messages must have issue number before anything else, e.g.
   "`#7 Fix logs not being present in logfiles`". This is checked in a pre-commit hook.
10. All commit messages need to be in imperative mode, so "`#2 Add ...`", NOT "`#2 Added ...`"
11. DO NOT commit directly to `master` branch. This way there is no pull request
    and no code review, so mistakes are far more likely to happen.
