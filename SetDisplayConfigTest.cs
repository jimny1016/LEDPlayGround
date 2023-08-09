using System;
using System.Runtime.InteropServices;

class SetDisplayConfigTest
{
    [DllImport("user32.dll")]
    public static extern int SetDisplayConfig(uint numPathArrayElements, IntPtr pathArray, uint numModeInfoArrayElements, IntPtr modeInfoArray, uint flags);

    [Flags]
    public enum DisplayConfigFlags: uint
    {
        SDC_TOPOLOGY_INTERNAL = 0x00000001,
        SDC_TOPOLOGY_CLONE = 0x00000002,
        SDC_TOPOLOGY_EXTEND = 0x00000004,
        SDC_TOPOLOGY_EXTERNAL = 0x00000008,
        SDC_APPLY = 0x00000080,
        SDC_SAVE_TO_DATABASE = 0x00001000,
        SDC_ALLOW_CHANGES = 0x00040000,
        SDC_USE_SUPPLIED_DISPLAY_CONFIG = 0x00000020
    }

    static SetDisplayConfigTest()
    {
        //DisplayConfigFlags.SDC_TOPOLOGY_INTERNAL
        // To switch to extended mode (extended display)
        //SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, (uint)(DisplayConfigFlags.SDC_TOPOLOGY_EXTERNAL | DisplayConfigFlags.SDC_APPLY));
        SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, (uint)DisplayConfigFlags.SDC_APPLY);
        // To switch to clone mode (mirrored display)
        // SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, (uint)(DisplayConfigFlags.SDC_TOPOLOGY_CLONE | DisplayConfigFlags.SDC_APPLY));
    }
}