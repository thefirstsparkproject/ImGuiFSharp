namespace ImGuiFSharp

open System
open Microsoft.FSharp.Core

/// F# helper that orchestrates the ImGui lifecycle, decoupled from Godot.
type ImGuiInstance(backend: IImGuiBackend) =

    let impl = ImGuiImpl()
    let mutable context = 0n

    // ── Public interface accessors (cast F# type to the declared interface) ──
    member _.GuiImpl    : IGuiFunctions    = impl :> IGuiFunctions
    member _.PlotImpl   : IPlotFunctions   = impl :> IPlotFunctions
    member _.Plot3DImpl : IPlot3DFunctions = impl :> IPlot3DFunctions
    member _.FontsImpl  : IFontFunctions   = impl :> IFontFunctions

    // Full-screen window settings
    member val FullScreenWindow = false with get, set
    member val FullScreenWindowTitle = "ImGuiWindow" with get, set
    member val FullScreenWindowNoHeader = false with get, set

    /// Activates this manager's native ImGui context
    member _.ActivateContext() =
        if context <> 0n then
            ImGuiNative.IGN_SetCurrentContext(context)

    /// Constrains all floating windows to the visible viewport boundaries
    member _.MoveWindowsToVisibleRange() =
        if context <> 0n then
            ImGuiNative.IGN_SetCurrentContext(context)
            ImGuiNative.IGN_MoveWindowsToVisibleRange()

    /// Call once to initialize the ImGui context and backend
    member _.Initialize() =
        context <- ImGuiNative.IGN_CreateContext()
        ImGuiNative.IGN_SetCurrentContext(context)
        backend.Initialize()

    /// Performs the full frame processing loop: starting a frame, invoking the builder, and rendering
    member this.Process(delta: float, w: float32, h: float32, builder: IGuiBuilder) =
        if context <> 0n then
            ImGuiNative.IGN_SetCurrentContext(context)
        backend.SetDisplaySize(w, h)
        ImGuiNative.IGN_SetDisplaySize(w, h)
        ImGuiNative.IGN_SetDeltaTime(float32 delta)
        backend.NewFrame(float32 delta)
        ImGuiNative.IGN_NewFrame()

        let api = {
            Gui = this.GuiImpl
            Plot = this.PlotImpl
            Plot3D = this.Plot3DImpl
            Fonts = this.FontsImpl
        }

        if this.FullScreenWindow then
            this.GuiImpl.SetNextWindowPos(0f, 0f)
            this.GuiImpl.SetNextWindowSize(w, h)
            
            let flags = 
                if this.FullScreenWindowNoHeader then
                    (1 <<< 0) ||| (1 <<< 1) ||| (1 <<< 2) ||| (1 <<< 5) ||| (1 <<< 13) ||| (1 <<< 17)
                else
                    (1 <<< 1) ||| (1 <<< 2) ||| (1 <<< 5)
            
            if this.GuiImpl.Begin(this.FullScreenWindowTitle, ?flags = Some flags) then
                builder.OnGui(api)
                this.GuiImpl.End()
        else
            builder.OnGui(api)

        ImGuiNative.IGN_Render()
        let drawData = ImGuiNative.IGN_GetDrawData()
        backend.Render(drawData)

    interface System.IDisposable with
        member this.Dispose() =
            if context <> 0n then
                ImGuiNative.IGN_SetCurrentContext(context)
            backend.Destroy()
            if context <> 0n then
                ImGuiNative.IGN_DestroyContext(context)
                context <- 0n
