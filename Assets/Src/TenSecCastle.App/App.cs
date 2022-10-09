using Rondo.Core;
using Rondo.Unity;
using TenSecCastle.Model;
using UnityEngine;

namespace TenSecCastle.App {
    public unsafe class App : App<AppModel, Msg, Obj> {
        protected override Runtime<AppModel, Msg, Obj>.Config NewConfig => Config.New;

        protected override IPresenter<Obj> NewPresenter(Transform parent) {
            return new Presenter<Obj>(parent);
        }
    }
}