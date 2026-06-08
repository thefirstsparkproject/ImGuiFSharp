namespace ImGuiFSharp

open System

/// Backend interface for decoupled rendering and input processing
type IImGuiBackend =
    abstract member Initialize : unit -> unit
    abstract member SetDisplaySize : width: float32 * height: float32 -> unit
    abstract member NewFrame : delta: float32 -> unit
    abstract member Render : drawData: nativeint -> unit
    abstract member Destroy : unit -> unit

/// Core ImGui widget functions
type IGuiFunctions =
    // Windows
    abstract Begin         : name: string * ?pOpen: bool ref * ?flags: int -> bool
    abstract End           : unit -> unit
    // Widgets
    abstract Button        : label: string * ?width: float32 * ?height: float32 -> bool
    abstract Text          : fmt: string -> unit
    abstract InputText     : label: string * buf: char[] * ?flags: int -> bool
    abstract InputFloat    : label: string * v: float32 ref * ?step: float32 * ?stepFast: float32 * ?fmt: string * ?flags: int -> bool
    abstract InputInt      : label: string * v: int ref * ?step: int * ?stepFast: int * ?flags: int -> bool
    abstract SliderFloat   : label: string * v: float32 ref * min: float32 * max: float32 * ?fmt: string * ?flags: int -> bool
    abstract SliderInt     : label: string * v: int ref * min: int * max: int * ?fmt: string * ?flags: int -> bool
    abstract Checkbox      : label: string * v: bool ref -> bool
    abstract CollapsingHeader: label: string * ?flags: int -> bool
    abstract TreeNode      : label: string -> bool
    abstract TreePop       : unit -> unit
    abstract Separator     : unit -> unit
    abstract SameLine      : ?offsetFromStartX: float32 * ?spacing: float32 -> unit
    abstract NewLine       : unit -> unit
    abstract Spacing       : unit -> unit
    abstract PushID        : id: string -> unit
    abstract PopID         : unit -> unit
    // Combo / Selectable
    abstract BeginCombo    : label: string * previewValue: string * ?flags: int -> bool
    abstract EndCombo      : unit -> unit
    abstract Selectable    : label: string * selected: bool * ?flags: int * ?width: float32 * ?height: float32 -> bool
    // Table
    abstract BeginTable    : id: string * columns: int * ?flags: int * ?outerWidth: float32 * ?outerHeight: float32 -> bool
    abstract EndTable      : unit -> unit
    abstract TableSetupColumn: label: string * ?flags: int * ?initWidth: float32 -> unit
    abstract TableNextRow  : ?rowFlags: int * ?minRowHeight: float32 -> unit
    abstract TableNextColumn: unit -> unit
    // Menus
    abstract BeginMenuBar  : unit -> bool
    abstract EndMenuBar    : unit -> unit
    abstract BeginMenu     : label: string * ?enabled: bool -> bool
    abstract EndMenu       : unit -> unit
    abstract MenuItem      : label: string * ?shortcut: string * ?selected: bool * ?enabled: bool -> bool
    // Drag
    abstract DragFloat     : label: string * v: float32 ref * ?speed: float32 * ?min: float32 * ?max: float32 * ?fmt: string * ?flags: int -> bool
    abstract DragInt       : label: string * v: int ref * ?speed: float32 * ?min: int * ?max: int * ?fmt: string * ?flags: int -> bool
    // Colour
    abstract ColorEdit4    : label: string * col: float32[] * ?flags: int -> bool
    // Misc
    abstract RadioButton   : label: string * active: bool -> bool
    abstract ProgressBar   : fraction: float32 * ?width: float32 * ?height: float32 * ?overlay: string -> unit
    abstract Image         : textureId: uint32 * width: float32 * height: float32 -> unit
    abstract ImageButton   : id: string * textureId: uint32 * width: float32 * height: float32 -> bool
    abstract SetNextWindowPos : x: float32 * y: float32 * ?cond: int -> unit
    abstract SetNextWindowSize: width: float32 * height: float32 * ?cond: int -> unit
    abstract ShowDemoWindow: ?pOpen: bool ref -> unit
    // Docking
    abstract DockSpace     : id: uint32 * width: float32 * height: float32 * ?flags: int -> uint32

    // Double-precision widgets
    abstract InputDouble   : label: string * v: double ref * ?step: double * ?stepFast: double * ?fmt: string * ?flags: int -> bool
    abstract DragDouble    : label: string * v: double ref * ?speed: float32 * ?min: double * ?max: double * ?fmt: string * ?flags: int -> bool
    abstract SliderDouble  : label: string * v: double ref * min: double * max: double * ?fmt: string * ?flags: int -> bool

    // Text variants
    abstract TextColored   : r: float32 * g: float32 * b: float32 * a: float32 * text: string -> unit
    abstract TextDisabled  : text: string -> unit
    abstract TextWrapped   : text: string -> unit
    abstract InputTextMultiline : label: string * buf: char[] * ?width: float32 * ?height: float32 * ?flags: int -> bool

    // Layout
    abstract BeginChild    : strId: string * ?width: float32 * ?height: float32 * ?border: bool * ?flags: int -> bool
    abstract EndChild      : unit -> unit
    abstract BeginGroup    : unit -> unit
    abstract EndGroup      : unit -> unit
    abstract Dummy         : width: float32 * height: float32 -> unit
    abstract Indent        : indentW: float32 -> unit
    abstract Unindent      : indentW: float32 -> unit
    abstract GetContentRegionAvail : unit -> float32 * float32
    abstract GetWindowSize : unit -> float32 * float32
    abstract GetWindowPos  : unit -> float32 * float32
    abstract SetNextWindowBgAlpha  : alpha: float32 -> unit

    // Style
    abstract PushStyleColor : idx: int * r: float32 * g: float32 * b: float32 * a: float32 -> unit
    abstract PopStyleColor  : ?count: int -> unit
    abstract PushStyleVar   : idx: int * valFloat: float32 -> unit
    abstract PushStyleVar   : idx: int * valVec2X: float32 * valVec2Y: float32 -> unit
    abstract PopStyleVar    : ?count: int -> unit

    // Queries
    abstract IsItemHovered  : ?flags: int -> bool
    abstract IsItemActive   : unit -> bool
    abstract IsItemClicked  : ?mouseButton: int -> bool
    abstract IsMouseClicked : button: int * ?repeat: bool -> bool
    abstract IsMouseDown    : button: int -> bool
    abstract IsMouseDoubleClicked : button: int -> bool
    abstract GetMousePos    : unit -> float32 * float32

    // Tooltips, popups, and modals
    abstract BeginTooltip   : unit -> unit
    abstract EndTooltip     : unit -> unit
    abstract SetTooltip     : text: string -> unit
    abstract BeginItemTooltip : unit -> bool
    abstract SetItemTooltip : text: string -> unit
    abstract OpenPopup      : strId: string * ?flags: int -> unit
    abstract BeginPopup     : strId: string * ?flags: int -> bool
    abstract BeginPopupModal: name: string * ?pOpen: bool ref * ?flags: int -> bool
    abstract EndPopup       : unit -> unit
    abstract CloseCurrentPopup : unit -> unit
    abstract BeginPopupContextItem : ?strId: string * ?flags: int -> bool
    abstract BeginPopupContextWindow : ?strId: string * ?flags: int -> bool

    // Tab Bars & List Boxes
    abstract BeginTabBar    : strId: string * ?flags: int -> bool
    abstract EndTabBar      : unit -> unit
    abstract BeginTabItem   : label: string * ?pOpen: bool ref * ?flags: int -> bool
    abstract EndTabItem     : unit -> unit
    abstract BeginListBox   : label: string * ?width: float32 * ?height: float32 -> bool
    abstract EndListBox     : unit -> unit

    // Canvas drawing
    abstract GetCursorScreenPos : unit -> float32 * float32
    abstract SetCursorScreenPos : x: float32 * y: float32 -> unit
    abstract InvisibleButton : strId: string * width: float32 * height: float32 * ?flags: int -> bool
    abstract DrawLine       : p1_x: float32 * p1_y: float32 * p2_x: float32 * p2_y: float32 * col: uint32 * ?thickness: float32 -> unit
    abstract DrawRect       : p1_x: float32 * p1_y: float32 * p2_x: float32 * p2_y: float32 * col: uint32 * ?rounding: float32 * ?flags: int * ?thickness: float32 -> unit
    abstract DrawRectFilled : p1_x: float32 * p1_y: float32 * p2_x: float32 * p2_y: float32 * col: uint32 * ?rounding: float32 * ?flags: int -> unit
    abstract DrawRectFilledMultiColor : p1_x: float32 * p1_y: float32 * p2_x: float32 * p2_y: float32 * colUprLeft: uint32 * colUprRight: uint32 * colBotRight: uint32 * colBotLeft: uint32 -> unit
    abstract DrawCircle     : centerX: float32 * centerY: float32 * radius: float32 * col: uint32 * ?numSegments: int * ?thickness: float32 -> unit
    abstract DrawCircleFilled : centerX: float32 * centerY: float32 * radius: float32 * col: uint32 * ?numSegments: int -> unit
    abstract DrawTriangleFilled : p1_x: float32 * p1_y: float32 * p2_x: float32 * p2_y: float32 * p3_x: float32 * p3_y: float32 * col: uint32 -> unit
    abstract DrawText       : posX: float32 * posY: float32 * col: uint32 * text: string -> unit
    abstract DrawPolyline   : pointsX: float32[] * pointsY: float32[] * col: uint32 * ?flags: int * ?thickness: float32 -> unit
    abstract DrawConvexPolyFilled : pointsX: float32[] * pointsY: float32[] * col: uint32 -> unit
    abstract DrawImage      : textureId: uint32 * p1_x: float32 * p1_y: float32 * p2_x: float32 * p2_y: float32 * ?uv1_x: float32 * ?uv1_y: float32 * ?uv2_x: float32 * ?uv2_y: float32 * ?col: uint32 -> unit
    abstract PushClipRect   : minX: float32 * minY: float32 * maxX: float32 * maxY: float32 * ?intersectWithCurrent: bool -> unit
    abstract PopClipRect    : unit -> unit

/// ImPlot 2D charting functions
type IPlotFunctions =
    abstract BeginPlot     : titleId: string * ?width: float32 * ?height: float32 * ?flags: int -> bool
    abstract EndPlot       : unit -> unit
    abstract SetupAxes     : xLabel: string * yLabel: string * ?xFlags: int * ?yFlags: int -> unit
    abstract PlotLine      : label: string * values: float32[] * ?xscale: float * ?x0: float -> unit
    abstract PlotBars      : label: string * values: float32[] * ?barSize: float * ?shift: float -> unit
    abstract PlotScatter   : label: string * xs: float32[] * ys: float32[] -> unit
    abstract PlotHeatmap   : label: string * values: float32[] * rows: int * cols: int * ?scaleMin: float * ?scaleMax: float -> unit
    abstract ShowDemoWindow: ?pOpen: bool ref -> unit

    // Legend & limits
    abstract SetupAxisLimits : axis: int * v_min: double * v_max: double * ?cond: int -> unit
    abstract SetNextAxesLimits : x_min: double * x_max: double * y_min: double * y_max: double * ?cond: int -> unit
    abstract SetupLegend    : location: int * ?flags: int -> unit
    abstract SetupAxisScale : axis: int * scale: int -> unit
    abstract SetupAxisFormat: axis: int * fmt: string -> unit

    // Double precision plotting
    abstract PlotLine      : label: string * values: double[] * ?xscale: double * ?x0: double -> unit
    abstract PlotLine      : label: string * xs: double[] * ys: double[] -> unit
    abstract PlotLine      : label: string * xs: DateTime[] * ys: double[] -> unit
    abstract PlotBars      : label: string * xs: double[] * ys: double[] * ?width: double -> unit
    abstract PlotScatter   : label: string * xs: double[] * ys: double[] -> unit
    abstract PlotShaded    : label: string * xs: double[] * ys1: double[] * ys2: double[] -> unit
    abstract PlotStairs    : label: string * xs: double[] * ys: double[] -> unit
    abstract PlotErrorBars : label: string * xs: double[] * ys: double[] * err: double[] -> unit
    abstract PlotPieChart  : labels: string[] * values: double[] * x: double * y: double * radius: double * ?labelFmt: string * ?angle0: double -> unit

    // Queries & coords
    abstract IsPlotHovered  : unit -> bool
    abstract GetPlotMousePos : yAxis: int -> double * double
    abstract PlotToPixels   : x: double * y: double * ?yAxis: int -> float32 * float32

    // Candlesticks
    abstract PlotCandles    : label: string * xs: double[] * opens: double[] * highs: double[] * lows: double[] * closes: double[] * ?width: double * ?bullColor: uint32 * ?bearColor: uint32 -> unit
    abstract PlotCandles    : label: string * xs: DateTime[] * opens: double[] * highs: double[] * lows: double[] * closes: double[] * ?width: double * ?bullColor: uint32 * ?bearColor: uint32 -> unit

/// ImPlot3D 3D plotting functions
type IPlot3DFunctions =
    abstract BeginPlot     : titleId: string * ?width: float32 * ?height: float32 * ?flags: int -> bool
    abstract EndPlot       : unit -> unit
    abstract SetupAxes     : xLabel: string * yLabel: string * zLabel: string * ?xFlags: int * ?yFlags: int * ?zFlags: int -> unit
    abstract PlotLine      : label: string * xs: float32[] * ys: float32[] * zs: float32[] -> unit
    abstract PlotScatter   : label: string * xs: float32[] * ys: float32[] * zs: float32[] -> unit
    abstract PlotSurface   : label: string * xs: float32[] * ys: float32[] * zs: float32[] * xCount: int * yCount: int -> unit
    abstract ShowDemoWindow: ?pOpen: bool ref -> unit

    // Double precision 3D plotting
    abstract PlotLine      : label: string * xs: double[] * ys: double[] * zs: double[] -> unit
    abstract PlotScatter   : label: string * xs: double[] * ys: double[] * zs: double[] -> unit
    abstract PlotSurface   : label: string * xs: double[] * ys: double[] * zs: double[] * xCount: int * yCount: int -> unit

/// Font management
type IFontFunctions =
    abstract AddDefaultFont  : unit -> int
    abstract AddFontFromFile : path: string * sizePixels: float32 -> int
    abstract Build           : unit -> bool

/// PinKind mirrors ax::NodeEditor::PinKind
type PinKind = Input = 0 | Output = 1

/// FlowDirection mirrors ax::NodeEditor::FlowDirection
type FlowDirection = Forward = 0 | Backward = 1

/// Node editor (imgui-node-editor by thedmd) high-level interface.
/// Each ImGuiInstance owns exactly one node-editor context lifetime;
/// use CreateEditor / DestroyEditor to manage additional contexts.
type INodeEditorFunctions =
    // ── Context ──────────────────────────────────────────────────────────
    /// Creates an editor context. Pass None for the default settings file.
    abstract CreateEditor       : ?settingsFile: string -> nativeint
    abstract DestroyEditor      : ctx: nativeint -> unit
    abstract SetCurrentEditor   : ctx: nativeint -> unit

    // ── Frame ─────────────────────────────────────────────────────────────
    /// Begin node editor canvas. w/h = 0 means fill available space.
    abstract Begin              : id: string * ?width: float32 * ?height: float32 -> unit
    abstract End                : unit -> unit

    // ── Nodes ─────────────────────────────────────────────────────────────
    abstract BeginNode          : nodeId: int64 -> unit
    abstract EndNode            : unit -> unit
    abstract SetNodePosition    : nodeId: int64 * x: float32 * y: float32 -> unit
    abstract GetNodePosition    : nodeId: int64 -> float32 * float32
    abstract GetNodeSize        : nodeId: int64 -> float32 * float32

    // ── Pins ──────────────────────────────────────────────────────────────
    abstract BeginPin           : pinId: int64 * kind: PinKind -> unit
    abstract EndPin             : unit -> unit
    abstract PinRect            : ax: float32 * ay: float32 * bx: float32 * by: float32 -> unit
    abstract PinPivotRect       : ax: float32 * ay: float32 * bx: float32 * by: float32 -> unit

    // ── Links ─────────────────────────────────────────────────────────────
    abstract Link               : linkId: int64 * startPinId: int64 * endPinId: int64
                                   * ?r: float32 * ?g: float32 * ?b: float32 * ?a: float32
                                   * ?thickness: float32 -> unit
    abstract Flow               : linkId: int64 * ?direction: FlowDirection -> unit

    // ── Selection & navigation ────────────────────────────────────────────
    abstract NavigateToContent  : ?duration: float32 -> unit
    abstract NavigateToSelection: ?zoomIn: bool * ?duration: float32 -> unit
    abstract IsNodeSelected     : nodeId: int64 -> bool
    abstract IsLinkSelected     : linkId: int64 -> bool
    abstract SelectNode         : nodeId: int64 * ?append: bool -> unit
    abstract DeselectNode       : nodeId: int64 -> unit
    abstract SelectLink         : linkId: int64 * ?append: bool -> unit
    abstract DeselectLink       : linkId: int64 -> unit
    abstract ClearSelection     : unit -> unit
    abstract GetSelectedObjectCount : unit -> int

    // ── Create interaction ────────────────────────────────────────────────
    /// Returns true while the user is hovering over a pin to create a link.
    abstract BeginCreate        : ?r: float32 * ?g: float32 * ?b: float32 * ?a: float32 * ?thickness: float32 -> bool
    abstract EndCreate          : unit -> unit
    /// Returns (true, startPinId, endPinId) when the user is dragging a new link.
    abstract QueryNewLink       : unit -> bool * int64 * int64
    /// Returns (true, pinId) when the user hovers empty canvas to create a node.
    abstract QueryNewNode       : unit -> bool * int64
    abstract AcceptNewItem      : ?r: float32 * ?g: float32 * ?b: float32 * ?a: float32 * ?thickness: float32 -> bool
    abstract RejectNewItem      : ?r: float32 * ?g: float32 * ?b: float32 * ?a: float32 * ?thickness: float32 -> unit

    // ── Delete interaction ────────────────────────────────────────────────
    abstract BeginDelete        : unit -> bool
    abstract EndDelete          : unit -> unit
    /// Returns (true, linkId, startPinId, endPinId) when a link is to be deleted.
    abstract QueryDeletedLink   : unit -> bool * int64 * int64 * int64
    /// Returns (true, nodeId) when a node is to be deleted.
    abstract QueryDeletedNode   : unit -> bool * int64
    abstract AcceptDeletedItem  : ?deleteDependencies: bool -> bool
    abstract RejectDeletedItem  : unit -> unit

    // ── Suspend / Resume ──────────────────────────────────────────────────
    abstract Suspend            : unit -> unit
    abstract Resume             : unit -> unit

    // ── Utility ───────────────────────────────────────────────────────────
    abstract DeleteNode         : nodeId: int64 -> bool
    abstract DeleteLink         : linkId: int64 -> bool

/// Bundle of the core ImGui/ImPlot/Font interfaces.
type GuiApi =
    {
        Gui : IGuiFunctions
        Plot : IPlotFunctions
        Plot3D : IPlot3DFunctions
        Fonts : IFontFunctions
        NodeEditor : INodeEditorFunctions
    }

/// The layout builder interface implemented to draw widgets.
type IGuiBuilder =
    abstract member OnGui : api: GuiApi -> unit
