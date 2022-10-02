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
                + ViewVictoryUI(model)
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
                            Selected = model->SelectedUnit.Test(out var selectedId) && (selectedId == unit.Id)
                    })
                ));
        }
        

        private static Obj ViewVictoryUI(GameModel model) {

            static bool PlayerWithId(Player player, ulong* winnerId) {
                return player.Id == *winnerId;
            }

            if (model.Winner.Test(out var winnerId)) {
                if (model.Players.First(Cf.New<Player,ulong,bool>(&PlayerWithId,winnerId)).Test(out var player)) {
                    switch (player.Kind) {
                        case PlayerKind.Human:
                            Debug.Log("win");
                            break;
                        case PlayerKind.AI:
                            Debug.Log("Lose");
                            break;
                    }
                }
            }

            return new("VictoryUI");
        }

        private static Obj ViewPlayerUI(GameModel model) {
            if (model.Players.First(&Utils.PlayerIsHuman).Test(out var player)) {
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
                                UI.RectTransform(anchorMin: new(0, 0), anchorMax: new(1, 0), offsetMin: 0, offsetMax: 0,
                                    pivot: new float2(0.5f, 0)),
                                UI.ContentSizeFitter(new(verticalFit: UI.FitMode.PreferredSize)),
                                UI.HorizontalLayoutGroup(new(
                                    childAlignment: UI.TextAnchor.MiddleCenter,
                                    padding: 1,
                                    childControl: true
                                ))
                            ),
                            children: player.Slots.Map(Cf.New<Slot, GameModel, Obj>(&ViewSlot, model))
                        ),
                        new Obj("TimerPanel",
                            components: new(
                                UI.RectTransform(/*anchorMin: 0, anchorMax: 0, offsetMin: 0,
                                    offsetMax: new float2(100,100), pivot: 0,*/0,100,0)
                                /*UI.ContentSizeFitter(new(verticalFit: UI.FitMode.PreferredSize)),
                                UI.HorizontalLayoutGroup(new(
                                    childAlignment: UI.TextAnchor.MiddleCenter,
                                    padding: 0,
                                    childControl: true
                                ))*/
                            ),
                            children: new(
                                new Obj("Icon",
                                    components: new(
                                        UI.RectTransform(anchorMin: 0, anchorMax: 1, offsetMin: 0,
                                            offsetMax: 0, pivot: 0),
                                        UI.Image(new(
                                            color: Colors.White,
                                            spriteAddress: (S) $"Assets/Prefabs/Icons/TimerSprite.png",
                                            fillAmount: model.Timeout / model.Interval,
                                            fillMethod: UI.FillMethod.Radial360,
                                            imageType: UI.ImageType.Filled
                                            
                                        )),
                                        UI.LayoutElement(new(float2.zero))
                                    )
                                )
                            ))
                    )
                );
            }

            return new("PlayerUI");
        }
        
        private static Obj ViewSlot(Slot slot, GameModel* model) {
            static TenSecCastle.Msg OnClick(Key key) {
                var slotKind = key.GetValue<SlotKind>();
                return Config.ToMsg(new Msg(MsgKind.SlotClicked) { Slot = slotKind });
            }

            var h = 1f;
            var spriteBack = $"Assets/Prefabs/Icons/1.png";
            var spriteAddress = $"Assets/Prefabs/Icons/{slot.Item.Id}.png";
            
            if (slot.SwapProgress < 0.25f) {
                if (slot.PrevItem.Test(out var prevItem)) {
                    spriteAddress = $"Assets/Prefabs/Icons/{prevItem.Id}.png";
                }
                else {
                    spriteAddress = spriteBack;
                }

                h = 1 - slot.SwapProgress * 4;
            } else if(slot.SwapProgress<0.5f) {
                spriteAddress = spriteBack;
                h = (slot.SwapProgress-0.25f) * 4;
            } else if(slot.SwapProgress<0.75f) {
                spriteAddress = spriteBack;
                h = 1 - (slot.SwapProgress-0.5f) * 4;
            } 
            else {
                h = (slot.SwapProgress-0.75f) * 4;
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
                                spriteAddress: (S) spriteAddress
                            )),
                            UI.LayoutElement(new(preferred: new  float2(180,180*h),layoutPriority:1))
                        )
                    )
                )
            );
        }
    }
}