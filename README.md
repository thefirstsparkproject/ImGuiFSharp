# ImGuiFSharp

Low-level .NET (F#/C#) bindings for [Dear ImGui](https://github.com/ocornut/imgui), [ImPlot](https://github.com/epezent/implot), and [ImPlot3D](https://github.com/brenocq/implot3d), compiled with a native C++ bridge. Targets **F# 10 / .NET 10**. Precompiled shared libraries for Windows, Linux, and macOS are bundled in a single NuGet package — no C++ toolchain needed to consume it.

## Architecture

```
┌──────────────────────────────────────────────────────────────┐
│  Builder.fs  (F# computation expression DSL)                 │
├──────────────────────────────────────────────────────────────┤
│  Gui.fs      (high-level static API — no pointers or arrays) │
├──────────────────────────────────────────────────────────────┤
│  internal/ImGuiInternal.fs   (F# helpers: BoolPtr,           │
│                               StringBuffer, ListClipper)      │
├──────────────────────────────────────────────────────────────┤
│  internal/ImGuiNativeInterface.fs  (all PInvoke declarations) │
├──────────────────────────────────────────────────────────────┤
│  cpp/bridge/imgui_bridge.{h,cpp}   (C extern "C" bridge)     │
├──────────────────────────────────────────────────────────────┤
│  NativeImGuiLib  (ImGui + ImPlot + ImPlot3D, read-only)      │
└──────────────────────────────────────────────────────────────┘
```

The pipeline enforces a strict layering rule: low-level details (pointers, `char*` buffers, native handles) never surface above `ImGuiInternal.fs`. `Gui.fs` and `Builder.fs` expose only idiomatic F# types.

## Usage

### Computation expression DSL (`Builder`)

```fsharp
open ImGuiFSharp

let mutable name = ref "World"
let items = Array.init 100_000 string

Builder.window "My App" {
    Builder.imWindow "Demo" {
        Builder.text "Hello!"
        Builder.inputText("Name", name)
        Builder.text $"Hello, {name.Value}!"

        // Large list — only visible rows are rendered
        Builder.clipper(items.Length, fun first last ->
            for i in first .. last - 1 do
                Builder.text items[i])
    }
}
```

### Direct `Gui` API

```fsharp
open ImGuiFSharp

// Text input — string ref, fully managed, auto-resizes
let query = ref ""
if Gui.InputText("Search", query) then
    printfn "Query: %s" query.Value

// Table with frozen header row and alternating row colours
if Gui.BeginTable("data", 3, Table.ScrollY ||| Table.RowBg, outerHeight = 400f) then
    Gui.TableSetupScrollFreeze(0, 1)           // freeze header
    Gui.TableSetupColumn("Symbol")
    Gui.TableSetupColumn("Price")
    Gui.TableSetupColumn("Change")
    Gui.TableNextRow(TableRow.Headers)
    use clipper = new ListClipper(rows.Length)
    while clipper.Step() do
        for i in clipper.DisplayStart .. clipper.DisplayEnd - 1 do
            Gui.TableNextRow()
            Gui.TableNextColumn(); Gui.Text rows[i].Symbol
            Gui.TableNextColumn(); Gui.Text $"%.2f{rows[i].Price}"
            Gui.TableNextColumn()
            let chg = rows[i].Change
            Gui.PushStyleColor(Col.Text, (if chg >= 0f then 0f else 1f), (if chg >= 0f then 1f else 0f), 0f, 1f)
            Gui.Text $"%+.2f%%{chg}"
            Gui.PopStyleColor()
    Gui.EndTable()
```

## Key Features

### String inputs — `string ref`

`InputText` and `InputTextMultiline` accept a `string ref` and handle all buffer management internally. The buffer lives on the unmanaged heap so ImGui's `realloc`-based resize callback is safe. No `char[]`, no manual sizing.

```fsharp
let note = ref "type here…"
Gui.InputTextMultiline("##note", note, height = 200f)
```

### ListClipper — virtual scrolling

`ListClipper` wraps `ImGuiListClipper` for O(1) row rendering regardless of list size. Use it with any table or list that has uniform row height.

```fsharp
use clipper = new ListClipper(1_000_000)
while clipper.Step() do
    for i in clipper.DisplayStart .. clipper.DisplayEnd - 1 do
        Gui.Text $"Row {i}"
```

Or via the Builder DSL:

```fsharp
Builder.clipper(data.Length, fun first last ->
    for i in first .. last - 1 do
        Builder.tableRow()
        Builder.text data[i].Label)
```

### New helpers added in this release

| API | Description |
|-----|-------------|
| `Gui.SeparatorText(label)` | Separator with embedded label |
| `Gui.SetNextItemWidth(w)` | Set width of the next widget |
| `Gui.CalcTextSize(text)` | Returns `(width, height)` |
| `Gui.GetFrameHeight()` / `GetFrameHeightWithSpacing()` | Row height helpers |
| `Gui.GetScrollY()` / `SetScrollY` / `SetScrollHereY` | Scroll control |
| `Gui.IsWindowFocused()` / `IsWindowHovered()` | Window state queries |
| `Gui.IsItemVisible()` / `IsItemEdited()` / `IsItemDeactivatedAfterEdit()` | Item state queries |
| `Gui.TableSetupScrollFreeze(cols, rows)` | Freeze table header/columns |
| `Gui.TableSetBgColor(target, color)` | Per-cell/row/column background |
| `Gui.SetItemDefaultFocus()` | Scroll combo/list to focused item |
| `Gui.PushItemFlag` / `PopItemFlag` | Disable/modify item behaviour |
| `Gui.SetNextWindowContentSize(w, h)` | Override scrollable content size |

## Building Native Binaries Locally

The repository ships precompiled binaries under `runtimes/` — no C++ toolchain is needed to use the NuGet package. To rebuild after modifying the C++ bridge:

```bash
./build_native.sh
```

This script will:
1. Initialize/update git submodules recursively.
2. Compile the bridge with CMake for the current platform.
3. Copy the output into the correct `runtimes/<rid>/native/` directory.

## Packaging

```bash
dotnet pack -c Release -o ../ImGuiNugets
```

Bundled runtimes:

| RID | Binary |
|-----|--------|
| `win-x64` | `ImGuiNative.dll` |
| `linux-x64` | `libImGuiNative.so` |
| `osx-x64` | `libImGuiNative.dylib` |
| `osx-arm64` | `libImGuiNative.dylib` |

## License

MIT — see [LICENSE](LICENSE).
