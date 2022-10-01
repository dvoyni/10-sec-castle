using Rondo.Core;
using Rondo.Core.Lib;
using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using Rondo.Unity;
using UnityEngine;

namespace TenSecCastle.App {
    public unsafe class App : App<AppModel, Msg, Obj> {
        private Runtime<AppModel, Msg, Obj>.Config _config = TenSecCastle.Config.New;
        private CLa<AppModel, Msg, AppModel, L<Cmd<Msg>>> _dumpUpdate;

        protected override Runtime<AppModel, Msg, Obj>.Config Config => _config;

        protected override IPresenter<Obj> NewPresenter(Transform parent) {
            return new Presenter<Obj>(parent);
        }

        private void OnDestroy() {
            _dumpUpdate.Dispose();
            _config.Dispose();
        }
    }
}