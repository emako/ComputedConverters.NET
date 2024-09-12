using System.Runtime.InteropServices;

namespace ComputedConverters;

internal static class Interop
{
    public static class Gdi32
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(nint hObject);
    }
}
