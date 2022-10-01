using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;

namespace TenSecCastle.Game {
    public static class Init {
        public static (Model, L<Cmd<Msg>>) InitGame() {
            return (new(), new());
        }
    }
}