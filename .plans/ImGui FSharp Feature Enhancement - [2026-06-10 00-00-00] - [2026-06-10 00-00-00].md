---
title: "ImGui FSharp Feature Enhancement"
description: "Add string-based InputText, Clipper support, split PInvoke to ImGuiNativeInterface.fs, add missing bridge functions, and update README"
status: Completed
tags: [feature, refactor, fsharp, imgui]
created: 2026-06-10T00:00:00
modified: 2026-06-10T12:00:00
session: 1
previous_session: null
---

# ImGui FSharp Feature Enhancement

## Description
> `2026-06-10 00:00:00`

Enhance the ImGuiFSharp library with:
1. Replace `char[]` InputText/InputTextMultiline with `string ref` (resizable, F# idiomatic)
2. Add `ImGuiListClipper` bridge + F# wrapper for efficient large-list rendering
3. Add missing bridge functions: `TableSetupScrollFreeze`, `SetNextItemWidth`, `SetNextWindowContentSize`, `IsWindowFocused`, `IsWindowHovered`, `GetScrollY`/`SetScrollY`, `GetFrameHeight`, `CalcTextSize`, `SeparatorText`
4. Split PInvoke declarations from `ImGuiInternal.fs` into `internal/ImGuiNativeInterface.fs`; `ImGuiInternal.fs` becomes pure F# wrappers
5. Update README

## Action Plan
> `2026-06-10 00:00:00`

- [x] Step 1 — Add missing C bridge functions to `imgui_bridge.h` and `imgui_bridge.cpp`
- [x] Step 2 — Create `internal/ImGuiNativeInterface.fs` with all PInvoke declarations
- [x] Step 3 — Rewrite `internal/ImGuiInternal.fs` as thin F# wrappers over ImGuiNativeInterface
- [x] Step 4 — Update `Gui.fs`: string ref InputText, Clipper, new widget wrappers
- [x] Step 5 — Update `Builder.fs`: string inputText/inputTextMultiline, clipper CE
- [x] Step 6 — Update `ImGuiFSharp.fsproj` to include new file
- [x] Step 7 — Update README.md

## Files Affected
> `2026-06-10 00:00:00`

| File | Change | Note |
|------|--------|------|
| `cpp/bridge/imgui_bridge.h` | modified | Add Clipper, TableSetupScrollFreeze, SetNextItemWidth, misc helpers |
| `cpp/bridge/imgui_bridge.cpp` | modified | Implement new bridge functions |
| `internal/ImGuiNativeInterface.fs` | created | All PInvoke declarations extracted here |
| `internal/ImGuiInternal.fs` | modified | Now only F# helpers (BoolPtr, etc.) |
| `Gui.fs` | modified | string ref InputText, Clipper, new widgets |
| `Builder.fs` | modified | string inputText, clipper CE |
| `ImGuiFSharp.fsproj` | modified | Add ImGuiNativeInterface.fs to compile order |
| `README.md` | modified | Document new features |
