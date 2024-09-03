using Meow3D;
using System.Numerics;

namespace Operation_Cats
{
    internal class Wall
    {
        public float x, y, z;

        public float width, height;

        public byte color;

        public Wall(float x, float y, float z, float width, float height, byte color)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.width = width;
            this.height = height;
            this.color = color;
        }

        public void Draw(Meow meow)
        {
            Vector3 pos1 = new Vector3(x, y, z);
            Vector3 pos2 = new Vector3(x + width, y, z);
            Vector3 pos3 = new Vector3(x + width, y + height, z);
            Vector3 pos4 = new Vector3(x, y + height, z);

            meow.DrawTriangles([pos1, pos2, pos3, pos4], [0, 1, 2, 2, 3, 0], [color, color, color, color, color, color], 'W');
        }
    }
}
