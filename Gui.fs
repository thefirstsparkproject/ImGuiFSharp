namespace ImGuiFSharp

open System
open System.Runtime.InteropServices

[<AbstractClass; Sealed>]
type public Gui = class

        static member Begin(name, ?pOpen, ?flags) =
            BoolPtr.withOptRef pOpen (fun ptr ->
                ImGuiNative.IGN_Begin(name, ptr, defaultArg flags 0))
        static member End() = ImGuiNative.IGN_End()

        static member Button(label, ?w, ?h) =
            ImGuiNative.IGN_Button(label, defaultArg w 0f, defaultArg h 0f)
        static member Text(text) = ImGuiNative.IGN_Text(text)

        static member InputText(label, buf: char[], ?flags) =
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

        static member InputFloat(label, v, ?step, ?stepFast, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_InputFloat(label, &vv, defaultArg step 0f, defaultArg stepFast 0f, defaultArg fmt "%.3f", defaultArg flags 0)
            v := vv; r
        static member InputInt(label, v, ?step, ?stepFast, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_InputInt(label, &vv, defaultArg step 1, defaultArg stepFast 100, defaultArg flags 0)
            v := vv; r
        static member SliderFloat(label, v, min, max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_SliderFloat(label, &vv, min, max, defaultArg fmt "%.3f", defaultArg flags 0)
            v := vv; r
        static member SliderInt(label, v, min, max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_SliderInt(label, &vv, min, max, defaultArg fmt "%d", defaultArg flags 0)
            v := vv; r
        static member Checkbox(label, v) =
            BoolPtr.withRef v (fun ptr -> ImGuiNative.IGN_Checkbox(label, ptr))
        static member CollapsingHeader(label, ?flags) =
            ImGuiNative.IGN_CollapsingHeader(label, defaultArg flags 0)
        static member TreeNode(label) = ImGuiNative.IGN_TreeNode(label)
        static member TreePop()       = ImGuiNative.IGN_TreePop()
        static member Separator()     = ImGuiNative.IGN_Separator()
        static member SameLine(?offset, ?spacing) =
            ImGuiNative.IGN_SameLine(defaultArg offset 0f, defaultArg spacing -1f)
        static member NewLine()  = ImGuiNative.IGN_NewLine_()
        static member Spacing()  = ImGuiNative.IGN_Spacing()
        static member PushID(id) = ImGuiNative.IGN_PushID_Str(id)
        static member PopID()    = ImGuiNative.IGN_PopID()

        static member BeginCombo(label, preview, ?flags) =
            ImGuiNative.IGN_BeginCombo(label, preview, defaultArg flags 0)
        static member EndCombo() = ImGuiNative.IGN_EndCombo()
        static member Selectable(label, selected, ?flags, ?width, ?height) =
            ImGuiNative.IGN_Selectable(label, selected, defaultArg flags 0, defaultArg width 0f, defaultArg height 0f)

        static member BeginTable(id, cols, ?flags, ?ow, ?oh) =
            ImGuiNative.IGN_BeginTable(id, cols, defaultArg flags 0, defaultArg ow 0f, defaultArg oh 0f)
        static member EndTable() = ImGuiNative.IGN_EndTable()
        static member TableSetupColumn(label, ?flags, ?init) =
            ImGuiNative.IGN_TableSetupColumn(label, defaultArg flags 0, defaultArg init 0f)
        static member TableNextRow(?rowFlags, ?minH) =
            ImGuiNative.IGN_TableNextRow(defaultArg rowFlags 0, defaultArg minH 0f)
        static member TableNextColumn() = ImGuiNative.IGN_TableNextColumn()

        static member BeginMenuBar() = ImGuiNative.IGN_BeginMenuBar()
        static member EndMenuBar()   = ImGuiNative.IGN_EndMenuBar()
        static member BeginMenu(label, ?enabled) =
            ImGuiNative.IGN_BeginMenu(label, defaultArg enabled true)
        static member EndMenu()      = ImGuiNative.IGN_EndMenu()
        static member MenuItem(label, ?shortcut, ?selected, ?enabled) =
            ImGuiNative.IGN_MenuItem(label, defaultArg shortcut (Unchecked.defaultof<string>), defaultArg selected false, defaultArg enabled true)

        static member DragFloat(label, v, ?speed, ?min, ?max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_DragFloat(label, &vv, defaultArg speed 1f, defaultArg min 0f, defaultArg max 0f, defaultArg fmt "%.3f", defaultArg flags 0)
            v := vv; r
        static member DragInt(label, v, ?speed, ?min, ?max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_DragInt(label, &vv, defaultArg speed 1f, defaultArg min 0, defaultArg max 0, defaultArg fmt "%d", defaultArg flags 0)
            v := vv; r

        static member ColorEdit4(label, col, ?flags) =
            let h = GCHandle.Alloc(col, GCHandleType.Pinned)
            try ImGuiNative.IGN_ColorEdit4(label, h.AddrOfPinnedObject(), defaultArg flags 0)
            finally h.Free()

        static member RadioButton(label, active) = ImGuiNative.IGN_RadioButton(label, active)
        static member ProgressBar(fraction, ?w, ?h, ?overlay) =
            ImGuiNative.IGN_ProgressBar(fraction, defaultArg w -1f, defaultArg h 0f, defaultArg overlay (Unchecked.defaultof<string>))
        static member Image(texId, w, h) = ImGuiNative.IGN_Image(texId, w, h)
        static member ImageButton(id, texId, w, h) = ImGuiNative.IGN_ImageButton(id, texId, w, h)
        static member SetNextWindowPos(x, y, ?cond) = ImGuiNative.IGN_SetNextWindowPos(x, y, defaultArg cond 0)
        static member SetNextWindowSize(w, h, ?cond) = ImGuiNative.IGN_SetNextWindowSize(w, h, defaultArg cond 0)
        static member ShowDemoWindow(?pOpen) =
            BoolPtr.withOptRef pOpen (fun ptr -> ImGuiNative.IGN_ShowDemoWindow(ptr))
        static member DockSpace(id, w, h, ?flags) =
            ImGuiNative.IGN_DockSpace(id, w, h, defaultArg flags 0)

        // Double-precision widgets
        static member InputDouble(label, v, ?step, ?stepFast, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_InputDouble(label, &vv, defaultArg step 0.0, defaultArg stepFast 0.0, defaultArg fmt "%.6f", defaultArg flags 0)
            v := vv; r

        static member DragDouble(label, v, ?speed, ?min, ?max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_DragDouble(label, &vv, defaultArg speed 1.0f, defaultArg min 0.0, defaultArg max 0.0, defaultArg fmt "%.6f", defaultArg flags 0)
            v := vv; r

        static member SliderDouble(label, v, min, max, ?fmt, ?flags) =
            let mutable vv = !v
            let r = ImGuiNative.IGN_SliderDouble(label, &vv, min, max, defaultArg fmt "%.6f", defaultArg flags 0)
            v := vv; r

        // Text variants
        static member TextColored(r, g, b, a, text) = ImGuiNative.IGN_TextColored(r, g, b, a, text)
        static member TextDisabled(text)           = ImGuiNative.IGN_TextDisabled(text)
        static member TextWrapped(text)            = ImGuiNative.IGN_TextWrapped(text)
        static member InputTextMultiline(label, buf: char[], ?width, ?height, ?flags) =
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
        static member BeginChild(strId, ?width, ?height, ?border, ?flags) =
            ImGuiNative.IGN_BeginChild(strId, defaultArg width 0.f, defaultArg height 0.f, defaultArg border false, defaultArg flags 0)
        static member EndChild() = ImGuiNative.IGN_EndChild()
        static member BeginGroup() = ImGuiNative.IGN_BeginGroup()
        static member EndGroup() = ImGuiNative.IGN_EndGroup()
        static member Dummy(w, h) = ImGuiNative.IGN_Dummy(w, h)
        static member Indent(w) = ImGuiNative.IGN_Indent(w)
        static member Unindent(w) = ImGuiNative.IGN_Unindent(w)
        static member GetContentRegionAvail() =
            let mutable x, y = 0.f, 0.f
            ImGuiNative.IGN_GetContentRegionAvail(&x, &y)
            (x, y)
        static member GetWindowSize() =
            let mutable x, y = 0.f, 0.f
            ImGuiNative.IGN_GetWindowSize(&x, &y)
            (x, y)
        static member GetWindowPos() =
            let mutable x, y = 0.f, 0.f
            ImGuiNative.IGN_GetWindowPos(&x, &y)
            (x, y)
        static member SetNextWindowBgAlpha(alpha) = ImGuiNative.IGN_SetNextWindowBgAlpha(alpha)

        // Style
        static member PushStyleColor(idx, r, g, b, a) = ImGuiNative.IGN_PushStyleColor(idx, r, g, b, a)
        static member PopStyleColor(?count)           = ImGuiNative.IGN_PopStyleColor(defaultArg count 1)
        static member PushStyleVar(idx, valFloat: float32) = ImGuiNative.IGN_PushStyleVar_Float(idx, valFloat)
        static member PushStyleVar(idx, valVec2X: float32, valVec2Y: float32) = ImGuiNative.IGN_PushStyleVar_Vec2(idx, valVec2X, valVec2Y)
        static member PopStyleVar(?count)             = ImGuiNative.IGN_PopStyleVar(defaultArg count 1)

        // Queries
        static member IsItemHovered(?flags)           = ImGuiNative.IGN_IsItemHovered(defaultArg flags 0)
        static member IsItemActive()                  = ImGuiNative.IGN_IsItemActive()
        static member IsItemClicked(?mouseButton)     = ImGuiNative.IGN_IsItemClicked(defaultArg mouseButton 0)
        static member IsMouseClicked(button, ?repeat) = ImGuiNative.IGN_IsMouseClicked(button, defaultArg repeat false)
        static member IsMouseDown(button)             = ImGuiNative.IGN_IsMouseDown(button)
        static member IsMouseDoubleClicked(button)    = ImGuiNative.IGN_IsMouseDoubleClicked(button)
        static member GetMousePos() =
            let mutable x, y = 0.f, 0.f
            ImGuiNative.IGN_GetMousePos(&x, &y)
            (x, y)

        // Tooltips, popups, and modals
        static member BeginTooltip() = ImGuiNative.IGN_BeginTooltip()
        static member EndTooltip()   = ImGuiNative.IGN_EndTooltip()
        static member SetTooltip(text) = ImGuiNative.IGN_SetTooltip(text)
        static member BeginItemTooltip() = ImGuiNative.IGN_BeginItemTooltip()
        static member SetItemTooltip(text) = ImGuiNative.IGN_SetItemTooltip(text)
        static member OpenPopup(strId, ?flags) = ImGuiNative.IGN_OpenPopup(strId, defaultArg flags 0)
        static member BeginPopup(strId, ?flags) = ImGuiNative.IGN_BeginPopup(strId, defaultArg flags 0)
        static member BeginPopupModal(name, ?pOpen, ?flags) =
            BoolPtr.withOptRef pOpen (fun ptr -> ImGuiNative.IGN_BeginPopupModal(name, ptr, defaultArg flags 0))
        static member EndPopup() = ImGuiNative.IGN_EndPopup()
        static member CloseCurrentPopup() = ImGuiNative.IGN_CloseCurrentPopup()
        static member BeginPopupContextItem(?strId, ?flags) =
            ImGuiNative.IGN_BeginPopupContextItem(defaultArg strId (Unchecked.defaultof<string>), defaultArg flags 1)
        static member BeginPopupContextWindow(?strId, ?flags) =
            ImGuiNative.IGN_BeginPopupContextWindow(defaultArg strId (Unchecked.defaultof<string>), defaultArg flags 1)

        // Tab Bars & List Boxes
        static member BeginTabBar(strId, ?flags) = ImGuiNative.IGN_BeginTabBar(strId, defaultArg flags 0)
        static member EndTabBar() = ImGuiNative.IGN_EndTabBar()
        static member BeginTabItem(label, ?pOpen, ?flags) =
            BoolPtr.withOptRef pOpen (fun ptr -> ImGuiNative.IGN_BeginTabItem(label, ptr, defaultArg flags 0))
        static member EndTabItem() = ImGuiNative.IGN_EndTabItem()
        static member BeginListBox(label, ?w, ?h) = ImGuiNative.IGN_BeginListBox(label, defaultArg w 0.f, defaultArg h 0.f)
        static member EndListBox() = ImGuiNative.IGN_EndListBox()

        // Canvas drawing
        static member GetCursorScreenPos() =
            let mutable x, y = 0.f, 0.f
            ImGuiNative.IGN_GetCursorScreenPos(&x, &y)
            (x, y)
        static member SetCursorScreenPos(x, y) = ImGuiNative.IGN_SetCursorScreenPos(x, y)
        static member InvisibleButton(strId, w, h, ?flags) = ImGuiNative.IGN_InvisibleButton(strId, w, h, defaultArg flags 0)
        static member DrawLine(p1_x, p1_y, p2_x, p2_y, col, ?thickness) =
            ImGuiNative.IGN_DrawList_AddLine(p1_x, p1_y, p2_x, p2_y, col, defaultArg thickness 1.f)
        static member DrawRect(p1_x, p1_y, p2_x, p2_y, col, ?rounding, ?flags, ?thickness) =
            ImGuiNative.IGN_DrawList_AddRect(p1_x, p1_y, p2_x, p2_y, col, defaultArg rounding 0.f, defaultArg flags 0, defaultArg thickness 1.f)
        static member DrawRectFilled(p1_x, p1_y, p2_x, p2_y, col, ?rounding, ?flags) =
            ImGuiNative.IGN_DrawList_AddRectFilled(p1_x, p1_y, p2_x, p2_y, col, defaultArg rounding 0.f, defaultArg flags 0)
        static member DrawRectFilledMultiColor(p1_x, p1_y, p2_x, p2_y, colUprLeft, colUprRight, colBotRight, colBotLeft) =
            ImGuiNative.IGN_DrawList_AddRectFilledMultiColor(p1_x, p1_y, p2_x, p2_y, colUprLeft, colUprRight, colBotRight, colBotLeft)
        static member DrawCircle(centerX, centerY, radius, col, ?numSegments, ?thickness) =
            ImGuiNative.IGN_DrawList_AddCircle(centerX, centerY, radius, col, defaultArg numSegments 0, defaultArg thickness 1.f)
        static member DrawCircleFilled(centerX, centerY, radius, col, ?numSegments) =
            ImGuiNative.IGN_DrawList_AddCircleFilled(centerX, centerY, radius, col, defaultArg numSegments 0)
        static member DrawTriangleFilled(p1_x, p1_y, p2_x, p2_y, p3_x, p3_y, col) =
            ImGuiNative.IGN_DrawList_AddTriangleFilled(p1_x, p1_y, p2_x, p2_y, p3_x, p3_y, col)
        static member DrawText(posX, posY, col, text) =
            ImGuiNative.IGN_DrawList_AddText(posX, posY, col, text)
        static member DrawPolyline(pointsX: float32[], pointsY: float32[], col, ?flags, ?thickness) =
            let count = min pointsX.Length pointsY.Length
            ImGuiNative.IGN_DrawList_AddPolyline(pointsX, pointsY, count, col, defaultArg flags 0, defaultArg thickness 1.f)
        static member DrawConvexPolyFilled(pointsX: float32[], pointsY: float32[], col) =
            let count = min pointsX.Length pointsY.Length
            ImGuiNative.IGN_DrawList_AddConvexPolyFilled(pointsX, pointsY, count, col)
        static member DrawImage(textureId, p1_x, p1_y, p2_x, p2_y, ?uv1_x, ?uv1_y, ?uv2_x, ?uv2_y, ?col) =
            ImGuiNative.IGN_DrawList_AddImage(textureId, p1_x, p1_y, p2_x, p2_y, defaultArg uv1_x 0.f, defaultArg uv1_y 0.f, defaultArg uv2_x 1.f, defaultArg uv2_y 1.f, defaultArg col 0xFFFFFFFFu)
        static member PushClipRect(minX, minY, maxX, maxY, ?intersectWithCurrent) =
            ImGuiNative.IGN_DrawList_PushClipRect(minX, minY, maxX, maxY, defaultArg intersectWithCurrent true)
        static member PopClipRect() = ImGuiNative.IGN_DrawList_PopClipRect()

        static member BeginPlot(titleId, ?w, ?h, ?flags) =
            ImGuiNative.IGN_Plot_BeginPlot(titleId, defaultArg w -1f, defaultArg h -1f, defaultArg flags 0)
        static member EndPlot() = ImGuiNative.IGN_Plot_EndPlot()
        static member SetupAxes(xLabel, yLabel, ?xFlags, ?yFlags) =
            ImGuiNative.IGN_Plot_SetupAxes(xLabel, yLabel, defaultArg xFlags 0, defaultArg yFlags 0)
        static member PlotLine(label, values: float32[], ?xscale, ?x0) =
            ImGuiNative.IGN_Plot_PlotLine_FloatPtrInt(label, values, values.Length, defaultArg xscale 1.0, defaultArg x0 0.0, 0, sizeof<float32>)
        static member PlotBars(label, values: float32[], ?barSize, ?shift) =
            ImGuiNative.IGN_Plot_PlotBars_FloatPtrInt(label, values, values.Length, defaultArg barSize 0.67, defaultArg shift 0.0, 0, sizeof<float32>)
        static member PlotScatter(label, xs: float32[], ys: float32[]) =
            ImGuiNative.IGN_Plot_PlotScatter_FloatPtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<float32>)
        static member PlotHeatmap(label, values, rows, cols, ?scaleMin, ?scaleMax) =
            ImGuiNative.IGN_Plot_PlotHeatmap(label, values, rows, cols, defaultArg scaleMin 0.0, defaultArg scaleMax 1.0, "%.1f")
        static member ShowPlotDemoWindow(?pOpen) =
            BoolPtr.withOptRef pOpen (fun ptr -> ImGuiNative.IGN_Plot_ShowDemoWindow(ptr))

        // Legend & limits
        static member SetupAxisLimits(axis, v_min, v_max, ?cond) =
            ImGuiNative.IGN_Plot_SetupAxisLimits(axis, v_min, v_max, defaultArg cond 0)
        static member SetNextAxesLimits(x_min, x_max, y_min, y_max, ?cond) =
            ImGuiNative.IGN_Plot_SetNextAxesLimits(x_min, x_max, y_min, y_max, defaultArg cond 0)
        static member SetupLegend(location, ?flags) =
            ImGuiNative.IGN_Plot_SetupLegend(location, defaultArg flags 0)
        static member SetupAxisScale(axis, scale) =
            ImGuiNative.IGN_Plot_SetupAxisScale(axis, scale)
        static member SetupAxisFormat(axis, fmt) =
            ImGuiNative.IGN_Plot_SetupAxisFormat(axis, fmt)

        // Double precision plotting
        static member PlotLine(label: string, values: double[], ?xscale: double, ?x0: double) =
            ImGuiNative.IGN_Plot_PlotLine_DoublePtrInt(label, values, values.Length, defaultArg xscale 1.0, defaultArg x0 0.0, 0, sizeof<double>)
        static member PlotLine(label: string, xs: double[], ys: double[]) =
            ImGuiNative.IGN_Plot_PlotLine_DoublePtrPtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<double>)
        static member PlotLine(label: string, xs: DateTime[], ys: double[]) =
            let timestamps = xs |> Array.map (fun dt -> float (DateTimeOffset(dt).ToUnixTimeSeconds()))
            ImGuiNative.IGN_Plot_PlotLine_DoublePtrPtr(label, timestamps, ys, min timestamps.Length ys.Length, 0, sizeof<double>)
        static member PlotBars(label: string, xs: double[], ys: double[], ?width: double) =
            ImGuiNative.IGN_Plot_PlotBars_DoublePtrPtr(label, xs, ys, min xs.Length ys.Length, defaultArg width 0.67, 0, sizeof<double>)
        static member PlotScatter(label: string, xs: double[], ys: double[]) =
            ImGuiNative.IGN_Plot_PlotScatter_DoublePtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<double>)
        static member PlotShaded(label: string, xs: double[], ys1: double[], ys2: double[]) =
            ImGuiNative.IGN_Plot_PlotShaded_DoublePtrPtr(label, xs, ys1, ys2, min xs.Length (min ys1.Length ys2.Length), 0, sizeof<double>)
        static member PlotStairs(label: string, xs: double[], ys: double[]) =
            ImGuiNative.IGN_Plot_PlotStairs_DoublePtrPtr(label, xs, ys, min xs.Length ys.Length, 0, sizeof<double>)
        static member PlotErrorBars(label: string, xs: double[], ys: double[], err: double[]) =
            ImGuiNative.IGN_Plot_PlotErrorBars_DoublePtr(label, xs, ys, err, min xs.Length (min ys.Length err.Length), 0, sizeof<double>)
        static member PlotPieChart(labels: string[], values: double[], x: double, y: double, radius: double, ?labelFmt: string, ?angle0: double) =
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
        static member IsPlotHovered() = ImGuiNative.IGN_Plot_IsPlotHovered()
        static member GetPlotMousePos(yAxis) =
            let mutable x, y = 0.0, 0.0
            ImGuiNative.IGN_Plot_GetPlotMousePos(&x, &y, yAxis)
            (x, y)
        static member PlotToPixels(x, y, ?yAxis) =
            let mutable pixX, pixY = 0.f, 0.f
            ImGuiNative.IGN_Plot_PlotToPixels(x, y, &pixX, &pixY, defaultArg yAxis 0)
            (pixX, pixY)

        // Candlesticks
        static member PlotCandles(label, xs: double[], opens: double[], highs: double[], lows: double[], closes: double[], ?width, ?bullColor, ?bearColor) =
            let count = min xs.Length (min opens.Length (min highs.Length (min lows.Length closes.Length)))
            ImGuiNative.IGN_Plot_PlotCandles(label, xs, opens, highs, lows, closes, count, defaultArg width 0.67, defaultArg bullColor 0xFF00FF00u, defaultArg bearColor 0xFF0000FFu, 0, sizeof<double>)
        static member PlotCandles(label, xs: DateTime[], opens: double[], highs: double[], lows: double[], closes: double[], ?width, ?bullColor, ?bearColor) =
            let timestamps = xs |> Array.map (fun dt -> float (DateTimeOffset(dt).ToUnixTimeSeconds()))
            let count = min timestamps.Length (min opens.Length (min highs.Length (min lows.Length closes.Length)))
            ImGuiNative.IGN_Plot_PlotCandles(label, timestamps, opens, highs, lows, closes, count, defaultArg width 0.67, defaultArg bullColor 0xFF00FF00u, defaultArg bearColor 0xFF0000FFu, 0, sizeof<double>)

        static member BeginPlot3D(titleId, ?w, ?h, ?flags) =
            ImGuiNative.IGN_Plot3D_BeginPlot(titleId, defaultArg w -1f, defaultArg h -1f, defaultArg flags 0)
        static member EndPlot3D() = ImGuiNative.IGN_Plot3D_EndPlot()
        static member SetupAxes3D(xLabel, yLabel, zLabel, ?xFlags, ?yFlags, ?zFlags) =
            ImGuiNative.IGN_Plot3D_SetupAxes(xLabel, yLabel, zLabel, defaultArg xFlags 0, defaultArg yFlags 0, defaultArg zFlags 0)
        static member PlotLine3D(label, xs: float32[], ys: float32[], zs: float32[]) =
            ImGuiNative.IGN_Plot3D_PlotLine(label, xs, ys, zs, min xs.Length (min ys.Length zs.Length), 0, sizeof<float32>)
        static member PlotScatter3D(label, xs: float32[], ys: float32[], zs: float32[]) =
            ImGuiNative.IGN_Plot3D_PlotScatter(label, xs, ys, zs, min xs.Length (min ys.Length zs.Length), 0, sizeof<float32>)
        static member PlotSurface3D(label, xs: float32[], ys: float32[], zs: float32[], xCount, yCount) =
            ImGuiNative.IGN_Plot3D_PlotSurface(label, xs, ys, zs, xCount, yCount, 0, xCount * sizeof<float32>)
        static member ShowPlot3DDemoWindow(?pOpen) =
            BoolPtr.withOptRef pOpen (fun ptr -> ImGuiNative.IGN_Plot3D_ShowDemoWindow(ptr))

        // Double precision 3D plotting
        static member PlotLine3D(label: string, xs: double[], ys: double[], zs: double[]) =
            ImGuiNative.IGN_Plot3D_PlotLine_Double(label, xs, ys, zs, min xs.Length (min ys.Length zs.Length), 0, sizeof<double>)
        static member PlotScatter3D(label: string, xs: double[], ys: double[], zs: double[]) =
            ImGuiNative.IGN_Plot3D_PlotScatter_Double(label, xs, ys, zs, min xs.Length (min ys.Length zs.Length), 0, sizeof<double>)
        static member PlotSurface3D(label: string, xs: double[], ys: double[], zs: double[], xCount, yCount) =
            ImGuiNative.IGN_Plot3D_PlotSurface_Double(label, xs, ys, zs, xCount, yCount, 0, xCount * sizeof<double>)

        static member Build()                     = ImGuiNative.IGN_Font_Build()
        static member AddDefaultFont()            = ImGuiNative.IGN_Font_AddDefault()
        static member AddFontFromFile(path, size) = ImGuiNative.IGN_Font_AddFromFile(path, size)
end