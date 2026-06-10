module ImGuiFSharp.Flags

open System

[<Flags>]
type Button =
    | None = 0
    | MouseButtonLeft = 1
    | MouseButtonRight = 2
    | MouseButtonMiddle = 4

[<Flags>]
type Window =
    | None = 0
    | NoTitleBar = 1
    | NoResize = 2
    | NoMove = 4
    | NoScrollbar = 8
    | NoScrollWithMouse = 16
    | NoCollapse = 32
    | AlwaysAutoResize = 64
    | NoBackground = 128
    | NoSavedSettings = 256
    | NoMouseInputs = 512
    | MenuBar = 1024
    | HorizontalScrollbar = 2048
    | NoFocusOnAppearing = 4096
    | NoBringToFrontOnFocus = 8192
    | AlwaysVerticalScrollbar = 16384
    | AlwaysHorizontalScrollbar = 32768
    | NoNavInputs = 65536
    | NoNavFocus = 131072
    | UnsavedDocument = 262144
    | NoDocking = 524288
    | NoNav = 196608 // NoNavInputs | NoNavFocus
    | NoDecoration = 43 // NoTitleBar | NoResize | NoScrollbar | NoCollapse
    | NoInputs = 197120 // NoMouseInputs | NoNavInputs | NoNavFocus

[<Flags>]
type Child =
    | None = 0
    | Borders = 1
    | AlwaysUseWindowPadding = 2
    | ResizeX = 4
    | ResizeY = 8
    | AutoResizeX = 16
    | AutoResizeY = 32
    | AlwaysAutoResize = 64
    | FrameStyle = 128
    | NavFlattened = 256

[<Flags>]
type DockNode =
    | None = 0
    | KeepAliveOnly = 1
    | NoDockingOverCentralNode = 4
    | PassthruCentralNode = 8
    | NoDockingSplit = 16
    | NoResize = 32
    | AutoHideTabBar = 64
    | NoUndocking = 128
    // Private/Internal flags
    | DockSpace = 1024
    | CentralNode = 2048
    | NoTabBar = 4096
    | HiddenTabBar = 8192
    | NoWindowMenuButton = 16384
    | NoCloseButton = 32768
    | NoResizeX = 65536
    | NoResizeY = 131072


[<Flags>]
type Combo =
    | None = 0
    | PopupAlignLeft = 1
    | HeightSmall = 2
    | HeightRegular = 4
    | HeightLarge = 8
    | HeightLargest = 16
    | NoArrowButton = 32
    | NoPreview = 64
    | WidthFitPreview = 128
    | HeightMask = 30 // HeightSmall | HeightRegular | HeightLarge | HeightLargest

[<Flags>]
type TabBar =
    | None = 0
    | Reorderable = 1
    | AutoSelectNewTabs = 2
    | TabListPopupButton = 4
    | NoCloseWithMiddleMouseButton = 8
    | NoTabListScrollingButtons = 16
    | NoTooltip = 32
    | DrawSelectedOverline = 64
    | FittingPolicyMixed = 128
    | FittingPolicyShrink = 256
    | FittingPolicyScroll = 512
    | FittingPolicyMask = 896 // FittingPolicyMixed | FittingPolicyShrink | FittingPolicyScroll
    | FittingPolicyDefault = 128 // FittingPolicyMixed

[<Flags>]
type TabItem =
    | None = 0
    | UnsavedDocument = 1
    | SetSelected = 2
    | NoCloseWithMiddleMouseButton = 4
    | NoPushId = 8
    | NoTooltip = 16
    | NoReorder = 32
    | Leading = 64
    | Trailing = 128
    | NoAssumedClosure = 256

[<Flags>]
type Popup =
    | None = 0
    | MouseButtonLeft = 4
    | MouseButtonRight = 8
    | MouseButtonMiddle = 12
    | NoReopen = 32
    | NoOpenOverExistingPopup = 128
    | NoOpenOverItems = 256
    | AnyPopupId = 1024
    | AnyPopupLevel = 2048
    | AnyPopup = 3072 // AnyPopupId | AnyPopupLevel
    | MouseButtonMask = 12

[<Flags>]
type Table =
    | None = 0
    | Resizable = 1
    | Reorderable = 2
    | Hideable = 4
    | Sortable = 8
    | NoSavedSettings = 16
    | ContextMenuInBody = 32
    | RowBg = 64
    | BordersInnerH = 128
    | BordersOuterH = 256
    | BordersInnerV = 512
    | BordersOuterV = 1024
    | BordersH = 384 // BordersInnerH | BordersOuterH
    | BordersV = 1536 // BordersInnerV | BordersOuterV
    | BordersInner = 640 // BordersInnerV | BordersInnerH
    | BordersOuter = 1280 // BordersOuterV | BordersOuterH
    | Borders = 1920 // BordersInner | BordersOuter
    | NoBordersInBody = 2048
    | NoBordersInBodyUntilResize = 4096
    | SizingFixedFit = 8192
    | SizingFixedSame = 16384
    | SizingStretchProp = 24576
    | SizingStretchSame = 32768
    | NoHostExtendX = 65536
    | NoHostExtendY = 131072
    | NoKeepColumnsVisible = 262144
    | PreciseWidths = 524288
    | NoClip = 1048576
    | PadOuterX = 2097152
    | NoPadOuterX = 4194304
    | NoPadInnerX = 8388608
    | ScrollX = 16777216
    | ScrollY = 33554432
    | SortMulti = 67108864
    | SortTristate = 134217728
    | HighlightHoveredColumn = 268435456

[<Flags>]
type TableColumn =
    | None = 0
    | Disabled = 1
    | DefaultHide = 2
    | DefaultSort = 4
    | WidthStretch = 8
    | WidthFixed = 16
    | NoResize = 32
    | NoReorder = 64
    | NoHide = 128
    | NoClip = 256
    | NoSort = 512
    | NoSortAscending = 1024
    | NoSortDescending = 2048
    | NoHeaderLabel = 4096
    | NoHeaderWidth = 8192
    | PreferSortAscending = 16384
    | PreferSortDescending = 32768
    | IndentEnable = 65536
    | IndentDisable = 131072
    | AngledHeader = 262144
    | IsEnabled = 16777216
    | IsVisible = 33554432
    | IsSorted = 67108864
    | IsHovered = 134217728
    | WidthMask = 24 // WidthStretch | WidthFixed
    | IndentMask = 196608 // IndentEnable | IndentDisable
    | StatusMask = 251658240 // IsEnabled | IsVisible | IsSorted | IsHovered

[<Flags>]
type TableRow =
    | None = 0
    | Headers = 1

[<Flags>]
type Selectable =
    | None = 0
    | NoAutoClosePopups = 1
    | SpanAllColumns = 2
    | AllowDoubleClick = 4
    | Disabled = 8
    | AllowOverlap = 16
    | Highlight = 32
    | SelectOnNav = 64

[<Flags>]
type InputText =
    | None = 0
    | CharsDecimal = 1
    | CharsHexadecimal = 2
    | CharsScientific = 4
    | CharsUppercase = 8
    | CharsNoBlank = 16
    | AllowTabInput = 32
    | EnterReturnsTrue = 64
    | EscapeClearsAll = 128
    | CtrlEnterForNewLine = 256
    | ReadOnly = 512
    | Password = 1024
    | AlwaysOverwrite = 2048
    | AutoSelectAll = 4096
    | ParseEmptyRefVal = 8192
    | DisplayEmptyRefVal = 16384
    | NoHorizontalScroll = 32768
    | NoUndoRedo = 65536
    | ElideLeft = 131072
    | CallbackCompletion = 262144
    | CallbackHistory = 524288
    | CallbackAlways = 1048576
    | CallbackCharFilter = 2097152
    | CallbackResize = 4194304
    | CallbackEdit = 8388608
    | WordWrap = 16777216

[<Flags>]
type Hovered =
    | None = 0
    | ChildWindows = 1
    | RootWindow = 2
    | AnyWindow = 4
    | NoPopupHierarchy = 8
    | DockHierarchy = 16
    | AllowWhenBlockedByPopup = 32
    | AllowWhenBlockedByActiveItem = 128
    | AllowWhenOverlappedByItem = 256
    | AllowWhenOverlappedByWindow = 512
    | AllowWhenDisabled = 1024
    | NoNavOverride = 2048
    | AllowWhenOverlapped = 768 // AllowWhenOverlappedByItem | AllowWhenOverlappedByWindow
    | RectOnly = 928 // AllowWhenBlockedByPopup | AllowWhenBlockedByActiveItem | AllowWhenOverlapped
    | RootAndChildWindows = 3 // RootWindow | ChildWindows
    | ForTooltip = 4096
    | Stationary = 8192
    | DelayNone = 16384
    | DelayShort = 32768
    | DelayNormal = 65536
    | NoSharedDelay = 131072

[<Flags>]
type ColorEdit =
    | None = 0
    | NoAlpha = 2
    | NoPicker = 4
    | NoOptions = 8
    | NoSmallPreview = 16
    | NoInputs = 32
    | NoTooltip = 64
    | NoLabel = 128
    | NoSidePreview = 256
    | NoDragDrop = 512
    | NoBorder = 1024
    | NoColorMarkers = 2048
    | AlphaOpaque = 4096
    | AlphaNoBg = 8192
    | AlphaPreviewHalf = 16384
    | AlphaBar = 262144
    | HDR = 524288
    | DisplayRGB = 1048576
    | DisplayHSV = 2097152
    | DisplayHex = 4194304
    | Uint8 = 8388608
    | Float = 16777216
    | PickerHueBar = 33554432
    | PickerHueWheel = 67108864
    | InputRGB = 134217728
    | InputHSV = 268435456

[<Flags>]
type Slider =
    | None = 0
    | Logarithmic = 32
    | NoRoundToFormat = 64
    | NoInput = 128
    | WrapAround = 256
    | ClampOnInput = 512
    | ClampZeroRange = 1024
    | NoSpeedTweaks = 2048
    | ColorMarkers = 4096
    | AlwaysClamp = 1536 // ClampOnInput | ClampZeroRange

[<Flags>]
type TreeNode =
    | None = 0
    | Selected = 1
    | Framed = 2
    | AllowOverlap = 4
    | NoTreePushOnOpen = 8
    | NoAutoOpenOnLog = 16
    | DefaultOpen = 32
    | OpenOnDoubleClick = 64
    | OpenOnArrow = 128
    | Leaf = 256
    | Bullet = 512
    | FramePadding = 1024
    | SpanAvailWidth = 2048
    | SpanFullWidth = 4096
    | SpanLabelWidth = 8192
    | SpanAllColumns = 16384
    | LabelSpanAllColumns = 32768
    | NavLeftJumpsToParent = 131072
    | CollapsingHeader = 26 // Framed | NoTreePushOnOpen | NoAutoOpenOnLog
    | DrawLinesNone = 262144
    | DrawLinesFull = 524288
    | DrawLinesToNodes = 1048576

[<Flags>]
type Plot =
    | None = 0
    | NoTitle = 1
    | NoLegend = 2
    | NoMouseText = 4
    | NoInputs = 8
    | NoMenus = 16
    | NoBoxSelect = 32
    | NoFrame = 64
    | Equal = 128
    | Crosshairs = 256
    | CanvasOnly = 55 // NoTitle | NoLegend | NoMenus | NoBoxSelect | NoMouseText

[<Flags>]
type PlotAxis =
    | None = 0
    | NoLabel = 1
    | NoGridLines = 2
    | NoTickMarks = 4
    | NoTickLabels = 8
    | NoInitialFit = 16
    | NoMenus = 32
    | NoSideSwitch = 64
    | NoHighlight = 128
    | Opposite = 256
    | Foreground = 512
    | Invert = 1024
    | AutoFit = 2048
    | RangeFit = 4096
    | PanStretch = 8192
    | LockMin = 16384
    | LockMax = 32768
    | Lock = 49152 // LockMin | LockMax
    | NoDecorations = 15 // NoLabel | NoGridLines | NoTickMarks | NoTickLabels
    | AuxDefault = 258 // NoGridLines | Opposite

[<Flags>]
type PlotLegend =
    | None = 0
    | NoButtons = 1
    | NoHighlightItem = 2
    | NoHighlightAxis = 4
    | NoMenus = 8
    | Outside = 16
    | Horizontal = 32
    | Sort = 64
    | Reverse = 128

[<Flags>]
type PlotLocation =
    | Center = 0
    | North = 1
    | South = 2
    | West = 4
    | East = 8
    | NorthWest = 5
    | NorthEast = 9
    | SouthWest = 6
    | SouthEast = 10

[<Flags>]
type Plot3D =
    | None = 0
    | NoTitle = 1
    | NoLegend = 2
    | NoMouseText = 4
    | NoClip = 8
    | NoMenus = 16
    | Equal = 32
    | NoRotate = 64
    | NoPan = 128
    | NoZoom = 256
    | NoInputs = 512
    | CanvasOnly = 7 // NoTitle | NoLegend | NoMouseText

[<Flags>]
type Plot3DAxis =
    | None = 0
    | NoLabel = 1
    | NoGridLines = 2
    | NoTickMarks = 4
    | NoTickLabels = 8
    | LockMin = 16
    | LockMax = 32
    | AutoFit = 64
    | Invert = 128
    | PanStretch = 256
    | Lock = 48 // LockMin | LockMax
    | NoDecorations = 11 // NoLabel | NoGridLines | NoTickLabels

[<Flags>]
type Plot3DLegend =
    | None = 0
    | NoButtons = 1
    | NoHighlightItem = 2
    | Horizontal = 4

[<Flags>]
type PlotBarGroups =
    | None = 0
    | Horizontal = 1024
    | Stacked = 2048

[<Flags>]
type Draw =
    | None = 0
    | Closed = 512
    | RoundCornersTopLeft = 16
    | RoundCornersTopRight = 32
    | RoundCornersBottomLeft = 64
    | RoundCornersBottomRight = 128
    | RoundCornersNone = 256
    | RoundCornersTop = 48 // RoundCornersTopLeft | RoundCornersTopRight
    | RoundCornersBottom = 192 // RoundCornersBottomLeft | RoundCornersBottomRight
    | RoundCornersLeft = 80 // RoundCornersTopLeft | RoundCornersBottomLeft
    | RoundCornersRight = 160 // RoundCornersTopRight | RoundCornersBottomRight
    | RoundCornersAll = 240 // RoundCornersTopLeft | RoundCornersTopRight | RoundCornersBottomLeft | RoundCornersBottomRight
    | RoundCornersDefault = 240 // RoundCornersAll

[<Flags>]
type ItemFlags =
    | None              = 0
    | NoTabStop         = 1
    | NoNav             = 2
    | NoNavDefaultFocus = 4
    | ButtonRepeat      = 8
    | AutoClosePopups   = 16
    | AllowDuplicateId  = 32
