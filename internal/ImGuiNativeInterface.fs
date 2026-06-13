namespace ImGuiFSharp

open System.Runtime.InteropServices
open ImGuiFSharp.Flags
open ImGuiFSharp.Enums

// ══════════════════════════════════════════════════════════════════════════════
// All PInvoke declarations for the native ImGui bridge.
// Nothing public lives here — use ImGuiInternal, Gui, or Builder instead.
// ══════════════════════════════════════════════════════════════════════════════
module internal ImGuiNative =
    [<Literal>]
    let LibName = "ImGuiNative"

    // ── Lifecycle ─────────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern nativeint IGN_CreateContext()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DestroyContext(nativeint ctx)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetCurrentContext(nativeint ctx)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_MoveWindowsToVisibleRange()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetDisplaySize(float32 w, float32 h)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetDeltaTime(float32 dt)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_NewFrame()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Render()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern nativeint IGN_GetDrawData()

    // ── Draw-data extraction ──────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern int IGN_DrawData_GetCmdListCount(nativeint drawData)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawData_GetCmdList(nativeint drawData, int idx,
        int& vtxCount, int& idxCount,
        nativeint& vtxPtr, nativeint& idxPtr, int& cmdCount)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawData_GetCmd(nativeint drawData, int listIdx, int cmdIdx,
        int& elemCount, uint32& texId,
        float32& clipX, float32& clipY, float32& clipZ, float32& clipW,
        uint32& idxOffset, uint32& vtxOffset)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawData_GetDisplayInfo(nativeint drawData,
        float32& posX, float32& posY, float32& sizeW, float32& sizeH,
        float32& fbScaleX, float32& fbScaleY)

    // ── Font atlas ────────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Font_Build()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Font_GetTexData(nativeint& pixels, int& width, int& height)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Font_SetTexID(uint32 id)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern int IGN_Font_AddDefault()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern int IGN_Font_AddFromFile(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string path, float32 sizePixels)

    // ── Input ─────────────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Input_SetMousePos(float32 x, float32 y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Input_SetMouseButton(int btn, [<MarshalAs(UnmanagedType.I1)>] bool down)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Input_SetMouseWheel(float32 x, float32 y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Input_AddKey(int imguiKey, [<MarshalAs(UnmanagedType.I1)>] bool down)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Input_AddChar(uint32 c)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Input_SetModifiers(
        [<MarshalAs(UnmanagedType.I1)>] bool ctrl,
        [<MarshalAs(UnmanagedType.I1)>] bool shift,
        [<MarshalAs(UnmanagedType.I1)>] bool alt,
        [<MarshalAs(UnmanagedType.I1)>] bool super)

    // ── Widgets ───────────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Begin(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string name, nativeint pOpen, Window flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_End()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Button(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, float32 w, float32 h)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Text([<MarshalAs(UnmanagedType.LPUTF8Str)>] string text)

    // InputText — raw buffer version (used internally only)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputText(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        nativeint buf, int bufSize, InputText flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputTextMultiline(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        nativeint buf, int bufSize, float32 w, float32 h, InputText flags)

    // InputText — string/resize version (F# string ref friendly)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputText_String(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        nativeint& buf, int& bufLen, int& bufCap, InputText flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputTextMultiline_String(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        nativeint& buf, int& bufLen, int& bufCap, float32 w, float32 h, InputText flags)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputFloat(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32& v, float32 step, float32 stepFast,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt, InputText flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputInt(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        int& v, int step, int stepFast, InputText flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_SliderFloat(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32& v, float32 min, float32 max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt, Slider flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_SliderInt(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        int& v, int min, int max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt, Slider flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Checkbox(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, nativeint v)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_CollapsingHeader(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, TreeNode flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_TreeNode(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TreePop()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginTable(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string id,
        int cols, Table flags, float32 outerW, float32 outerH)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndTable()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TableSetupColumn(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, TableColumn flags, float32 init)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TableNextRow(TableRow rowFlags, float32 minH)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_TableNextColumn()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TableSetupScrollFreeze(int cols, int rows)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TableSetBgColor(int target, uint32 color, int column_n)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginCombo(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string preview, Combo flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndCombo()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Selectable(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        [<MarshalAs(UnmanagedType.I1)>] bool selected, Selectable flags, float32 w, float32 h)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginMenuBar()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndMenuBar()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginMenu(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        [<MarshalAs(UnmanagedType.I1)>] bool enabled)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndMenu()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_MenuItem(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string shortcut,
        [<MarshalAs(UnmanagedType.I1)>] bool selected,
        [<MarshalAs(UnmanagedType.I1)>] bool enabled)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Separator()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SeparatorText([<MarshalAs(UnmanagedType.LPUTF8Str)>] string label)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SameLine(float32 offset, float32 spacing)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_NewLine_()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Spacing()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PushID_Str([<MarshalAs(UnmanagedType.LPUTF8Str)>] string id)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PopID()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern uint32 IGN_GetID([<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_ColorEdit4(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, nativeint col, ColorEdit flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_DragFloat(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32& v, float32 speed, float32 min, float32 max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt, Slider flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_DragInt(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        int& v, float32 speed, int min, int max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt, Slider flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_RadioButton(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        [<MarshalAs(UnmanagedType.I1)>] bool active)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_ProgressBar(float32 fraction, float32 w, float32 h,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string overlay)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Image(uint32 texId, float32 w, float32 h, float32 uv0_x, float32 uv0_y, float32 uv1_x, float32 uv1_y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_ImageButton(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string id,
        uint32 texId, float32 w, float32 h, float32 uv0_x, float32 uv0_y, float32 uv1_x, float32 uv1_y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetNextWindowPos(float32 x, float32 y, Cond cond)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetNextWindowSize(float32 w, float32 h, Cond cond)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetNextWindowBgAlpha(float32 alpha)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetNextWindowContentSize(float32 w, float32 h)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetNextItemWidth(float32 itemWidth)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_ShowDemoWindow(nativeint pOpen)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern uint32 IGN_DockSpace(uint32 id, float32 w, float32 h, DockNode flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern uint32 IGN_DockSpaceOverViewport(uint32 dockspace_id, DockNode flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetNextWindowDockID(uint32 dock_id, Cond cond)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern uint32 IGN_GetWindowDockID()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsWindowDocked()

    // DockBuilder API
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DockBuilderDockWindow(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string window_name, uint32 node_id)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern uint32 IGN_DockBuilderAddNode(uint32 node_id, DockNode flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DockBuilderSetNodeFlags(uint32 node_id, DockNode flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DockBuilderRemoveNode(uint32 node_id)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DockBuilderRemoveNodeDockedWindows(
        uint32 node_id, [<MarshalAs(UnmanagedType.I1)>] bool clear_settings_refs)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DockBuilderRemoveNodeChildNodes(uint32 node_id)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DockBuilderSetNodePos(uint32 node_id, float32 pos_x, float32 pos_y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DockBuilderSetNodeSize(uint32 node_id, float32 size_x, float32 size_y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern uint32 IGN_DockBuilderSplitNode(
        uint32 node_id, Dir split_dir, float32 size_ratio_for_node_at_dir,
        uint32& out_id_at_dir, uint32& out_id_at_opposite_dir)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DockBuilderFinish(uint32 node_id)

    // ── ImPlot ────────────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Plot_BeginPlot(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string titleId,
        float32 w, float32 h, Plot flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_EndPlot()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetupAxes(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string xLabel,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string yLabel,
        Flags.PlotAxis xFlags, Flags.PlotAxis yFlags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotLine_FloatPtrInt(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] values, int count, double xscale, double x0, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotBars_FloatPtrInt(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] values, int count, double barSize, double shift, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotScatter_FloatPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotHeatmap(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] values, int rows, int cols,
        double scaleMin, double scaleMax,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_ShowDemoWindow(nativeint pOpen)

    // ── ImPlot3D ──────────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_CreateContext()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_DestroyContext()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Plot3D_BeginPlot(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string titleId,
        float32 w, float32 h, Plot3D flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_EndPlot()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_SetupAxes(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string xLabel,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string yLabel,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string zLabel,
        Plot3DAxis xFlags, Plot3DAxis yFlags, Plot3DAxis zFlags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotLine(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, float32[] zs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotScatter(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, float32[] zs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotSurface(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, float32[] zs,
        int xCount, int yCount, int offset, int rowStride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_ShowDemoWindow(nativeint pOpen)

    // ── Double-precision widgets ──────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputDouble(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double& v, double step, double step_fast,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string format, InputText flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_DragDouble(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double& v, float32 speed, double v_min, double v_max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string format, Slider flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_SliderDouble(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double& v, double v_min, double v_max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string format, Slider flags)

    // ── Text variants ─────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TextColored(float32 r, float32 g, float32 b, float32 a,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string text)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TextDisabled([<MarshalAs(UnmanagedType.LPUTF8Str)>] string text)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TextWrapped([<MarshalAs(UnmanagedType.LPUTF8Str)>] string text)

    // ── Layout ────────────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginChild(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, float32 w, float32 h,
        [<MarshalAs(UnmanagedType.I1)>] bool border, Child flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndChild()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_BeginGroup()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndGroup()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Dummy(float32 w, float32 h)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Indent(float32 indent_w)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Unindent(float32 indent_w)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_GetContentRegionAvail(float32& x, float32& y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_GetWindowPos(float32& x, float32& y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_GetWindowSize(float32& x, float32& y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_GetMainViewportWorkPos(float32& x, float32& y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_GetMainViewportWorkSize(float32& x, float32& y)

    // ── Style ─────────────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PushStyleColor(Col idx, float32 r, float32 g, float32 b, float32 a)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PopStyleColor(int count)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PushStyleVar_Float(StyleVar idx, float32 value)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PushStyleVar_Vec2(StyleVar idx, float32 x, float32 y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PopStyleVar(int count)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PushItemFlag(int option, [<MarshalAs(UnmanagedType.I1)>] bool enabled)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PopItemFlag()

    // ── Queries ───────────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsItemHovered(Hovered flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsItemActive()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsItemClicked(MouseButton mouse_button)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsItemVisible()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsItemEdited()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsItemDeactivatedAfterEdit()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsMouseClicked(MouseButton button,
        [<MarshalAs(UnmanagedType.I1)>] bool repeat)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsMouseDown(MouseButton button)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsMouseDoubleClicked(MouseButton button)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_GetMousePos(float32& x, float32& y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsWindowFocused(int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsWindowHovered(int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetItemDefaultFocus()

    // ── Scroll ────────────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern float32 IGN_GetScrollY()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern float32 IGN_GetScrollMaxY()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetScrollY(float32 scroll_y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetScrollHereY(float32 center_y_ratio)

    // ── Metrics / text size ───────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_CalcTextSize([<MarshalAs(UnmanagedType.LPUTF8Str)>] string text,
        float32& out_w, float32& out_h)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern float32 IGN_GetFrameHeight()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern float32 IGN_GetTextLineHeight()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern float32 IGN_GetTextLineHeightWithSpacing()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern float32 IGN_GetFrameHeightWithSpacing()

    // ── Tooltips, popups, modals ──────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_BeginTooltip()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndTooltip()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetTooltip([<MarshalAs(UnmanagedType.LPUTF8Str)>] string text)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginItemTooltip()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetItemTooltip([<MarshalAs(UnmanagedType.LPUTF8Str)>] string text)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_OpenPopup([<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, Popup flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginPopup(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, Popup flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginPopupModal(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string name, nativeint p_open, Window flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndPopup()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_CloseCurrentPopup()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginPopupContextItem(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, Popup flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginPopupContextWindow(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, Popup flags)

    // ── Tab Bars & List Boxes ─────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginTabBar(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, TabBar flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndTabBar()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginTabItem(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, nativeint p_open, TabItem flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndTabItem()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginListBox(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, float32 w, float32 h)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndListBox()

    // ── Canvas drawing ────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_GetCursorScreenPos(float32& x, float32& y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetCursorScreenPos(float32 x, float32 y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InvisibleButton(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, float32 w, float32 h, Button flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddLine(float32 p1_x, float32 p1_y, float32 p2_x, float32 p2_y,
        uint32 col, float32 thickness)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddRect(float32 p1_x, float32 p1_y, float32 p2_x, float32 p2_y,
        uint32 col, float32 rounding, Draw flags, float32 thickness)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddRectFilled(float32 p1_x, float32 p1_y, float32 p2_x, float32 p2_y,
        uint32 col, float32 rounding, Draw flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddRectFilledMultiColor(float32 p1_x, float32 p1_y,
        float32 p2_x, float32 p2_y,
        uint32 col_upr_left, uint32 col_upr_right, uint32 col_bot_right, uint32 col_bot_left)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddCircle(float32 center_x, float32 center_y, float32 radius,
        uint32 col, int num_segments, float32 thickness)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddCircleFilled(float32 center_x, float32 center_y, float32 radius,
        uint32 col, int num_segments)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddTriangleFilled(float32 p1_x, float32 p1_y,
        float32 p2_x, float32 p2_y, float32 p3_x, float32 p3_y, uint32 col)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddText(float32 pos_x, float32 pos_y, uint32 col,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string text_begin)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddPolyline(float32[] points_x, float32[] points_y,
        int num_points, uint32 col, Draw flags, float32 thickness)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddConvexPolyFilled(float32[] points_x, float32[] points_y,
        int num_points, uint32 col)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddImage(uint32 user_texture_id,
        float32 p1_x, float32 p1_y, float32 p2_x, float32 p2_y,
        float32 uv1_x, float32 uv1_y, float32 uv2_x, float32 uv2_y, uint32 col)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_PushClipRect(float32 clip_rect_min_x, float32 clip_rect_min_y,
        float32 clip_rect_max_x, float32 clip_rect_max_y,
        [<MarshalAs(UnmanagedType.I1)>] bool intersect_with_current_clip_rect)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_PopClipRect()

    // ── ImPlot double-precision ───────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetupAxisLimits(PlotAxis axis, double v_min, double v_max, Cond cond)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetNextAxesLimits(double x_min, double x_max, double y_min, double y_max, Cond cond)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetupLegend(PlotLocation location, PlotLegend flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetupAxisScale(PlotAxis axis, PlotScale scale)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetupAxisFormat(PlotAxis axis,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotLine_DoublePtrInt(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] values, int count, double xscale, double x0, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotLine_DoublePtrPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotBars_DoublePtrPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, int count, double width, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotScatter_DoublePtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotShaded_DoublePtrPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_id,
        double[] xs, double[] ys1, double[] ys2, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotStairs_DoublePtrPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_id,
        double[] xs, double[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotErrorBars_DoublePtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_id,
        double[] xs, double[] ys, double[] err, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotPieChart(
        nativeint label_ids, double[] values, int count, double x, double y, double radius,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_fmt, double angle0)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Plot_IsPlotHovered()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_GetPlotMousePos(double& x, double& y, int y_axis)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotToPixels(double x, double y, float32& pix_x, float32& pix_y, int y_axis)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotCandles(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_id,
        double[] xs, double[] opens, double[] highs, double[] lows, double[] closes,
        int count, double width, uint32 bullColor, uint32 bearColor, int offset, int stride)

    // ── ImPlot3D double-precision ─────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotLine_Double(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, double[] zs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotScatter_Double(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, double[] zs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotSurface_Double(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, double[] zs, int xCount, int yCount, int offset, int rowStride)

    // ── ImPlot float variants ─────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotShaded_FloatPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys1, float32[] ys2, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotStairs_FloatPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotErrorBars_FloatPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, float32[] err, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotPieChart_Float(
        nativeint label_ids, float32[] values, int count, double x, double y, double radius,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_fmt, double angle0)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotBubbles_FloatPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, float32[] szs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotBubbles_DoublePtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, double[] szs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotPolygon_FloatPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotPolygon_DoublePtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotBarGroups_FloatPtr(
        nativeint label_ids, float32[] values, int item_count, int group_count,
        double group_size, double shift, PlotBarGroups flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotBarGroups_DoublePtr(
        nativeint label_ids, double[] values, int item_count, int group_count,
        double group_size, double shift, PlotBarGroups flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotStems_FloatPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, int count, double ref_, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotStems_DoublePtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, int count, double ref_, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotInfLines_FloatPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] values, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotInfLines_DoublePtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] values, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern double IGN_Plot_PlotHistogram_FloatPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] values, int count, int bins, double bar_scale,
        double range_min, double range_max, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern double IGN_Plot_PlotHistogram_DoublePtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] values, int count, int bins, double bar_scale,
        double range_min, double range_max, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern double IGN_Plot_PlotHistogram2D_FloatPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, int count, int x_bins, int y_bins,
        double xmin, double xmax, double ymin, double ymax, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern double IGN_Plot_PlotHistogram2D_DoublePtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, int count, int x_bins, int y_bins,
        double xmin, double xmax, double ymin, double ymax, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotDigital_FloatPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotDigital_DoublePtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotText(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string text,
        double x, double y, float32 pix_offset_x, float32 pix_offset_y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotDummy([<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_id)

    // ── ImPlot3D extended ─────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotTriangle(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, float32[] zs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotTriangle_Double(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, double[] zs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotQuad(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, float32[] zs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotQuad_Double(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, double[] zs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotMesh(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32[] xs, float32[] ys, float32[] zs, uint32[] idxs,
        int vtx_count, int idx_count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotMesh_Double(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        double[] xs, double[] ys, double[] zs, uint32[] idxs,
        int vtx_count, int idx_count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotText(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string text,
        double x, double y, double z, double angle, float32 pix_offset_x, float32 pix_offset_y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotDummy([<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_id)

    // ── Texture ───────────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern uint32 IGN_LoadTextureFromMemory(nativeint rgba_pixels, int width, int height)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_FreeTexture(uint32 texId)

    // ── List Clipper ──────────────────────────────────────────────────────────
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern nativeint IGN_Clipper_Create()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Clipper_Destroy(nativeint clipper)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Clipper_Begin(nativeint clipper, int items_count, float32 items_height)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Clipper_Step(nativeint clipper)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Clipper_End(nativeint clipper)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern int IGN_Clipper_GetDisplayStart(nativeint clipper)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern int IGN_Clipper_GetDisplayEnd(nativeint clipper)
