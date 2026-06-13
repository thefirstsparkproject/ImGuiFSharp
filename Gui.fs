namespace ImGuiFSharp

open System
open System.Numerics
open System.Reflection
open System.Runtime.InteropServices
open ImGuiFSharp.Flags
open ImGuiFSharp.Enums

// ══════════════════════════════════════════════════════════════════════════════
// ListClipper — wraps the native ImGuiListClipper for efficient large-list
// rendering. Dispose() (or `use`) is required to call End + Destroy.
// ══════════════════════════════════════════════════════════════════════════════

/// Efficient virtual-scroll clipper for large uniform-height lists.
/// Wrap your table/list rows in this to only render visible items.
///
///   use clipper = new ListClipper(itemCount)
///   while clipper.Step() do
///       for i in clipper.DisplayStart .. clipper.DisplayEnd - 1 do
///           Gui.TableNextRow()
///           ...
[<Sealed>]
type ListClipper(itemCount: int, ?itemHeight: float32) =
    let ptr = ImGuiNative.IGN_Clipper_Create()
    do ImGuiNative.IGN_Clipper_Begin(ptr, itemCount, defaultArg itemHeight -1.f)

    member _.Step()         = ImGuiNative.IGN_Clipper_Step(ptr)
    member _.DisplayStart   = ImGuiNative.IGN_Clipper_GetDisplayStart(ptr)
    member _.DisplayEnd     = ImGuiNative.IGN_Clipper_GetDisplayEnd(ptr)

    interface IDisposable with
        member _.Dispose() =
            ImGuiNative.IGN_Clipper_End(ptr)
            ImGuiNative.IGN_Clipper_Destroy(ptr)

// ══════════════════════════════════════════════════════════════════════════════
// Gui — high-level F# façade. No pointers, char arrays, or unsafe types here.
// ══════════════════════════════════════════════════════════════════════════════

type ImageInfo =
    {
        Position : System.Drawing.Point
        Size : System.Drawing.Size
        U : Vector2
        V : Vector2
        mutable TextureId : uint32
    }

[<AbstractClass; Sealed>]
type public Gui =

    // ── Initialization & Font Management ─────────────────────────────────────
    static member AddDefaultFont()            = ImGuiNative.IGN_Font_AddDefault()
    static member AddFontFromFile(path, size) = ImGuiNative.IGN_Font_AddFromFile(path, size)
    static member Build()                     = ImGuiNative.IGN_Font_Build()

    // ── Windows, Child Windows & Docking ─────────────────────────────────────
    static member Begin(name, ?pOpen, ?flags: Window) =
        BoolPtr.withOptRef pOpen (fun ptr ->
            ImGuiNative.IGN_Begin(name, ptr, defaultArg flags Window.None))
    static member End() = ImGuiNative.IGN_End()

    static member BeginChild(strId, ?width, ?height, ?border, ?flags: Child) =
        ImGuiNative.IGN_BeginChild(strId,
            defaultArg width 0.f, defaultArg height 0.f,
            defaultArg border false, defaultArg flags Child.None)
    static member EndChild() = ImGuiNative.IGN_EndChild()

    static member DockSpace(id, w, h, ?flags: DockNode) =
        ImGuiNative.IGN_DockSpace(id, w, h, defaultArg flags DockNode.None)

    static member DockSpaceOverViewport(?dockspaceId, ?flags: DockNode) =
        ImGuiNative.IGN_DockSpaceOverViewport(
            defaultArg dockspaceId 0u, defaultArg flags DockNode.None)

    static member SetNextWindowDockID(dockId, ?cond: Cond) =
        ImGuiNative.IGN_SetNextWindowDockID(dockId, defaultArg cond Cond.None)

    static member GetWindowDockID() =
        ImGuiNative.IGN_GetWindowDockID()

    static member IsWindowDocked() =
        ImGuiNative.IGN_IsWindowDocked()

    static member DockBuilderDockWindow(windowName, nodeId) =
        ImGuiNative.IGN_DockBuilderDockWindow(windowName, nodeId)

    static member DockBuilderAddNode(?nodeId, ?flags: DockNode) =
        ImGuiNative.IGN_DockBuilderAddNode(
            defaultArg nodeId 0u, defaultArg flags DockNode.None)

    static member DockBuilderSetNodeFlags(nodeId, flags: DockNode) =
        ImGuiNative.IGN_DockBuilderSetNodeFlags(nodeId, flags)

    static member DockBuilderRemoveNode(nodeId) =
        ImGuiNative.IGN_DockBuilderRemoveNode(nodeId)

    static member DockBuilderRemoveNodeDockedWindows(nodeId, ?clearSettingsRefs) =
        ImGuiNative.IGN_DockBuilderRemoveNodeDockedWindows(
            nodeId, defaultArg clearSettingsRefs true)

    static member DockBuilderRemoveNodeChildNodes(nodeId) =
        ImGuiNative.IGN_DockBuilderRemoveNodeChildNodes(nodeId)

    static member DockBuilderSetNodePos(nodeId, x, y) =
        ImGuiNative.IGN_DockBuilderSetNodePos(nodeId, x, y)

    static member DockBuilderSetNodeSize(nodeId, w, h) =
        ImGuiNative.IGN_DockBuilderSetNodeSize(nodeId, w, h)

    static member DockBuilderSplitNode(nodeId, splitDir: Dir, sizeRatioForNodeAtDir: float32) =
        let mutable outIdAtDir = 0u
        let mutable outIdAtOppositeDir = 0u
        let _ = ImGuiNative.IGN_DockBuilderSplitNode(
            nodeId, splitDir, sizeRatioForNodeAtDir, &outIdAtDir, &outIdAtOppositeDir)
        outIdAtDir, outIdAtOppositeDir

    static member DockBuilderFinish(nodeId) =
        ImGuiNative.IGN_DockBuilderFinish(nodeId)

    static member SetNextWindowPos(x, y, ?cond: Cond) =
        ImGuiNative.IGN_SetNextWindowPos(x, y, defaultArg cond Cond.None)
    static member SetNextWindowSize(w, h, ?cond: Cond) =
        ImGuiNative.IGN_SetNextWindowSize(w, h, defaultArg cond Cond.None)
    static member SetNextWindowBgAlpha(alpha) =
        ImGuiNative.IGN_SetNextWindowBgAlpha(alpha)
    static member SetNextWindowContentSize(w, h) =
        ImGuiNative.IGN_SetNextWindowContentSize(w, h)

    static member GetWindowPos() =
        let mutable x, y = 0.f, 0.f
        ImGuiNative.IGN_GetWindowPos(&x, &y)
        x, y
    static member GetWindowSize() =
        let mutable x, y = 0.f, 0.f
        ImGuiNative.IGN_GetWindowSize(&x, &y)
        x, y

    static member GetMainViewportWorkPos() =
        let mutable x, y = 0.f, 0.f
        ImGuiNative.IGN_GetMainViewportWorkPos(&x, &y)
        x, y

    static member GetMainViewportWorkSize() =
        let mutable x, y = 0.f, 0.f
        ImGuiNative.IGN_GetMainViewportWorkSize(&x, &y)
        x, y

    static member IsWindowFocused(?flags: Hovered) =
        ImGuiNative.IGN_IsWindowFocused(int (defaultArg flags Hovered.None))
    static member IsWindowHovered(?flags: Hovered) =
        ImGuiNative.IGN_IsWindowHovered(int (defaultArg flags Hovered.None))

    static member ShowDemoWindow(?pOpen) =
        BoolPtr.withOptRef pOpen ImGuiNative.IGN_ShowDemoWindow

    // ── Layout & Formatting ───────────────────────────────────────────────────
    static member Separator()     = ImGuiNative.IGN_Separator()
    static member SeparatorText(label) = ImGuiNative.IGN_SeparatorText(label)
    static member SameLine(?offset, ?spacing) =
        ImGuiNative.IGN_SameLine(defaultArg offset 0f, defaultArg spacing -1f)
    static member NewLine()   = ImGuiNative.IGN_NewLine_()
    static member Spacing()   = ImGuiNative.IGN_Spacing()
    static member Dummy(w, h) = ImGuiNative.IGN_Dummy(w, h)
    static member Indent(w)   = ImGuiNative.IGN_Indent(w)
    static member Unindent(w) = ImGuiNative.IGN_Unindent(w)

    static member BeginGroup() = ImGuiNative.IGN_BeginGroup()
    static member EndGroup()   = ImGuiNative.IGN_EndGroup()

    static member GetContentRegionAvail() =
        let mutable x, y = 0.f, 0.f
        ImGuiNative.IGN_GetContentRegionAvail(&x, &y)
        x, y

    static member SetNextItemWidth(w) = ImGuiNative.IGN_SetNextItemWidth(w)
    static member SetItemDefaultFocus() = ImGuiNative.IGN_SetItemDefaultFocus()

    static member PushID(id) = ImGuiNative.IGN_PushID_Str(id)
    static member PopID()    = ImGuiNative.IGN_PopID()
    static member GetID(id)  = ImGuiNative.IGN_GetID(id)

    // ── Scroll ────────────────────────────────────────────────────────────────
    static member GetScrollY()          = ImGuiNative.IGN_GetScrollY()
    static member GetScrollMaxY()       = ImGuiNative.IGN_GetScrollMaxY()
    static member SetScrollY(y)         = ImGuiNative.IGN_SetScrollY(y)
    static member SetScrollHereY(?ratio) = ImGuiNative.IGN_SetScrollHereY(defaultArg ratio 0.5f)

    // ── Metrics ───────────────────────────────────────────────────────────────
    static member CalcTextSize(text) =
        let mutable w, h = 0.f, 0.f
        ImGuiNative.IGN_CalcTextSize(text, &w, &h)
        w, h
    static member GetFrameHeight()               = ImGuiNative.IGN_GetFrameHeight()
    static member GetTextLineHeight()            = ImGuiNative.IGN_GetTextLineHeight()
    static member GetTextLineHeightWithSpacing() = ImGuiNative.IGN_GetTextLineHeightWithSpacing()
    static member GetFrameHeightWithSpacing()    = ImGuiNative.IGN_GetFrameHeightWithSpacing()

    // ── Basic Widgets ─────────────────────────────────────────────────────────
    static member Button(label, ?w, ?h) =
        ImGuiNative.IGN_Button(label, defaultArg w 0f, defaultArg h 0f)
    static member InvisibleButton(strId, w, h, ?flags: Button) =
        ImGuiNative.IGN_InvisibleButton(strId, w, h, defaultArg flags Button.None)
    static member Checkbox(label, v) =
        BoolPtr.withRef v (fun ptr -> ImGuiNative.IGN_Checkbox(label, ptr))
    static member RadioButton(label, active) = ImGuiNative.IGN_RadioButton(label, active)
    static member ProgressBar(fraction, ?w, ?h, ?overlay) =
        ImGuiNative.IGN_ProgressBar(fraction, defaultArg w -1f, defaultArg h 0f,
            defaultArg overlay Unchecked.defaultof<string>)
    static member Image(texId, w, h, ?uv0_x, ?uv0_y, ?uv1_x, ?uv1_y) =
        ImGuiNative.IGN_Image(texId, w, h, defaultArg uv0_x 0.f, defaultArg uv0_y 0.f, defaultArg uv1_x 1.f, defaultArg uv1_y 1.f)
    static member Image(info: ImageInfo) =
        ImGuiNative.IGN_Image(info.TextureId, float32 info.Size.Width, float32 info.Size.Height, info.U.X, info.U.Y, info.V.X, info.V.Y)

    static member ImageButton(id, texId, w, h, ?uv0_x, ?uv0_y, ?uv1_x, ?uv1_y) =
        ImGuiNative.IGN_ImageButton(id, texId, w, h, defaultArg uv0_x 0.f, defaultArg uv0_y 0.f, defaultArg uv1_x 1.f, defaultArg uv1_y 1.f)
    static member ImageButton(id: string, info: ImageInfo) =
        ImGuiNative.IGN_ImageButton(id, info.TextureId, float32 info.Size.Width, float32 info.Size.Height, info.U.X, info.U.Y, info.V.X, info.V.Y)
    static member Selectable(label, selected, ?flags: Selectable, ?width, ?height) =
        ImGuiNative.IGN_Selectable(label, selected,
            defaultArg flags Selectable.None, defaultArg width 0f, defaultArg height 0f)

    // ── Text Display ──────────────────────────────────────────────────────────
    static member Text(text)              = ImGuiNative.IGN_Text(text)
    static member TextColored(r, g, b, a, text) = ImGuiNative.IGN_TextColored(r, g, b, a, text)
    static member TextDisabled(text)      = ImGuiNative.IGN_TextDisabled(text)
    static member TextWrapped(text)       = ImGuiNative.IGN_TextWrapped(text)

    // ── Text Input — string ref (resizable, idiomatic F#) ────────────────────
    /// Single-line text input backed by a resizable string.
    /// Returns true when the value changed. The string ref is updated in place.
    static member InputText(label, value: string ref, ?flags: InputText) =
        use sb = new StringBuffer(value.Value, 256)
        let mutable p   = sb.Ptr
        let mutable len = sb.Len
        let mutable cap = sb.Cap
        let changed =
            ImGuiNative.IGN_InputText_String(label, &p, &len, &cap,
                defaultArg flags InputText.None)
        // Sync back — pointer/len/cap may have changed via realloc
        sb.Ptr <- p; sb.Len <- len; sb.Cap <- cap
        if changed then value.Value <- sb.Read()
        changed

    /// Multi-line text input backed by a resizable string.
    static member InputTextMultiline(label, value: string ref, ?width, ?height, ?flags: InputText) =
        use sb = new StringBuffer(value.Value, 256)
        let mutable p   = sb.Ptr
        let mutable len = sb.Len
        let mutable cap = sb.Cap
        let changed =
            ImGuiNative.IGN_InputTextMultiline_String(label, &p, &len, &cap,
                defaultArg width 0.f, defaultArg height 0.f,
                defaultArg flags InputText.None)
        sb.Ptr <- p; sb.Len <- len; sb.Cap <- cap
        if changed then value.Value <- sb.Read()
        changed

    // ── Numerical Inputs, Sliders & Drags ────────────────────────────────────
    static member InputInt(label, v: int ref, ?step, ?stepFast, ?flags: InputText) =
        let mutable vv = v.Value
        let r = ImGuiNative.IGN_InputInt(label, &vv,
                    defaultArg step 1, defaultArg stepFast 100,
                    defaultArg flags InputText.None)
        v.Value <- vv; r

    static member InputFloat(label, v: float32 ref, ?step, ?stepFast, ?fmt, ?flags: InputText) =
        let mutable vv = v.Value
        let r = ImGuiNative.IGN_InputFloat(label, &vv,
                    defaultArg step 0f, defaultArg stepFast 0f,
                    defaultArg fmt "%.3f", defaultArg flags InputText.None)
        v.Value <- vv; r

    static member InputDouble(label, v: double ref, ?step, ?stepFast, ?fmt, ?flags: InputText) =
        let mutable vv = v.Value
        let r = ImGuiNative.IGN_InputDouble(label, &vv,
                    defaultArg step 0.0, defaultArg stepFast 0.0,
                    defaultArg fmt "%.6f", defaultArg flags InputText.None)
        v.Value <- vv; r

    static member DragInt(label, v: int ref, ?speed, ?min, ?max, ?fmt, ?flags: Slider) =
        let mutable vv = v.Value
        let r = ImGuiNative.IGN_DragInt(label, &vv,
                    defaultArg speed 1f, defaultArg min 0, defaultArg max 0,
                    defaultArg fmt "%d", defaultArg flags Slider.None)
        v.Value <- vv; r

    static member DragFloat(label, v: float32 ref, ?speed, ?min, ?max, ?fmt, ?flags: Slider) =
        let mutable vv = v.Value
        let r = ImGuiNative.IGN_DragFloat(label, &vv,
                    defaultArg speed 1f, defaultArg min 0f, defaultArg max 0f,
                    defaultArg fmt "%.3f", defaultArg flags Slider.None)
        v.Value <- vv; r

    static member DragDouble(label, v: double ref, ?speed, ?min, ?max, ?fmt, ?flags: Slider) =
        let mutable vv = v.Value
        let r = ImGuiNative.IGN_DragDouble(label, &vv,
                    defaultArg speed 1.0f, defaultArg min 0.0, defaultArg max 0.0,
                    defaultArg fmt "%.6f", defaultArg flags Slider.None)
        v.Value <- vv; r

    static member SliderInt(label, v: int ref, min, max, ?fmt, ?flags: Slider) =
        let mutable vv = v.Value
        let r = ImGuiNative.IGN_SliderInt(label, &vv, min, max,
                    defaultArg fmt "%d", defaultArg flags Slider.None)
        v.Value <- vv; r

    static member SliderFloat(label, v: float32 ref, min, max, ?fmt, ?flags: Slider) =
        let mutable vv = v.Value
        let r = ImGuiNative.IGN_SliderFloat(label, &vv, min, max,
                    defaultArg fmt "%.3f", defaultArg flags Slider.None)
        v.Value <- vv; r

    static member SliderDouble(label, v: double ref, min, max, ?fmt, ?flags: Slider) =
        let mutable vv = v.Value
        let r = ImGuiNative.IGN_SliderDouble(label, &vv, min, max,
                    defaultArg fmt "%.6f", defaultArg flags Slider.None)
        v.Value <- vv; r

    // ── Color Editing ─────────────────────────────────────────────────────────
    static member ColorEdit4(label, color: Color ref, ?flags: ColorEdit) =
        let col = [| color.Value.R; color.Value.G; color.Value.B; color.Value.A |]
        let h = GCHandle.Alloc(col, GCHandleType.Pinned)
        try
            let changed =
                ImGuiNative.IGN_ColorEdit4(label, h.AddrOfPinnedObject(),
                    defaultArg flags ColorEdit.None)
            if changed then
                color.Value <- { R = col.[0]; G = col.[1]; B = col.[2]; A = col.[3] }
            changed
        finally h.Free()

    static member ColorEdit4(label, col: float32[], ?flags: ColorEdit) =
        let h = GCHandle.Alloc(col, GCHandleType.Pinned)
        try ImGuiNative.IGN_ColorEdit4(label, h.AddrOfPinnedObject(),
                defaultArg flags ColorEdit.None)
        finally h.Free()

    // ── Containers & Trees ────────────────────────────────────────────────────
    static member CollapsingHeader(label, ?flags: TreeNode) =
        ImGuiNative.IGN_CollapsingHeader(label, defaultArg flags TreeNode.None)
    static member TreeNode(label) = ImGuiNative.IGN_TreeNode(label)
    static member TreePop()       = ImGuiNative.IGN_TreePop()

    static member BeginCombo(label, preview, ?flags: Combo) =
        ImGuiNative.IGN_BeginCombo(label, preview, defaultArg flags Combo.None)
    static member EndCombo() = ImGuiNative.IGN_EndCombo()

    static member BeginListBox(label, ?w, ?h) =
        ImGuiNative.IGN_BeginListBox(label, defaultArg w 0.f, defaultArg h 0.f)
    static member EndListBox() = ImGuiNative.IGN_EndListBox()

    static member BeginTabBar(strId, ?flags: TabBar) =
        ImGuiNative.IGN_BeginTabBar(strId, defaultArg flags TabBar.None)
    static member EndTabBar() = ImGuiNative.IGN_EndTabBar()
    static member BeginTabItem(label, ?pOpen, ?flags: TabItem) =
        BoolPtr.withOptRef pOpen (fun ptr ->
            ImGuiNative.IGN_BeginTabItem(label, ptr, defaultArg flags TabItem.None))
    static member EndTabItem() = ImGuiNative.IGN_EndTabItem()

    // ── Tables ────────────────────────────────────────────────────────────────
    static member BeginTable(id, cols, ?flags: Table, ?ow, ?oh) =
        ImGuiNative.IGN_BeginTable(id, cols,
            defaultArg flags Table.None, defaultArg ow 0f, defaultArg oh 0f)
    static member EndTable() = ImGuiNative.IGN_EndTable()
    static member TableSetupColumn(label, ?flags: TableColumn, ?init) =
        ImGuiNative.IGN_TableSetupColumn(label,
            defaultArg flags TableColumn.None, defaultArg init 0f)
    static member TableSetupScrollFreeze(cols, rows) =
        ImGuiNative.IGN_TableSetupScrollFreeze(cols, rows)
    static member TableNextRow(?rowFlags: TableRow, ?minH) =
        ImGuiNative.IGN_TableNextRow(defaultArg rowFlags TableRow.None, defaultArg minH 0f)
    static member TableNextColumn() = ImGuiNative.IGN_TableNextColumn()
    static member TableSetBgColor(target: TableBgTarget, color: Color, ?columnN) =
        ImGuiNative.IGN_TableSetBgColor(int target, color.ToUint32(), defaultArg columnN -1)

    // ── Menus, Popups & Tooltips ──────────────────────────────────────────────
    static member BeginMenuBar() = ImGuiNative.IGN_BeginMenuBar()
    static member EndMenuBar()   = ImGuiNative.IGN_EndMenuBar()
    static member BeginMenu(label, ?enabled) =
        ImGuiNative.IGN_BeginMenu(label, defaultArg enabled true)
    static member EndMenu()      = ImGuiNative.IGN_EndMenu()
    static member MenuItem(label, ?shortcut, ?selected, ?enabled) =
        ImGuiNative.IGN_MenuItem(label,
            defaultArg shortcut Unchecked.defaultof<string>,
            defaultArg selected false, defaultArg enabled true)

    static member OpenPopup(strId, ?flags: Popup) =
        ImGuiNative.IGN_OpenPopup(strId, defaultArg flags Popup.None)
    static member BeginPopup(strId, ?flags: Popup) =
        ImGuiNative.IGN_BeginPopup(strId, defaultArg flags Popup.None)
    static member BeginPopupModal(name, ?pOpen, ?flags: Window) =
        BoolPtr.withOptRef pOpen (fun ptr ->
            ImGuiNative.IGN_BeginPopupModal(name, ptr, defaultArg flags Window.None))
    static member EndPopup()            = ImGuiNative.IGN_EndPopup()
    static member CloseCurrentPopup()   = ImGuiNative.IGN_CloseCurrentPopup()
    static member BeginPopupContextItem(?strId, ?flags: Popup) =
        ImGuiNative.IGN_BeginPopupContextItem(
            defaultArg strId Unchecked.defaultof<string>,
            defaultArg flags Popup.MouseButtonRight)
    static member BeginPopupContextWindow(?strId, ?flags: Popup) =
        ImGuiNative.IGN_BeginPopupContextWindow(
            defaultArg strId Unchecked.defaultof<string>,
            defaultArg flags Popup.MouseButtonRight)

    static member BeginTooltip()    = ImGuiNative.IGN_BeginTooltip()
    static member EndTooltip()      = ImGuiNative.IGN_EndTooltip()
    static member SetTooltip(text)  = ImGuiNative.IGN_SetTooltip(text)
    static member BeginItemTooltip() = ImGuiNative.IGN_BeginItemTooltip()
    static member SetItemTooltip(text) = ImGuiNative.IGN_SetItemTooltip(text)

    // ── Style Configuration ───────────────────────────────────────────────────
    static member PushStyleColor(idx: Col, r, g, b, a) =
        ImGuiNative.IGN_PushStyleColor(idx, r, g, b, a)
    static member PopStyleColor(?count) =
        ImGuiNative.IGN_PopStyleColor(defaultArg count 1)
    static member PushStyleVar(idx: StyleVar, v: float32) =
        ImGuiNative.IGN_PushStyleVar_Float(idx, v)
    static member PushStyleVar(idx: StyleVar, x: float32, y: float32) =
        ImGuiNative.IGN_PushStyleVar_Vec2(idx, x, y)
    static member PopStyleVar(?count) =
        ImGuiNative.IGN_PopStyleVar(defaultArg count 1)
    static member PushItemFlag(option: ItemFlags, enabled) =
        ImGuiNative.IGN_PushItemFlag(int option, enabled)
    static member PopItemFlag() = ImGuiNative.IGN_PopItemFlag()

    // ── Input Queries & State ─────────────────────────────────────────────────
    static member IsItemHovered(?flags: Hovered) =
        ImGuiNative.IGN_IsItemHovered(defaultArg flags Hovered.None)
    static member IsItemActive()  = ImGuiNative.IGN_IsItemActive()
    static member IsItemVisible() = ImGuiNative.IGN_IsItemVisible()
    static member IsItemEdited()  = ImGuiNative.IGN_IsItemEdited()
    static member IsItemDeactivatedAfterEdit() = ImGuiNative.IGN_IsItemDeactivatedAfterEdit()
    static member IsItemClicked(?mouseButton: MouseButton) =
        ImGuiNative.IGN_IsItemClicked(defaultArg mouseButton MouseButton.Left)
    static member IsMouseClicked(button: MouseButton, ?repeat) =
        ImGuiNative.IGN_IsMouseClicked(button, defaultArg repeat false)
    static member IsMouseDown(button: MouseButton) = ImGuiNative.IGN_IsMouseDown(button)
    static member IsMouseDoubleClicked(button: MouseButton) =
        ImGuiNative.IGN_IsMouseDoubleClicked(button)
    static member GetMousePos() =
        let mutable x, y = 0.f, 0.f
        ImGuiNative.IGN_GetMousePos(&x, &y)
        x, y

    // ── Canvas & Custom Drawing ───────────────────────────────────────────────
    static member GetCursorScreenPos() =
        let mutable x, y = 0.f, 0.f
        ImGuiNative.IGN_GetCursorScreenPos(&x, &y)
        x, y
    static member SetCursorScreenPos(x, y) = ImGuiNative.IGN_SetCursorScreenPos(x, y)
    static member DrawLine(p1_x, p1_y, p2_x, p2_y, col: Color, ?thickness) =
        ImGuiNative.IGN_DrawList_AddLine(p1_x, p1_y, p2_x, p2_y, col.ToUint32(), defaultArg thickness 1.f)
    static member DrawRect(p1_x, p1_y, p2_x, p2_y, col: Color, ?rounding, ?flags: Draw, ?thickness) =
        ImGuiNative.IGN_DrawList_AddRect(p1_x, p1_y, p2_x, p2_y, col.ToUint32(),
            defaultArg rounding 0.f, defaultArg flags Draw.None, defaultArg thickness 1.f)
    static member DrawRectFilled(p1_x, p1_y, p2_x, p2_y, col: Color, ?rounding, ?flags: Draw) =
        ImGuiNative.IGN_DrawList_AddRectFilled(p1_x, p1_y, p2_x, p2_y, col.ToUint32(),
            defaultArg rounding 0.f, defaultArg flags Draw.None)
    static member DrawRectFilledMultiColor(p1_x, p1_y, p2_x, p2_y, colUL: Color, colUR: Color, colBR: Color, colBL: Color) =
        ImGuiNative.IGN_DrawList_AddRectFilledMultiColor(p1_x, p1_y, p2_x, p2_y,
            colUL.ToUint32(), colUR.ToUint32(), colBR.ToUint32(), colBL.ToUint32())
    static member DrawCircle(cx, cy, r, col: Color, ?numSegments, ?thickness) =
        ImGuiNative.IGN_DrawList_AddCircle(cx, cy, r, col.ToUint32(),
            defaultArg numSegments 0, defaultArg thickness 1.f)
    static member DrawCircleFilled(cx, cy, r, col: Color, ?numSegments) =
        ImGuiNative.IGN_DrawList_AddCircleFilled(cx, cy, r, col.ToUint32(), defaultArg numSegments 0)
    static member DrawTriangleFilled(p1_x, p1_y, p2_x, p2_y, p3_x, p3_y, col: Color) =
        ImGuiNative.IGN_DrawList_AddTriangleFilled(p1_x, p1_y, p2_x, p2_y, p3_x, p3_y, col.ToUint32())
    static member DrawText(posX, posY, col: Color, text) =
        ImGuiNative.IGN_DrawList_AddText(posX, posY, col.ToUint32(), text)
    static member DrawPolyline(pointsX: float32[], pointsY: float32[], col: Color, ?flags: Draw, ?thickness) =
        ImGuiNative.IGN_DrawList_AddPolyline(pointsX, pointsY,
            min pointsX.Length pointsY.Length, col.ToUint32(),
            defaultArg flags Draw.None, defaultArg thickness 1.f)
    static member DrawConvexPolyFilled(pointsX: float32[], pointsY: float32[], col: Color) =
        ImGuiNative.IGN_DrawList_AddConvexPolyFilled(pointsX, pointsY,
            min pointsX.Length pointsY.Length, col.ToUint32())
    static member DrawImage(texId, p1_x, p1_y, p2_x, p2_y,
                            ?uv1_x, ?uv1_y, ?uv2_x, ?uv2_y, ?col: Color) =
        let color = defaultArg col Color.Default
        ImGuiNative.IGN_DrawList_AddImage(texId, p1_x, p1_y, p2_x, p2_y,
            defaultArg uv1_x 0.f, defaultArg uv1_y 0.f,
            defaultArg uv2_x 1.f, defaultArg uv2_y 1.f,
            color.ToUint32())
    static member PushClipRect(minX, minY, maxX, maxY, ?intersectWithCurrent) =
        ImGuiNative.IGN_DrawList_PushClipRect(minX, minY, maxX, maxY,
            defaultArg intersectWithCurrent true)
    static member PopClipRect() = ImGuiNative.IGN_DrawList_PopClipRect()

    // ── ImPlot 2D ─────────────────────────────────────────────────────────────
    static member BeginPlot(titleId, ?w, ?h, ?flags: Plot) =
        ImGuiNative.IGN_Plot_BeginPlot(titleId,
            defaultArg w -1f, defaultArg h -1f, defaultArg flags Plot.None)
    static member EndPlot() = ImGuiNative.IGN_Plot_EndPlot()
    static member SetupAxes(xLabel, yLabel, ?xFlags: Flags.PlotAxis, ?yFlags: Flags.PlotAxis) =
        ImGuiNative.IGN_Plot_SetupAxes(xLabel, yLabel,
            defaultArg xFlags Flags.PlotAxis.None, defaultArg yFlags Flags.PlotAxis.None)
    static member SetupAxisLimits(axis: PlotAxis, v_min, v_max, ?cond: Cond) =
        ImGuiNative.IGN_Plot_SetupAxisLimits(axis, v_min, v_max, defaultArg cond Cond.None)
    static member SetNextAxesLimits(x_min, x_max, y_min, y_max, ?cond: Cond) =
        ImGuiNative.IGN_Plot_SetNextAxesLimits(x_min, x_max, y_min, y_max, defaultArg cond Cond.None)
    static member SetupLegend(location: PlotLocation, ?flags: PlotLegend) =
        ImGuiNative.IGN_Plot_SetupLegend(location, defaultArg flags PlotLegend.None)
    static member SetupAxisScale(axis: PlotAxis, scale: PlotScale) =
        ImGuiNative.IGN_Plot_SetupAxisScale(axis, scale)
    static member SetupAxisFormat(axis: PlotAxis, fmt) =
        ImGuiNative.IGN_Plot_SetupAxisFormat(axis, fmt)
    static member ShowPlotDemoWindow(?pOpen) =
        BoolPtr.withOptRef pOpen ImGuiNative.IGN_Plot_ShowDemoWindow

    static member PlotLine(label, values: float32[], ?xScale, ?x0) =
        ImGuiNative.IGN_Plot_PlotLine_FloatPtrInt(label, values, values.Length,
            defaultArg xScale 1.0, defaultArg x0 0.0, 0, sizeof<float32>)
    static member PlotLine(label: string, values: double[], ?xScale: double, ?x0: double) =
        ImGuiNative.IGN_Plot_PlotLine_DoublePtrInt(label, values, values.Length,
            defaultArg xScale 1.0, defaultArg x0 0.0, 0, sizeof<double>)
    static member PlotLine(label: string, xs: double[], ys: double[]) =
        ImGuiNative.IGN_Plot_PlotLine_DoublePtrPtr(label, xs, ys,
            min xs.Length ys.Length, 0, sizeof<double>)
    static member PlotLine(label: string, xs: DateTime[], ys: double[]) =
        let ts = xs |> Array.map (fun dt -> float (DateTimeOffset(dt).ToUnixTimeSeconds()))
        ImGuiNative.IGN_Plot_PlotLine_DoublePtrPtr(label, ts, ys,
            min ts.Length ys.Length, 0, sizeof<double>)

    static member PlotBars(label, values: float32[], ?barSize, ?shift) =
        ImGuiNative.IGN_Plot_PlotBars_FloatPtrInt(label, values, values.Length,
            defaultArg barSize 0.67, defaultArg shift 0.0, 0, sizeof<float32>)
    static member PlotBars(label: string, xs: double[], ys: double[], ?width: double) =
        ImGuiNative.IGN_Plot_PlotBars_DoublePtrPtr(label, xs, ys,
            min xs.Length ys.Length, defaultArg width 0.67, 0, sizeof<double>)

    static member PlotScatter(label, xs: float32[], ys: float32[]) =
        ImGuiNative.IGN_Plot_PlotScatter_FloatPtr(label, xs, ys,
            min xs.Length ys.Length, 0, sizeof<float32>)
    static member PlotScatter(label: string, xs: double[], ys: double[]) =
        ImGuiNative.IGN_Plot_PlotScatter_DoublePtr(label, xs, ys,
            min xs.Length ys.Length, 0, sizeof<double>)

    static member PlotShaded(label, xs: float32[], ys1: float32[], ys2: float32[]) =
        ImGuiNative.IGN_Plot_PlotShaded_FloatPtr(label, xs, ys1, ys2,
            min xs.Length (min ys1.Length ys2.Length), 0, sizeof<float32>)
    static member PlotShaded(label: string, xs: double[], ys1: double[], ys2: double[]) =
        ImGuiNative.IGN_Plot_PlotShaded_DoublePtrPtr(label, xs, ys1, ys2,
            min xs.Length (min ys1.Length ys2.Length), 0, sizeof<double>)

    static member PlotStairs(label, xs: float32[], ys: float32[]) =
        ImGuiNative.IGN_Plot_PlotStairs_FloatPtr(label, xs, ys,
            min xs.Length ys.Length, 0, sizeof<float32>)
    static member PlotStairs(label: string, xs: double[], ys: double[]) =
        ImGuiNative.IGN_Plot_PlotStairs_DoublePtrPtr(label, xs, ys,
            min xs.Length ys.Length, 0, sizeof<double>)

    static member PlotErrorBars(label, xs: float32[], ys: float32[], err: float32[]) =
        ImGuiNative.IGN_Plot_PlotErrorBars_FloatPtr(label, xs, ys, err,
            min xs.Length (min ys.Length err.Length), 0, sizeof<float32>)
    static member PlotErrorBars(label: string, xs: double[], ys: double[], err: double[]) =
        ImGuiNative.IGN_Plot_PlotErrorBars_DoublePtr(label, xs, ys, err,
            min xs.Length (min ys.Length err.Length), 0, sizeof<double>)

    static member PlotPieChart(labels: string[], values: float32[], x, y, radius,
                               ?labelFmt: string, ?angle0: double) =
        let count = min labels.Length values.Length
        let handles  = Array.zeroCreate<GCHandle> count
        let pointers = Array.zeroCreate<nativeint> count
        try
            for i in 0 .. count - 1 do
                let bytes = System.Text.Encoding.UTF8.GetBytes(labels[i] + "\0")
                handles[i]  <- GCHandle.Alloc(bytes, GCHandleType.Pinned)
                pointers[i] <- handles[i].AddrOfPinnedObject()
            let ph = GCHandle.Alloc(pointers, GCHandleType.Pinned)
            try ImGuiNative.IGN_Plot_PlotPieChart_Float(ph.AddrOfPinnedObject(), values, count,
                    x, y, radius, defaultArg labelFmt "%p", defaultArg angle0 90.0)
            finally ph.Free()
        finally for h in handles do if h.IsAllocated then h.Free()

    static member PlotPieChart(labels: string[], values: double[], x, y, radius,
                               ?labelFmt: string, ?angle0: double) =
        let count = min labels.Length values.Length
        let handles  = Array.zeroCreate<GCHandle> count
        let pointers = Array.zeroCreate<nativeint> count
        try
            for i in 0 .. count - 1 do
                let bytes = System.Text.Encoding.UTF8.GetBytes(labels[i] + "\0")
                handles[i]  <- GCHandle.Alloc(bytes, GCHandleType.Pinned)
                pointers[i] <- handles[i].AddrOfPinnedObject()
            let ph = GCHandle.Alloc(pointers, GCHandleType.Pinned)
            try ImGuiNative.IGN_Plot_PlotPieChart(ph.AddrOfPinnedObject(), values, count,
                    x, y, radius, defaultArg labelFmt "%p", defaultArg angle0 90.0)
            finally ph.Free()
        finally for h in handles do if h.IsAllocated then h.Free()

    static member PlotHistogram(label, values: float32[], ?bins, ?barScale, ?rangeMin, ?rangeMax) =
        ImGuiNative.IGN_Plot_PlotHistogram_FloatPtr(label, values, values.Length,
            defaultArg bins -1, defaultArg barScale 1.0,
            defaultArg rangeMin 0.0, defaultArg rangeMax 0.0, 0, sizeof<float32>)
    static member PlotHistogram(label: string, values: double[], ?bins, ?barScale, ?rangeMin, ?rangeMax) =
        ImGuiNative.IGN_Plot_PlotHistogram_DoublePtr(label, values, values.Length,
            defaultArg bins -1, defaultArg barScale 1.0,
            defaultArg rangeMin 0.0, defaultArg rangeMax 0.0, 0, sizeof<double>)
    static member PlotHistogram2D(label, xs: float32[], ys: float32[],
                                  ?xBins, ?yBins, ?xMin, ?xMax, ?yMin, ?yMax) =
        ImGuiNative.IGN_Plot_PlotHistogram2D_FloatPtr(label, xs, ys,
            min xs.Length ys.Length,
            defaultArg xBins -1, defaultArg yBins -1,
            defaultArg xMin 0.0, defaultArg xMax 0.0,
            defaultArg yMin 0.0, defaultArg yMax 0.0, 0, sizeof<float32>)
    static member PlotHistogram2D(label: string, xs: double[], ys: double[],
                                  ?xBins, ?yBins, ?xMin, ?xMax, ?yMin, ?yMax) =
        ImGuiNative.IGN_Plot_PlotHistogram2D_DoublePtr(label, xs, ys,
            min xs.Length ys.Length,
            defaultArg xBins -1, defaultArg yBins -1,
            defaultArg xMin 0.0, defaultArg xMax 0.0,
            defaultArg yMin 0.0, defaultArg yMax 0.0, 0, sizeof<double>)

    static member PlotDigital(label, xs: float32[], ys: float32[]) =
        ImGuiNative.IGN_Plot_PlotDigital_FloatPtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<float32>)
    static member PlotDigital(label: string, xs: double[], ys: double[]) =
        ImGuiNative.IGN_Plot_PlotDigital_DoublePtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<double>)

    static member PlotStems(label, xs: float32[], ys: float32[], ?ref_) =
        ImGuiNative.IGN_Plot_PlotStems_FloatPtr(label, xs, ys, min xs.Length ys.Length,
            defaultArg ref_ 0.0, 0, sizeof<float32>)
    static member PlotStems(label: string, xs: double[], ys: double[], ?ref_) =
        ImGuiNative.IGN_Plot_PlotStems_DoublePtr(label, xs, ys, min xs.Length ys.Length,
            defaultArg ref_ 0.0, 0, sizeof<double>)

    static member PlotInfLines(label, values: float32[]) =
        ImGuiNative.IGN_Plot_PlotInfLines_FloatPtr(label, values, values.Length, 0, sizeof<float32>)
    static member PlotInfLines(label: string, values: double[]) =
        ImGuiNative.IGN_Plot_PlotInfLines_DoublePtr(label, values, values.Length, 0, sizeof<double>)

    static member PlotBubbles(label, xs: float32[], ys: float32[], szs: float32[]) =
        ImGuiNative.IGN_Plot_PlotBubbles_FloatPtr(label, xs, ys, szs,
            min xs.Length (min ys.Length szs.Length), 0, sizeof<float32>)
    static member PlotBubbles(label: string, xs: double[], ys: double[], szs: double[]) =
        ImGuiNative.IGN_Plot_PlotBubbles_DoublePtr(label, xs, ys, szs,
            min xs.Length (min ys.Length szs.Length), 0, sizeof<double>)

    static member PlotPolygon(label, xs: float32[], ys: float32[]) =
        ImGuiNative.IGN_Plot_PlotPolygon_FloatPtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<float32>)
    static member PlotPolygon(label: string, xs: double[], ys: double[]) =
        ImGuiNative.IGN_Plot_PlotPolygon_DoublePtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<double>)

    static member PlotBarGroups(labels: string[], values: float32[], groupCount,
                                ?groupSize, ?shift, ?flags: PlotBarGroups) =
        let handles  = Array.zeroCreate<GCHandle> labels.Length
        let pointers = Array.zeroCreate<nativeint> labels.Length
        try
            for i in 0 .. labels.Length - 1 do
                let bytes = System.Text.Encoding.UTF8.GetBytes(labels[i] + "\0")
                handles[i]  <- GCHandle.Alloc(bytes, GCHandleType.Pinned)
                pointers[i] <- handles[i].AddrOfPinnedObject()
            let ph = GCHandle.Alloc(pointers, GCHandleType.Pinned)
            try ImGuiNative.IGN_Plot_PlotBarGroups_FloatPtr(ph.AddrOfPinnedObject(), values,
                    labels.Length, groupCount,
                    defaultArg groupSize 0.67, defaultArg shift 0.0, defaultArg flags PlotBarGroups.None)
            finally ph.Free()
        finally for h in handles do if h.IsAllocated then h.Free()

    static member PlotBarGroups(labels: string[], values: double[], groupCount,
                                ?groupSize, ?shift, ?flags: PlotBarGroups) =
        let handles  = Array.zeroCreate<GCHandle> labels.Length
        let pointers = Array.zeroCreate<nativeint> labels.Length
        try
            for i in 0 .. labels.Length - 1 do
                let bytes = System.Text.Encoding.UTF8.GetBytes(labels[i] + "\0")
                handles[i]  <- GCHandle.Alloc(bytes, GCHandleType.Pinned)
                pointers[i] <- handles[i].AddrOfPinnedObject()
            let ph = GCHandle.Alloc(pointers, GCHandleType.Pinned)
            try ImGuiNative.IGN_Plot_PlotBarGroups_DoublePtr(ph.AddrOfPinnedObject(), values,
                    labels.Length, groupCount,
                    defaultArg groupSize 0.67, defaultArg shift 0.0, defaultArg flags PlotBarGroups.None)
            finally ph.Free()
        finally for h in handles do if h.IsAllocated then h.Free()

    static member PlotCandles(label, xs: double[], opens: double[], highs: double[], lows: double[], closes: double[],
                              ?width, ?bullColor: Color, ?bearColor: Color) =
        let count = min xs.Length (min opens.Length (min highs.Length (min lows.Length closes.Length)))
        let bc = defaultArg (Option.map (fun (c: Color) -> c.ToUint32()) bullColor) 0xFF00FF00u
        let br = defaultArg (Option.map (fun (c: Color) -> c.ToUint32()) bearColor) 0xFF0000FFu
        ImGuiNative.IGN_Plot_PlotCandles(label, xs, opens, highs, lows, closes, count,
            defaultArg width 0.67, bc, br, 0, sizeof<double>)
    static member PlotCandles(label, xs: DateTime[], opens: double[], highs: double[], lows: double[], closes: double[],
                              ?width, ?bullColor: Color, ?bearColor: Color) =
        let ts = xs |> Array.map (fun dt -> float (DateTimeOffset(dt).ToUnixTimeSeconds()))
        let count = min ts.Length (min opens.Length (min highs.Length (min lows.Length closes.Length)))
        let bc = defaultArg (Option.map (fun (c: Color) -> c.ToUint32()) bullColor) 0xFF00FF00u
        let br = defaultArg (Option.map (fun (c: Color) -> c.ToUint32()) bearColor) 0xFF0000FFu
        ImGuiNative.IGN_Plot_PlotCandles(label, ts, opens, highs, lows, closes, count,
            defaultArg width 0.67, bc, br, 0, sizeof<double>)

    static member IsPlotHovered() = ImGuiNative.IGN_Plot_IsPlotHovered()
    static member GetPlotMousePos(yAxis) =
        let mutable x, y = 0.0, 0.0
        ImGuiNative.IGN_Plot_GetPlotMousePos(&x, &y, yAxis)
        x, y
    static member PlotToPixels(x, y, ?yAxis) =
        let mutable px, py = 0.f, 0.f
        ImGuiNative.IGN_Plot_PlotToPixels(x, y, &px, &py, defaultArg yAxis 0)
        px, py
    static member PlotText(text: string, x, y, ?pixOffsetX, ?pixOffsetY) =
        ImGuiNative.IGN_Plot_PlotText(text, x, y,
            defaultArg pixOffsetX 0f, defaultArg pixOffsetY 0f)
    static member PlotDummy(labelId: string) = ImGuiNative.IGN_Plot_PlotDummy(labelId)

    // ── ImPlot 3D ─────────────────────────────────────────────────────────────
    static member BeginPlot3D(titleId, ?w, ?h, ?flags: Plot3D) =
        ImGuiNative.IGN_Plot3D_BeginPlot(titleId,
            defaultArg w -1f, defaultArg h -1f, defaultArg flags Plot3D.None)
    static member EndPlot3D() = ImGuiNative.IGN_Plot3D_EndPlot()
    static member SetupAxes3D(xLabel, yLabel, zLabel,
                              ?xFlags: Plot3DAxis, ?yFlags: Plot3DAxis, ?zFlags: Plot3DAxis) =
        ImGuiNative.IGN_Plot3D_SetupAxes(xLabel, yLabel, zLabel,
            defaultArg xFlags Plot3DAxis.None,
            defaultArg yFlags Plot3DAxis.None,
            defaultArg zFlags Plot3DAxis.None)
    static member ShowPlot3DDemoWindow(?pOpen) =
        BoolPtr.withOptRef pOpen ImGuiNative.IGN_Plot3D_ShowDemoWindow

    static member PlotLine3D(label, xs: float32[], ys: float32[], zs: float32[]) =
        ImGuiNative.IGN_Plot3D_PlotLine(label, xs, ys, zs,
            min xs.Length (min ys.Length zs.Length), 0, sizeof<float32>)
    static member PlotLine3D(label: string, xs: double[], ys: double[], zs: double[]) =
        ImGuiNative.IGN_Plot3D_PlotLine_Double(label, xs, ys, zs,
            min xs.Length (min ys.Length zs.Length), 0, sizeof<double>)

    static member PlotScatter3D(label, xs: float32[], ys: float32[], zs: float32[]) =
        ImGuiNative.IGN_Plot3D_PlotScatter(label, xs, ys, zs,
            min xs.Length (min ys.Length zs.Length), 0, sizeof<float32>)
    static member PlotScatter3D(label: string, xs: double[], ys: double[], zs: double[]) =
        ImGuiNative.IGN_Plot3D_PlotScatter_Double(label, xs, ys, zs,
            min xs.Length (min ys.Length zs.Length), 0, sizeof<double>)

    static member PlotSurface3D(label, xs: float32[], ys: float32[], zs: float32[], xCount, yCount) =
        ImGuiNative.IGN_Plot3D_PlotSurface(label, xs, ys, zs, xCount, yCount, 0, xCount * sizeof<float32>)
    static member PlotSurface3D(label: string, xs: double[], ys: double[], zs: double[], xCount, yCount) =
        ImGuiNative.IGN_Plot3D_PlotSurface_Double(label, xs, ys, zs, xCount, yCount, 0, xCount * sizeof<double>)

    static member PlotTriangle3D(label, xs: float32[], ys: float32[], zs: float32[]) =
        ImGuiNative.IGN_Plot3D_PlotTriangle(label, xs, ys, zs,
            min xs.Length (min ys.Length zs.Length), 0, sizeof<float32>)
    static member PlotTriangle3D(label: string, xs: double[], ys: double[], zs: double[]) =
        ImGuiNative.IGN_Plot3D_PlotTriangle_Double(label, xs, ys, zs,
            min xs.Length (min ys.Length zs.Length), 0, sizeof<double>)

    static member PlotQuad3D(label, xs: float32[], ys: float32[], zs: float32[]) =
        ImGuiNative.IGN_Plot3D_PlotQuad(label, xs, ys, zs,
            min xs.Length (min ys.Length zs.Length), 0, sizeof<float32>)
    static member PlotQuad3D(label: string, xs: double[], ys: double[], zs: double[]) =
        ImGuiNative.IGN_Plot3D_PlotQuad_Double(label, xs, ys, zs,
            min xs.Length (min ys.Length zs.Length), 0, sizeof<double>)

    static member PlotMesh3D(label, xs: float32[], ys: float32[], zs: float32[], indices: uint32[]) =
        ImGuiNative.IGN_Plot3D_PlotMesh(label, xs, ys, zs, indices,
            xs.Length, indices.Length, 0, sizeof<float32>)
    static member PlotMesh3D(label: string, xs: double[], ys: double[], zs: double[], indices: uint32[]) =
        ImGuiNative.IGN_Plot3D_PlotMesh_Double(label, xs, ys, zs, indices,
            xs.Length, indices.Length, 0, sizeof<double>)

    static member PlotText3D(text, x, y, z, ?angle, ?pixOffsetX, ?pixOffsetY) =
        ImGuiNative.IGN_Plot3D_PlotText(text, x, y, z,
            defaultArg angle 0.0, defaultArg pixOffsetX 0f, defaultArg pixOffsetY 0f)
    static member PlotDummy3D(labelId: string) = ImGuiNative.IGN_Plot3D_PlotDummy(labelId)

    // ── Inspector — reflection-based property editor ──────────────────────────
    static member Inspector(prefix: string, o: obj) =
        Gui.PushID(prefix)
        let result =
            if Object.ReferenceEquals(o, null) then Gui.Text("null"); false
            else
                let mutable changed = false
                let t = o.GetType()
                for prop in t.GetProperties(BindingFlags.Public ||| BindingFlags.Instance) do
                    let name = prop.Name
                    let value = prop.GetValue(o)
                    Gui.PushID(name)
                    let canWrite = prop.CanWrite && (match prop.SetMethod with null -> false | m -> m.IsPublic)
                    if canWrite then
                        match value with
                        | :? string as s ->
                            let v = ref s
                            if Gui.InputText(name, v) then prop.SetValue(o, v.Value); changed <- true
                        | :? int as i ->
                            let v = ref i
                            if Gui.DragInt(name, v) then prop.SetValue(o, v.Value); changed <- true
                        | :? float32 as f ->
                            let v = ref f
                            if Gui.DragFloat(name, v) then prop.SetValue(o, v.Value); changed <- true
                        | :? double as d ->
                            let v = ref d
                            if Gui.DragDouble(name, v) then prop.SetValue(o, v.Value); changed <- true
                        | :? bool as b ->
                            let v = ref b
                            if Gui.Checkbox(name, v) then prop.SetValue(o, v.Value); changed <- true
                        | _ ->
                            Gui.Text($"{name}: {value}")
                    else
                        Gui.Text($"{name}: {value}")
                    Gui.PopID()
                changed
        Gui.PopID()
        result

    // ── Texture ───────────────────────────────────────────────────────────────
    /// Upload raw RGBA pixel data to the GPU. Caller must ensure a GL context
    /// is current (i.e. call Window.MakeCurrent() first).
    /// Returns a texture ID usable with Image / ImageButton / DrawImage.
    static member LoadTexture(pixels: byte[], width: int, height: int) : uint32 =
        let handle = GCHandle.Alloc(pixels, GCHandleType.Pinned)
        try
            ImGuiNative.IGN_LoadTextureFromMemory(handle.AddrOfPinnedObject(), width, height)
        finally
            handle.Free()

    /// Delete a texture previously created with LoadTexture.
    static member FreeTexture(texId: uint32) =
        ImGuiNative.IGN_FreeTexture(texId)
