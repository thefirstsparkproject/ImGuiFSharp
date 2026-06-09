namespace ImGuiFSharp

open ImGuiFSharp.NodeEditorNative

/// PinKind mirrors ax::NodeEditor::PinKind
type PinKind = Input = 0 | Output = 1

/// FlowDirection mirrors ax::NodeEditor::FlowDirection
type FlowDirection = Forward = 0 | Backward = 1

[<AbstractClass; Sealed>]
type public NodeEditor = class

    // ── Context ─────────────────────────────────────────────────────────────

    static member CreateEditor(?settingsFile: string) =
        let sf = defaultArg settingsFile ""
        IGNE_CreateEditor(sf)

    static member DestroyEditor(ctx: nativeint) =
        IGNE_DestroyEditor(ctx)

    static member SetCurrentEditor(ctx: nativeint) =
        IGNE_SetCurrentEditor(ctx)

    // ── Frame ───────────────────────────────────────────────────────────────

    static member Begin(id: string, ?width: float32, ?height: float32) =
        let w = defaultArg width 0f
        let h = defaultArg height 0f
        IGNE_Begin(id, w, h)

    static member End() =
        IGNE_End()

    // ── Nodes ───────────────────────────────────────────────────────────────

    static member BeginNode(nodeId: int64) =
        IGNE_BeginNode(nodeId)

    static member EndNode() =
        IGNE_EndNode()

    static member SetNodePosition(nodeId: int64, x: float32, y: float32) =
        IGNE_SetNodePosition(nodeId, x, y)

    static member GetNodePosition(nodeId: int64) =
        let mutable x = 0f
        let mutable y = 0f
        IGNE_GetNodePosition(nodeId, &x, &y)
        (x, y)

    static member GetNodeSize(nodeId: int64) =
        let mutable w = 0f
        let mutable h = 0f
        IGNE_GetNodeSize(nodeId, &w, &h)
        (w, h)

    // ── Pins ────────────────────────────────────────────────────────────────

    static member BeginPin(pinId: int64, kind: PinKind) =
        IGNE_BeginPin(pinId, int kind)

    static member EndPin() =
        IGNE_EndPin()

    static member PinRect(ax: float32, ay: float32, bx: float32, by: float32) =
        IGNE_PinRect(ax, ay, bx, by)

    static member PinPivotRect(ax: float32, ay: float32, bx: float32, by: float32) =
        IGNE_PinPivotRect(ax, ay, bx, by)

    // ── Links ───────────────────────────────────────────────────────────────

    static member Link(linkId: int64, startPinId: int64, endPinId: int64, ?r: float32, ?g: float32, ?b: float32, ?a: float32, ?thickness: float32) =
        IGNE_Link(linkId, startPinId, endPinId,
                  defaultArg r 1f, defaultArg g 1f, defaultArg b 1f, defaultArg a 1f,
                  defaultArg thickness 1f)

    static member Flow(linkId: int64, ?direction: FlowDirection) =
        IGNE_Flow(linkId, int (defaultArg direction FlowDirection.Forward))

    // ── Selection & navigation ──────────────────────────────────────────────

    static member NavigateToContent(?duration: float32) =
        IGNE_NavigateToContent(defaultArg duration -1f)

    static member NavigateToSelection(?zoomIn: bool, ?duration: float32) =
        IGNE_NavigateToSelection(defaultArg zoomIn false, defaultArg duration -1f)

    static member IsNodeSelected(nodeId: int64) =
        IGNE_IsNodeSelected(nodeId)

    static member IsLinkSelected(linkId: int64) =
        IGNE_IsLinkSelected(linkId)

    static member SelectNode(nodeId: int64, ?append: bool) =
        IGNE_SelectNode(nodeId, defaultArg append false)

    static member DeselectNode(nodeId: int64) =
        IGNE_DeselectNode(nodeId)

    static member SelectLink(linkId: int64, ?append: bool) =
        IGNE_SelectLink(linkId, defaultArg append false)

    static member DeselectLink(linkId: int64) =
        IGNE_DeselectLink(linkId)

    static member ClearSelection() =
        IGNE_ClearSelection()

    static member GetSelectedObjectCount() =
        IGNE_GetSelectedObjectCount()

    // ── Create interaction ──────────────────────────────────────────────────

    static member BeginCreate(?r: float32, ?g: float32, ?b: float32, ?a: float32, ?thickness: float32) =
        IGNE_BeginCreate(defaultArg r 1f, defaultArg g 1f, defaultArg b 1f,
                          defaultArg a 1f, defaultArg thickness 1f)

    static member EndCreate() =
        IGNE_EndCreate()

    static member QueryNewLink() =
        let mutable s = 0L
        let mutable e = 0L
        let ok = IGNE_QueryNewLink(&s, &e)
        (ok, s, e)

    static member QueryNewNode() =
        let mutable p = 0L
        let ok = IGNE_QueryNewNode(&p)
        (ok, p)

    static member AcceptNewItem(?r: float32, ?g: float32, ?b: float32, ?a: float32, ?thickness: float32) =
        IGNE_AcceptNewItem(defaultArg r 0.5f, defaultArg g 1f, defaultArg b 0.5f,
                           defaultArg a 1f, defaultArg thickness 1f)

    static member RejectNewItem(?r: float32, ?g: float32, ?b: float32, ?a: float32, ?thickness: float32) =
        IGNE_RejectNewItem(defaultArg r 1f, defaultArg g 0f, defaultArg b 0f,
                           defaultArg a 1f, defaultArg thickness 2f)

    // ── Delete interaction ──────────────────────────────────────────────────

    static member BeginDelete() =
        IGNE_BeginDelete()

    static member EndDelete() =
        IGNE_EndDelete()

    static member QueryDeletedLink() =
        let mutable lid = 0L
        let mutable s   = 0L
        let mutable e   = 0L
        let ok = IGNE_QueryDeletedLink(&lid, &s, &e)
        (ok, lid, s, e)

    static member QueryDeletedNode() =
        let mutable n = 0L
        let ok = IGNE_QueryDeletedNode(&n)
        (ok, n)

    static member AcceptDeletedItem(?deleteDependencies: bool) =
        IGNE_AcceptDeletedItem(defaultArg deleteDependencies true)

    static member RejectDeletedItem() =
        IGNE_RejectDeletedItem()

    // ── Suspend / Resume ─────────────────────────────────────────────────────

    static member Suspend() =
        IGNE_Suspend()

    static member Resume() =
        IGNE_Resume()

    // ── Utility ──────────────────────────────────────────────────────────────

    static member DeleteNode(nodeId: int64) =
        IGNE_DeleteNode(nodeId)

    static member DeleteLink(linkId: int64) =
        IGNE_DeleteLink(linkId)

end
