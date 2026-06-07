namespace ImGuiFSharp

open System
open System.Runtime.InteropServices

// ══════════════════════════════════════════════════════════════════════════════
// A. PInvoke declarations
// ══════════════════════════════════════════════════════════════════════════════
module public ImGuiNative =
    [<Literal>]
    let LibName = "ImGuiNative"

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

    // Input
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

    // Widgets
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Begin(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string name, nativeint pOpen, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_End()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Button(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, float32 w, float32 h)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Text([<MarshalAs(UnmanagedType.LPUTF8Str)>] string text)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputText(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        nativeint buf, int bufSize, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputFloat(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32& v, float32 step, float32 stepFast,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputInt(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        int& v, int step, int stepFast, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_SliderFloat(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32& v, float32 min, float32 max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_SliderInt(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        int& v, int min, int max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Checkbox(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, nativeint v)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_CollapsingHeader(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_TreeNode(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TreePop()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginTable(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string id,
        int cols, int flags, float32 outerW, float32 outerH)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndTable()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TableSetupColumn(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, int flags, float32 init)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TableNextRow(int rowFlags, float32 minH)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TableNextColumn()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginCombo(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string preview, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndCombo()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Selectable(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        [<MarshalAs(UnmanagedType.I1)>] bool selected, int flags, float32 w, float32 h)
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
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_ColorEdit4(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, nativeint col, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_DragFloat(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        float32& v, float32 speed, float32 min, float32 max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_DragInt(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        int& v, float32 speed, int min, int max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_RadioButton(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label,
        [<MarshalAs(UnmanagedType.I1)>] bool active)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_ProgressBar(float32 fraction, float32 w, float32 h,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string overlay)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Image(uint32 texId, float32 w, float32 h)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_ImageButton(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string id,
        uint32 texId, float32 w, float32 h)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetNextWindowPos(float32 x, float32 y, int cond)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetNextWindowSize(float32 w, float32 h, int cond)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_ShowDemoWindow(nativeint pOpen)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern uint32 IGN_DockSpace(uint32 id, float32 w, float32 h, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DockSpaceOverViewport(int flags)

    // ImPlot
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_CreateContext()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_DestroyContext()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Plot_BeginPlot(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string titleId,
        float32 w, float32 h, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_EndPlot()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetupAxes(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string xLabel,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string yLabel,
        int xFlags, int yFlags)
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

    // ImPlot3D
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_CreateContext()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_DestroyContext()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Plot3D_BeginPlot(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string titleId,
        float32 w, float32 h, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_EndPlot()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_SetupAxes(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string xLabel,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string yLabel,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string zLabel,
        int xFlags, int yFlags, int zFlags)
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

    // Double-precision widgets
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputDouble(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double& v, double step, double step_fast,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string format, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_DragDouble(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double& v, float32 speed, double v_min, double v_max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string format, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_SliderDouble(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double& v, double v_min, double v_max,
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string format, int flags)

    // Text variants
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TextColored(float32 r, float32 g, float32 b, float32 a, [<MarshalAs(UnmanagedType.LPUTF8Str)>] string text)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TextDisabled([<MarshalAs(UnmanagedType.LPUTF8Str)>] string text)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_TextWrapped([<MarshalAs(UnmanagedType.LPUTF8Str)>] string text)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InputTextMultiline(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, nativeint buf, int bufSize, float32 w, float32 h, int flags)

    // Layout
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginChild(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, float32 w, float32 h, [<MarshalAs(UnmanagedType.I1)>] bool border, int flags)
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
    extern void IGN_GetWindowSize(float32& x, float32& y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_GetWindowPos(float32& x, float32& y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetNextWindowBgAlpha(float32 alpha)

    // Style
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PushStyleColor(int idx, float32 r, float32 g, float32 b, float32 a)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PopStyleColor(int count)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PushStyleVar_Float(int idx, float32 value)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PushStyleVar_Vec2(int idx, float32 x, float32 y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_PopStyleVar(int count)

    // Queries
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsItemHovered(int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsItemActive()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsItemClicked(int mouse_button)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsMouseClicked(int button, [<MarshalAs(UnmanagedType.I1)>] bool repeat)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsMouseDown(int button)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_IsMouseDoubleClicked(int button)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_GetMousePos(float32& x, float32& y)

    // Tooltips, popups, modals
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
    extern void IGN_OpenPopup([<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginPopup([<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginPopupModal(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string name, nativeint p_open, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndPopup()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_CloseCurrentPopup()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginPopupContextItem([<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginPopupContextWindow([<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, int flags)

    // Tab Bars & List Boxes
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginTabBar([<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndTabBar()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginTabItem(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, nativeint p_open, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndTabItem()
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_BeginListBox([<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, float32 w, float32 h)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_EndListBox()

    // Canvas drawing
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_GetCursorScreenPos(float32& x, float32& y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_SetCursorScreenPos(float32 x, float32 y)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_InvisibleButton(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string str_id, float32 w, float32 h, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddLine(float32 p1_x, float32 p1_y, float32 p2_x, float32 p2_y, uint32 col, float32 thickness)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddRect(float32 p1_x, float32 p1_y, float32 p2_x, float32 p2_y, uint32 col, float32 rounding, int flags, float32 thickness)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddRectFilled(float32 p1_x, float32 p1_y, float32 p2_x, float32 p2_y, uint32 col, float32 rounding, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddRectFilledMultiColor(float32 p1_x, float32 p1_y, float32 p2_x, float32 p2_y, uint32 col_upr_left, uint32 col_upr_right, uint32 col_bot_right, uint32 col_bot_left)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddCircle(float32 center_x, float32 center_y, float32 radius, uint32 col, int num_segments, float32 thickness)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddCircleFilled(float32 center_x, float32 center_y, float32 radius, uint32 col, int num_segments)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddTriangleFilled(float32 p1_x, float32 p1_y, float32 p2_x, float32 p2_y, float32 p3_x, float32 p3_y, uint32 col)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddText(float32 pos_x, float32 pos_y, uint32 col, [<MarshalAs(UnmanagedType.LPUTF8Str)>] string text_begin)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddPolyline(float32[] points_x, float32[] points_y, int num_points, uint32 col, int flags, float32 thickness)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddConvexPolyFilled(float32[] points_x, float32[] points_y, int num_points, uint32 col)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_AddImage(uint32 user_texture_id, float32 p1_x, float32 p1_y, float32 p2_x, float32 p2_y, float32 uv1_x, float32 uv1_y, float32 uv2_x, float32 uv2_y, uint32 col)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_PushClipRect(float32 clip_rect_min_x, float32 clip_rect_min_y, float32 clip_rect_max_x, float32 clip_rect_max_y, [<MarshalAs(UnmanagedType.I1)>] bool intersect_with_current_clip_rect)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_DrawList_PopClipRect()

    // ImPlot double-precision
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetupAxisLimits(int axis, double v_min, double v_max, int cond)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetNextAxesLimits(double x_min, double x_max, double y_min, double y_max, int cond)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetupLegend(int location, int flags)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetupAxisScale(int axis, int scale)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_SetupAxisFormat(int axis, [<MarshalAs(UnmanagedType.LPUTF8Str)>] string fmt)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotLine_DoublePtrInt(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double[] values, int count, double xscale, double x0, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotLine_DoublePtrPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double[] xs, double[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotBars_DoublePtrPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double[] xs, double[] ys, int count, double width, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotScatter_DoublePtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double[] xs, double[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotShaded_DoublePtrPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_id, double[] xs, double[] ys1, double[] ys2, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotStairs_DoublePtrPtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_id, double[] xs, double[] ys, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot_PlotErrorBars_DoublePtr(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_id, double[] xs, double[] ys, double[] err, int count, int offset, int stride)
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
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label_id, double[] xs, double[] opens, double[] highs, double[] lows, double[] closes,
        int count, double width, uint32 bullColor, uint32 bearColor, int offset, int stride)

    // ImPlot3D double-precision
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotLine_Double(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double[] xs, double[] ys, double[] zs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotScatter_Double(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double[] xs, double[] ys, double[] zs, int count, int offset, int stride)
    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Plot3D_PlotSurface_Double(
        [<MarshalAs(UnmanagedType.LPUTF8Str)>] string label, double[] xs, double[] ys, double[] zs, int xCount, int yCount, int offset, int rowStride)

// Helper: pin a bool ref and call a native function that may modify it
module private BoolPtr =
    let inline withRef (r: bool ref) (f: nativeint -> 'a) =
        let arr = [| (if !r then 1uy else 0uy) |]
        let h = GCHandle.Alloc(arr, GCHandleType.Pinned)
        try
            let result = f (h.AddrOfPinnedObject())
            r := (arr.[0] <> 0uy)
            result
        finally h.Free()

    let inline withOptRef (r: bool ref option) (f: nativeint -> 'a) =
        match r with
        | None   -> f 0n
        | Some r -> withRef r f

// ══════════════════════════════════════════════════════════════════════════════
// B. ImGuiImpl — implements the decoupled interface specifications
// ══════════════════════════════════════════════════════════════════════════════
type public ImGuiImpl() =

    interface IGuiFunctions with
        member _.Begin(name, ?pOpen, ?flags) =
            BoolPtr.withOptRef pOpen (fun ptr ->
                ImGuiNative.IGN_Begin(name, ptr, defaultArg flags 0))
        member _.End() = ImGuiNative.IGN_End()

        member _.Button(label, ?w, ?h) =
            ImGuiNative.IGN_Button(label, defaultArg w 0f, defaultArg h 0f)
        member _.Text(text) = ImGuiNative.IGN_Text(text)

        member _.InputText(label, buf, ?flags) =
            let bytes = System.Text.Encoding.UTF8.GetBytes(new string(buf))
            let managed = Array.zeroCreate<byte> (bytes.Length + 1)
            Buffer.BlockCopy(bytes, 0, managed, 0, bytes.Length)
            let h = GCHandle.Alloc(managed, GCHandleType.Pinned)
            try
                let r = ImGuiNative.IGN_InputText(label, h.AddrOfPinnedObject(), managed.Length, defaultArg flags 0)
                if r then
                    let decoded = System.Text.Encoding.UTF8.GetString(managed).TrimEnd('\000')
                    let src = decoded.ToCharArray()
                    let len = min src.Length buf.Length
                    Array.blit src 0 buf 0 len
                    for i in len .. buf.Length - 1 do buf.[i] <- '\000'
                r
            finally h.Free()

        member _.InputFloat(label, v, ?step, ?stepFast, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_InputFloat(label, &vv, defaultArg step 0f, defaultArg stepFast 0f, defaultArg fmt "%.3f", defaultArg flags 0)
            v := vv; r
        member _.InputInt(label, v, ?step, ?stepFast, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_InputInt(label, &vv, defaultArg step 1, defaultArg stepFast 100, defaultArg flags 0)
            v := vv; r
        member _.SliderFloat(label, v, min, max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_SliderFloat(label, &vv, min, max, defaultArg fmt "%.3f", defaultArg flags 0)
            v := vv; r
        member _.SliderInt(label, v, min, max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_SliderInt(label, &vv, min, max, defaultArg fmt "%d", defaultArg flags 0)
            v := vv; r
        member _.Checkbox(label, v) =
            BoolPtr.withRef v (fun ptr -> ImGuiNative.IGN_Checkbox(label, ptr))
        member _.CollapsingHeader(label, ?flags) =
            ImGuiNative.IGN_CollapsingHeader(label, defaultArg flags 0)
        member _.TreeNode(label) = ImGuiNative.IGN_TreeNode(label)
        member _.TreePop()       = ImGuiNative.IGN_TreePop()
        member _.Separator()     = ImGuiNative.IGN_Separator()
        member _.SameLine(?offset, ?spacing) =
            ImGuiNative.IGN_SameLine(defaultArg offset 0f, defaultArg spacing -1f)
        member _.NewLine()  = ImGuiNative.IGN_NewLine_()
        member _.Spacing()  = ImGuiNative.IGN_Spacing()
        member _.PushID(id) = ImGuiNative.IGN_PushID_Str(id)
        member _.PopID()    = ImGuiNative.IGN_PopID()

        member _.BeginCombo(label, preview, ?flags) =
            ImGuiNative.IGN_BeginCombo(label, preview, defaultArg flags 0)
        member _.EndCombo() = ImGuiNative.IGN_EndCombo()
        member _.Selectable(label, selected, ?flags, ?width, ?height) =
            ImGuiNative.IGN_Selectable(label, selected, defaultArg flags 0, defaultArg width 0f, defaultArg height 0f)

        member _.BeginTable(id, cols, ?flags, ?ow, ?oh) =
            ImGuiNative.IGN_BeginTable(id, cols, defaultArg flags 0, defaultArg ow 0f, defaultArg oh 0f)
        member _.EndTable() = ImGuiNative.IGN_EndTable()
        member _.TableSetupColumn(label, ?flags, ?init) =
            ImGuiNative.IGN_TableSetupColumn(label, defaultArg flags 0, defaultArg init 0f)
        member _.TableNextRow(?rowFlags, ?minH) =
            ImGuiNative.IGN_TableNextRow(defaultArg rowFlags 0, defaultArg minH 0f)
        member _.TableNextColumn() = ImGuiNative.IGN_TableNextColumn()

        member _.BeginMenuBar() = ImGuiNative.IGN_BeginMenuBar()
        member _.EndMenuBar()   = ImGuiNative.IGN_EndMenuBar()
        member _.BeginMenu(label, ?enabled) =
            ImGuiNative.IGN_BeginMenu(label, defaultArg enabled true)
        member _.EndMenu()      = ImGuiNative.IGN_EndMenu()
        member _.MenuItem(label, ?shortcut, ?selected, ?enabled) =
            ImGuiNative.IGN_MenuItem(label, defaultArg shortcut null, defaultArg selected false, defaultArg enabled true)

        member _.DragFloat(label, v, ?speed, ?min, ?max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_DragFloat(label, &vv, defaultArg speed 1f, defaultArg min 0f, defaultArg max 0f, defaultArg fmt "%.3f", defaultArg flags 0)
            v := vv; r
        member _.DragInt(label, v, ?speed, ?min, ?max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_DragInt(label, &vv, defaultArg speed 1f, defaultArg min 0, defaultArg max 0, defaultArg fmt "%d", defaultArg flags 0)
            v := vv; r

        member _.ColorEdit4(label, col, ?flags) =
            let h = GCHandle.Alloc(col, GCHandleType.Pinned)
            try ImGuiNative.IGN_ColorEdit4(label, h.AddrOfPinnedObject(), defaultArg flags 0)
            finally h.Free()

        member _.RadioButton(label, active) = ImGuiNative.IGN_RadioButton(label, active)
        member _.ProgressBar(fraction, ?w, ?h, ?overlay) =
            ImGuiNative.IGN_ProgressBar(fraction, defaultArg w -1f, defaultArg h 0f, defaultArg overlay null)
        member _.Image(texId, w, h) = ImGuiNative.IGN_Image(texId, w, h)
        member _.ImageButton(id, texId, w, h) = ImGuiNative.IGN_ImageButton(id, texId, w, h)
        member _.SetNextWindowPos(x, y, ?cond) = ImGuiNative.IGN_SetNextWindowPos(x, y, defaultArg cond 0)
        member _.SetNextWindowSize(w, h, ?cond) = ImGuiNative.IGN_SetNextWindowSize(w, h, defaultArg cond 0)
        member _.ShowDemoWindow(?pOpen) =
            BoolPtr.withOptRef pOpen (fun ptr -> ImGuiNative.IGN_ShowDemoWindow(ptr))
        member _.DockSpace(id, w, h, ?flags) =
            ImGuiNative.IGN_DockSpace(id, w, h, defaultArg flags 0)

        // Double-precision widgets
        member _.InputDouble(label, v, ?step, ?stepFast, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_InputDouble(label, &vv, defaultArg step 0.0, defaultArg stepFast 0.0, defaultArg fmt "%.6f", defaultArg flags 0)
            v := vv; r

        member _.DragDouble(label, v, ?speed, ?min, ?max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_DragDouble(label, &vv, defaultArg speed 1.0f, defaultArg min 0.0, defaultArg max 0.0, defaultArg fmt "%.6f", defaultArg flags 0)
            v := vv; r

        member _.SliderDouble(label, v, min, max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_SliderDouble(label, &vv, min, max, defaultArg fmt "%.6f", defaultArg flags 0)
            v := vv; r

        // Text variants
        member _.TextColored(r, g, b, a, text) = ImGuiNative.IGN_TextColored(r, g, b, a, text)
        member _.TextDisabled(text)           = ImGuiNative.IGN_TextDisabled(text)
        member _.TextWrapped(text)            = ImGuiNative.IGN_TextWrapped(text)
        member _.InputTextMultiline(label, buf, ?width, ?height, ?flags) =
            let bytes = System.Text.Encoding.UTF8.GetBytes(new string(buf))
            let managed = Array.zeroCreate<byte> (bytes.Length + 1)
            Buffer.BlockCopy(bytes, 0, managed, 0, bytes.Length)
            let h = GCHandle.Alloc(managed, GCHandleType.Pinned)
            try
                let r = ImGuiNative.IGN_InputTextMultiline(label, h.AddrOfPinnedObject(), managed.Length, defaultArg width 0.f, defaultArg height 0.f, defaultArg flags 0)
                if r then
                    let decoded = System.Text.Encoding.UTF8.GetString(managed).TrimEnd('\000')
                    let src = decoded.ToCharArray()
                    let len = min src.Length buf.Length
                    Array.blit src 0 buf 0 len
                    for i in len .. buf.Length - 1 do buf.[i] <- '\000'
                r
            finally h.Free()

        // Layout
        member _.BeginChild(strId, ?width, ?height, ?border, ?flags) =
            ImGuiNative.IGN_BeginChild(strId, defaultArg width 0.f, defaultArg height 0.f, defaultArg border false, defaultArg flags 0)
        member _.EndChild() = ImGuiNative.IGN_EndChild()
        member _.BeginGroup() = ImGuiNative.IGN_BeginGroup()
        member _.EndGroup() = ImGuiNative.IGN_EndGroup()
        member _.Dummy(w, h) = ImGuiNative.IGN_Dummy(w, h)
        member _.Indent(w) = ImGuiNative.IGN_Indent(w)
        member _.Unindent(w) = ImGuiNative.IGN_Unindent(w)
        member _.GetContentRegionAvail() =
            let mutable x, y = 0.f, 0.f
            ImGuiNative.IGN_GetContentRegionAvail(&x, &y)
            (x, y)
        member _.GetWindowSize() =
            let mutable x, y = 0.f, 0.f
            ImGuiNative.IGN_GetWindowSize(&x, &y)
            (x, y)
        member _.GetWindowPos() =
            let mutable x, y = 0.f, 0.f
            ImGuiNative.IGN_GetWindowPos(&x, &y)
            (x, y)
        member _.SetNextWindowBgAlpha(alpha) = ImGuiNative.IGN_SetNextWindowBgAlpha(alpha)

        // Style
        member _.PushStyleColor(idx, r, g, b, a) = ImGuiNative.IGN_PushStyleColor(idx, r, g, b, a)
        member _.PopStyleColor(?count)           = ImGuiNative.IGN_PopStyleColor(defaultArg count 1)
        member _.PushStyleVar(idx, valFloat: float32) = ImGuiNative.IGN_PushStyleVar_Float(idx, valFloat)
        member _.PushStyleVar(idx, valVec2X: float32, valVec2Y: float32) = ImGuiNative.IGN_PushStyleVar_Vec2(idx, valVec2X, valVec2Y)
        member _.PopStyleVar(?count)             = ImGuiNative.IGN_PopStyleVar(defaultArg count 1)

        // Queries
        member _.IsItemHovered(?flags)           = ImGuiNative.IGN_IsItemHovered(defaultArg flags 0)
        member _.IsItemActive()                  = ImGuiNative.IGN_IsItemActive()
        member _.IsItemClicked(?mouseButton)     = ImGuiNative.IGN_IsItemClicked(defaultArg mouseButton 0)
        member _.IsMouseClicked(button, ?repeat) = ImGuiNative.IGN_IsMouseClicked(button, defaultArg repeat false)
        member _.IsMouseDown(button)             = ImGuiNative.IGN_IsMouseDown(button)
        member _.IsMouseDoubleClicked(button)    = ImGuiNative.IGN_IsMouseDoubleClicked(button)
        member _.GetMousePos() =
            let mutable x, y = 0.f, 0.f
            ImGuiNative.IGN_GetMousePos(&x, &y)
            (x, y)

        // Tooltips, popups, and modals
        member _.BeginTooltip() = ImGuiNative.IGN_BeginTooltip()
        member _.EndTooltip()   = ImGuiNative.IGN_EndTooltip()
        member _.SetTooltip(text) = ImGuiNative.IGN_SetTooltip(text)
        member _.BeginItemTooltip() = ImGuiNative.IGN_BeginItemTooltip()
        member _.SetItemTooltip(text) = ImGuiNative.IGN_SetItemTooltip(text)
        member _.OpenPopup(strId, ?flags) = ImGuiNative.IGN_OpenPopup(strId, defaultArg flags 0)
        member _.BeginPopup(strId, ?flags) = ImGuiNative.IGN_BeginPopup(strId, defaultArg flags 0)
        member _.BeginPopupModal(name, ?pOpen, ?flags) =
            BoolPtr.withOptRef pOpen (fun ptr -> ImGuiNative.IGN_BeginPopupModal(name, ptr, defaultArg flags 0))
        member _.EndPopup() = ImGuiNative.IGN_EndPopup()
        member _.CloseCurrentPopup() = ImGuiNative.IGN_CloseCurrentPopup()
        member _.BeginPopupContextItem(?strId, ?flags) =
            ImGuiNative.IGN_BeginPopupContextItem(defaultArg strId null, defaultArg flags 1)
        member _.BeginPopupContextWindow(?strId, ?flags) =
            ImGuiNative.IGN_BeginPopupContextWindow(defaultArg strId null, defaultArg flags 1)

        // Tab Bars & List Boxes
        member _.BeginTabBar(strId, ?flags) = ImGuiNative.IGN_BeginTabBar(strId, defaultArg flags 0)
        member _.EndTabBar() = ImGuiNative.IGN_EndTabBar()
        member _.BeginTabItem(label, ?pOpen, ?flags) =
            BoolPtr.withOptRef pOpen (fun ptr -> ImGuiNative.IGN_BeginTabItem(label, ptr, defaultArg flags 0))
        member _.EndTabItem() = ImGuiNative.IGN_EndTabItem()
        member _.BeginListBox(label, ?w, ?h) = ImGuiNative.IGN_BeginListBox(label, defaultArg w 0.f, defaultArg h 0.f)
        member _.EndListBox() = ImGuiNative.IGN_EndListBox()

        // Canvas drawing
        member _.GetCursorScreenPos() =
            let mutable x, y = 0.f, 0.f
            ImGuiNative.IGN_GetCursorScreenPos(&x, &y)
            (x, y)
        member _.SetCursorScreenPos(x, y) = ImGuiNative.IGN_SetCursorScreenPos(x, y)
        member _.InvisibleButton(strId, w, h, ?flags) = ImGuiNative.IGN_InvisibleButton(strId, w, h, defaultArg flags 0)
        member _.DrawLine(p1_x, p1_y, p2_x, p2_y, col, ?thickness) =
            ImGuiNative.IGN_DrawList_AddLine(p1_x, p1_y, p2_x, p2_y, col, defaultArg thickness 1.f)
        member _.DrawRect(p1_x, p1_y, p2_x, p2_y, col, ?rounding, ?flags, ?thickness) =
            ImGuiNative.IGN_DrawList_AddRect(p1_x, p1_y, p2_x, p2_y, col, defaultArg rounding 0.f, defaultArg flags 0, defaultArg thickness 1.f)
        member _.DrawRectFilled(p1_x, p1_y, p2_x, p2_y, col, ?rounding, ?flags) =
            ImGuiNative.IGN_DrawList_AddRectFilled(p1_x, p1_y, p2_x, p2_y, col, defaultArg rounding 0.f, defaultArg flags 0)
        member _.DrawRectFilledMultiColor(p1_x, p1_y, p2_x, p2_y, colUprLeft, colUprRight, colBotRight, colBotLeft) =
            ImGuiNative.IGN_DrawList_AddRectFilledMultiColor(p1_x, p1_y, p2_x, p2_y, colUprLeft, colUprRight, colBotRight, colBotLeft)
        member _.DrawCircle(centerX, centerY, radius, col, ?numSegments, ?thickness) =
            ImGuiNative.IGN_DrawList_AddCircle(centerX, centerY, radius, col, defaultArg numSegments 0, defaultArg thickness 1.f)
        member _.DrawCircleFilled(centerX, centerY, radius, col, ?numSegments) =
            ImGuiNative.IGN_DrawList_AddCircleFilled(centerX, centerY, radius, col, defaultArg numSegments 0)
        member _.DrawTriangleFilled(p1_x, p1_y, p2_x, p2_y, p3_x, p3_y, col) =
            ImGuiNative.IGN_DrawList_AddTriangleFilled(p1_x, p1_y, p2_x, p2_y, p3_x, p3_y, col)
        member _.DrawText(posX, posY, col, text) =
            ImGuiNative.IGN_DrawList_AddText(posX, posY, col, text)
        member _.DrawPolyline(pointsX, pointsY, col, ?flags, ?thickness) =
            let count = min pointsX.Length pointsY.Length
            ImGuiNative.IGN_DrawList_AddPolyline(pointsX, pointsY, count, col, defaultArg flags 0, defaultArg thickness 1.f)
        member _.DrawConvexPolyFilled(pointsX, pointsY, col) =
            let count = min pointsX.Length pointsY.Length
            ImGuiNative.IGN_DrawList_AddConvexPolyFilled(pointsX, pointsY, count, col)
        member _.DrawImage(textureId, p1_x, p1_y, p2_x, p2_y, ?uv1_x, ?uv1_y, ?uv2_x, ?uv2_y, ?col) =
            ImGuiNative.IGN_DrawList_AddImage(textureId, p1_x, p1_y, p2_x, p2_y, defaultArg uv1_x 0.f, defaultArg uv1_y 0.f, defaultArg uv2_x 1.f, defaultArg uv2_y 1.f, defaultArg col 0xFFFFFFFFu)
        member _.PushClipRect(minX, minY, maxX, maxY, ?intersectWithCurrent) =
            ImGuiNative.IGN_DrawList_PushClipRect(minX, minY, maxX, maxY, defaultArg intersectWithCurrent true)
        member _.PopClipRect() = ImGuiNative.IGN_DrawList_PopClipRect()

    interface IPlotFunctions with
        member _.BeginPlot(titleId, ?w, ?h, ?flags) =
            ImGuiNative.IGN_Plot_BeginPlot(titleId, defaultArg w -1f, defaultArg h -1f, defaultArg flags 0)
        member _.EndPlot() = ImGuiNative.IGN_Plot_EndPlot()
        member _.SetupAxes(xLabel, yLabel, ?xFlags, ?yFlags) =
            ImGuiNative.IGN_Plot_SetupAxes(xLabel, yLabel, defaultArg xFlags 0, defaultArg yFlags 0)
        member _.PlotLine(label, values: float32[], ?xscale, ?x0) =
            ImGuiNative.IGN_Plot_PlotLine_FloatPtrInt(label, values, values.Length, defaultArg xscale 1.0, defaultArg x0 0.0, 0, sizeof<float32>)
        member _.PlotBars(label, values: float32[], ?barSize, ?shift) =
            ImGuiNative.IGN_Plot_PlotBars_FloatPtrInt(label, values, values.Length, defaultArg barSize 0.67, defaultArg shift 0.0, 0, sizeof<float32>)
        member _.PlotScatter(label, xs: float32[], ys: float32[]) =
            ImGuiNative.IGN_Plot_PlotScatter_FloatPtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<float32>)
        member _.PlotHeatmap(label, values, rows, cols, ?scaleMin, ?scaleMax) =
            ImGuiNative.IGN_Plot_PlotHeatmap(label, values, rows, cols, defaultArg scaleMin 0.0, defaultArg scaleMax 1.0, "%.1f")
        member _.ShowDemoWindow(?pOpen) =
            BoolPtr.withOptRef pOpen (fun ptr -> ImGuiNative.IGN_Plot_ShowDemoWindow(ptr))

        // Legend & limits
        member _.SetupAxisLimits(axis, v_min, v_max, ?cond) =
            ImGuiNative.IGN_Plot_SetupAxisLimits(axis, v_min, v_max, defaultArg cond 0)
        member _.SetNextAxesLimits(x_min, x_max, y_min, y_max, ?cond) =
            ImGuiNative.IGN_Plot_SetNextAxesLimits(x_min, x_max, y_min, y_max, defaultArg cond 0)
        member _.SetupLegend(location, ?flags) =
            ImGuiNative.IGN_Plot_SetupLegend(location, defaultArg flags 0)
        member _.SetupAxisScale(axis, scale) =
            ImGuiNative.IGN_Plot_SetupAxisScale(axis, scale)
        member _.SetupAxisFormat(axis, fmt) =
            ImGuiNative.IGN_Plot_SetupAxisFormat(axis, fmt)

        // Double precision plotting
        member _.PlotLine(label: string, values: double[], ?xscale: double, ?x0: double) =
            ImGuiNative.IGN_Plot_PlotLine_DoublePtrInt(label, values, values.Length, defaultArg xscale 1.0, defaultArg x0 0.0, 0, sizeof<double>)
        member _.PlotLine(label: string, xs: double[], ys: double[]) =
            ImGuiNative.IGN_Plot_PlotLine_DoublePtrPtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<double>)
        member _.PlotLine(label: string, xs: DateTime[], ys: double[]) =
            let timestamps = xs |> Array.map (fun dt -> float (DateTimeOffset(dt).ToUnixTimeSeconds()))
            ImGuiNative.IGN_Plot_PlotLine_DoublePtrPtr(label, timestamps, ys, min timestamps.Length ys.Length, 0, sizeof<double>)
        member _.PlotBars(label: string, xs: double[], ys: double[], ?width: double) =
            ImGuiNative.IGN_Plot_PlotBars_DoublePtrPtr(label, xs, ys, min xs.Length ys.Length, defaultArg width 0.67, 0, sizeof<double>)
        member _.PlotScatter(label: string, xs: double[], ys: double[]) =
            ImGuiNative.IGN_Plot_PlotScatter_DoublePtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<double>)
        member _.PlotShaded(label: string, xs: double[], ys1: double[], ys2: double[]) =
            ImGuiNative.IGN_Plot_PlotShaded_DoublePtrPtr(label, xs, ys1, ys2, min xs.Length (min ys1.Length ys2.Length), 0, sizeof<double>)
        member _.PlotStairs(label: string, xs: double[], ys: double[]) =
            ImGuiNative.IGN_Plot_PlotStairs_DoublePtrPtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<double>)
        member _.PlotErrorBars(label: string, xs: double[], ys: double[], err: double[]) =
            ImGuiNative.IGN_Plot_PlotErrorBars_DoublePtr(label, xs, ys, err, min xs.Length (min ys.Length err.Length), 0, sizeof<double>)
        member _.PlotPieChart(labels: string[], values: double[], x: double, y: double, radius: double, ?labelFmt: string, ?angle0: double) =
            let count = min labels.Length values.Length
            let handles = Array.zeroCreate<GCHandle> count
            let ptrs = Array.zeroCreate<nativeint> count
            try
                for i in 0 .. count - 1 do
                    let bytes = System.Text.Encoding.UTF8.GetBytes(labels.[i] + "\0")
                    handles.[i] <- GCHandle.Alloc(bytes, GCHandleType.Pinned)
                    ptrs.[i] <- handles.[i].AddrOfPinnedObject()
                let ptrsHandle = GCHandle.Alloc(ptrs, GCHandleType.Pinned)
                try
                    ImGuiNative.IGN_Plot_PlotPieChart(ptrsHandle.AddrOfPinnedObject(), values, count, x, y, radius, defaultArg labelFmt "%p", defaultArg angle0 90.0)
                finally ptrsHandle.Free()
            finally
                for h in handles do if h.IsAllocated then h.Free()

        // Queries & coords
        member _.IsPlotHovered() = ImGuiNative.IGN_Plot_IsPlotHovered()
        member _.GetPlotMousePos(yAxis) =
            let mutable x, y = 0.0, 0.0
            ImGuiNative.IGN_Plot_GetPlotMousePos(&x, &y, yAxis)
            (x, y)
        member _.PlotToPixels(x, y, ?yAxis) =
            let mutable pixX, pixY = 0.f, 0.f
            ImGuiNative.IGN_Plot_PlotToPixels(x, y, &pixX, &pixY, defaultArg yAxis 0)
            (pixX, pixY)

        // Candlesticks
        member _.PlotCandles(label, xs: double[], opens: double[], highs: double[], lows: double[], closes: double[], ?width, ?bullColor, ?bearColor) =
            let count = min xs.Length (min opens.Length (min highs.Length (min lows.Length closes.Length)))
            ImGuiNative.IGN_Plot_PlotCandles(label, xs, opens, highs, lows, closes, count, defaultArg width 0.67, defaultArg bullColor 0xFF00FF00u, defaultArg bearColor 0xFF0000FFu, 0, sizeof<double>)
        member _.PlotCandles(label, xs: DateTime[], opens: double[], highs: double[], lows: double[], closes: double[], ?width, ?bullColor, ?bearColor) =
            let timestamps = xs |> Array.map (fun dt -> float (DateTimeOffset(dt).ToUnixTimeSeconds()))
            let count = min timestamps.Length (min opens.Length (min highs.Length (min lows.Length closes.Length)))
            ImGuiNative.IGN_Plot_PlotCandles(label, timestamps, opens, highs, lows, closes, count, defaultArg width 0.67, defaultArg bullColor 0xFF00FF00u, defaultArg bearColor 0xFF0000FFu, 0, sizeof<double>)

    interface IPlot3DFunctions with
        member _.BeginPlot(titleId, ?w, ?h, ?flags) =
            ImGuiNative.IGN_Plot3D_BeginPlot(titleId, defaultArg w -1f, defaultArg h -1f, defaultArg flags 0)
        member _.EndPlot() = ImGuiNative.IGN_Plot3D_EndPlot()
        member _.SetupAxes(xLabel, yLabel, zLabel, ?xFlags, ?yFlags, ?zFlags) =
            ImGuiNative.IGN_Plot3D_SetupAxes(xLabel, yLabel, zLabel, defaultArg xFlags 0, defaultArg yFlags 0, defaultArg zFlags 0)
        member _.PlotLine(label, xs: float32[], ys: float32[], zs: float32[]) =
            ImGuiNative.IGN_Plot3D_PlotLine(label, xs, ys, zs, min xs.Length (min ys.Length zs.Length), 0, sizeof<float32>)
        member _.PlotScatter(label, xs: float32[], ys: float32[], zs: float32[]) =
            ImGuiNative.IGN_Plot3D_PlotScatter(label, xs, ys, zs, min xs.Length (min ys.Length zs.Length), 0, sizeof<float32>)
        member _.PlotSurface(label, xs: float32[], ys: float32[], zs: float32[], xCount, yCount) =
            ImGuiNative.IGN_Plot3D_PlotSurface(label, xs, ys, zs, xCount, yCount, 0, xCount * sizeof<float32>)
        member _.ShowDemoWindow(?pOpen) =
            BoolPtr.withOptRef pOpen (fun ptr -> ImGuiNative.IGN_Plot3D_ShowDemoWindow(ptr))

        // Double precision 3D plotting
        member _.PlotLine(label: string, xs: double[], ys: double[], zs: double[]) =
            ImGuiNative.IGN_Plot3D_PlotLine_Double(label, xs, ys, zs, min xs.Length (min ys.Length zs.Length), 0, sizeof<double>)
        member _.PlotScatter(label: string, xs: double[], ys: double[], zs: double[]) =
            ImGuiNative.IGN_Plot3D_PlotScatter_Double(label, xs, ys, zs, min xs.Length (min ys.Length zs.Length), 0, sizeof<double>)
        member _.PlotSurface(label: string, xs: double[], ys: double[], zs: double[], xCount, yCount) =
            ImGuiNative.IGN_Plot3D_PlotSurface_Double(label, xs, ys, zs, xCount, yCount, 0, xCount * sizeof<double>)

    interface IFontFunctions with
        member _.Build()                     = ImGuiNative.IGN_Font_Build()
        member _.AddDefaultFont()            = ImGuiNative.IGN_Font_AddDefault()
        member _.AddFontFromFile(path, size) = ImGuiNative.IGN_Font_AddFromFile(path, size)
