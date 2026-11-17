# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Razor Community Edition: a Windows assistant for Ultima Online. The project aims at "quality of life" improvements while keeping the original Razor developers' approach, and it avoids the heavy automation and scripting found in other UO assistants. Weigh new features against that goal. ClassicUO is the main supported client. The original OSI 5.x/7.x clients still work, but they get limited support.

## Build

You need Visual Studio (MSBuild) with the C++ workload (toolsets v142/v143), and the .NET Framework 4.7.2 targeting pack. The solution mixes old-style (non-SDK) C# projects that use `packages.config` with native `.vcxproj` projects, so `dotnet build` won't work. Use `msbuild`, as CI does (`.github/workflows/build-razor.yml`):

```
msbuild Razor.sln /t:Restore /p:Configuration=Release /p:RestorePackagesConfig=true
msbuild Razor.sln /t:Build /p:Configuration=Release /p:Platform=x64
msbuild Razor.sln /t:Build /p:Configuration=Release /p:Platform="Any CPU"   # maps to the x86 build
```

- Output goes to `bin/x64/<Config>/` and `bin/Win32/<Config>/`. A post-build step xcopies everything in `etc/` (language files, `*.def`, `*.xml` data, native DLLs) into both output folders.
- There is no test project. CI only checks that the solution builds. `build-razor.yml` is the one shared build definition. `build-only.yml` calls it for PRs, and `build.yml` calls it for pushes to `main` and for releases.
- The docs site in `help/` is built with MkDocs Material (`cd help && pip install -r requirements.txt && mkdocs build`). CI first installs a patched Pygments 2.21.0 that registers the `help/pygments.*.razor` lexer and style for Razor script syntax highlighting (see `build-site.yml`). Only pushes to `main` deploy the site.

## Versioning and releases

- The version is `Major.Minor.<run number>.0`. Major.Minor comes from `Razor/Properties/AssemblyInfo.cs`, which is checked in as `x.y.0.0`. CI stamps the run number of `build.yml` at build time and checks that both `Razor.exe` builds carry exactly that version.
- Every push to `main` replaces the `Razor-dev-preview` pre-release.
- To release, go to Actions → **Build & Release** → Run workflow on `main`. This creates a draft release `v<version>` with `Razor-x86-<version>.zip` and `Razor-x64-<version>.zip` and auto-generated notes. Edit the notes and publish the draft. Then update the download links in `help/docs/download.md`.
- For a new minor version, bump `AssemblyInfo.cs` (for example to `1.11.0.0`) before running the release.

## Code constraints

- `Razor/Razor.csproj` is an old-style project with an explicit `<Compile Include=...>` list. **Add every new `.cs` file to the csproj by hand**, or it won't compile.
- The target is .NET Framework 4.7.2 with `LangVersion` 7.3. Don't use C# 8+ features (switch expressions, `using` declarations, nullable reference types, ranges/indices, default interface methods).
- `AllowUnsafeBlocks` is on. Unsafe code appears at the native/plugin boundaries.
- Root namespace is `Assistant`. `Razor/UltimaSDK` uses the `Ultima` namespace.

## Architecture

### Two hosting modes

`Client` (`Razor/Client/Client.cs`) is an abstract base class with one implementation per client. `Client.Instance` and `Client.IsOSI` are set by `Client.Init(isOSI)`:

- **ClassicUO plugin** (`Client/ClassicUO.cs`, `ClassicUOClient`): ClassicUO loads Razor as a plugin and calls the unsafe `Engine.Install(PluginHeader*)` entry point. Razor hooks the plugin header's send/recv function pointers (via `cuoapi.dll`) and sees every packet in-process.
- **OSI client** (`Client/OSI.cs`, `OSIClient`): `Engine.Main` launches `client.exe` and injects the native `Loader` and `Crypt` DLLs (C++ projects in the solution), which handle encryption, Huffman compression, and memory patching. `Platform.dll` (C++) supplies a few Win32 helpers through P/Invoke in `Razor/Platform.cs`.

When behavior differs between the clients, branch on `Client.IsOSI` or add an abstract member to `Client`.

### Startup and auto-initialization

`Engine` (`Core/Main.cs`) loads settings, language files, and the last profile, then uses reflection to call **every `public static void Initialize()`** in the assembly (types in the `Ultima` namespace are skipped). Subsystems register themselves this way, so a new manager usually only needs a static `Initialize()`.

### Packet pipeline

- `Network/PacketHandler.cs` keeps per-packet-ID lists of **viewers** (read-only `PacketReader`) and **filters** (mutable `Packet`). Either kind can set `args.Block = true` to drop the packet.
- Most handlers are registered in `Network/Handlers.cs` `Initialize()` with `RegisterServerToClientViewer`/`Filter` and `RegisterClientToServerViewer`/`Filter`. These handlers keep the world model (`World`, `PlayerData`, `Mobile`, `Item` in `Core/`) up to date.
- Outgoing packets are defined in `Network/Packets.cs` and sent with `Client.Instance.SendToServer` / `SendToClient`.
- `Filters/` contains packet-based filters (light, weather, sounds, messages, and others) built on the same mechanism.

### Feature bits

`FeatureBit` in `Client.cs` lists features a shard can turn off. Check `Client.Instance.AllowBit(FeatureBit.X)` before running a gated feature.

### Macros vs. scripts

- **Macros** (`Macros/`): recorded `MacroAction` subclasses in `Actions.cs`. Each action implements `ToScript()`, which converts the macro into Razor script.
- **Razor scripts** (`Scripts/`): a custom command language with a lexer and interpreter in `Scripts/Engine/`. Commands, aliases, and expressions are registered with `Interpreter.RegisterCommandHandler`, `RegisterAliasHandler`, and `RegisterExpressionHandler` from the `Register()` methods in `Commands.cs`, `AgentCommands.cs`, `SpeechCommands.cs`, `TargetCommands.cs`, `Aliases.cs`, and `Expressions.cs`. Those `Register()` methods are called from `ScriptManager`.
- Adding a new script command or expression takes three changes:
  1. Register the handler.
  2. Add the keyword to the editor's autocomplete list and its `ToolTipDescriptions` entry in `Scripts/ScriptManager.cs`. The editor is the bundled `FastColoredTextBox` project.
  3. Document it in `help/docs/guide/` (`commands.md`, `expressions.md`, and so on).
- Script recording calls `ScriptManager.AddToScript(...)` from packet handlers.

### Other subsystems

- `Agents/`: organizer, scavenger, restock, buy/sell, use-once, and other agents. Each serializes its settings into the profile XML.
- `HotKeys/`: hotkey registration and handlers.
- `Gumps/`: Razor's own in-game gumps (`Gumps/Internal/`).
- `UI/`: the WinForms UI. The main window is `UI/Razor.cs` (`MainForm`), split across several partial/designer files.
- `UltimaSDK/`: readers for UO client data files (art, tiledata, maps, cliloc, hues).

### Settings and localization

- Per-character **profile** settings are declared with `AddProperty("Name", default)` in `UI/Config.cs` and read with `Config.GetBool`, `Config.GetInt`, and `Config.GetString`. Profiles are XML files in the user's `Profiles` directory.
- App-wide settings use `Config.GetAppSetting<T>` and `Config.SetAppSetting`.
- UI strings use the `LocString` enum (`UI/Languages.cs`) and resolve through `Language.GetString`. The text lives in `etc/Language/Razor_lang.<LANG>` (default `ENU`). A new `LocString` needs an entry in the language files.

## Contributing notes

From CONTRIBUTING.md: branch from `main`, and when a PR contains several changes, make one commit per change. The project is licensed under GPLv3, and source files carry the GPL license header region.
