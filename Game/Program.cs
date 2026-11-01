using Raylib_cs;
using System.Numerics;

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

    Camera3D camera = new Camera3D();
    internal void Init() {
        Raylib.InitWindow(800, 480, "Hello, World");
        Raylib.SetTargetFPS(60);

        camera.Position = new Vector3(0, 10, 10);
        camera.Target = Vector3.Zero;
        camera.Up = new Vector3(0, 1, 0);
        camera.FovY = 45;
        camera.Projection = CameraProjection.Perspective;
    }

    internal void Update() {
        Raylib.UpdateCamera(ref camera, CameraMode.Orbital);
    }

    internal void Draw() {
        //draw UI
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.White);

        Raylib.BeginMode3D(camera);
        Raylib.DrawGrid(10, 1);
        Raylib.DrawCube(Vector3.Zero, 1, 1, 1, Color.Red);
        Raylib.EndMode3D();

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