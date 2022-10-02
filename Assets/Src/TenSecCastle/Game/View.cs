using Rondo.Core.Lib;
using Rondo.Core.Lib.Containers;
using Rondo.Unity;
using Rondo.Unity.Components;
using Rondo.Unity.Utils;
using TenSecCastle.Model;
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
            return model.Units.Map(&ViewUnit);
        }

        private static Obj ViewUnit(Unit unit) {
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
                components: new(
                    Rendering.Transform(pos, quaternion.LookRotation(math.normalize(dir), new float3(0, 1, 0))),
                    Prefab.WithData("Assets/Prefabs/Unit.prefab", unit)
                ));
        }

        private static Obj ViewPlayerUI(GameModel model) {
            static bool IsHuman(Player player) {
                return player.Kind == PlayerKind.Human;
            }

            if (model.Players.First(&IsHuman).Test(out var player)) {
                return new("PlayerUI",
                    components: new(
                        UI.Canvas(new(renderMode: UI.RenderMode.ScreenSpaceOverlay)),
                        UI.CanvasScaler(new(
                            uiScaleMode: UI.ScaleMode.ScaleWithScreenSize,
                            referenceResolution: new(2560, 1140),
                            screenMatchMode: UI.ScreenMatchMode.MatchWidthOrHeight,
                            matchWidthOrHeight: 0.5f
                        )),
                        UI.GraphicsRaycaster(new(blockingObjects: UI.BlockingObjects.All))
                    ),
                    children: new(
                        new Obj("SlotsPanel",
                            components: new(
                                UI.RectTransform(anchorMin: new(0, 0), anchorMax: new(1, 0), offsetMin: 0, offsetMax: 0),
                                UI.ContentSizeFitter(new(verticalFit: UI.FitMode.PreferredSize)),
                                UI.HorizontalLayoutGroup(new(
                                    childAlignment: UI.TextAnchor.MiddleCenter,
                                    padding: 12,
                                    childControl: true
                                ))
                            ),
                            children: player.Slots.Map(Cf.New<Slot, GameModel, Obj>(&ViewSlot, model))
                        )
                    )
                );
            }

            return new();
        }

        private static Obj ViewSlot(Slot slot, GameModel* model) {
            static TenSecCastle.Msg OnClick(Key key) {
                var slotKind = key.GetValue<SlotKind>();
                return Config.ToMsg(new Msg(MsgKind.SlotClicked) { Slot = slotKind });
            }

            return new Obj($"Slot:{slot.Item.SlotKind}",
                key: Key.New(slot.Item.SlotKind),
                components: new(
                    UI.Image(new(color: Colors.White)),
                    UI.VerticalLayoutGroup(new(childControl: true)),
                    UI.Button<TenSecCastle.Msg>(new(&OnClick))
                ),
                children: new(
                    new Obj("Icon",
                        components: new(
                            UI.Image(new(
                                color: Colors.White,
                                spriteAddress: (S)$"Assets/Prefabs/Icons/1.png" //{slot.Item.Id}
                            ))
                        )
                    )
                )
            );
        }
    }
}