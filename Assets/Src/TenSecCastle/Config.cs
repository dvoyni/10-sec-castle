using System;
using System.IO;
using Rondo.Core;
using Rondo.Core.Lib;
using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using Rondo.Core.Memory;
using Rondo.Unity;
using Rondo.Unity.Utils;

namespace TenSecCastle {
    public static unsafe class Config {
        public static Runtime<Model, Msg, Obj>.Config New => new() {
                Init = CLf.New(&Init),
                Update = CLf.New<Msg, Model, (Model, L<Cmd<Msg>>)>(&Update),
                Subscribe = CLf.New<Model, L<Sub<Msg>>>(&Subscribe),
                View = CLf.New<Model, Obj>(&View),
                Fail = Maybe<CLa<Exception, Model, Msg>>.Just(new CLa<Exception, Model, Msg>(&Fail)),
                Reset = Maybe<CLa<Model>>.Just(new CLa<Model>(&Reset)),
        };

        private static (Model, L<Cmd<Msg>>) Init() {
            return ToModelCmd(Game.Init.InitGame());
        }

        private static (Model, L<Cmd<Msg>>) Update(Msg msg, Model model) {
            switch (msg.Screen) {
                case Screen.Game:
                    return ToModelCmd(Game.Update.UpdateGame(msg.GameMsg, model.GameModel));
            }

            return (model, new());
        }

        private static L<Sub<Msg>> Subscribe(Model model) {
            switch (model.Screen) {
                case Screen.Game:
                    return Game.Subscribe.SubscribeGame(model.GameModel).Map(&ToMsg);
            }

            return new();
        }

        private static Obj View(Model model) {
            switch (model.Screen) {
                case Screen.Game:
                    return Game.View.ViewGame(model.GameModel);
            }

            return new Obj();
        }

        private static void Fail(Exception ex, Model model, Msg msg) {
            Debug.Log(
                $"{ex.Message}\n{ex.StackTrace}\nModel:\n{Serializer.Stringify(model)}\nMsg:{Serializer.Stringify(msg)}"
            );
        }

        private static void Reset(Model dumpedModel) {
            if (Directory.Exists(Debug.DebugDumDir)) {
                Directory.Delete(Debug.DebugDumDir, true);
            }
        }

        private static (Model, L<Cmd<Msg>>) ToModelCmd((Game.Model model, L<Cmd<Game.Msg>> cmds) t) {
            return (ToModel(t.model), t.cmds.Map(&ToMsg));
        }

        private static Model ToModel(Game.Model model) {
            return new Model(Screen.Game) { GameModel = model };
        }

        public static Msg ToMsg(Game.Msg msg) {
            return new Msg(Screen.Game) { GameMsg = msg };
        }
    }
}