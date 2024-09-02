using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace ComputedConverters;

[ValueConversion(typeof(byte[]), typeof(string))]
public sealed class ByteArrayToHexStringConverter : SingletonValueConverterBase<ByteArrayToHexStringConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is byte[] byteArray)
        {
            return ToHexString(byteArray);
        }

        return null;
    }

    public override object? ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture)
    {
        if (value is string base64String)
        {
            return FromHexString(base64String);
        }

        return null;
    }

    public static string ToHexString(byte[] bytes)
    {
        if (bytes.Length == 0)
        {
            return string.Empty;
        }

        return HexConverter.ToString(bytes, HexConverter.Casing.Upper);
    }

    public static byte[] FromHexString(string @string)
    {
        char[] chars = @string?.ToCharArray() ?? [];

        if (chars.Length == 0)
        {
            return [];
        }

        if ((uint)chars.Length % 2 != 0)
        {
            throw new FormatException("The input is not a valid hex string as its length is not a multiple of 2.");
        }

        byte[] result = new byte[chars.Length >> 1];

        if (!HexConverter.TryDecodeFromUtf16(chars, result, out _))
        {
            throw new FormatException("The input is not a valid hex string as it contains a non-hex character.");
        }

        return result;
    }
}

file static class HexConverter
{
    public enum Casing : uint
    {
        Upper = 0,
        Lower = 0x2020U,
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ToCharsBuffer(byte value, char[] buffer, int startingIndex = 0, Casing casing = Casing.Upper)
    {
        uint difference = (((uint)value & 0xF0U) << 4) + ((uint)value & 0x0FU) - 0x8989U;
        uint packedResult = ((((uint)(-(int)difference) & 0x7070U) >> 4) + difference + 0xB9B9U) | (uint)casing;

        buffer[startingIndex + 1] = (char)(packedResult & 0xFF);
        buffer[startingIndex] = (char)(packedResult >> 8);
    }

    public static string ToString(byte[] bytes, Casing casing = Casing.Upper)
    {
        char[] result = new char[bytes.Length * 2];

        int pos = 0;
        foreach (byte b in bytes)
        {
            ToCharsBuffer(b, result, pos, casing);
            pos += 2;
        }
        return result.ToString()!;
    }

    public static bool TryDecodeFromUtf16(char[] chars, byte[] bytes, out int charsProcessed)
    {
        return TryDecodeFromUtf16_Scalar(chars, bytes, out charsProcessed);
    }

    private static bool TryDecodeFromUtf16_Scalar(char[] chars, byte[] bytes, out int charsProcessed)
    {
        Debug.Assert(chars.Length % 2 == 0, "Un-even number of characters provided");
        Debug.Assert(chars.Length / 2 == bytes.Length, "Target buffer not right-sized for provided characters");

        int i = 0;
        int j = 0;
        int byteLo = 0;
        int byteHi = 0;
        while (j < bytes.Length)
        {
            byteLo = FromChar(chars[i + 1]);
            byteHi = FromChar(chars[i]);

            if ((byteLo | byteHi) == 0xFF)
            {
                break;
            }

            bytes[j++] = (byte)((byteHi << 4) | byteLo);
            i += 2;
        }

        if (byteLo == 0xFF)
        {
            i++;
        }

        charsProcessed = i;
        return (byteLo | byteHi) != 0xFF;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FromChar(int c)
    {
        return c >= CharToHexLookup.Length ? 0xFF : CharToHexLookup[c];
    }

    public static byte[] CharToHexLookup =>
    [
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0x0,  0x1,  0x2,  0x3,  0x4,  0x5,  0x6,  0x7,  0x8,  0x9,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xA,  0xB,  0xC,  0xD,  0xE,  0xF,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xa,  0xb,  0xc,  0xd,  0xe,  0xf,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
    ];
}
