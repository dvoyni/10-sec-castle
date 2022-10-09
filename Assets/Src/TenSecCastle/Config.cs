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
using TenSecCastle.Splash;

namespace TenSecCastle {
    public static unsafe class Config {
        public static Runtime<AppModel, Msg, Obj>.Config New => new() {
                Init = CLf.New(&Init),
                Update = CLf.New<Msg, AppModel, (AppModel, L<Cmd>)>(&Update),
                Subscribe = CLf.New<AppModel, L<Sub>>(&Subscribe),
                View = CLf.New<AppModel, Obj>(&View),
                Fail = Maybe<CLa<Exception, AppModel, Msg>>.Just(new CLa<Exception, AppModel, Msg>(&Fail)),
        };

        private static (AppModel, L<Cmd>) Init() {
            return ToModelCmd(SplashScreen.Init());
        }

        private static (AppModel, L<Cmd>) Update(Msg msg, AppModel model) {
            switch (msg.Screen) {
                case Screen.Game:
                    return ToModelCmd(Game.Update.UpdateGame(msg.GameMsg, model.GameModel));
                case Screen.Splash:
                    if (msg.SplashMsg.Proceed) {
                        return ToModelCmd(Game.Init.InitGame());
                    }
                    return ToModelCmd(SplashScreen.Update(msg.SplashMsg, model.SplashModel));
            }

            return (model, new());
        }

        private static L<Sub> Subscribe(AppModel model) {
            switch (model.Screen) {
                case Screen.Game:
                    return Game.Subscribe.SubscribeGame(model.GameModel);
                case Screen.Splash:
                    return SplashScreen.Subscribe(model.SplashModel);
            }

            return new();
        }

        private static Obj View(AppModel model) {
            switch (model.Screen) {
                case Screen.Game:
                    return Game.View.ViewGame(model.GameModel);
                case Screen.Splash:
                    return SplashScreen.View(model.SplashModel);
            }

            return new Obj();
        }

        private static void Fail(Exception ex, AppModel model, Msg msg) {
            Debug.Log(
                $"{ex.Message}\n{ex.StackTrace}\nModel:\n{Serializer.Stringify(model)}\nMsg:{Serializer.Stringify(msg)}"
            );
        }

        private static (AppModel, L<Cmd>) ToModelCmd((GameModel model, L<Cmd> cmds) t) {
            return (ToModel(t.model), t.cmds);
        }

        private static AppModel ToModel(GameModel model) {
            return new AppModel(Screen.Game) { GameModel = model };
        }

        public static Msg ToMsg(GameMsg gameMsg) {
            return new Msg(Screen.Game) { GameMsg = gameMsg };
        }

        private static (AppModel, L<Cmd>) ToModelCmd((SplashModel model, L<Cmd> cmds) t) {
            return (ToModel(t.model), t.cmds);
        }

        private static AppModel ToModel(SplashModel model) {
            return new AppModel(Screen.Splash) { SplashModel = model };
        }

        public static Msg ToMsg(SplashMsg msg) {
            return new Msg(Screen.Splash) { SplashMsg = msg };
        }
    }
}