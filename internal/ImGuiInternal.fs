namespace ImGuiFSharp

open System
open System.Runtime.InteropServices
open System.Text

// ══════════════════════════════════════════════════════════════════════════════
// F# helpers sitting above the raw PInvoke layer.
// PInvoke declarations live in ImGuiNativeInterface.fs (module ImGuiNative).
// ══════════════════════════════════════════════════════════════════════════════

/// Pin a bool ref across a native call that may flip it via a byte pointer.
module internal BoolPtr =
    let inline withRef (r: bool ref) (f: nativeint -> 'a) =
        let arr = [| (if r.Value then 1uy else 0uy) |]
        let h = GCHandle.Alloc(arr, GCHandleType.Pinned)
        try
            let result = f (h.AddrOfPinnedObject())
            r.Value <- arr[0] <> 0uy
            result
        finally h.Free()

    let inline withOptRef (r: bool ref option) (f: nativeint -> 'a) =
        match r with
        | None   -> f 0n
        | Some r -> withRef r f

// ══════════════════════════════════════════════════════════════════════════════
// Managed heap buffer for resizable InputText.
//
// The native side uses malloc/realloc (via the CallbackResize path). We give it
// a GC-pinned native buffer and copy back to the F# string ref on each change.
//
// Usage (internal):
//   use sb = new StringBuffer(initialValue, minCapacity = 256)
//   let changed = ImGuiNative.IGN_InputText_String(label, &sb.Ptr, &sb.Len, &sb.Cap, flags)
//   if changed then value.Value <- sb.Read()
// ══════════════════════════════════════════════════════════════════════════════
[<Sealed>]
type internal StringBuffer(initial: string, minCapacity: int) =
    // Start with enough room; the C++ side will realloc if more is needed.
    let initBytes = Encoding.UTF8.GetBytes(initial)
    let initCap   = max minCapacity (initBytes.Length + 64)
    // Allocate unmanaged so realloc in C++ is safe.
    let mutable ptr = Marshal.AllocHGlobal(initCap)
    let mutable len = initBytes.Length
    let mutable cap = initCap

    do
        Marshal.Copy(initBytes, 0, ptr, initBytes.Length)
        Marshal.WriteByte(ptr + nativeint initBytes.Length, 0uy)  // null-terminator

    /// Pointer to the buffer — passed by ref so C++ realloc can update it.
    member _.Ptr with get() : nativeint = ptr and set v = ptr <- v
    member _.Len with get() : int       = len and set v = len <- v
    member _.Cap with get() : int       = cap and set v = cap <- v

    /// Read the current UTF-8 buffer back as a .NET string.
    member _.Read() =
        if ptr = 0n then ""
        else
            // Find actual null-terminated length (may be shorter than len after edits)
            let mutable n = 0
            while n < cap && Marshal.ReadByte(ptr + nativeint n) <> 0uy do n <- n + 1
            let bytes = Array.zeroCreate<byte> n
            Marshal.Copy(ptr, bytes, 0, n)
            Encoding.UTF8.GetString(bytes)

    interface IDisposable with
        member _.Dispose() =
            if ptr <> 0n then
                Marshal.FreeHGlobal(ptr)
                ptr <- 0n
