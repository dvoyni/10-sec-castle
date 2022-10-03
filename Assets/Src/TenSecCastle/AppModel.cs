using TenSecCastle.Model;
using TenSecCastle.Splash;

namespace TenSecCastle {
    public struct AppModel {
        public AppModel(Screen screen) : this() {
            Screen = screen;
        }

        public Screen Screen;
        public GameModel GameModel;
        public SplashModel SplashModel;
    }

    public enum Screen {
        Splash,
        Game,
    }
}