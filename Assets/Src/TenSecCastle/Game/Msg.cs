using TenSecCastle.Model;

namespace TenSecCastle.Game {
    public struct Msg {
        public Msg(MsgKind kind) : this() {
            Kind = kind;
        }

        public readonly MsgKind Kind;
        public float DeltaTime;
        public SlotKind Slot;
    }

    public enum MsgKind {
        Tick,
        SlotClicked
    }
}