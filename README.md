# 🖤 Funny — Discord Rich Presence 🧡

[![Platform](https://img.shields.io/badge/Platform-Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)](https://github.com/)
[![Language](https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://github.com/)
[![Framework](https://img.shields.io/badge/.NET%20Framework-4.0%2B-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Discord](https://img.shields.io/badge/Discord-Rich%20Presence-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.com/)
[![Status](https://img.shields.io/badge/Performance-0%25%20CPU%20%7C%20Minimal%20RAM-brightgreen?style=for-the-badge)](https://github.com/)

An ultra-lightweight, native **Win32 Discord Rich Presence (RPC)** desktop client written in pure **C#** with **zero external dependencies**. No heavy frameworks, no bloated NuGet packages — just raw speed, clean dark aesthetics, and instant Discord integration. ⚡

---

## ✨ Features

- 🎮 **Direct Discord IPC Integration**  
  Communicates straight with Discord's local Named Pipes (`\\.\pipe\discord-ipc-0` through `9`). No Discord GameSDK dlls or third-party wrappers required!
- 🪶 **Extreme Low Resource Footprint**  
  Consumes `0.0% CPU` and utilizes aggressive unmanaged memory trimming (`EmptyWorkingSet` & `SetProcessWorkingSetSize`) to keep RAM consumption near zero.
- 🎨 **Sleek Native Win32 UI**  
  Rendered entirely with native GDI (`WM_PAINT`, `DrawText`, Segoe UI font) on a modern dark canvas (`#18181B`) with signature accent styling (`#EB823B`).
- 🚀 **Zero Dependency Build**  
  Compiles with the built-in Microsoft .NET Framework C# compiler (`csc.exe`) found on every modern Windows PC. No Visual Studio installation needed!
- 📦 **Standalone Portable Executable**  
  Single `.exe` file (~160 KB) with embedded icon support. No setup or installer required.

---

## 🖥️ Preview

```text
┌────────────────────────────────────────────────────────┐
│  🖤 Porn Hub                                     [—][X]│
├────────────────────────────────────────────────────────┤
│                                                        │
│                       Porn Hub                         │
│      Status: Active • Discord Rich Presence Connected  │
│         0.0% CPU  |  Ultra-low Memory  |  Dev by S4KA  │
│                                                        │
└────────────────────────────────────────────────────────┘
```

---

## 📋 Requirements

* **Operating System**: Windows 7 / 8 / 10 / 11 (x64)
* **Runtime**: .NET Framework 4.0 or higher *(pre-installed on Windows 10 & 11)*
* **Discord**: Official Discord Desktop client running in the background

---

## 🚀 How to Build & Run

### Method 1: Using `build.bat` (Recommended) 🛠️
Simply double-click [`build.bat`](file:///c:/Users/S4KA/Music/Porn%20Hub%20-%20Dev%20by%20S4KA/build.bat) in the project directory:

```cmd
build.bat
```
This automatically invokes the 64-bit compiler with full optimization (`/optimize+`) and packages `app.ico` into `Porn Hub.exe`.

---

### Method 2: Manual Compilation via Command Prompt 💻
Run the following command directly in your command line:

```cmd
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /target:winexe /optimize+ /platform:x64 /win32icon:"app.ico" /out:"Porn Hub.exe" "Program.cs"
```

---

### 🏃 Running the Application
1. Open and log into your **Discord Desktop** app.
2. Double-click `Porn Hub.exe`.
3. Check your Discord profile — your status will immediately reflect your custom activity! 🚀

---

## ⚙️ Customization

You can easily customize the application by editing [`Program.cs`](file:///c:/Users/S4KA/Music/Porn%20Hub%20-%20Dev%20by%20S4KA/Program.cs):

### 1. Change Discord Application Details
Locate `DiscordRpc` class around line 338:
```csharp
private const string CLIENT_ID = "1555522311011049534"; // Replace with your Discord Application ID
```

### 2. Change Activity Status & Text
Modify the JSON payload in the `Worker()` method:
```csharp
string activity = "{\"cmd\":\"SET_ACTIVITY\",\"args\":{\"pid\":" + pid + ",\"activity\":{" +
    "\"details\":\"Dev by S4KA\"," +
    "\"state\":\"Active\"," +
    "\"timestamps\":{\"start\":" + startTime + "}," +
    "\"assets\":{" +
    "\"large_image\":\"logo\"," +
    "\"large_text\":\"Porn Hub\"" +
    "}" +
    "}},\"nonce\":\"1\"}";
```

### 3. Change Window Title & Dimensions
In `CustomWndProc` or `Main()`, adjust the window sizes and text colors:
- Background color: `0x001B1818` (Dark Charcoal)
- Title Accent color: `0x00EB823B` (Orange/Amber)

---

## 📁 Project Structure

```text
├── 📜 Program.cs    # Full source code (Win32 window + Named Pipe Discord RPC)
├── 🖼️ app.ico       # Application icon
├── ⚙️ build.bat      # One-click automated compilation script
├── 🚀 Porn Hub.exe  # Compiled executable
└── 📖 README.md     # Documentation
```

---

## 🛡️ Disclaimer

> [!NOTE]  
> This project is created for educational, customization, and aesthetic demonstration purposes. All trademarks belong to their respective owners.

---

## 👤 Author

* **Developer**: **S4KA**  
* Feel free to star ⭐ this repository if you find it helpful!
