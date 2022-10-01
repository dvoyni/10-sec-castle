using Rondo.Core.Lib.Containers;
using Rondo.Unity;
using Rondo.Unity.Components;
using TenSecCastle.Model;
using Unity.Mathematics;

namespace TenSecCastle.Game {
    public static unsafe partial class View {
        public static Obj ViewGame(GameModel model) {
            return new Obj("Root",
                children: new L<Obj>(
                    ViewField()
                ) + ViewUnits(model)
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
            return model.Units.Map(&ViewUnit);
        }

        private static Obj ViewUnit(Unit unit) {
            var cellSize = 1.5f;
            var pos = new float3(unit.Cell.x, 0, unit.Cell.y) * cellSize;
            var dir = math.normalize(new float3(unit.Direction.x, 0, unit.Direction.y));
            if (unit.State == UnitState.Moving) {
                pos = math.lerp(pos - dir * cellSize, pos, unit.StateProgress);
            }
            return new Obj($"Unit:{unit.Id}",
                components: new(
                    Rendering.Transform(pos, quaternion.LookRotation(dir, new float3(0, 1, 0))),
                    Prefab.WithData("Assets/Prefabs/Unit.prefab", unit)
                ));
        }
    }
}