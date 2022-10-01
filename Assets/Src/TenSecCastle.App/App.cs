using Rondo.Core;
using Rondo.Core.Lib;
using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using Rondo.Unity;
using UnityEngine;

namespace TenSecCastle.App {
    public unsafe class App : App<Model, Msg, Obj> {
        private Runtime<Model, Msg, Obj>.Config _config = TenSecCastle.Config.New;
        private CLa<Model, Msg, Model, L<Cmd<Msg>>> _dumpUpdate;

        protected override Runtime<Model, Msg, Obj>.Config Config => _config;

        protected override IPresenter<Obj> NewPresenter(Transform parent) {
            return new Presenter<Obj>(parent);
        }

        private void OnDestroy() {
            _dumpUpdate.Dispose();
            _config.Dispose();
        }
    }
}