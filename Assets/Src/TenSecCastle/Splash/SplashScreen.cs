using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using Rondo.Unity;
using Rondo.Unity.Components;
using Rondo.Unity.Utils;
using Unity.Mathematics;

namespace TenSecCastle.Splash {
    public struct SplashModel { }

    public struct SplashMsg {
        public bool Proceed;
    }

    public static unsafe class SplashScreen {
        public static (SplashModel, L<Cmd<SplashMsg>>) Init() {
            return new();
        }

        public static (SplashModel, L<Cmd<SplashMsg>>) Update(SplashMsg msg, SplashModel model) {
            return (new(), new());
        }

        public static Obj View(SplashModel model) {
            var info = new L<S>(
                (S)"10 Sec Castle",
                (S)"",
                (S)"Objective: Destroy enemy castle.",
                (S)"Every 10 seconds your unit is spawned with chosen equipment.",
                (S)"To change equipment click on the slot.",
                (S)"Click on unit to get info about its equipment.",
                (S)"GLHF, Ludum Dare 51",
                (S)""
            );

            static Obj ViewText(int n, S text) {
                return new Obj($"Line:{n}",
                    components: new(
                        UI.Text(new(
                            color: Colors.White,
                            text: text,
                            fontSize: n < 1 ? 64 : 48,
                            fontAddress: (S)"Assets/Data/Alkalami-Regular.ttf"
                        ))
                    )
                );
            }

            static Msg OnButtonClick(Key key) {
                return Config.ToMsg(new SplashMsg { Proceed = true });
            }

            return new Obj("SplashScreen",
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
                    new Obj("Panel",
                        components: new(
                            UI.ContentSizeFitter(new(UI.FitMode.PreferredSize, UI.FitMode.PreferredSize)),
                            UI.VerticalLayoutGroup(new(
                                childControl: true, padding: 64, spacing: 8, childExpand: new(true, false)
                            )),
                            UI.Image(new(color: new float4(0.25f, 0.25f, 0.25f, 0.25f)))
                        ),
                        children: info.IndexedMap(&ViewText) +
                        new Obj("Button",
                            components: new(
                                UI.Image(new(color: new(0.25f, 0.5f, 0.4f, 1))),
                                UI.Button<Msg>(new(onClick: &OnButtonClick)),
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
                                            text: (S)"Start",
                                            fontSize: 64,
                                            fontAddress: (S)"Assets/Data/Alkalami-Regular.ttf"
                                        ))
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