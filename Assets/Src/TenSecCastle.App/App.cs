using Rondo.Core;
using Rondo.Unity;
using UnityEngine;

namespace TenSecCastle.App {
    public unsafe class App : App<AppModel, Msg, Obj> {
        private Runtime<AppModel, Msg, Obj>.Config _config = TenSecCastle.Config.New;

        protected override Runtime<AppModel, Msg, Obj>.Config Config => _config;

        protected override IPresenter<Obj> NewPresenter(Transform parent) {
            return new Presenter<Obj>(parent);
        }

        private void OnDestroy() {
            _config.Dispose();
        }
    }
}