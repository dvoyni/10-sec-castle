using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using Rondo.Unity.Subs;
using TenSecCastle.Model;

namespace TenSecCastle.Game {
    public static unsafe class Subscribe {
        public static L<Sub> SubscribeGame(GameModel model) {
            if (model.Winner.Test(out _)) {
                return new();
            }
            return new(
                Timer.Tick(&OnTick),
                Input.PointerEvent(&OnPointerEvent)
            );
        }

        private static Maybe<Msg> OnTick(Timer.TickData data) {
            return Maybe<Msg>.Just(Config.ToMsg(new GameMsg(MsgKind.Tick) { DeltaTime = data.Delta }));
        }

        private static Maybe<Msg> OnPointerEvent(Input.PointerEventData data) {
            if (data.Kind == Input.PointerEventKind.Down) {
                if (data.ObjKeys.First().Test(out var key)) {
                    return Maybe<Msg>.Just(
                        Config.ToMsg(new GameMsg(MsgKind.UnitClicked) { Id = Maybe<ulong>.Just(key.GetValue<ulong>()) })
                    );
                }
                return Maybe<Msg>.Just(Config.ToMsg(new GameMsg(MsgKind.UnitClicked)));
            }
            return Maybe<Msg>.Nothing;
        }
    }
}