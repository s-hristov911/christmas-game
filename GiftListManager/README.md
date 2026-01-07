# Santa's Gift List Manager

A WinForms utility for keeping Santa's gift assignments organized. Add or edit present entries, flag recipients as Naughty or Nice, and filter the roster with a single dropdown.

## Prerequisites
- Windows 10 or newer
- [.NET 8 SDK](https://dotnet.microsoft.com/download) (build + run)

## Run the App
```bash
cd GiftListManager
 dotnet run
```

## Features
- Add or update a gift with the recipient name and disposition.
- Remove the selected gift from Santa's roster.
- Quick filter between all assignments, Nice kids only, or Naughty follow-ups.
- Sample seed data shows how the board works when the form loads.

## Project Structure
- `GiftListManager.csproj` — WinForms project file using net8.0-windows.
- `Program.cs` / `ApplicationConfiguration.cs` — App entry point and DPI configuration.
- `Forms/MainForm.*` — Designer and logic for the dashboard layout plus CRUD handlers.
- `Models/GiftItem.cs` — Data model + Naughty/Nice enum.

## Next Ideas
1. Persist the list to JSON so Santa can keep edits between sessions.
2. Add sortable grid columns and search.
3. Wire this UI to the Unity Catch-the-Gifts minigame for shared data.
