namespace TenSecCastle.Game {
    public enum MsgKind {
    }

    public struct Msg {
        public Msg(MsgKind kind) : this() {
            Kind = kind;
        }

        public readonly MsgKind Kind;
    }
}