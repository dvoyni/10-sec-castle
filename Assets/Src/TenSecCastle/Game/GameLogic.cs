using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using TenSecCastle.Model;

namespace TenSecCastle.Game {
    public static class GameLogic {
        public static (GameModel, L<Cmd<Msg>>) UpdateTick(GameModel model, float dt) {
            model.Timeout -= dt;

            UpdateUnits(ref model, dt);

            if (model.Timeout == 0) {
                AI(ref model);
                Spawn(ref model);
            }

            return (model, new());
        }

        private static void UpdateUnits(ref GameModel model, float dt) { }

        private static void AI(ref GameModel model) { }

        private static void Spawn(ref GameModel model) {
            
        }
    }
}