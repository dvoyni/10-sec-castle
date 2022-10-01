namespace TenSecCastle.Game {
    public struct Msg {
        public Msg(MsgKind kind) : this() {
            Kind = kind;
        }

        public readonly MsgKind Kind;
        public float DeltaTime;
    }

    public enum MsgKind {
        Tick,
    }
}