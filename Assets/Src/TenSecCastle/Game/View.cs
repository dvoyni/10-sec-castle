using Rondo.Core.Lib;
using Rondo.Core.Lib.Containers;
using Rondo.Unity;
using Rondo.Unity.Components;
using Rondo.Unity.Utils;
using TenSecCastle.Model;
using TenSecCastle.View;
using Unity.Mathematics;

namespace TenSecCastle.Game {
    public static unsafe partial class View {
        public static Obj ViewGame(GameModel model) {
            return new Obj("Root",
                children: new L<Obj>(
                    ViewField()
                )
                + ViewPlayerUI(model)
                + ViewUnits(model)
            );
        }

        private static Obj ViewField() {
            return new Obj("Field",
                components: new(
                    Prefab.Static("Assets/Prefabs/Environment.prefab")
                )
            );
        }

        private static L<Obj> ViewUnits(GameModel model) {
            return model.Units.Map(Cf.New<Unit, GameModel, Obj>(&ViewUnit, model));
        }
        

        private static Obj ViewUnit(Unit unit, GameModel* model) {
            var cellSize = 1.5f;
            var pos = new float3(unit.Cell.x, 0, unit.Cell.y) * cellSize;
            var dir = new float3(unit.MoveDirection.x, 0, unit.MoveDirection.y);
            switch (unit.State) {
                case UnitState.Moving:
                    pos = math.lerp(pos - dir * cellSize, pos, unit.StateProgress);
                    break;
                case UnitState.Attacking:
                    dir = new float3(unit.AttackDirection.x, 0, unit.AttackDirection.y);
                    break;
                case UnitState.Dieing:
                    pos += new float3(0, -math.max(unit.StateTime - 1, 0) * 0.5f, 0);
                    break;
            }
            return new Obj($"Unit:{unit.Id}",
                key: Key.New(unit.Id),
                components: new(
                    Rendering.Transform(pos, quaternion.LookRotation(math.normalize(dir), new float3(0, 1, 0))),
                    Prefab.WithData("Assets/Prefabs/Unit.prefab", new UnitViewData {
                            Unit = unit,
                            SelectedUnitId = model->SelectedUnitID.Test(out var selectedId) && (selectedId == unit.Id)
                    })
                ));
        }

        private static Obj ViewPlayerUI(GameModel model) {
           
            static bool PlayerWithId(Player player, ulong* winnerId) => player.Id == *winnerId;
            static bool UnitIsSelected(Unit unit, ulong* id) => unit.Id == *id;

            var isPlayerWin = Maybe<bool>.Nothing;
            var player = Maybe<Player>.Nothing;
            var AI = Maybe<Player>.Nothing;
            var selectedUnitSlots = Maybe<L<ulong>>.Nothing;
            
            if (model.Winner.Test(out var winnerId)) {
                if (model.Players.First(Cf.New<Player, ulong, bool>(&PlayerWithId, winnerId)).Test(out var playerWin))
                    isPlayerWin = Maybe<bool>.Just(playerWin.Kind==PlayerKind.Human);
            }
            
            if(model.Players.First(&Utils.PlayerIsHuman).Test(out var playerValue)) {
                player = Maybe<Player>.Just(playerValue);
            }
            
            if(model.Players.First(&Utils.PlayerIsAI).Test(out var AIvalue)) {
                AI = Maybe<Player>.Just(AIvalue);
            }

            if (model.SelectedUnitID.Test(out var userId)) {
                if (model.Units.First(Cf.New<Unit, ulong, bool>(&UnitIsSelected, userId)).Test(out var unitValue)) {
                    var items = new L<ulong>();
                    items += unitValue.ArmorId;
                    items += unitValue.WeaponId;
                    items += unitValue.JewelryId;
                    selectedUnitSlots = Maybe<L<ulong>>.Just(items);
                }
            }

            return new Obj($"PlayerUI",
                components: new(
                    Prefab.WithData("Assets/Prefabs/PlayerUI.prefab", new PlayerUIViewData {
                        PlayerWon = isPlayerWin,
                        PlayerSlots = player.ValueOrDefault.Slots,
                        CastleHitPoints = player.ValueOrDefault.CastleHitPoints,
                        Coins = player.ValueOrDefault.Coins,
                        SelectedUnitSlots = selectedUnitSlots,
                        EnemyCastleHitPoints = AI.ValueOrDefault.CastleHitPoints,
                        TimeToSpawn = model.Timeout,
                        MaxTimeToSpawn = model.Interval
                    })
                ));
        }
    }
}