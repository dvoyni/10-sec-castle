using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using TenSecCastle.Model;

namespace TenSecCastle.Game {
    public static class Init {
        public static (GameModel, L<Cmd<GameMsg>>) InitGame() {
            return (Utils.NewModel, new());
        }
    }
}