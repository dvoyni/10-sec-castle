using Rondo.Core.Lib.Containers;
using TenSecCastle.Model;

namespace TenSecCastle.Game {
    public struct GameMsg {
        public GameMsg(MsgKind kind) : this() {
            Kind = kind;
        }

        public readonly MsgKind Kind;
        public float DeltaTime;
        public SlotKind Slot;
        public Maybe<ulong> Id;
    }

    public enum MsgKind {
        Tick,
        SlotClicked,
        Restart,
        UnitClicked,
    }
}