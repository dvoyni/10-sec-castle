using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using Rondo.Unity.Subs;

namespace TenSecCastle.Game {
    public static unsafe class Subscribe {
        public static L<Sub<Msg>> SubscribeGame(Model model) {
            return new(
                Timer.Tick(&OnTick)
            );
        }

        private static Maybe<Msg> OnTick(Timer.TickData data) {
            return Maybe<Msg>.Nothing;
        }
    }
}