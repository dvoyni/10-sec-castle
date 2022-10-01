using System.Runtime.InteropServices;

namespace TenSecCastle {
    [StructLayout(LayoutKind.Explicit)]
    public struct Msg {
        public Msg(Screen screen) : this() {
            Screen = screen;
        }

        [FieldOffset(0)] public readonly Screen Screen;
        [FieldOffset(4)] public Game.Msg GameMsg;
    }
}