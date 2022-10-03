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
        
        private static Obj ViewSelectedUnit(GameModel model) {
            
            
            /*static bool UnitIsSelected(Unit unit, ulong* id) {
                return unit.Id == *id;

            }

            if (model.SelectedUnitID.Test(out var userId)) {
                
                if (model.Units.First(Cf.New<Unit,ulong,bool>(&UnitIsSelected,userId)).Test(out var unit)) {
                    Debug.Log(unit.Id);
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
                                children: model.SelectedUnitID.Slots.Map(Cf.New<Slot, GameModel, Obj>(&ViewSlot, model))
                            )));
                }
            }
            */
            
            /*
            static bool UnitIsSelected(Unit unit, Maybe<ulong>* id) {
                return unit.Id == *id->Test();
            }
            
            static bool UnitIsSelected2(Unit unit, Maybe<ulong>* id) {
                id.Test(var our userId)
                return unit.Id == *id->Test();
            }

            var selectedId = model.SelectedUnitID;
            
            if (model.Units.First(Cf.New<Unit,Maybe<ulong>,bool>(&UnitIsSelected,selectedId)).Test(out var unit))
            */
                
            //if (model.Units.First(&UnitIsSelected).Test(out var unit))
            /*{
                
            }*/
            return new Obj($"SelectedUnit");
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
           
            static bool PlayerWithId(Player player, ulong* winnerId) {
                return player.Id == *winnerId;
            }

            Maybe<bool> isPlayerWin = Maybe<bool>.Nothing;

            if (model.Winner.Test(out var winnerId)) {
                if (model.Players.First(Cf.New<Player, ulong, bool>(&PlayerWithId, winnerId)).Test(out var player))
                    isPlayerWin = Maybe<bool>.Just(player.Kind==PlayerKind.Human);
            }
            
            //if(model.Players.First())

            return new Obj($"PlayerUI",
                components: new(
                    Prefab.WithData("Assets/Prefabs/PlayerUI.prefab", new PlayerUIViewData {
                       // PlayerWon = isPlayerWin
                       // PlayerSlots
                        /*SelectedUnitSlot
                        CastleHitPoints
                        Coins
                        EnemyCastleHitPoints
                        TimeToSpawn
                        MaxTimeToSpawn*/
                        
                    })
                ));
           
        }
        
        
        
    }
}