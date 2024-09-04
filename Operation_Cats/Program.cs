using Meow3D;
using System.Diagnostics;
using System.Numerics;

namespace Operation_Cats
{
    internal class Program
    {
        static readonly string cat1 = " █▄▄█       ";
        static readonly string cat2 = "▐O██O▌     █";
        static readonly string cat3 = " ▀████████▀▀";
        static readonly string cat4 = "  ▐███████  ";
        static readonly string cat5 = "   █    █   ";

        static readonly string catE1 = "Meow           ";
        static readonly string catE2 = "    █▄▄█       ";
        static readonly string catE3 = "   ▐O██O▌     █";
        static readonly string catE4 = "    ▀████████▀▀";
        static readonly string catE5 = "     ▐███████  ";
        static readonly string catE6 = "      █    █   ";

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
            Stopwatch sw_ = new Stopwatch();

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

                float catX = 0f;
                float catY = 0f;
                float catZ = 0f;

                Wall[] walls = new Wall[wallNumber];

                for (int i = 0; i < walls.Length; i++)
                    walls[i] = new Wall(rnd.Next(-3500, 3500) / 1000f, 1f, rnd.Next(-5000, 1000) / 1000f, rnd.Next(0, 3000) / 1000f, -2f, (byte)rnd.Next(16, 232));

                DrawCat(catX, catY, catZ);

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

                        switch (key)
                        {
                            case ConsoleKey.W:
                                meow.MoveCameraForward(0.1f);

                                foreach (Wall w in walls)
                                {
                                    if (w.x <= meow.cameraPos.X && w.x + w.width >= meow.cameraPos.X && w.z - 0.1f <= meow.cameraPos.Z && w.z + 0.1f >= meow.cameraPos.Z)
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
                                meow.cameraRotaion *= Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), MathF.PI / fps * 1.5f);
                                break;

                            case ConsoleKey.RightArrow:
                                meow.cameraRotaion *= Quaternion.CreateFromAxisAngle(new Vector3(0, -1, 0), MathF.PI / fps * 1.5f);
                                break;
                        }

                        if (key == ConsoleKey.C)
                        {
                            if (meow.cameraPos.Z >= catZ - 1f)
                            {
                                sw.Stop();
                                Thread.Sleep(2000);
                                break;
                            }
                        }

                        if (key == ConsoleKey.Escape)
                            break;
                    }

                    if (sw.ElapsedMilliseconds >= 10000)
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

                    DrawCat(catX, catY, catZ);

                    foreach (Wall w in walls)
                        w.Draw(meow);

                    meow.Draw();
                }
            }
        }

        static void DrawCat(float x, float y, float z)
        {
            x += meow.cameraPos.X / 10;

            Vector2 pos = meow.ToScreenPos(meow.Project(Vector3.Transform(new Vector3(x, y, 4), meow.cameraRotaion), out bool draw));

            if (!draw)
                return;

            Write((int)pos.X, (int)pos.Y, cat1);
            Write((int)pos.X, (int)pos.Y + 1, cat2);
            Write((int)pos.X, (int)pos.Y + 2, cat3);
            Write((int)pos.X, (int)pos.Y + 3, cat4);
            Write((int)pos.X, (int)pos.Y + 4, cat5);

            if (meow.cameraPos.Z >= z - 1f)
                Write((int)pos.X, (int)pos.Y + 5, (10f - sw.ElapsedMilliseconds / 1000f).ToString() + "Press C");
            else
                Write((int)pos.X, (int)pos.Y + 5, (10f - sw.ElapsedMilliseconds / 1000f).ToString());
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
