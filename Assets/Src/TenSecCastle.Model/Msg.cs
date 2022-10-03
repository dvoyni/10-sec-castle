using System.Runtime.InteropServices;

namespace TenSecCastle.Model {
    [StructLayout(LayoutKind.Explicit)]
    public struct Msg {
        public Msg(Screen screen) : this() {
            Screen = screen;
        }

        [FieldOffset(0)] public readonly Screen Screen;
        [FieldOffset(4)] public bool Navigate;
        [FieldOffset(8)] public GameMsg GameMsg;
        [FieldOffset(8)] public SplashMsg SplashMsg;
    }
}