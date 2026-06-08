namespace ImGuiFSharp

open NodeEditorNative

/// Concrete implementation of INodeEditorFunctions backed by the native bridge.
type NodeEditorImpl() =

    interface INodeEditorFunctions with

        // ── Context ───────────────────────────────────────────────────────────

        member _.CreateEditor(?settingsFile) =
            let sf = defaultArg settingsFile ""
            IGNE_CreateEditor(sf)

        member _.DestroyEditor(ctx) =
            IGNE_DestroyEditor(ctx)

        member _.SetCurrentEditor(ctx) =
            IGNE_SetCurrentEditor(ctx)

        // ── Frame ─────────────────────────────────────────────────────────────

        member _.Begin(id, ?width, ?height) =
            let w = defaultArg width 0f
            let h = defaultArg height 0f
            IGNE_Begin(id, w, h)

        member _.End() =
            IGNE_End()

        // ── Nodes ─────────────────────────────────────────────────────────────

        member _.BeginNode(nodeId) =
            IGNE_BeginNode(nodeId)

        member _.EndNode() =
            IGNE_EndNode()

        member _.SetNodePosition(nodeId, x, y) =
            IGNE_SetNodePosition(nodeId, x, y)

        member _.GetNodePosition(nodeId) =
            let mutable x = 0f
            let mutable y = 0f
            IGNE_GetNodePosition(nodeId, &x, &y)
            x, y

        member _.GetNodeSize(nodeId) =
            let mutable w = 0f
            let mutable h = 0f
            IGNE_GetNodeSize(nodeId, &w, &h)
            w, h

        // ── Pins ──────────────────────────────────────────────────────────────

        member _.BeginPin(pinId, kind) =
            IGNE_BeginPin(pinId, int kind)

        member _.EndPin() =
            IGNE_EndPin()

        member _.PinRect(ax, ay, bx, by) =
            IGNE_PinRect(ax, ay, bx, by)

        member _.PinPivotRect(ax, ay, bx, by) =
            IGNE_PinPivotRect(ax, ay, bx, by)

        // ── Links ─────────────────────────────────────────────────────────────

        member _.Link(linkId, startPinId, endPinId, ?r, ?g, ?b, ?a, ?thickness) =
            IGNE_Link(linkId, startPinId, endPinId,
                      defaultArg r 1f, defaultArg g 1f, defaultArg b 1f, defaultArg a 1f,
                      defaultArg thickness 1f)

        member _.Flow(linkId, ?direction) =
            IGNE_Flow(linkId, int (defaultArg direction FlowDirection.Forward))

        // ── Selection & navigation ────────────────────────────────────────────

        member _.NavigateToContent(?duration) =
            IGNE_NavigateToContent(defaultArg duration -1f)

        member _.NavigateToSelection(?zoomIn, ?duration) =
            IGNE_NavigateToSelection(defaultArg zoomIn false, defaultArg duration -1f)

        member _.IsNodeSelected(nodeId) =
            IGNE_IsNodeSelected(nodeId)

        member _.IsLinkSelected(linkId) =
            IGNE_IsLinkSelected(linkId)

        member _.SelectNode(nodeId, ?append) =
            IGNE_SelectNode(nodeId, defaultArg append false)

        member _.DeselectNode(nodeId) =
            IGNE_DeselectNode(nodeId)

        member _.SelectLink(linkId, ?append) =
            IGNE_SelectLink(linkId, defaultArg append false)

        member _.DeselectLink(linkId) =
            IGNE_DeselectLink(linkId)

        member _.ClearSelection() =
            IGNE_ClearSelection()

        member _.GetSelectedObjectCount() =
            IGNE_GetSelectedObjectCount()

        // ── Create interaction ────────────────────────────────────────────────

        member _.BeginCreate(?r, ?g, ?b, ?a, ?thickness) =
            IGNE_BeginCreate(defaultArg r 1f, defaultArg g 1f, defaultArg b 1f,
                              defaultArg a 1f, defaultArg thickness 1f)

        member _.EndCreate() =
            IGNE_EndCreate()

        member _.QueryNewLink() =
            let mutable s = 0L
            let mutable e = 0L
            let ok = IGNE_QueryNewLink(&s, &e)
            ok, s, e

        member _.QueryNewNode() =
            let mutable p = 0L
            let ok = IGNE_QueryNewNode(&p)
            ok, p

        member _.AcceptNewItem(?r, ?g, ?b, ?a, ?thickness) =
            IGNE_AcceptNewItem(defaultArg r 0.5f, defaultArg g 1f, defaultArg b 0.5f,
                               defaultArg a 1f, defaultArg thickness 1f)

        member _.RejectNewItem(?r, ?g, ?b, ?a, ?thickness) =
            IGNE_RejectNewItem(defaultArg r 1f, defaultArg g 0f, defaultArg b 0f,
                               defaultArg a 1f, defaultArg thickness 2f)

        // ── Delete interaction ────────────────────────────────────────────────

        member _.BeginDelete() =
            IGNE_BeginDelete()

        member _.EndDelete() =
            IGNE_EndDelete()

        member _.QueryDeletedLink() =
            let mutable lid = 0L
            let mutable s   = 0L
            let mutable e   = 0L
            let ok = IGNE_QueryDeletedLink(&lid, &s, &e)
            ok, lid, s, e

        member _.QueryDeletedNode() =
            let mutable n = 0L
            let ok = IGNE_QueryDeletedNode(&n)
            ok, n

        member _.AcceptDeletedItem(?deleteDependencies) =
            IGNE_AcceptDeletedItem(defaultArg deleteDependencies true)

        member _.RejectDeletedItem() =
            IGNE_RejectDeletedItem()

        // ── Suspend / Resume ──────────────────────────────────────────────────

        member _.Suspend() =
            IGNE_Suspend()

        member _.Resume() =
            IGNE_Resume()

        // ── Utility ─────────────────────────────────────────────────────

        member _.DeleteNode(nodeId) =
            IGNE_DeleteNode(nodeId)

        member _.DeleteLink(linkId) =
            IGNE_DeleteLink(linkId)

