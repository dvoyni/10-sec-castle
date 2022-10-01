using System;
using System.IO;
using Rondo.Core;
using Rondo.Core.Lib;
using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using Rondo.Core.Memory;
using Rondo.Unity;
using Rondo.Unity.Utils;
using TenSecCastle.Model;

namespace TenSecCastle {
    public static unsafe class Config {
        public static Runtime<AppModel, Msg, Obj>.Config New => new() {
                Init = CLf.New(&Init),
                Update = CLf.New<Msg, AppModel, (AppModel, L<Cmd<Msg>>)>(&Update),
                Subscribe = CLf.New<AppModel, L<Sub<Msg>>>(&Subscribe),
                View = CLf.New<AppModel, Obj>(&View),
                Fail = Maybe<CLa<Exception, AppModel, Msg>>.Just(new CLa<Exception, AppModel, Msg>(&Fail)),
                Reset = Maybe<CLa<AppModel>>.Just(new CLa<AppModel>(&Reset)),
        };

        private static (AppModel, L<Cmd<Msg>>) Init() {
            return ToModelCmd(Game.Init.InitGame());
        }

        private static (AppModel, L<Cmd<Msg>>) Update(Msg msg, AppModel model) {
            switch (msg.Screen) {
                case Screen.Game:
                    return ToModelCmd(Game.Update.UpdateGame(msg.GameMsg, model.GameModel));
            }

            return (model, new());
        }

        private static L<Sub<Msg>> Subscribe(AppModel model) {
            switch (model.Screen) {
                case Screen.Game:
                    return Game.Subscribe.SubscribeGame(model.GameModel).Map(&ToMsg);
            }

            return new();
        }

        private static Obj View(AppModel model) {
            switch (model.Screen) {
                case Screen.Game:
                    return Game.View.ViewGame(model.GameModel);
            }

            return new Obj();
        }

        private static void Fail(Exception ex, AppModel model, Msg msg) {
            Debug.Log(
                $"{ex.Message}\n{ex.StackTrace}\nModel:\n{Serializer.Stringify(model)}\nMsg:{Serializer.Stringify(msg)}"
            );
        }

        private static void Reset(AppModel dumpedModel) {
            if (Directory.Exists(Debug.DebugDumDir)) {
                Directory.Delete(Debug.DebugDumDir, true);
            }
        }

        private static (AppModel, L<Cmd<Msg>>) ToModelCmd((GameModel model, L<Cmd<Game.Msg>> cmds) t) {
            return (ToModel(t.model), t.cmds.Map(&ToMsg));
        }

        private static AppModel ToModel(GameModel model) {
            return new AppModel(Screen.Game) { GameModel = model };
        }

        public static Msg ToMsg(Game.Msg msg) {
            return new Msg(Screen.Game) { GameMsg = msg };
        }
    }
}