# ImGuiFSharp

Low-level .NET (F#/C#) bindings for ImGui, ImPlot, and ImPlot3D, compiled with a native C++ bridge. This library packages multi-platform precompiled shared libraries (`.so`, `.dll`, `.dylib`) directly inside a single NuGet package for easy consumption.

## Architecture

This project wraps `ImGuiNative` (the C++ bridge project included as a git submodule in `cpp/`) and provides a safe/unmanaged P/Invoke layer.

```
+-----------------------------------------------------------+
|                       ImGuiFSharp                         |
| (F# modules, IGuiFunctions, ImGuiInstance, ImGuiInternal) |
+-----------------------------------------------------------+
                              |
                              v (Bundled Native Libraries)
+-----------------------------------------------------------+
|                       ImGuiNative                         |
| (C++ compiled bridge for ImGui + ImPlot + ImPlot3D)       |
+-----------------------------------------------------------+
```

## Building Native Binaries Locally

The repository comes with precompiled binaries under `runtimes/` so you do not need a C++ compiler to pack or use the NuGet package. However, if you update the C++ bridge code inside the `cpp` submodule, you can rebuild the native binaries locally using:

```bash
./build_native.sh
```

This script will:
1. Initialize/update the C++ submodules recursively.
2. Compile the C++ bridge for your current operating system using CMake.
3. Automatically copy the compiled binary into the correct `runtimes/` subdirectory (e.g. `runtimes/linux-x64/native/`).

## Packaging to NuGet

To pack this library into a NuGet package:

```bash
dotnet pack -c Release -o ../ImGuiNugets
```

The resulting package contains both the F# assembly and the native libraries under `runtimes/` organized by platform:
- `runtimes/win-x64/native/ImGuiNative.dll`
- `runtimes/linux-x64/native/libImGuiNative.so`
- `runtimes/osx-x64/native/libImGuiNative.dylib`
- `runtimes/osx-arm64/native/libImGuiNative.dylib`

## License

This project is licensed under the MIT License - see the LICENSE file for details.
