namespace ImGuiFSharp

open System
open System.Collections.Generic
open ImGuiFSharp.Flags
open ImGuiFSharp.Enums

// ══════════════════════════════════════════════════════════════════════════════
// Config Records (kept for genuinely multi-field configurations)
// ══════════════════════════════════════════════════════════════════════════════

type WindowConfig =
    { Width     : int
      Height    : int
      Resizable : bool
      VSync     : bool
      ClearR    : float32
      ClearG    : float32
      ClearB    : float32 }

    static member Default =
        { Width     = 1280
          Height    = 720
          Resizable = true
          VSync     = true
          ClearR    = 0.15f
          ClearG    = 0.16f
          ClearB    = 0.18f }

type ChildConfig =
    { Width  : float32
      Height : float32
      Border : bool
      Flags  : Child }

    static member Default =
        { Width  = 0.f
          Height = 0.f
          Border = false
          Flags  = Child.None }

type TableConfig =
    { Columns     : int
      Flags       : Table
      OuterWidth  : float32
      OuterHeight : float32 }
    static member Default =
        { Columns     = 1
          Flags       = Table.None
          OuterWidth  = 0.f
          OuterHeight = 0.f }

type EditorConfig =
    { SettingsFile : string option
      Width        : float32
      Height       : float32 }
    static member Default =
        { SettingsFile = None
          Width        = 0.f
          Height       = 0.f }

type PlotConfig =
    { Width   : float32
      Height  : float32
      Flags   : Plot
      XLabel  : string
      YLabel  : string
      XFlags  : Flags.PlotAxis
      YFlags  : Flags.PlotAxis }
    static member Default =
        { Width   = -1.f
          Height  = -1.f
          Flags   = Plot.None
          XLabel  = ""
          YLabel  = ""
          XFlags  = Flags.PlotAxis.None
          YFlags  = Flags.PlotAxis.None }

type Plot3DConfig =
    { Width   : float32
      Height  : float32
      Flags   : Plot3D
      XLabel  : string
      YLabel  : string
      ZLabel  : string
      XFlags  : Plot3DAxis
      YFlags  : Plot3DAxis
      ZFlags  : Plot3DAxis }
    static member Default =
        { Width   = -1.f
          Height  = -1.f
          Flags   = Plot3D.None
          XLabel  = ""
          YLabel  = ""
          ZLabel  = ""
          XFlags  = Plot3DAxis.None
          YFlags  = Plot3DAxis.None
          ZFlags  = Plot3DAxis.None }

type CreateConfig =
    { R         : float32
      G         : float32
      B         : float32
      A         : float32
      Thickness : float32 }
    static member Default =
        { R         = 1.0f
          G         = 1.0f
          B         = 1.0f
          A         = 1.0f
          Thickness = 1.0f }

// ══════════════════════════════════════════════════════════════════════════════
// Lazy Node Editor Context Cache
// ══════════════════════════════════════════════════════════════════════════════

module internal EditorContextCache =
    let private cache = Dictionary<string, nativeint>()

    let getOrCreate (id: string) (settingsFile: string option) =
        match cache.TryGetValue(id) with
        | true, ctx -> ctx
        | false, _ ->
            let ctx = NodeEditor.CreateEditor(defaultArg settingsFile "")
            cache.Add(id, ctx)
            ctx

    let destroyAll () =
        for kvp in cache do NodeEditor.DestroyEditor(kvp.Value)
        cache.Clear()

// ══════════════════════════════════════════════════════════════════════════════
// Computation Expression Builders
// ══════════════════════════════════════════════════════════════════════════════

type ScopeBuilder(beginAction: unit -> bool, endAction: unit -> unit, alwaysEnd: bool) =
    member _.Zero() = fun () -> ()
    member _.Yield(expr: unit) = fun () -> expr
    member _.Delay(f: unit -> unit -> unit) = fun () -> f() ()
    member _.Combine(a: unit -> unit, b: unit -> unit) = fun () -> a(); b()
    member _.Run(f: unit -> unit) =
        let opened = beginAction()
        if opened then
            try f()
            finally if not alwaysEnd then endAction()
        if alwaysEnd then endAction()
    member _.For(seq: seq<'a>, body: 'a -> unit -> unit) =
        fun () -> for x in seq do body x ()
    member _.While(guard: unit -> bool, body: unit -> unit) =
        fun () -> while guard() do body()
    member _.TryWith(body: unit -> unit, handler: exn -> unit -> unit) =
        fun () -> try body() with ex -> handler ex ()
    member _.TryFinally(body: unit -> unit, compensation: unit -> unit) =
        fun () -> try body() finally compensation()

type WindowLoopBuilder(title: string, cfg: WindowConfig) =
    member _.Zero() = fun () -> ()
    member _.Yield(expr: unit) = fun () -> expr
    member _.Delay(f: unit -> unit -> unit) = fun () -> f() ()
    member _.Combine(a: unit -> unit, b: unit -> unit) = fun () -> a(); b()
    member _.Run(f: unit -> unit) =
        use win = new ImGuiFSharp.Window(cfg.Width, cfg.Height, title, cfg.Resizable)
        win.VSync <- cfg.VSync
        win.RegisterDraw(fun _ -> f())
        win.Run(cfg.ClearR, cfg.ClearG, cfg.ClearB)
        EditorContextCache.destroyAll()

// ══════════════════════════════════════════════════════════════════════════════
// Scope smart constructors
// ══════════════════════════════════════════════════════════════════════════════

module private Scope =
    /// Push/pop style: begin always succeeds, end always runs.
    let always beginA endA = ScopeBuilder((fun () -> beginA(); true), endA, true)
    /// ImGui Begin*/End* pair: end runs only when Begin returned true.
    let cond beginA endA   = ScopeBuilder(beginA, endA, false)
    /// Conditional content with no matching End (e.g., CollapsingHeader).
    let openOnly beginA    = ScopeBuilder(beginA, (fun () -> ()), false)

// ══════════════════════════════════════════════════════════════════════════════
// Top-level DSL Type
// ══════════════════════════════════════════════════════════════════════════════

type Builder =

    // ── Application Entry ───────────────────────────────────────────────────
    static member window (title: string, ?cfg: WindowConfig) =
        WindowLoopBuilder(title, defaultArg cfg WindowConfig.Default)

    // ── Windows & Scopes ────────────────────────────────────────────────────
    static member imWindow (name: string, ?pOpen: bool ref, ?flags: Window) =
        Scope.always
            (fun () -> Gui.Begin(name, ?pOpen = pOpen, flags = defaultArg flags Window.None) |> ignore)
            Gui.End

    static member child (id: string, ?cfg: ChildConfig) =
        let c = defaultArg cfg ChildConfig.Default
        Scope.always (fun () -> Gui.BeginChild(id, c.Width, c.Height, c.Border, c.Flags) |> ignore) Gui.EndChild

    static member group    = Scope.always Gui.BeginGroup Gui.EndGroup
    static member idScope id = Scope.always (fun () -> Gui.PushID id) Gui.PopID

    static member styleColor (idx: Col) (r, g, b, a) =
        Scope.always (fun () -> Gui.PushStyleColor(idx, r, g, b, a)) Gui.PopStyleColor
    static member styleVarFloat (idx: StyleVar) (value: float32) =
        Scope.always (fun () -> Gui.PushStyleVar(idx, value)) Gui.PopStyleVar
    static member styleVarVec (idx: StyleVar) (x, y) =
        Scope.always (fun () -> Gui.PushStyleVar(idx, x, y)) Gui.PopStyleVar

    static member collapsingHeader (label: string, ?flags: TreeNode) =
        Scope.openOnly (fun () -> Gui.CollapsingHeader(label, defaultArg flags TreeNode.None))

    static member treeNode label = Scope.cond (fun () -> Gui.TreeNode label) Gui.TreePop

    static member tabBar (id: string, ?flags: TabBar) =
        Scope.cond (fun () -> Gui.BeginTabBar(id, defaultArg flags TabBar.None)) Gui.EndTabBar

    static member tabItem (label: string, ?pOpen: bool ref, ?flags: TabItem) =
        Scope.cond
            (fun () -> Gui.BeginTabItem(label, ?pOpen = pOpen, flags = defaultArg flags TabItem.None))
            Gui.EndTabItem

    static member menuBar = Scope.cond Gui.BeginMenuBar Gui.EndMenuBar

    static member menu (label: string, ?enabled: bool) =
        Scope.cond (fun () -> Gui.BeginMenu(label, defaultArg enabled true)) Gui.EndMenu

    static member popup (id: string, ?flags: Popup) =
        Scope.cond (fun () -> Gui.BeginPopup(id, defaultArg flags Popup.None)) Gui.EndPopup

    static member popupModal (name: string, ?pOpen: bool ref, ?flags: Window) =
        Scope.cond
            (fun () -> Gui.BeginPopupModal(name, ?pOpen = pOpen, flags = defaultArg flags Window.None))
            Gui.EndPopup

    static member tooltip     = Scope.always Gui.BeginTooltip Gui.EndTooltip
    static member itemTooltip = Scope.cond Gui.BeginItemTooltip Gui.EndTooltip

    static member combo (label: string, preview: string, ?flags: Combo) =
        Scope.cond (fun () -> Gui.BeginCombo(label, preview, defaultArg flags Combo.None)) Gui.EndCombo

    static member listBox (label: string, ?width: float32, ?height: float32) =
        Scope.cond
            (fun () -> Gui.BeginListBox(label, defaultArg width 0.f, defaultArg height 0.f))
            Gui.EndListBox

    static member table (id: string, ?cfg: TableConfig) =
        let c = defaultArg cfg TableConfig.Default
        Scope.cond (fun () -> Gui.BeginTable(id, c.Columns, c.Flags, c.OuterWidth, c.OuterHeight)) Gui.EndTable

    static member clipRect (minX, minY, maxX, maxY, ?intersect) =
        Scope.always (fun () -> Gui.PushClipRect(minX, minY, maxX, maxY, defaultArg intersect true)) Gui.PopClipRect

    // ── Basic Widgets ────────────────────────────────────────────────────────
    static member button (label, ?w, ?h)               = Gui.Button(label, ?w = w, ?h = h)
    static member invisibleButton (id, w, h, ?flags: Button)   = Gui.InvisibleButton(id, w, h, ?flags = flags)
    static member checkbox (label, value)              = Gui.Checkbox(label, value)
    static member radioButton (label, active)          = Gui.RadioButton(label, active)
    static member progressBar (fraction, ?w, ?h, ?overlay) = Gui.ProgressBar(fraction, ?w = w, ?h = h, ?overlay = overlay)
    static member image (texId, w, h)                  = Gui.Image(texId, w, h)
    static member imageButton (id, texId, w, h)        = Gui.ImageButton(id, texId, w, h)
    static member selectable (label, isSelected, ?flags: Selectable, ?width, ?height) =
        Gui.Selectable(label, isSelected, ?flags = flags, ?width = width, ?height = height)

    // ── Text & Layout ────────────────────────────────────────────────────────
    static member text msg = Gui.Text(msg)
    static member textColored (r, g, b, a, msg) = Gui.TextColored(r, g, b, a, msg)
    static member textDisabled msg = Gui.TextDisabled(msg)
    static member textWrapped msg = Gui.TextWrapped(msg)

    static member separator ()                  = Gui.Separator()
    static member sameLine (?offset, ?spacing)  = Gui.SameLine(?offset = offset, ?spacing = spacing)
    static member newLine ()                    = Gui.NewLine()
    static member spacing ()                    = Gui.Spacing()
    static member dummy (w, h)                  = Gui.Dummy(w, h)
    static member indent w = Gui.Indent(w)
    static member unindent w = Gui.Unindent(w)

    // ── Text Inputs ──────────────────────────────────────────────────────────
    static member inputText (label, value: string ref, ?flags: InputText) =
        Gui.InputText(label, value, ?flags = flags)
    static member inputTextMultiline (label, value: string ref, ?width, ?height, ?flags: InputText) =
        Gui.InputTextMultiline(label, value, ?width = width, ?height = height, ?flags = flags)

    // ── Numerical Inputs ─────────────────────────────────────────────────────
    static member inputInt (label, v: int ref, ?step, ?stepFast, ?flags: InputText) =
        Gui.InputInt(label, v, ?step = step, ?stepFast = stepFast, ?flags = flags)
    static member inputFloat (label, v: float32 ref, ?step, ?stepFast, ?fmt, ?flags: InputText) =
        Gui.InputFloat(label, v, ?step = step, ?stepFast = stepFast, ?fmt = fmt, ?flags = flags)
    static member inputDouble (label, v: double ref, ?step, ?stepFast, ?fmt, ?flags: InputText) =
        Gui.InputDouble(label, v, ?step = step, ?stepFast = stepFast, ?fmt = fmt, ?flags = flags)

    // ── Drag Widgets ─────────────────────────────────────────────────────────
    static member dragInt (label, v: int ref, ?speed, ?min, ?max, ?fmt, ?flags: Slider) =
        Gui.DragInt(label, v, ?speed = speed, ?min = min, ?max = max, ?fmt = fmt, ?flags = flags)
    static member dragFloat (label, v: float32 ref, ?speed, ?min, ?max, ?fmt, ?flags: Slider) =
        Gui.DragFloat(label, v, ?speed = speed, ?min = min, ?max = max, ?fmt = fmt, ?flags = flags)
    static member dragDouble (label, v: double ref, ?speed, ?min, ?max, ?fmt, ?flags: Slider) =
        Gui.DragDouble(label, v, ?speed = speed, ?min = min, ?max = max, ?fmt = fmt, ?flags = flags)

    // ── Sliders ──────────────────────────────────────────────────────────────
    static member sliderInt (label, v: int ref, min, max, ?fmt, ?flags: Slider) =
        Gui.SliderInt(label, v, min, max, ?fmt = fmt, ?flags = flags)
    static member sliderFloat (label, v: float32 ref, min, max, ?fmt, ?flags: Slider) =
        Gui.SliderFloat(label, v, min, max, ?fmt = fmt, ?flags = flags)
    static member sliderDouble (label, v: double ref, min, max, ?fmt, ?flags: Slider) =
        Gui.SliderDouble(label, v, min, max, ?fmt = fmt, ?flags = flags)

    // ── Color ────────────────────────────────────────────────────────────────
    static member colorEdit (label, col, ?flags: ColorEdit) = Gui.ColorEdit4(label, col, ?flags = flags)

    // ── Table Cells ──────────────────────────────────────────────────────────
    static member tableColumn (label, ?flags: TableColumn, ?initWidth) =
        Gui.TableSetupColumn(label, ?flags = flags, ?init = initWidth)
    static member tableRow (?flags: TableRow, ?minHeight) =
        Gui.TableNextRow(?rowFlags = flags, ?minH = minHeight)
    static member tableCell () = Gui.TableNextColumn() |> ignore

    // ── Popups & Menu Items ──────────────────────────────────────────────────
    static member openPopup (id, ?flags: Popup)   = Gui.OpenPopup(id, ?flags = flags)
    static member closePopup ()            = Gui.CloseCurrentPopup()
    static member menuItem (label, ?shortcut, ?selected, ?enabled) =
        Gui.MenuItem(label, ?shortcut = shortcut, ?selected = selected, ?enabled = enabled)

    // ── Layout Helpers ───────────────────────────────────────────────────────
    static member separatorText label             = Gui.SeparatorText(label)
    static member setNextItemWidth w              = Gui.SetNextItemWidth(w)
    static member setItemDefaultFocus ()          = Gui.SetItemDefaultFocus()
    static member calcTextSize text               = Gui.CalcTextSize(text)
    static member getFrameHeight ()               = Gui.GetFrameHeight()
    static member getTextLineHeight ()            = Gui.GetTextLineHeight()
    static member getTextLineHeightWithSpacing () = Gui.GetTextLineHeightWithSpacing()
    static member getFrameHeightWithSpacing ()    = Gui.GetFrameHeightWithSpacing()

    // ── Scroll ────────────────────────────────────────────────────────────────
    static member getScrollY ()          = Gui.GetScrollY()
    static member getScrollMaxY ()       = Gui.GetScrollMaxY()
    static member setScrollY y           = Gui.SetScrollY(y)
    static member setScrollHereY ?ratio  = Gui.SetScrollHereY(?ratio = ratio)

    // ── Window State ──────────────────────────────────────────────────────────
    static member isWindowFocused (?flags: Hovered) = Gui.IsWindowFocused(?flags = flags)
    static member isWindowHovered (?flags: Hovered) = Gui.IsWindowHovered(?flags = flags)

    // ── Table Extras ──────────────────────────────────────────────────────────
    static member tableSetupScrollFreeze (cols, rows) = Gui.TableSetupScrollFreeze(cols, rows)
    static member tableSetBgColor (target: TableBgTarget, color: uint32, ?col) =
        Gui.TableSetBgColor(target, color, ?columnN = col)

    // ── Item Flags ────────────────────────────────────────────────────────────
    static member itemFlag (option: ItemFlags) (enabled: bool) =
        Scope.always (fun () -> Gui.PushItemFlag(option, enabled)) Gui.PopItemFlag

    // ── Item State Queries ────────────────────────────────────────────────────
    static member isVisible ()                    = Gui.IsItemVisible()
    static member isEdited ()                     = Gui.IsItemEdited()
    static member isDeactivatedAfterEdit ()       = Gui.IsItemDeactivatedAfterEdit()

    // ── Queries & State ──────────────────────────────────────────────────────
    static member isHovered (?flags: Hovered)              = Gui.IsItemHovered(?flags = flags)
    static member isActive ()                     = Gui.IsItemActive()
    static member isClicked (?button: MouseButton)             = Gui.IsItemClicked(?mouseButton = button)
    static member isMouseClicked (button: MouseButton, ?repeat) = Gui.IsMouseClicked(button, ?repeat = repeat)
    static member isMouseDown (button: MouseButton)            = Gui.IsMouseDown(button)
    static member isMouseDoubleClicked (button: MouseButton)   = Gui.IsMouseDoubleClicked(button)
    static member getMousePos ()                  = Gui.GetMousePos()

    // ── List Clipper ──────────────────────────────────────────────────────────
    /// Efficiently render only the visible rows of a large uniform-height list.
    ///
    ///   do! Builder.clipper(rowCount, fun start finish ->
    ///         for i in start .. finish - 1 do
    ///             Builder.tableRow()
    ///             Builder.text $"Row {i}")
    static member clipper (itemCount: int, body: int -> int -> unit, ?itemHeight: float32) =
        fun () ->
            use c = new ListClipper(itemCount, ?itemHeight = itemHeight)
            while c.Step() do
                body c.DisplayStart c.DisplayEnd

    // ── Canvas & Custom Drawing ──────────────────────────────────────────────
    static member getCursorPos ()               = Gui.GetCursorScreenPos()
    static member setCursorPos (x, y)           = Gui.SetCursorScreenPos(x, y)

    static member drawLine (x1, y1, x2, y2, col, ?thickness) =
        Gui.DrawLine(x1, y1, x2, y2, col, ?thickness = thickness)
    static member drawRect (x1, y1, x2, y2, col, ?rounding, ?flags: Draw, ?thickness) =
        Gui.DrawRect(x1, y1, x2, y2, col, ?rounding = rounding, ?flags = flags, ?thickness = thickness)
    static member drawRectFilled (x1, y1, x2, y2, col, ?rounding, ?flags: Draw) =
        Gui.DrawRectFilled(x1, y1, x2, y2, col, ?rounding = rounding, ?flags = flags)
    static member drawRectFilledMultiColor (x1, y1, x2, y2, colUL, colUR, colBR, colBL) =
        Gui.DrawRectFilledMultiColor(x1, y1, x2, y2, colUL, colUR, colBR, colBL)
    static member drawCircle (cx, cy, radius, col, ?segments, ?thickness) =
        Gui.DrawCircle(cx, cy, radius, col, ?numSegments = segments, ?thickness = thickness)
    static member drawCircleFilled (cx, cy, radius, col, ?segments) =
        Gui.DrawCircleFilled(cx, cy, radius, col, ?numSegments = segments)
    static member drawTriangleFilled (x1, y1, x2, y2, x3, y3, col) =
        Gui.DrawTriangleFilled(x1, y1, x2, y2, x3, y3, col)
    static member drawText (px, py, col, text) =
        Gui.DrawText(px, py, col, text)
    static member drawPolyline (xs, ys, col, ?flags: Draw, ?thickness) =
        Gui.DrawPolyline(xs, ys, col, ?flags = flags, ?thickness = thickness)
    static member drawConvexPolyFilled (xs, ys, col) =
        Gui.DrawConvexPolyFilled(xs, ys, col)
    static member drawImage (texId, x1, y1, x2, y2, ?uv1x, ?uv1y, ?uv2x, ?uv2y, ?col) =
        Gui.DrawImage(texId, x1, y1, x2, y2, ?uv1_x = uv1x, ?uv1_y = uv1y, ?uv2_x = uv2x, ?uv2_y = uv2y, ?col = col)

    static member pushClipRect (minX, minY, maxX, maxY, ?intersect) =
        Gui.PushClipRect(minX, minY, maxX, maxY, ?intersectWithCurrent = intersect)
    static member popClipRect () = Gui.PopClipRect()

    // ── ImPlot 2D ────────────────────────────────────────────────────────────
    static member plot (title: string, ?cfg: PlotConfig) =
        let c = defaultArg cfg PlotConfig.Default
        Scope.cond
            (fun () ->
                if Gui.BeginPlot(title, c.Width, c.Height, c.Flags) then
                    Gui.SetupAxes(c.XLabel, c.YLabel, c.XFlags, c.YFlags)
                    true
                else false)
            Gui.EndPlot

    static member setupAxes (xLabel, yLabel, ?xFlags: Flags.PlotAxis, ?yFlags: Flags.PlotAxis) =
        Gui.SetupAxes(xLabel, yLabel, ?xFlags = xFlags, ?yFlags = yFlags)
    static member setupLegend (location: PlotLocation, ?flags: PlotLegend) =
        Gui.SetupLegend(location, ?flags = flags)
    static member setupAxisLimits (axis: PlotAxis, vMin, vMax, ?cond: Cond) =
        Gui.SetupAxisLimits(axis, vMin, vMax, ?cond = cond)
    static member setupAxisScale (axis: PlotAxis, scale: PlotScale)  = Gui.SetupAxisScale(axis, scale)
    static member setupAxisFormat (axis: PlotAxis, fmt)   = Gui.SetupAxisFormat(axis, fmt)
    static member setNextAxesLimits (x0, x1, y0, y1, ?cond: Cond) =
        Gui.SetNextAxesLimits(x0, x1, y0, y1, ?cond = cond)

    // PlotLine — overloaded on data shape / element type
    static member plotLine (label, values: float32[], ?xScale, ?x0) =
        Gui.PlotLine(label, values, ?xScale = xScale, ?x0 = x0)
    static member plotLine (label: string, values: double[], ?xScale: double, ?x0: double) =
        Gui.PlotLine(label, values, ?xScale = xScale, ?x0 = x0)
    static member plotLine (label: string, xs: double[], ys: double[]) =
        Gui.PlotLine(label, xs, ys)
    static member plotLine (label: string, xs: DateTime[], ys: double[]) =
        Gui.PlotLine(label, xs, ys)

    // PlotBars
    static member plotBars (label, values: float32[], ?barSize, ?shift) =
        Gui.PlotBars(label, values, ?barSize = barSize, ?shift = shift)
    static member plotBars (label: string, xs: double[], ys: double[], ?width) =
        Gui.PlotBars(label, xs, ys, ?width = width)

    // PlotScatter
    static member plotScatter (label, xs: float32[], ys: float32[]) = Gui.PlotScatter(label, xs, ys)
    static member plotScatter (label: string, xs: double[], ys: double[]) = Gui.PlotScatter(label, xs, ys)

    // PlotShaded
    static member plotShaded (label, xs: float32[], ys1: float32[], ys2: float32[]) =
        Gui.PlotShaded(label, xs, ys1, ys2)
    static member plotShaded (label: string, xs: double[], ys1: double[], ys2: double[]) =
        Gui.PlotShaded(label, xs, ys1, ys2)

    // PlotStairs
    static member plotStairs (label, xs: float32[], ys: float32[]) = Gui.PlotStairs(label, xs, ys)
    static member plotStairs (label: string, xs: double[], ys: double[]) = Gui.PlotStairs(label, xs, ys)

    // PlotErrorBars
    static member plotErrorBars (label, xs: float32[], ys: float32[], err: float32[]) =
        Gui.PlotErrorBars(label, xs, ys, err)
    static member plotErrorBars (label: string, xs: double[], ys: double[], err: double[]) =
        Gui.PlotErrorBars(label, xs, ys, err)

    // PlotPieChart
    static member plotPieChart (labels: string[], values: float32[], x, y, r, ?fmt, ?angle0) =
        Gui.PlotPieChart(labels, values, x, y, r, ?labelFmt = fmt, ?angle0 = angle0)
    static member plotPieChart (labels: string[], values: double[], x, y, r, ?fmt, ?angle0) =
        Gui.PlotPieChart(labels, values, x, y, r, ?labelFmt = fmt, ?angle0 = angle0)

    // PlotHistogram
    static member plotHistogram (label, values: float32[], ?bins, ?scale, ?rMin, ?rMax) =
        Gui.PlotHistogram(label, values, ?bins = bins, ?barScale = scale, ?rangeMin = rMin, ?rangeMax = rMax)
    static member plotHistogram (label: string, values: double[], ?bins, ?scale, ?rMin, ?rMax) =
        Gui.PlotHistogram(label, values, ?bins = bins, ?barScale = scale, ?rangeMin = rMin, ?rangeMax = rMax)

    // PlotHistogram2D
    static member plotHistogram2D (label, xs: float32[], ys: float32[], ?xBins, ?yBins, ?xMin, ?xMax, ?yMin, ?yMax) =
        Gui.PlotHistogram2D(label, xs, ys, ?xBins = xBins, ?yBins = yBins, ?xMin = xMin, ?xMax = xMax, ?yMin = yMin, ?yMax = yMax)
    static member plotHistogram2D (label: string, xs: double[], ys: double[], ?xBins, ?yBins, ?xMin, ?xMax, ?yMin, ?yMax) =
        Gui.PlotHistogram2D(label, xs, ys, ?xBins = xBins, ?yBins = yBins, ?xMin = xMin, ?xMax = xMax, ?yMin = yMin, ?yMax = yMax)

    // PlotDigital
    static member plotDigital (label, xs: float32[], ys: float32[]) = Gui.PlotDigital(label, xs, ys)
    static member plotDigital (label: string, xs: double[], ys: double[]) = Gui.PlotDigital(label, xs, ys)

    // PlotStems
    static member plotStems (label, xs: float32[], ys: float32[], ?ref_) =
        Gui.PlotStems(label, xs, ys, ?ref_ = ref_)
    static member plotStems (label: string, xs: double[], ys: double[], ?ref_) =
        Gui.PlotStems(label, xs, ys, ?ref_ = ref_)

    // PlotInfLines
    static member plotInfLines (label, values: float32[]) = Gui.PlotInfLines(label, values)
    static member plotInfLines (label: string, values: double[]) = Gui.PlotInfLines(label, values)

    // PlotBubbles
    static member plotBubbles (label, xs: float32[], ys: float32[], szs: float32[]) =
        Gui.PlotBubbles(label, xs, ys, szs)
    static member plotBubbles (label: string, xs: double[], ys: double[], szs: double[]) =
        Gui.PlotBubbles(label, xs, ys, szs)

    // PlotPolygon
    static member plotPolygon (label, xs: float32[], ys: float32[]) = Gui.PlotPolygon(label, xs, ys)
    static member plotPolygon (label: string, xs: double[], ys: double[]) = Gui.PlotPolygon(label, xs, ys)

    // PlotBarGroups
    static member plotBarGroups (labels, values: float32[], groupCount, ?groupSize, ?shift, ?flags: PlotBarGroups) =
        Gui.PlotBarGroups(labels, values, groupCount, ?groupSize = groupSize, ?shift = shift, ?flags = flags)
    static member plotBarGroups (labels, values: double[], groupCount, ?groupSize, ?shift, ?flags: PlotBarGroups) =
        Gui.PlotBarGroups(labels, values, groupCount, ?groupSize = groupSize, ?shift = shift, ?flags = flags)

    // PlotCandles
    static member plotCandles (label, xs: double[], opens, highs, lows, closes, ?width, ?bull, ?bear) =
        Gui.PlotCandles(label, xs, opens, highs, lows, closes, ?width = width, ?bullColor = bull, ?bearColor = bear)
    static member plotCandles (label, xs: DateTime[], opens, highs, lows, closes, ?width, ?bull, ?bear) =
        Gui.PlotCandles(label, xs, opens, highs, lows, closes, ?width = width, ?bullColor = bull, ?bearColor = bear)

    // Plot Queries & Utilities
    static member plotText (text: string, x, y, ?offX, ?offY) =
        Gui.PlotText(text, x, y, ?pixOffsetX = offX, ?pixOffsetY = offY)
    static member plotDummy labelId = Gui.PlotDummy(labelId)
    static member isPlotHovered ()             = Gui.IsPlotHovered()
    static member getPlotMousePos yAxis = Gui.GetPlotMousePos(yAxis)
    static member plotToPixels (x, y, ?axis)   = Gui.PlotToPixels(x, y, ?yAxis = axis)

    // ── ImPlot 3D ────────────────────────────────────────────────────────────
    static member plot3d (title: string, ?cfg: Plot3DConfig) =
        let c = defaultArg cfg Plot3DConfig.Default
        Scope.cond
            (fun () ->
                if Gui.BeginPlot3D(title, c.Width, c.Height, c.Flags) then
                    Gui.SetupAxes3D(c.XLabel, c.YLabel, c.ZLabel, c.XFlags, c.YFlags, c.ZFlags)
                    true
                else false)
            Gui.EndPlot3D

    static member plotLine3D (label, xs: float32[], ys: float32[], zs: float32[]) =
        Gui.PlotLine3D(label, xs, ys, zs)
    static member plotLine3D (label: string, xs: double[], ys: double[], zs: double[]) =
        Gui.PlotLine3D(label, xs, ys, zs)

    static member plotScatter3D (label, xs: float32[], ys: float32[], zs: float32[]) =
        Gui.PlotScatter3D(label, xs, ys, zs)
    static member plotScatter3D (label: string, xs: double[], ys: double[], zs: double[]) =
        Gui.PlotScatter3D(label, xs, ys, zs)

    static member plotSurface3D (label, xs: float32[], ys: float32[], zs: float32[], xCount, yCount) =
        Gui.PlotSurface3D(label, xs, ys, zs, xCount, yCount)
    static member plotSurface3D (label: string, xs: double[], ys: double[], zs: double[], xCount, yCount) =
        Gui.PlotSurface3D(label, xs, ys, zs, xCount, yCount)

    static member plotTriangle3D (label, xs: float32[], ys: float32[], zs: float32[]) =
        Gui.PlotTriangle3D(label, xs, ys, zs)
    static member plotTriangle3D (label: string, xs: double[], ys: double[], zs: double[]) =
        Gui.PlotTriangle3D(label, xs, ys, zs)

    static member plotQuad3D (label, xs: float32[], ys: float32[], zs: float32[]) =
        Gui.PlotQuad3D(label, xs, ys, zs)
    static member plotQuad3D (label: string, xs: double[], ys: double[], zs: double[]) =
        Gui.PlotQuad3D(label, xs, ys, zs)

    static member plotMesh3D (label, xs: float32[], ys: float32[], zs: float32[], idx: uint32[]) =
        Gui.PlotMesh3D(label, xs, ys, zs, idx)
    static member plotMesh3D (label: string, xs: double[], ys: double[], zs: double[], idx: uint32[]) =
        Gui.PlotMesh3D(label, xs, ys, zs, idx)

    static member plotText3D (text: string, x, y, z, ?angle, ?offX, ?offY) =
        Gui.PlotText3D(text, x, y, z, ?angle = angle, ?pixOffsetX = offX, ?pixOffsetY = offY)
    static member plotDummy3D labelId = Gui.PlotDummy3D(labelId)

    // ── Node Editor ──────────────────────────────────────────────────────────
    static member editor (id: string, ?cfg: EditorConfig) =
        let c = defaultArg cfg EditorConfig.Default
        ScopeBuilder(
            (fun () ->
                let ctx = EditorContextCache.getOrCreate id c.SettingsFile
                NodeEditor.SetCurrentEditor(ctx)
                NodeEditor.Begin(id, c.Width, c.Height)
                true),
            (fun () ->
                NodeEditor.End()
                NodeEditor.SetCurrentEditor(IntPtr.Zero)),
            true)

    static member node nodeId = Scope.always (fun () -> NodeEditor.BeginNode nodeId) NodeEditor.EndNode
    static member pin (pinId, kind) = Scope.always (fun () -> NodeEditor.BeginPin(pinId, kind)) NodeEditor.EndPin

    static member setNodePosition (nodeId, x, y) = NodeEditor.SetNodePosition(nodeId, x, y)
    static member getNodePosition nodeId = NodeEditor.GetNodePosition(nodeId)
    static member getNodeSize nodeId = NodeEditor.GetNodeSize(nodeId)

    static member pinRect (ax, ay, bx, by)       = NodeEditor.PinRect(ax, ay, bx, by)
    static member pinPivotRect (ax, ay, bx, by)  = NodeEditor.PinPivotRect(ax, ay, bx, by)

    static member link (linkId, startPin, endPin, ?r, ?g, ?b, ?a, ?thickness) =
        NodeEditor.Link(linkId, startPin, endPin, ?r = r, ?g = g, ?b = b, ?a = a, ?thickness = thickness)
    static member flow (linkId, ?direction) =
        NodeEditor.Flow(linkId, ?direction = direction)

    static member create (?cfg: CreateConfig) =
        let c = defaultArg cfg CreateConfig.Default
        ScopeBuilder((fun () -> NodeEditor.BeginCreate(c.R, c.G, c.B, c.A, c.Thickness)), NodeEditor.EndCreate, true)

    static member queryNewLink ()  = NodeEditor.QueryNewLink()
    static member queryNewNode ()  = NodeEditor.QueryNewNode()

    // Note: AcceptNewItem / RejectNewItem have different native defaults than CreateConfig.Default
    // (green / red respectively), so we forward optionals rather than routing through the record.
    static member acceptNewItem (?cfg: CreateConfig) =
        match cfg with
        | Some c -> NodeEditor.AcceptNewItem(c.R, c.G, c.B, c.A, c.Thickness)
        | None   -> NodeEditor.AcceptNewItem()
    static member rejectNewItem (?cfg: CreateConfig) =
        match cfg with
        | Some c -> NodeEditor.RejectNewItem(c.R, c.G, c.B, c.A, c.Thickness)
        | None   -> NodeEditor.RejectNewItem()

    static member delete = ScopeBuilder((fun () -> NodeEditor.BeginDelete()), NodeEditor.EndDelete, true)
    static member queryDeletedLink () = NodeEditor.QueryDeletedLink()
    static member queryDeletedNode () = NodeEditor.QueryDeletedNode()
    static member acceptDelete ?deps = NodeEditor.AcceptDeletedItem(?deleteDependencies = deps)
    static member rejectDelete ()      = NodeEditor.RejectDeletedItem()

    static member isNodeSelected nodeId = NodeEditor.IsNodeSelected(nodeId)
    static member isLinkSelected linkId = NodeEditor.IsLinkSelected(linkId)
    static member selectNode (id, ?append)       = NodeEditor.SelectNode(id, ?append = append)
    static member selectLink (id, ?append)       = NodeEditor.SelectLink(id, ?append = append)
    static member deselectNode id = NodeEditor.DeselectNode(id)
    static member deselectLink id = NodeEditor.DeselectLink(id)
    static member clearSelection ()              = NodeEditor.ClearSelection()
    static member selectedCount ()               = NodeEditor.GetSelectedObjectCount()

    static member navigateToContent ?duration = NodeEditor.NavigateToContent(?duration = duration)
    static member navigateToSelection (?zoomIn, ?duration) =
        NodeEditor.NavigateToSelection(?zoomIn = zoomIn, ?duration = duration)

    static member suspend   = Scope.always NodeEditor.Suspend NodeEditor.Resume
    static member deleteNode nodeId = NodeEditor.DeleteNode(nodeId)
    static member deleteLink linkId = NodeEditor.DeleteLink(linkId)
