using System.Runtime.InteropServices;
using TenSecCastle.Splash;

namespace TenSecCastle {
    [StructLayout(LayoutKind.Explicit)]
    public struct Msg {
        public Msg(Screen screen) : this() {
            Screen = screen;
        }

        [FieldOffset(0)] public readonly Screen Screen;
        [FieldOffset(4)] public bool Navigate;
        [FieldOffset(8)] public Game.GameMsg GameMsg;
        [FieldOffset(8)] public SplashMsg SplashMsg;
    }
}