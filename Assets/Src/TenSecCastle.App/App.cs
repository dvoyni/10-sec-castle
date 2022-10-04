using Rondo.Core;
using Rondo.Core.Lib.Containers;
using Rondo.Core.Memory;
using Rondo.Unity;
using TenSecCastle.Model;
using Unity.Mathematics;
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

        private static void ProduceGenerics() {
            Serializer.__ProduceGenericStruct<AppModel>();
            Serializer.__ProduceGenericStruct<SplashModel>();
            Serializer.__ProduceGenericStruct<GameModel>();
            Serializer.__ProduceGenericStruct<Item>();
            Serializer.__ProduceGenericStruct<Player>();
            Serializer.__ProduceGenericStruct<Slot>();
            Serializer.__ProduceGenericStruct<Unit>();
            Serializer.__ProduceGenericStruct<Attribute>();
            Serializer.__ProduceGenericStruct<int2>();
            Serializer.__ProduceGenericStruct<Maybe<ulong>>();
            Serializer.__ProduceGenericL<Item>();
            Serializer.__ProduceGenericL<Player>();
            Serializer.__ProduceGenericL<Unit>();
            Serializer.__ProduceGenericL<Slot>();
            Serializer.__ProduceGenericL<int2>();
            Serializer.__ProduceGenericL<Attribute>();

            Serializer.__ProduceGenericStruct<S>();
            Serializer.__ProduceGenericStruct<Ts>();
            Serializer.__ProduceGenericStruct<Ref>();
            Serializer.__ProduceGenericStruct<Key>();
            Serializer.__ProduceGenericStruct<Obj>();
            Serializer.__ProduceGenericStruct<ObjRef>();
            Serializer.__ProduceGenericStruct<Comp>();
            Serializer.__ProduceGenericD<ulong, Comp>();
            Serializer.__ProduceGenericD<S, Obj>();
        }
    }
}