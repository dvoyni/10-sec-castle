using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using TenSecCastle.Model;
using Unity.Mathematics;

namespace TenSecCastle.Game {
    public static class Init {
        public static (GameModel, L<Cmd<Msg>>) InitGame() {
            var model = new GameModel {
                    Items = GameConfig.Items,
                    BasicUnit = GameConfig.BasicUnit,
                    FieldSize = new int2(2, 12),
                    Interval = 2,
                    BaseIncome = 1,
                    MoveAxis = new int2(0, 1),
                    Timeout = 2,
            };
            model.Players = new(
                new Player {
                        Id = 1,
                        Kind = PlayerKind.Human,
                        Slots = GameLogic.ShuffleSlots(model),
                        SpawnDirection = new int2(0, 1),
                        SpawnPoints = new L<int2>(
                            new int2(-1, 0),
                            new(model.FieldSize.x, 0)
                        ),
                },
                new Player {
                        Id = 2,
                        Kind = PlayerKind.AI,
                        Slots = GameLogic.ShuffleSlots(model),
                        SpawnDirection = new int2(0, -1),
                        SpawnPoints = new L<int2>(
                            new int2(-1, model.FieldSize.y - 1),
                            new(model.FieldSize.x, model.FieldSize.y - 1)
                        ),
                }
            );
            return (model, new());
        }
    }
}