using System;
using Raylib_cs;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace Trucle_3D
{
    class Program
    {
        public static void Main()
        {
            // initialize
            const int WINHEIGHT = 512;
            const int WINWIDTH = 1024;

            Raylib.InitWindow(WINWIDTH, WINHEIGHT, "Basic 3D");
            
            // camera --- set this right so you can actually see the stuff you put
            Camera3D camera = new()
            {
                Position = new Vector3(-50f, 25f, 25f), // up 15 backward 25
                Target = new Vector3(0.0f, -10f, 0.0f), // looking at the origin
                Up = new Vector3(0.0f, 1.0f, 0.0f), 
                FovY = 60.0f, // field of view
                Projection = CameraProjection.Perspective

            };

            // run at 60 FPS
            Raylib.SetTargetFPS(60);
            
            // player
            Vector3 player = new(-15f, -15f, 8.5f);


            // spikes
            Vector3 spike1 = new(50, 0, 8);
            int spike1Size = Raylib.GetRandomValue(3, 7);
            Vector3 spike2 = new(125, 0, 8);
            int spike2Size = Raylib.GetRandomValue(3, 7);
            
            
            // gravity
            float velocityY = 0f;
            int gravity = -15;
            float dt = 0;
            bool onGround = true;
            
            // ground
            Vector3 ground = new(75f, -15f, 5f);
            Vector3 groundS = new(250f, 10f, 25f);

            // pre-game, post-game
            bool gameOn = false;
            bool isDeath = false;
  

            while(!Raylib.WindowShouldClose())
            {

                // pre / post game stuff
                if (gameOn == false)
                {
                    if (Raylib.IsMouseButtonDown(MouseButton.Left))
                    {
                        gameOn = true;
                    }

                    // start screen
                }


                if (isDeath == true)
                {
                    if (Raylib.IsMouseButtonDown(MouseButton.Left))
                    {
                        isDeath = false;
                    }

                    // death screen
                }

                if (gameOn == true && isDeath == false)
                {
                    // update variables

                // gravity // jumping
                 dt = Raylib.GetFrameTime();
                 velocityY += gravity * dt;
                 player.Y += velocityY * -gravity * dt;
                 if (player.Y < 2.5f) {player.Y = 2.5f; velocityY = 0f; onGround = true;}
                 if (Raylib.IsKeyPressed(KeyboardKey.Space) || Raylib.IsMouseButtonPressed(MouseButton.Left) && onGround) {onGround = false; velocityY = 5f;}


                 // spikes
                 if (spike1.X < -50) // spike 1
                 {
                    spike1.X = Raylib.GetRandomValue(50, 125);
                    spike1Size = Raylib.GetRandomValue(3, 7);
                 }


                 if (spike2.X < -75)  // spike 2
                 {
                    spike2.X = Raylib.GetRandomValue(50, 125);
                    spike2Size = Raylib.GetRandomValue(3, 7);
                 }

                 spike1.X -= 1;
                 spike2.X -= 1;


                 if (
                 Raylib.CheckCollisionSpheres(player, 2.5f, spike1, spike1Size/2) ||
                 Raylib.CheckCollisionSpheres(player, 2.5f, spike2, spike2Size/2)
                 ) 
                 {
                    // game over
                    isDeath = true;
                    // player reset
                    player = new(-15f, -15f, 8.5f);
                    // spike reset
                    spike2 = new(125, 0, 8);
                    spike1 = new(50, 0, 8);
                 }

                // making sure jumps aren't too impossible
                 if (Math.Abs(spike1.X - spike2.X) < 35)
                    {
                        spike1.X += 25;
                    }
                }

                


                
                



                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);
                Raylib.BeginMode3D(camera);

 
                // Ground
                Raylib.DrawCubeV(ground, groundS, Color.Red);
                Raylib.DrawCubeWiresV(ground, groundS, Color.Black);
                
                if (gameOn == true && isDeath == false)
                {
                     // player
                    Raylib.DrawSphere(player, 2.5f, Color.Blue);

                    // draw spikes (cylinders)
                    Raylib.DrawCylinder(spike1, 0f, spike1Size/2, spike1Size, 8, Color.Gold);
                    Raylib.DrawCylinder(spike2, 0f, spike2Size/2, spike2Size, 8, Color.Gold);
                    Raylib.DrawCylinderWires(spike1, 0f, spike1Size/2, spike1Size, 8, Color.Black);
                    Raylib.DrawCylinderWires(spike2, 0f, spike2Size/2, spike2Size, 8, Color.Black);
                }


                Raylib.EndMode3D();
                if (gameOn == false)
                {
                    Raylib.DrawText("Click to start", (WINWIDTH/2) - 200, WINHEIGHT/2, 65, Color.Black);
                }          

                if (isDeath == true)
                {
                    Raylib.DrawText("Click to play again", (WINWIDTH/2) - 175, WINHEIGHT/2, 65, Color.Black);
                }      

                Raylib.EndDrawing();
            }

            // Unloading
            Raylib.CloseWindow();
        }
    }
}