#!/usr/bin/env bash
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

echo "=== ImGuiFSharp Native Build ==="

# Check cmake
if ! command -v cmake &>/dev/null; then
  echo "ERROR: cmake not found. Install cmake >= 3.24." && exit 1
fi

# Build C++ bridge
BUILD_DIR="cpp/build"
mkdir -p "$BUILD_DIR"
cmake -S cpp -B "$BUILD_DIR" -DCMAKE_BUILD_TYPE=Release
cmake --build "$BUILD_DIR" --config Release -j"$(nproc 2>/dev/null || sysctl -n hw.ncpu 2>/dev/null || echo 4)"
cmake --install "$BUILD_DIR" --config Release

# Determine OS and copy built libraries to appropriate runtime folder
OS_NAME="$(uname -s)"
case "$OS_NAME" in
  Linux*)
    echo "Detected Linux. Copying to runtimes/linux-x64/native/..."
    mkdir -p runtimes/linux-x64/native
    cp addons/ImGuiNative/libImGuiNative.so runtimes/linux-x64/native/
    ;;
  Darwin*)
    echo "Detected macOS. Copying to runtimes/osx-x64/native/ and runtimes/osx-arm64/native/..."
    mkdir -p runtimes/osx-x64/native runtimes/osx-arm64/native
    cp addons/ImGuiNative/libImGuiNative.dylib runtimes/osx-x64/native/
    cp addons/ImGuiNative/libImGuiNative.dylib runtimes/osx-arm64/native/
    ;;
  CYGWIN*|MINGW32*|MSYS*|MINGW*)
    echo "Detected Windows environment. Copying to runtimes/win-x64/native/..."
    mkdir -p runtimes/win-x64/native
    cp addons/ImGuiNative/ImGuiNative.dll runtimes/win-x64/native/
    ;;
  *)
    echo "Unknown OS: $OS_NAME. Please copy the built library from addons/ImGuiNative/ to runtimes manually."
    ;;
esac

echo "=== Native Build Done ==="
