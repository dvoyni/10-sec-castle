using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;

namespace TenSecCastle.Game {
    public static unsafe class Subscribe {
        public static L<Sub<Msg>> SubscribeGame(Model model) {
            return new();
        }
    }
}