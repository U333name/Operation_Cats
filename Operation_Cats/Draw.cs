using System.Numerics;
using System.Text;

namespace Operation_Cats
{
    internal class Draw
    {
        private char[,] buffer;
        private int[,] contourX;
        public int width;
        public int height;

        public Vector3 cameraPos;

        public Draw()
        {
            buffer = new char[Console.WindowWidth, Console.WindowHeight];
            width = Console.WindowWidth;
            height = Console.WindowHeight;
            contourX = new int[height, 2];
            cameraPos = new Vector3(0, 0, -10);
        }

        public void Clear()
        {
            for (int i = 0; i < buffer.GetLength(0); i++)
            {
                for (int j = 0; j < buffer.GetLength(1); j++)
                {
                    buffer[i, j] = ' ';
                }
            }
        }

        public void Write(int x, int y, char c)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
                return;

            buffer[x, y] = c;
        }

        public void Update()
        {
            if (Console.WindowWidth != width || Console.WindowHeight != height)
            {
                buffer = new char[Console.WindowWidth, Console.WindowHeight];
                width = Console.WindowWidth;
                height = Console.WindowHeight;
                contourX = new int[height, 2];

                try
                {
                    Console.CursorVisible = false;
                }
                catch { }
            }
        }

        public void Write(int x, int y, string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (x + i >= width)
                    break;

                if (x < 0 || y < 0 || y >= height)
                    break;

                buffer[x + i, y] = s[i];
            }
        }

        public void Draw_()
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < buffer.GetLength(1); i++)
            {
                StringBuilder sb = new StringBuilder();

                for (int j = 0; j < buffer.GetLength(0); j++)
                {
                    if (i == buffer.GetLength(1) - 1 && j == buffer.GetLength(0) - 1)
                        continue;

                    sb.Append(buffer[j, i]);
                }
                
                Console.Write(sb.ToString());
            }
        }

        public Vector2 Project(Vector3 vec, out bool draw)
        {
            draw = cameraPos.Z < vec.Z;
            vec -= cameraPos;
            Vector4 vec4 = new Vector4(vec.X, vec.Y, vec.Z * -2.020202f, vec.Z * -1f);
            vec4 /= vec4.W;
            return new Vector2(vec4.X, vec4.Y);
        }

        public Vector2 ToScreenPos(Vector2 vec)
        {
            float x = width / 2f; ;
            x += vec.X * (width / 2f);
            float y = height / 2f;
            y += vec.Y * (height / 2f);
            return new Vector2(x, y);
        }

        private void ScanLine(int x1, int y1, int x2, int y2)
        {
            int sx = 0, sy = 0, dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0, x = 0, y = 0, m = 0, n = 0, k = 0, cnt = 0;

            sx = x2 - x1;
            sy = y2 - y1;

            if (sx > 0)
                dx1 = 1;
            else if (sx < 0)
                dx1 = -1;
            else
                dx2 = 0;

            if (sy > 0)
                dy1 = 1;
            else if (sy < 0)
                dy1 = -1;
            else
                dy1 = 0;

            m = Math.Abs(sx);
            n = Math.Abs(sy);
            dx2 = dx1;
            dy2 = 0;

            if (m < n)
            {
                m = Math.Abs(sy);
                n = Math.Abs(sx);
                dx2 = 0;
                dy2 = dy1;
            }

            x = x1;
            y = y1;
            cnt = m + 1;
            k = n / 2;

            while (cnt > 0)
            {
                cnt--;

                if ((y >= 0) && (y < height))
                {
                    if (x < contourX[y, 0])
                        contourX[y, 0] = x;

                    if (x > contourX[y, 1])
                        contourX[y, 1] = x;
                }

                k += n;

                if (k < m)
                {
                    x += dx2;
                    y += dy2;
                }
                else
                {
                    k -= m;
                    x += dx1;
                    y += dy1;
                }
            }
        }

        public void DrawTriangle(Vector2 p0, Vector2 p1, Vector2 p2, char c)
        {
            int y;

            for (y = 0; y < height; y++)
            {
                contourX[y, 0] = int.MaxValue;
                contourX[y, 1] = int.MinValue;
            }
            
            ScanLine((int)p0.X, (int)p0.Y, (int)p1.X, (int)p1.Y);
            ScanLine((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y);
            ScanLine((int)p2.X, (int)p2.Y, (int)p0.X, (int)p0.Y);
            
            for (y = 0; y < height; y++)
            {
                if (contourX[y, 1] >= contourX[y, 0])
                {
                    int x = contourX[y, 0];
                    int len = 1 + contourX[y, 1] - contourX[y, 0];

                    while (len > 0)
                    {
                        len--;

                        Write(x++, y, c);
                    }
                }
            }
        }
    }
}
