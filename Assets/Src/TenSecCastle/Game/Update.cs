using System;
using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using Rondo.Unity.Utils;
using TenSecCastle.Model;

namespace TenSecCastle.Game {
    public static class Update {
        public static (GameModel, L<Cmd<Msg>>) UpdateGame(Msg msg, GameModel model) {
            switch (msg.Kind) {
                case MsgKind.Tick:
                    return GameLogic.UpdateTick(model, msg.DeltaTime);
                case MsgKind.SlotClicked:
                    return RerollSlot(model, msg.Slot);
            }
            throw new NotImplementedException("Message is not handled");
        }

        private static (GameModel, L<Cmd<Msg>>) RerollSlot(GameModel model, SlotKind slot) {
            Debug.Log("Clicked slot: " + slot);
            return (model, new()); //TODO
        }
    }
}