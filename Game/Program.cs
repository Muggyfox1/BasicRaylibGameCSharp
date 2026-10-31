using Raylib_cs;

namespace Game;

internal static class Program {
    // STAThread is required if you deploy using NativeAOT on Windows
    // See https://github.com/raylib-cs/raylib-cs/issues/301
    [System.STAThread]
    public static void Main() {
        var game = new Game();
        game.Init();

        while(!Raylib.WindowShouldClose()) {
            game.Update();
            game.Draw();
        }

        game.Close();
        Raylib.CloseWindow();
    }
}

class Game {
    internal void Init() {
        Raylib.InitWindow(800, 480, "Hello, World");
    }

    internal void Update() {
        //throw new NotImplementedException();
    }

    internal void Draw() {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.White);
        
        Raylib.DrawRectangle(10, 10, 108, 20, new Color(0, 0, 0, 100));
        Raylib.DrawFPS(12, 12);

        Raylib.EndDrawing();
    }

    internal void Close() {
        //throw new NotImplementedException();
    }
}

abstract class GameEntity {
    public abstract void Draw();
    public abstract void Update();
}

abstract class GameObject {
    public abstract void Draw();
}