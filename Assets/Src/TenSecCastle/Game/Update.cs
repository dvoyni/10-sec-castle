using System;
using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using TenSecCastle.Model;

namespace TenSecCastle.Game {
    public static class Update {
        public static (GameModel, L<Cmd<Msg>>) UpdateGame(Msg msg, GameModel model) {
            switch (msg.Kind) {
                case MsgKind.Tick:
                    return GameLogic.UpdateTick(model, msg.DeltaTime);
            }
            throw new NotImplementedException("Message is not handled");
        }
    }
}