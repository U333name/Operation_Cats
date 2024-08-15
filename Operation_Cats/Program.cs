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

        static Draw draw = new Draw();
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

            while (true)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                draw = new Draw();
                sw = new Stopwatch();

                float catX = 0f;
                float catY = 0f;
                float catZ = 0f;

                Wall[] walls = new Wall[wallNumber];

                for (int i = 0; i < walls.Length; i++)
                    walls[i] = new Wall(rnd.Next(-3500, 3500) / 1000f, 1f, rnd.Next(-5000, 1000) / 1000f, rnd.Next(0, 2000) / 1000f, -2f);

                DrawCat(catX, catY, catZ);

                try
                {
                    Console.CursorVisible = false;
                }
                catch { }

                sw.Start();

                while (true)
                {
                    draw.Update();
                    draw.Clear();

                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;

                        bool wall = false;

                        switch (key)
                        {
                            case ConsoleKey.W:
                                draw.cameraPos.Z += 0.1f;

                                foreach (Wall w in walls)
                                {
                                    if (w.x <= draw.cameraPos.X && w.x + w.width >= draw.cameraPos.X && w.z - 0.1f <= draw.cameraPos.Z && w.z + 0.1f >= draw.cameraPos.Z)
                                    {
                                        wall = true;
                                        break;
                                    }
                                }

                                if (wall)
                                    draw.cameraPos.Z -= 0.1f;
                                break;

                            case ConsoleKey.S:
                                draw.cameraPos.Z -= 0.1f;

                                foreach (Wall w in walls)
                                {
                                    if (w.x <= draw.cameraPos.X && w.x + w.width >= draw.cameraPos.X && w.z - 0.1f <= draw.cameraPos.Z && w.z + 0.1f >= draw.cameraPos.Z)
                                    {
                                        wall = true;
                                        break;
                                    }
                                }

                                if (wall)
                                    draw.cameraPos.Z += 0.1f;
                                break;

                            case ConsoleKey.A:
                                if (draw.cameraPos.X < 2.5f)
                                    draw.cameraPos.X += 0.1f;

                                foreach (Wall w in walls)
                                {
                                    if (w.x <= draw.cameraPos.X && w.x + w.width >= draw.cameraPos.X && w.z - 0.1f <= draw.cameraPos.Z && w.z + 0.1f >= draw.cameraPos.Z)
                                    {
                                        wall = true;
                                        break;
                                    }
                                }

                                if (wall)
                                    draw.cameraPos.X -= 0.1f;
                                break;

                            case ConsoleKey.D:
                                if (draw.cameraPos.X > -2.5f)
                                    draw.cameraPos.X -= 0.1f;

                                foreach (Wall w in walls)
                                {
                                    if (w.x <= draw.cameraPos.X && w.x + w.width >= draw.cameraPos.X && w.z - 0.1f <= draw.cameraPos.Z && w.z + 0.1f >= draw.cameraPos.Z)
                                    {
                                        wall = true;
                                        break;
                                    }
                                }

                                if (wall)
                                    draw.cameraPos.X += 0.1f;
                                break;
                        }

                        if (key == ConsoleKey.C)
                        {
                            if (draw.cameraPos.Z >= catZ - 1f)
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

                    DrawCat(catX, catY, catZ);

                    foreach (Wall w in walls)
                        w.Draw(draw);

                    draw.Draw_();
                }
            }
        }

        static void DrawCat(float x, float y, float z)
        {
            x += draw.cameraPos.X / 10;
            Vector2 pos = draw.ToScreenPos(new Vector2(x, y));

            draw.Write((int)pos.X, (int)pos.Y, cat1);
            draw.Write((int)pos.X, (int)pos.Y + 1, cat2);
            draw.Write((int)pos.X, (int)pos.Y + 2, cat3);
            draw.Write((int)pos.X, (int)pos.Y + 3, cat4);
            draw.Write((int)pos.X, (int)pos.Y + 4, cat5);

            if (draw.cameraPos.Z >= z - 1f)
                draw.Write((int)pos.X, (int)pos.Y + 5, (10f - sw.ElapsedMilliseconds / 1000f).ToString() + "Press C");
            else
                draw.Write((int)pos.X, (int)pos.Y + 5, (10f - sw.ElapsedMilliseconds / 1000f).ToString());
        }

        static void DrawCatE()
        {
            Console.Clear();

            int x = draw.width / 2 - catE1.Length / 2;
            int y = draw.height / 2 - catE1.Length / 2;

            draw.Write(x, y, catE1);
            draw.Write(x, y + 1, catE2);
            draw.Write(x, y + 2, catE3);
            draw.Write(x, y + 3, catE4);
            draw.Write(x, y + 4, catE5);
            draw.Write(x, y + 5, catE6);
            draw.Draw_();
        }
    }
}
