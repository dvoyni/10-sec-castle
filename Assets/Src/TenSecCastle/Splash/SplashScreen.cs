using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using Rondo.Unity;
using Rondo.Unity.Components;
using Rondo.Unity.Utils;
using TenSecCastle.Model;
using Unity.Mathematics;

namespace TenSecCastle.Splash {
    public static unsafe class SplashScreen {
        public static (SplashModel, L<Cmd<SplashMsg>>) Init() {
            return new();
        }

        public static (SplashModel, L<Cmd<SplashMsg>>) Update(SplashMsg msg, SplashModel model) {
            return (new(), new());
        }

        private static readonly ObjRef _buttonRef = ObjRef.New();

        public static Obj View(SplashModel model) {
            static Msg OnButtonClick(Key key) {
                return Config.ToMsg(new SplashMsg { Proceed = true });
            }

            return new Obj("SplashScreenRoot",
                children: new(
                    new Obj("Scene",
                        components: new(
                            Prefab.Static("Assets/Prefabs/Environment_menu.prefab")
                        )
                    ),
                    new Obj("SplashScreen",
                        components: new(
                            UI.Canvas(new(renderMode: UI.RenderMode.ScreenSpaceOverlay)),
                            UI.CanvasScaler(new(
                                uiScaleMode: UI.ScaleMode.ScaleWithScreenSize,
                                referenceResolution: new float2(2560, 1440),
                                screenMatchMode: UI.ScreenMatchMode.MatchWidthOrHeight,
                                matchWidthOrHeight: 0.5f
                            )),
                            UI.GraphicsRaycaster(new(blockingObjects: UI.BlockingObjects.All))
                        ),
                        children: new(
                            new Obj("Image",
                                components: new(
                                    UI.RectTransform(new(0.5f, .95f), new(0.5f, .95f), 0, 0, new(0.5f, 1)),
                                    UI.Image(new(color: Colors.White, spriteAddress: (S)"Assets/Data/UI/BF_logo.png")),
                                    UI.ContentSizeFitter(new(UI.FitMode.PreferredSize, UI.FitMode.PreferredSize))
                                )
                            ),
                            new Obj("Button",
                                objRef: _buttonRef,
                                components: new(
                                    UI.RectTransform(new(0.5f, .05f), new(0.5f, .05f), 0, 0, new(0.5f, 0)),
                                    UI.Image(new(color: Colors.White, spriteAddress: (S)"Assets/Data/UI/start_btn.png")),
                                    UI.ContentSizeFitter(new(UI.FitMode.PreferredSize, UI.FitMode.PreferredSize)),
                                    UI.Button<Msg>(new(
                                        onClick: &OnButtonClick,
                                        transition: UI.Transition.ColorTint,
                                        colors: new(
                                            new(0.85f, 0.85f, 0.85f, 1),
                                            Colors.White,
                                            new(0.5f, 0.5f, 0.5f, 1),
                                            Colors.White, Colors.White
                                        ),
                                        targetGraphic: _buttonRef
                                    )),
                                    UI.HorizontalLayoutGroup(new(
                                        childAlignment: UI.TextAnchor.MiddleCenter,
                                        childControl: true,
                                        padding: 16
                                    ))
                                ),
                                children: new(
                                    new Obj("Text",
                                        components: new(
                                            UI.Text(new(
                                                color: Colors.White,
                                                text: (S)"Start\nBattle",
                                                fontSize: 64,
                                                fontAddress: (S)"Assets/Data/Font/AdLibRg.ttf"
                                            ))
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );
        }

        public static L<Sub<SplashMsg>> Subscribe(SplashModel model) {
            return new();
        }
    }
}