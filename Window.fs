namespace ImGuiFSharp

open System
open System.Runtime.InteropServices
open ImGuiFSharp.WindowInternal

type public Window(width: int, height: int, title: string, [<Optional; DefaultParameterValue(true)>] resizable: bool) =
    let handle = IGN_Window_Create(width, height, title, resizable)
    
    let guiEvent = Event<Window>()
    let resizeEvent = Event<int * int>()
    let dropEvent = Event<string[]>()
    let focusEvent = Event<bool>()
    
    let mutable drawFunc: (Window -> unit) option = None
    let mutable closeCheck: (unit -> bool) option = None
    
    // Keep delegates alive to prevent Garbage Collection
    let mutable resizeDel: IGN_ResizeCallback option = None
    let mutable dropDel: IGN_DropCallback option = None
    let mutable focusDel: IGN_FocusCallback option = None
    let mutable closeDel: IGN_CloseCallback option = None

    let onResize = IGN_ResizeCallback(fun win w h ->
        resizeEvent.Trigger(w, h)
    )
    
    let onDrop = IGN_DropCallback(fun win count ptr ->
        let arr = Array.zeroCreate<string> count
        for i in 0 .. count - 1 do
            let strPtr = Marshal.ReadIntPtr(ptr, i * IntPtr.Size)
            let s = Marshal.PtrToStringUTF8(strPtr)
            arr[i] <- Option.ofObj s |> Option.defaultValue ""
        dropEvent.Trigger(arr)
    )
    
    let onFocus = IGN_FocusCallback(fun win focused ->
        focusEvent.Trigger(focused)
    )
    
    let onClose = IGN_CloseCallback(fun win ->
        match closeCheck with
        | Some f -> f()
        | None -> true
    )

    do
        if handle = IntPtr.Zero then failwith "Failed to create window"
        
        resizeDel <- Some onResize
        dropDel <- Some onDrop
        focusDel <- Some onFocus
        closeDel <- Some onClose
        
        IGN_Window_SetResizeCallback(handle, onResize)
        IGN_Window_SetDropCallback(handle, onDrop)
        IGN_Window_SetFocusCallback(handle, onFocus)
        IGN_Window_SetCloseCallback(handle, onClose)

    member this.Handle = handle
    
    // Switch active contexts (GLFW context + ImGuiContext + NodeEditorContext)
    member this.MakeCurrent() =
        IGN_Window_MakeCurrent(handle)

    // Events
    member this.OnGui = guiEvent.Publish
    member this.Resize = resizeEvent.Publish
    member this.Drop = dropEvent.Publish
    member this.Focus = focusEvent.Publish

    // Callbacks & loop control
    member this.RegisterDraw(f: Window -> unit) = drawFunc <- Some f
    member this.RegisterCloseCheck(f: unit -> bool) = closeCheck <- Some f

    // Window properties
    member this.Size
        with get() =
            let mutable w, h = 0, 0
            IGN_Window_GetSize(handle, &w, &h)
            (w, h)
        and set(w, h) =
            IGN_Window_SetSize(handle, w, h)

    member this.Position
        with get() =
            let mutable x, y = 0, 0
            IGN_Window_GetPosition(handle, &x, &y)
            (x, y)
        and set(x, y) =
            IGN_Window_SetPosition(handle, x, y)

    member this.Title
        with set(value: string) =
            IGN_Window_SetTitle(handle, value)

    member this.VSync
        with set(value: bool) =
            IGN_Window_SetVSync(handle, value)

    member this.IsFocused = IGN_Window_IsFocused(handle)

    member this.ShouldClose
        with get() = IGN_Window_ShouldClose(handle)
        and set(value: bool) = IGN_Window_SetShouldClose(handle, value)

    static member PollEvents() =
        IGN_Window_PollEvents()

    member this.Run([<Optional; DefaultParameterValue(0.15f)>] clearR: float32,
                    [<Optional; DefaultParameterValue(0.16f)>] clearG: float32,
                    [<Optional; DefaultParameterValue(0.18f)>] clearB: float32) =
        while not (IGN_Window_ShouldClose(handle)) do
            IGN_Window_PollEvents()
            IGN_Window_NewFrame(handle)
            
            // Execute drawing inside active context
            drawFunc |> Option.iter (fun f -> f this)
            guiEvent.Trigger(this)
            
            IGN_Window_Render(handle, clearR, clearG, clearB, 1.0f)

    member this.Step([<Optional; DefaultParameterValue(0.15f)>] clearR: float32,
                     [<Optional; DefaultParameterValue(0.16f)>] clearG: float32,
                     [<Optional; DefaultParameterValue(0.18f)>] clearB: float32) =
        if not (IGN_Window_ShouldClose(handle)) then
            IGN_Window_NewFrame(handle)
            drawFunc |> Option.iter (fun f -> f this)
            guiEvent.Trigger(this)
            IGN_Window_Render(handle, clearR, clearG, clearB, 1.0f)

    interface IDisposable with
        member this.Dispose() =
            if handle <> IntPtr.Zero then
                IGN_Window_Destroy(handle)
