namespace ImGuiFSharp

open System
open System.Runtime.InteropServices

[<UnmanagedFunctionPointer(CallingConvention.Winapi)>]
type IGN_ResizeCallback = delegate of window: nativeint * w: int * h: int -> unit

[<UnmanagedFunctionPointer(CallingConvention.Winapi)>]
type IGN_DropCallback = delegate of window: nativeint * count: int * paths: nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Winapi)>]
type IGN_FocusCallback = delegate of window: nativeint * [<MarshalAs(UnmanagedType.I1)>] focused: bool -> unit

[<UnmanagedFunctionPointer(CallingConvention.Winapi)>]
type IGN_CloseCallback = delegate of window: nativeint -> [<MarshalAs(UnmanagedType.I1)>] bool

module public WindowInternal =
    [<Literal>]
    let LibName = "ImGuiNative"

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern nativeint IGN_Window_Create(int width, int height, [<MarshalAs(UnmanagedType.LPUTF8Str)>] string title, [<MarshalAs(UnmanagedType.I1)>] bool resizable)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_Destroy(nativeint handle)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Window_ShouldClose(nativeint handle)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_SetShouldClose(nativeint handle, [<MarshalAs(UnmanagedType.I1)>] bool shouldClose)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_PollEvents()

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_MakeCurrent(nativeint handle)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_NewFrame(nativeint handle)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_Render(nativeint handle, float32 clearR, float32 clearG, float32 clearB, float32 clearA)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_GetSize(nativeint handle, int& w, int& h)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_SetSize(nativeint handle, int w, int h)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_GetPosition(nativeint handle, int& x, int& y)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_SetPosition(nativeint handle, int x, int y)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_SetTitle(nativeint handle, [<MarshalAs(UnmanagedType.LPUTF8Str)>] string title)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_SetVSync(nativeint handle, [<MarshalAs(UnmanagedType.I1)>] bool enable)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern [<MarshalAs(UnmanagedType.I1)>] bool IGN_Window_IsFocused(nativeint handle)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_SetResizeCallback(nativeint handle, IGN_ResizeCallback cb)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_SetDropCallback(nativeint handle, IGN_DropCallback cb)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_SetFocusCallback(nativeint handle, IGN_FocusCallback cb)

    [<DllImport(LibName, CallingConvention = CallingConvention.Cdecl)>]
    extern void IGN_Window_SetCloseCallback(nativeint handle, IGN_CloseCallback cb)
