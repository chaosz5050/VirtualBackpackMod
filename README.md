# VirtualBackpackMod for Empyrion: Galactic Survival

A simple and persistent **virtual backpack mod** for Empyrion multiplayer servers.

This mod provides every player with a 40-slot private inventory accessible via the `/vb` command. Items placed inside are saved per-player and persist across play sessions and server restarts.

![Empyrion Server Helper](https://img.shields.io/badge/Platform-Linux-blue)
![Python](https://img.shields.io/badge/Python-3.8%2B-green)
![License](https://img.shields.io/badge/License-CC%20BY--NC--SA%204.0-orange)

## Short version
- Copy ../VirtualBackpackMod.dll and /bin/Releases/netstandard2.0/VirtualBackpackMod.yaml to the mods directory on your Empyrion server and reboot.

I take no reponsibility for lost items or other proplems. It's just a hobby project for my own server and I decided to share it :)

## 🎯 Features

- 🧳 `/vb1` and `/vb2`command to open two 40-slot virtual backpacks
- 💾 Automatic saving when items are moved or the backpack is closed
- 🔁 Persistent JSON-based storage per player
- ✅ Works on Linux servers (tested on Hosting Havoc)
- 🚫 No EAH, no ModHelper — lightweight and standalone

---

## 💻 Requirements

- Empyrion server with modding support
- `Mif.dll` and `protobuf-net.dll` from Empyrion’s `Managed` folder
- `.NET SDK` installed (for building on Linux)
- Newtonsoft.Json (included as DLL)

---

## 🏗️ Build Instructions (Linux / CachyOS)

```bash
# Clone or extract the mod source
cd VirtualBackpackMod

# Ensure you have the Empyrion references
cp /path/to/empyrion/EmpyrionDedicated_Data/Managed/*.dll References/

# Build the project
dotnet build -c Release
Compiled DLL will be located in:

bash
Copy
Edit
bin/Release/netstandard2.0/VirtualBackpackMod.dll
📂 Server Installation
Upload the following files to your Empyrion server:

bash
Copy
Edit
Content/Mods/VirtualBackpackMod/
├── VirtualBackpackMod.dll
├── VirtualBackpackMod.yaml
└── PlayerData/            # auto-created to hold per-player JSON saves
Restart the server after uploading.

🔧 Mod YAML Example
yaml
Copy
Edit
ModName: VirtualBackpackMod
Version: 1.0.0
Author: René
Description: A persistent 40-slot virtual backpack mod for Empyrion
GameVersion: 1.0.0
ModGuid: VirtualBackpackMod-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
ModType: ModInterface
ModDllName: VirtualBackpackMod.dll
ModClass: VirtualBackpackMod
LoadOnStart: true
LoadOn:
  - Dedicated
  - Playfield
Dependencies: []
🚀 Usage
Join the server

Type /vb in chat

A 40-slot backpack will appear

Any items placed inside are saved instantly

Reopening /vb will restore your items

📚 Future Features (Planned)
/vb1, /vb2, etc. for multiple backpacks

Admin tools to view/edit other players’ backpacks

Shared faction storage

Integration with Linux-based external management tool


**🔗 [Discord Server](https://discord.gg/WFtZRWVB)**

Get help with:
- Tool setup and configuration
- Server administration tips
- Bug reports and feature requests
- General Empyrion server management

## License

This project is licensed under the **Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License**.

### What this means:
- ✅ **You can freely use** this software for personal and non-commercial purposes
- ✅ **You can modify** and redistribute the code
- ✅ **You can share** it with others
- 🚫 **You cannot sell** this software or use it for commercial purposes
- 🔄 **Any modifications** must be shared under the same license
- 👤 **You must give credit** to the original author

**Full license text**: [CC BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/)

### Commercial Use
If you're interested in commercial use or licensing, please contact the maintainers through our Discord community.
