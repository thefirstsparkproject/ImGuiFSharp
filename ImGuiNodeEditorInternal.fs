module internal ImGuiFSharp.NodeEditorNative

open System.Runtime.InteropServices

[<Literal>]
let private lib = "ImGuiNative"

// ── Context ───────────────────────────────────────────────────────────────────

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern nativeint IGNE_CreateEditor(string settingsFile)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_DestroyEditor(nativeint ctx)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_SetCurrentEditor(nativeint ctx)

// ── Frame ─────────────────────────────────────────────────────────────────────

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_Begin(string id, float32 w, float32 h)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_End()

// ── Nodes ─────────────────────────────────────────────────────────────────────

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_BeginNode(int64 nodeId)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_EndNode()

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_SetNodePosition(int64 nodeId, float32 x, float32 y)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_GetNodePosition(int64 nodeId, float32& x, float32& y)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_GetNodeSize(int64 nodeId, float32& w, float32& h)

// ── Pins ──────────────────────────────────────────────────────────────────────

/// kind: 0 = Input, 1 = Output
[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_BeginPin(int64 pinId, int kind)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_EndPin()

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_PinRect(float32 ax, float32 ay, float32 bx, float32 by)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_PinPivotRect(float32 ax, float32 ay, float32 bx, float32 by)

// ── Links ─────────────────────────────────────────────────────────────────────

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_Link(int64 linkId, int64 startPinId, int64 endPinId,
                       float32 r, float32 g, float32 b, float32 a, float32 thickness)

/// direction: 0 = Forward, 1 = Backward
[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_Flow(int64 linkId, int direction)

// ── Selection & navigation ────────────────────────────────────────────────────

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_NavigateToContent(float32 duration)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_NavigateToSelection(bool zoomIn, float32 duration)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_IsNodeSelected(int64 nodeId)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_IsLinkSelected(int64 linkId)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_SelectNode(int64 nodeId, bool append)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_DeselectNode(int64 nodeId)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_SelectLink(int64 linkId, bool append)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_DeselectLink(int64 linkId)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_ClearSelection()

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern int IGNE_GetSelectedObjectCount()

// ── Create interaction ────────────────────────────────────────────────────────

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_BeginCreate(float32 r, float32 g, float32 b, float32 a, float32 thickness)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_EndCreate()

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_QueryNewLink(int64& startPinId, int64& endPinId)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_QueryNewNode(int64& pinId)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_AcceptNewItem(float32 r, float32 g, float32 b, float32 a, float32 thickness)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_RejectNewItem(float32 r, float32 g, float32 b, float32 a, float32 thickness)

// ── Delete interaction ────────────────────────────────────────────────────────

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_BeginDelete()

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_EndDelete()

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_QueryDeletedLink(int64& linkId, int64& startPinId, int64& endPinId)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_QueryDeletedNode(int64& nodeId)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_AcceptDeletedItem(bool deleteDependencies)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_RejectDeletedItem()

// ── Suspend / Resume ──────────────────────────────────────────────────────────

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_Suspend()

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern unit IGNE_Resume()

// ── Utility ───────────────────────────────────────────────────────────────────

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_DeleteNode(int64 nodeId)

[<DllImport(lib, CallingConvention = CallingConvention.Cdecl)>]
extern bool IGNE_DeleteLink(int64 linkId)
