using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using Rondo.Unity.Subs;
using TenSecCastle.Model;

namespace TenSecCastle.Game {
    public static unsafe class Subscribe {
        public static L<Sub<GameMsg>> SubscribeGame(GameModel model) {
            if (model.Winner.Test(out _)) {
                return new();
            }
            return new(
                Timer.Tick(&OnTick),
                Input.PointerEvent(&OnPointerEvent)
            );
        }

        private static Maybe<GameMsg> OnTick(Timer.TickData data) {
            return Maybe<GameMsg>.Just(new GameMsg(MsgKind.Tick) { DeltaTime = data.Delta });
        }

        private static Maybe<GameMsg> OnPointerEvent(Input.PointerEventData data) {
            if (data.Kind == Input.PointerEventKind.Down) {
                if (data.ObjKeys.First().Test(out var key)) {
                    return Maybe<GameMsg>.Just(
                        new GameMsg(MsgKind.UnitClicked) { Id = Maybe<ulong>.Just(key.GetValue<ulong>()) }
                    );
                }
                Maybe<GameMsg>.Just(new GameMsg(MsgKind.UnitClicked));
            }
            return Maybe<GameMsg>.Nothing;
        }
    }
}