using Meow3D;
using System.Diagnostics;
using System.Numerics;

namespace Operation_Cats
{
    internal class Program
    {
        static readonly string catE1 = "Meow           ";
        static readonly string catE2 = "    █▄▄█       ";
        static readonly string catE3 = "   ▐O██O▌     █";
        static readonly string catE4 = "    ▀████████▀▀";
        static readonly string catE5 = "     ▐███████  ";
        static readonly string catE6 = "      █    █   ";

        static readonly string cat1 = " █▄▄█       ";
        static readonly string cat2 = "▐O██O▌     █";
        static readonly string cat3 = " ▀████████▀▀";
        static readonly string cat4 = "  ▐███████  ";
        static readonly string cat5 = "   █    █   ";

        static Meow meow = new Meow();
        static Stopwatch sw = new Stopwatch();

        static void Main(string[] args)
        {
            int wallNumber = 0;

            while (true)
            {
                Console.Write("Write wall number(10-50): ");
                string? s = Console.ReadLine();

                if (int.TryParse(s, out wallNumber) && wallNumber >= 10 && wallNumber <= 50)
                    break;
                else
                    Console.WriteLine("Write correct number");
            }

            Random rnd = new Random(DateTime.UtcNow.GetHashCode());
            uint fps = 80;
            uint frames = 0;
            Stopwatch sw_ = Stopwatch.StartNew();

            bool pause = false;

            while (true)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                meow = new Meow()
                {
                    cameraPos = new Vector3(0, 0, -5)
                };

                sw = new Stopwatch();

                float catX = rnd.Next(-2000, 2000) / 1000f;
                float catY = 0f;
                float catZ = 0f;

                Wall[] walls = new Wall[wallNumber];

                float wallZ = -4f;

                for (int i = 0; i < walls.Length; i++)
                {
                    walls[i] = new Wall(rnd.Next(-2500, 2500) / 1000f, 1f, wallZ, rnd.Next(0, 2000) / 1000f, -2f, (byte)rnd.Next(16, 232));
                    wallZ += 0.4f;
                }

                try
                {
                    Console.CursorVisible = false;
                }
                catch { }

                sw.Start();

                while (true)
                {
                    meow.UpdateSize();
                    meow.Clear();

                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;

                        bool wall = false;

                        if (key == ConsoleKey.P)
                        {
                            pause = !pause;
                        }

                        if (pause)
                        {
                            sw.Stop();

                            Thread.Sleep(100);
                            continue;
                        }

                        if (!pause && !sw.IsRunning)
                        {
                            sw.Start();
                        }

                        switch (key)
                        {
                            case ConsoleKey.W:
                                meow.MoveCameraForward(0.1f);

                                if (meow.cameraPos.X <= -2.5f)
                                    meow.cameraPos.X = -2.45f;

                                if (meow.cameraPos.X >= 2.5f)
                                    meow.cameraPos.X = 2.45f;

                                foreach (Wall w in walls)
                                {
                                    if (w.x <= meow.cameraPos.X && w.x + w.width >= meow.cameraPos.X && w.z - 0.05f <= meow.cameraPos.Z && w.z + 0.05f >= meow.cameraPos.Z)
                                    {
                                        wall = true;
                                        break;
                                    }
                                }

                                if (wall)
                                    meow.MoveCameraBackward(0.1f);
                                break;

                            case ConsoleKey.S:
                                meow.MoveCameraBackward(0.1f);

                                if (meow.cameraPos.X <= -2.5f)
                                    meow.cameraPos.X = -2.45f;

                                if (meow.cameraPos.X >= 2.5f)
                                    meow.cameraPos.X = 2.45f;

                                foreach (Wall w in walls)
                                {
                                    if (w.x <= meow.cameraPos.X && w.x + w.width >= meow.cameraPos.X && w.z - 0.1f <= meow.cameraPos.Z && w.z + 0.1f >= meow.cameraPos.Z)
                                    {
                                        wall = true;
                                        break;
                                    }
                                }

                                if (wall)
                                    meow.MoveCameraForward(0.1f);
                                break;

                            case ConsoleKey.A:
                                meow.MoveCameraLeft(0.1f);

                                if (meow.cameraPos.X <= -2.5f)
                                    meow.cameraPos.X = -2.45f;

                                if (meow.cameraPos.X >= 2.5f)
                                    meow.cameraPos.X = 2.45f;

                                foreach (Wall w in walls)
                                {
                                    if (w.x <= meow.cameraPos.X && w.x + w.width >= meow.cameraPos.X && w.z - 0.1f <= meow.cameraPos.Z && w.z + 0.1f >= meow.cameraPos.Z)
                                    {
                                        wall = true;
                                        break;
                                    }
                                }

                                if (wall)
                                    meow.MoveCameraRight(0.1f);
                                break;

                            case ConsoleKey.D:
                                meow.MoveCameraRight(0.1f);

                                if (meow.cameraPos.X <= -2.5f)
                                    meow.cameraPos.X = -2.45f;

                                if (meow.cameraPos.X >= 2.5f)
                                    meow.cameraPos.X = 2.45f;

                                foreach (Wall w in walls)
                                {
                                    if (w.x <= meow.cameraPos.X && w.x + w.width >= meow.cameraPos.X && w.z - 0.1f <= meow.cameraPos.Z && w.z + 0.1f >= meow.cameraPos.Z)
                                    {
                                        wall = true;
                                        break;
                                    }
                                }

                                if (wall)
                                    meow.MoveCameraLeft(0.1f);
                                break;

                            case ConsoleKey.LeftArrow:
                                meow.cameraRotaion *= Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), MathF.PI / fps * 2.5f);
                                break;

                            case ConsoleKey.RightArrow:
                                meow.cameraRotaion *= Quaternion.CreateFromAxisAngle(new Vector3(0, -1, 0), MathF.PI / fps * 2.5f);
                                break;
                        }

                        if (key == ConsoleKey.C)
                        {
                            if (Vector3.Distance(meow.cameraPos, new Vector3(catX, catY, catZ)) <= 0.5f)
                            {
                                sw.Stop();
                                Thread.Sleep(2000);
                                break;
                            }
                        }

                        if (key == ConsoleKey.Escape)
                            break;
                    }

                    if (sw.ElapsedMilliseconds >= 15000)
                    {
                        DrawCatE();
                        Console.Beep();
                        Thread.Sleep(2000);
                        break;
                    }

                    if (sw_.ElapsedMilliseconds >= 1000)
                    {
                        fps = frames;
                        frames = 0;
                        sw_.Restart();
                    }

                    frames++;

                    if (OperatingSystem.IsWindows())
                    {
                        Console.Title = $"Operation_Cats FPS: {fps} Walls: {wallNumber}";
                    }

                    DrawCat(catX, catY, catZ);  

                    foreach (Wall w in walls)
                        w.Draw(meow);

                    meow.Draw();
                }
            }
        }

        static void DrawCat(float x, float y, float z)
        {
            Vector3 vec = Vector3.Transform(new Vector3(meow.cameraPos.X, 0, 0), meow.cameraRotaion);

            Vector3 pos_ = Vector3.Transform(new Vector3(x, y, 0) - vec, meow.cameraRotaion);
            Vector2 pos = meow.ToScreenPos(new Vector2(pos_.X, pos_.Y));

            int _x = (int)pos.X;
            int _y = (int)pos.Y;

            Write(_x, _y, cat1);
            Write(_x, _y + 1, cat2);
            Write(_x, _y + 2, cat3);
            Write(_x, _y + 3, cat4);
            Write(_x, _y + 4, cat5);

            if (Vector3.Distance(meow.cameraPos, new Vector3(x, y, z)) <= 0.5f)
                Write(_x, _y + 5, (15 - sw.Elapsed.TotalSeconds).ToString() + " Press C");
            else
                Write(_x, _y + 5, (15 - sw.Elapsed.TotalSeconds).ToString());
        }

        static void DrawCatE()
        {
            Console.Clear();

            int x = meow.width / 2 - catE1.Length / 2;
            int y = meow.height / 2 - 2;

            meow.Clear();
            Write(x, y, catE1);
            Write(x, y + 1, catE2);
            Write(x, y + 2, catE3);
            Write(x, y + 3, catE4);
            Write(x, y + 4, catE5);
            Write(x, y + 5, catE6);
            meow.Draw();
        }

        static void Write(int x, int y, string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                meow.Write(i + x, y, s[i], 15);
            }
        }
    }
}
