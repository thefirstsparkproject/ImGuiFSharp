

Make sure to use F# 10 language standard, and .NET 10.

Read:
- @cpp/imgui/imgui.h : NativeImGuiLib
- @cpp/bridge/imgui_bridge.h : NativeBridg
- @internal/ImGuiInternal.fs
- @Flags.fs
- @Enums.fs
- @Gui.fs
- @Builder.fs

Change it make add feature from the ImGui library, add Features to handle the string type in text input, resizable, and remove the char[] version.
Also add Clipper feature for visual scrolling table those should be easy to add in the Gui.fs, or Builder.fs and integreate perfectly.

The goal is to use this library to create apps (mostly data heavy, research, trading, visualisation, scripting apps), then add the features you find possibly missing for achieving this goal who are not exposed in the NativeBrige, imgui_bridge.h. And integrate them in the high level fs construction.

If you find more readable to split the bridge into multiple files do it.
If you need to add Flags or Enums add them in the same manner as in Flags.fs and Enums.fs.

If you need to expose callbacks to the public Gui.fs, Builder.fs make them present as a F# function (fun ...  -> ... ).

NativeImGuiLib (read but dont change) -> NativeBridge (from here up you will change) -> ImGuiInternal  -> Gui -> Builder.

Keep the NodeEditor/Plot/Plot3D part roughly the same (minor change implementation details dont matter).

When you can make certain features hidden in Gui.fs do it.
Like TextInput should take f# resizable string only, high level type only (array, seq, ResizeArray, string, avoid pointers in Gui.fs and up).
If you need to move pinvoke in another file create ImGuiNativeInterface.fs in the internal folder, the pinvoke are there, if it is the case ImGuiInternal.fs just wrap them into higher level contruction F# 10, .Net 10.


After that update the README.md.