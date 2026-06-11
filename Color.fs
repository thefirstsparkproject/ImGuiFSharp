namespace ImGuiFSharp

open System
open System.Globalization


/// A fast, lightweight struct representing an RGBA color using normalized float32 channels.
[<Struct>]
type Color = { R: float32; G: float32; B: float32; A: float32 } with

    /// The default color: opaque white.
    static member Default =
        { R = 1.0f; G = 1.0f; B = 1.0f; A = 1.0f }

    static member Create(r: float32, g: float32, b: float32) =
        { R = r; G = g; B = b; A = 1.0f }

    static member Create(r: float32, g: float32, b: float32, a: float32) =
        { R = r; G = g; B = b; A = a }

    static member Create(r: float, g: float, b: float) =
        Color.Create(float32 r, float32 g, float32 b)

    static member Create(r: float, g: float, b: float, a: float) =
        Color.Create(float32 r, float32 g, float32 b, float32 a)

    static member Create(r: byte, g: byte, b: byte) =
        Color.Create(float32 r / 255.0f, float32 g / 255.0f, float32 b / 255.0f)

    static member Create(r: byte, g: byte, b: byte, a: byte) =
        Color.Create(float32 r / 255.0f, float32 g / 255.0f, float32 b / 255.0f, float32 a / 255.0f)

    /// Creates a Color from a 24-bit RGB integer formatted as 0xRRGGBB.
    static member RgbFrom(value: uint32) =
        Color.Create(
            byte ((value >>> 16) &&& 0xFFu),
            byte ((value >>> 8) &&& 0xFFu),
            byte (value &&& 0xFFu))

    /// Creates a Color from a 32-bit RGBA integer formatted as 0xRRGGBBAA.
    static member RgbaFrom(value: uint32) =
        Color.Create(
            byte ((value >>> 24) &&& 0xFFu),
            byte ((value >>> 16) &&& 0xFFu),
            byte ((value >>> 8) &&& 0xFFu),
            byte (value &&& 0xFFu))

    /// Parses "#RRGGBB", "RRGGBB", "#RRGGBBAA", or "RRGGBBAA".
    static member TryCreate(hex: string) : Result<Color, string> =
        if String.IsNullOrWhiteSpace hex then
            Error "Hex color cannot be empty."
        else
            let span =
                let trimmed = hex.AsSpan().Trim()
                if trimmed.Length > 0 && trimmed[0] = '#' then
                    trimmed.Slice(1)
                else
                    trimmed

            if span.Length <> 6 && span.Length <> 8 then
                Error "Hex color must be 6 or 8 hexadecimal characters."
            else
                let mutable value = 0u

                if UInt32.TryParse(span, NumberStyles.HexNumber, CultureInfo.InvariantCulture, &value) then
                    if span.Length = 6 then
                        Ok(Color.RgbFrom value)
                    else
                        Ok(Color.RgbaFrom value)
                else
                    Error "Hex color contains invalid hexadecimal characters."

    /// Parses a hex color string or raises ArgumentException on invalid input.
    static member Create(hex: string) =
        match Color.TryCreate hex with
        | Ok color -> color
        | Error message -> invalidArg (nameof hex) message

    /// Packs the color into a uint32 formatted as 0xRRGGBBAA.
    member this.ToUint32() =
        let toUint (f: float32) =
            let v = f * 255.0f + 0.5f
            if v <= 0.0f then 0u
            elif v >= 255.0f then 255u
            else uint32 v

        (toUint this.R <<< 24)
        ||| (toUint this.G <<< 16)
        ||| (toUint this.B <<< 8)
        ||| toUint this.A

    /// Converts the color to an HTML hex string: "#RRGGBB" or "#RRGGBBAA".
    member this.ToHtml() =
        let toByte (f: float32) =
            let v = f * 255.0f + 0.5f
            if v <= 0.0f then 0uy
            elif v >= 255.0f then 255uy
            else byte v

        let r = toByte this.R
        let g = toByte this.G
        let b = toByte this.B
        let a = toByte this.A

        if a = 255uy then
            $"#{r:X2}{g:X2}{b:X2}"
        else
            $"#{r:X2}{g:X2}{b:X2}{a:X2}"

/// Pre-parsed CSS named colors.
module Colors =
    let private get hex = Color.Create hex

    let transparent = Color.Create(0uy, 0uy, 0uy, 0uy)

    let aliceBlue = get "F0F8FF"
    let antiqueWhite = get "FAEBD7"
    let aqua = get "00FFFF"
    let aquamarine = get "7FFFD4"
    let azure = get "F0FFFF"
    let beige = get "F5F5DC"
    let bisque = get "FFE4C4"
    let black = get "000000"
    let blanchedAlmond = get "FFEBCD"
    let blue = get "0000FF"
    let blueViolet = get "8A2BE2"
    let brown = get "A52A2A"
    let burlyWood = get "DEB887"
    let cadetBlue = get "5F9EA0"
    let chartreuse = get "7FFF00"
    let chocolate = get "D2691E"
    let coral = get "FF7F50"
    let cornflowerBlue = get "6495ED"
    let cornsilk = get "FFF8DC"
    let crimson = get "DC143C"
    let cyan = get "00FFFF"
    let darkBlue = get "00008B"
    let darkCyan = get "008B8B"
    let darkGoldenrod = get "B8860B"
    let darkGoldenRod = darkGoldenrod
    let darkGray = get "A9A9A9"
    let darkGrey = darkGray
    let darkGreen = get "006400"
    let darkKhaki = get "BDB76B"
    let darkMagenta = get "8B008B"
    let darkOliveGreen = get "556B2F"
    let darkOrange = get "FF8C00"
    let darkOrchid = get "9932CC"
    let darkRed = get "8B0000"
    let darkSalmon = get "E9967A"
    let darkSeaGreen = get "8FBC8F"
    let darkSlateBlue = get "483D8B"
    let darkSlateGray = get "2F4F4F"
    let darkSlateGrey = darkSlateGray
    let darkTurquoise = get "00CED1"
    let darkViolet = get "9400D3"
    let deepPink = get "FF1493"
    let deepSkyBlue = get "00BFFF"
    let dimGray = get "696969"
    let dimGrey = dimGray
    let dodgerBlue = get "1E90FF"
    let fireBrick = get "B22222"
    let floralWhite = get "FFFAF0"
    let forestGreen = get "228B22"
    let fuchsia = get "FF00FF"
    let gainsboro = get "DCDCDC"
    let ghostWhite = get "F8F8FF"
    let gold = get "FFD700"
    let goldenrod = get "DAA520"
    let goldenRod = goldenrod
    let gray = get "808080"
    let grey = gray
    let green = get "008000"
    let greenYellow = get "ADFF2F"
    let honeydew = get "F0FFF0"
    let honeyDew = honeydew
    let hotPink = get "FF69B4"
    let indianRed = get "CD5C5C"
    let indigo = get "4B0082"
    let ivory = get "FFFFF0"
    let khaki = get "F0E68C"
    let lavender = get "E6E6FA"
    let lavenderBlush = get "FFF0F5"
    let lawnGreen = get "7CFC00"
    let lemonChiffon = get "FFFACD"
    let lightBlue = get "ADD8E6"
    let lightCoral = get "F08080"
    let lightCyan = get "E0FFFF"
    let lightGoldenrodYellow = get "FAFAD2"
    let lightGoldenRodYellow = lightGoldenrodYellow
    let lightGray = get "D3D3D3"
    let lightGrey = lightGray
    let lightGreen = get "90EE90"
    let lightPink = get "FFB6C1"
    let lightSalmon = get "FFA07A"
    let lightSeaGreen = get "20B2AA"
    let lightSkyBlue = get "87CEFA"
    let lightSlateGray = get "778899"
    let lightSlateGrey = lightSlateGray
    let lightSteelBlue = get "B0C4DE"
    let lightYellow = get "FFFFE0"
    let lime = get "00FF00"
    let limeGreen = get "32CD32"
    let linen = get "FAF0E6"
    let magenta = get "FF00FF"
    let maroon = get "800000"
    let mediumAquamarine = get "66CDAA"
    let mediumAquaMarine = mediumAquamarine
    let mediumBlue = get "0000CD"
    let mediumOrchid = get "BA55D3"
    let mediumPurple = get "9370DB"
    let mediumSeaGreen = get "3CB371"
    let mediumSlateBlue = get "7B68EE"
    let mediumSpringGreen = get "00FA9A"
    let mediumTurquoise = get "48D1CC"
    let mediumVioletRed = get "C71585"
    let midnightBlue = get "191970"
    let mintCream = get "F5FFFA"
    let mistyRose = get "FFE4E1"
    let moccasin = get "FFE4B5"
    let navajoWhite = get "FFDEAD"
    let navy = get "000080"
    let oldLace = get "FDF5E6"
    let olive = get "808000"
    let oliveDrab = get "6B8E23"
    let orange = get "FFA500"
    let orangeRed = get "FF4500"
    let orchid = get "DA70D6"
    let paleGoldenrod = get "EEE8AA"
    let paleGoldenRod = paleGoldenrod
    let paleGreen = get "98FB98"
    let paleTurquoise = get "AFEEEE"
    let paleVioletRed = get "DB7093"
    let papayaWhip = get "FFEFD5"
    let peachPuff = get "FFDAB9"
    let peru = get "CD853F"
    let pink = get "FFC0CB"
    let plum = get "DDA0DD"
    let powderBlue = get "B0E0E6"
    let purple = get "800080"
    let rebeccaPurple = get "663399"
    let red = get "FF0000"
    let rosyBrown = get "BC8F8F"
    let royalBlue = get "4169E1"
    let saddleBrown = get "8B4513"
    let salmon = get "FA8072"
    let sandyBrown = get "F4A460"
    let seaGreen = get "2E8B57"
    let seashell = get "FFF5EE"
    let seaShell = seashell
    let sienna = get "A0522D"
    let silver = get "C0C0C0"
    let skyBlue = get "87CEEB"
    let slateBlue = get "6A5ACD"
    let slateGray = get "708090"
    let slateGrey = slateGray
    let snow = get "FFFAFA"
    let springGreen = get "00FF7F"
    let steelBlue = get "4682B4"
    let tan = get "D2B48C"
    let teal = get "008080"
    let thistle = get "D8BFD8"
    let tomato = get "FF6347"
    let turquoise = get "40E0D0"
    let violet = get "EE82EE"
    let wheat = get "F5DEB3"
    let white = get "FFFFFF"
    let whiteSmoke = get "F5F5F5"
    let yellow = get "FFFF00"
    let yellowGreen = get "9ACD32"
    
    
    