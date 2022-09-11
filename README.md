# dd2-bot

**Obs: the bot is not fully functional yet, see [To Do](#to-do) section**

## Gettings started

1. Restore Nuget packages: `dotnet restore`
2. Get the application storage path: `dotnet run --project src/ConsoleApp settings:get`
3. Store templates on the `Templates path` displayed in the `step 2`
4. Run the bot `dotnet run --project src/ConsoleApp`

## To Do

1. Scrap templates of DD2 build phase
2. Develop an installation script to copy templates automatically
3. Press keys inside the game when a match is found