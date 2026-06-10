namespace ImGuiFSharp

open System

/// Represents the declaration of a single window in a multi-window application
type WindowDefinition = {
    Title  : string
    Config : WindowConfig
    Draw   : unit -> unit
}

/// Builder for nested window declarations
type AppWindowBuilder(title: string, cfg: WindowConfig) =
    member _.Zero() = fun () -> ()
    member _.Yield(expr: unit) = fun () -> expr
    member _.Delay(f: unit -> unit -> unit) = fun () -> f() ()
    member _.Combine(a: unit -> unit, b: unit -> unit) = fun () -> a(); b()
    member _.Run(f: unit -> unit) : WindowDefinition =
        { Title = title; Config = cfg; Draw = f }

/// Builder for the multi-window application
type ApplicationBuilder() =
    member _.Zero() = []
    member _.Yield(w: WindowDefinition) = [w]
    member _.Delay(f: unit -> WindowDefinition list) = f()
    member _.Combine(a: WindowDefinition list, b: WindowDefinition list) = a @ b
    member _.Run(wins: WindowDefinition list) = new Application(wins)

/// Manages concurrent execution of multiple GLFW/OpenGL/ImGui windows
and [<Sealed>] Application(windows: WindowDefinition list) =
    member _.Run() =
        // 1. Initialize all native windows as mutable options
        let nativeWins =
            windows
            |> List.map (fun def ->
                let win = new ImGuiFSharp.Window(def.Config.Width, def.Config.Height, def.Title, def.Config.Resizable)
                win.VSync <- def.Config.VSync
                win.RegisterDraw(fun _ -> def.Draw())
                ref (Some win), def.Config
            )

        try
            // 2. Loop concurrently until all windows are closed
            let mutable running = true
            while running do
                ImGuiFSharp.Window.PollEvents()
                let mutable anyActive = false
                for winRef, config in nativeWins do
                    match winRef.Value with
                    | Some win ->
                        if win.ShouldClose then
                            (win :> IDisposable).Dispose()
                            winRef.Value <- None
                        else
                            anyActive <- true
                            win.MakeCurrent()
                            win.Step(config.ClearR, config.ClearG, config.ClearB)
                    | None -> ()
                running <- anyActive
        finally
            // 3. Clean up any remaining windows
            for winRef, _ in nativeWins do
                match winRef.Value with
                | Some win ->
                    try (win :> IDisposable).Dispose() with _ -> ()
                    winRef.Value <- None
                | None -> ()
            EditorContextCache.destroyAll()

type window(title: string, ?cfg: WindowConfig) =
    inherit AppWindowBuilder(title, defaultArg cfg WindowConfig.Default)

type appWindow(title: string, ?cfg: WindowConfig) =
    inherit AppWindowBuilder(title, defaultArg cfg WindowConfig.Default)

[<AutoOpen>]
module ApplicationBuilderModule =
    let application = ApplicationBuilder()
