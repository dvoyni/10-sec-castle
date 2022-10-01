namespace TenSecCastle {
    public struct Model {
        public Model(Screen screen) : this() {
            Screen = screen;
        }

        public Screen Screen;
        public Game.Model GameModel;
    }

    public enum Screen {
        Game,
    }
}