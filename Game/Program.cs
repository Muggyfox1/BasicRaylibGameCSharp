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
    Vector3 cameraOffset = new Vector3(0, 3, -4);
    Vector2 cameraRotation = new Vector2(0, 0);

    bool isMouseLocked = false;
    Vector2 mouseDeltaThisFrame = Vector2.Zero;
    float mouseSensitivity = 0.1f;

    Vector3 playerPosition = Vector3.Zero;
    float playerSpeed = 5f;

    internal void Init() {
        Raylib.InitWindow(800, 480, "Hello, World");
        Raylib.SetTargetFPS(60);

        camera.Position = cameraOffset;
        camera.Target = Vector3.Zero;
        camera.Up = new Vector3(0, 1, 0);
        camera.FovY = 45;
        camera.Projection = CameraProjection.Perspective;

        Raylib.DisableCursor();
        isMouseLocked = true;
    }

    internal void Update() {
        float deltaTime = Raylib.GetFrameTime();

        if(Raylib.IsKeyPressed(KeyboardKey.Escape)) {
            isMouseLocked = !isMouseLocked;
            if(isMouseLocked) {
                Raylib.DisableCursor();
            }
            else {
                Raylib.EnableCursor();
            }
        }

        mouseDeltaThisFrame = Raylib.GetMouseDelta();
        if(isMouseLocked) {
            cameraRotation.X += mouseDeltaThisFrame.X * mouseSensitivity; // yaw
            cameraRotation.Y += mouseDeltaThisFrame.Y * mouseSensitivity; // pitch
            cameraRotation.Y = Math.Clamp(cameraRotation.Y, -89f, 89f);
        }

        float yawRad = cameraRotation.X * Raylib.DEG2RAD;
        float yawRadSin = MathF.Sin(yawRad);
        float yawRadCos = MathF.Cos(yawRad);

        float pitchRad = cameraRotation.Y * Raylib.DEG2RAD;
        float pitchRadSin = MathF.Sin(pitchRad);
        float pitchRadCos = MathF.Cos(pitchRad);

        float moveInputX = Raylib.IsKeyDown(KeyboardKey.A) ? 1 : Raylib.IsKeyDown(KeyboardKey.D) ? -1 : 0;
        float moveInputY = Raylib.IsKeyDown(KeyboardKey.W) ? 1 : Raylib.IsKeyDown(KeyboardKey.S) ? -1 : 0;
        Vector2 playerInput = new Vector2(moveInputX, moveInputY);
        
        Vector3 cameraForward = Vector3.Normalize(new Vector3(yawRadSin, 0f, yawRadCos));
        Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, cameraForward));
        
        Vector3 moveDirection = (cameraRight * playerInput.X) + (cameraForward * playerInput.Y);
        if(moveDirection.LengthSquared() > 0f) {
            moveDirection = Vector3.Normalize(moveDirection);
        }

        playerPosition += moveDirection * playerSpeed * deltaTime;

        float orbitDistance = cameraOffset.Length();
        Vector3 orbitDirection = new Vector3(
            pitchRadCos * yawRadSin,
            pitchRadSin,
            pitchRadCos * yawRadCos
        );

        camera.Position = playerPosition - orbitDirection * orbitDistance;
        camera.Target = playerPosition;
        //Raylib.UpdateCamera(ref camera, CameraMode.ThirdPerson);
    }

    internal void Draw() {
        //draw UI
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.White);

        Raylib.BeginMode3D(camera);
        {
            Raylib.DrawGrid(10, 1);

            //draw player
            Raylib.DrawCube(playerPosition, 1, 1, 1, Color.Red);
        }

        Raylib.EndMode3D();

        //draw FPS with panel
        Raylib.DrawRectangle(10, 10, 108, 20, new Color(0, 0, 0, 100));
        Raylib.DrawFPS(12, 12);

        //Debug Text
        //Raylib.DrawText(cameraRotation.ToString(), 12, 32, 20, Color.Black);

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