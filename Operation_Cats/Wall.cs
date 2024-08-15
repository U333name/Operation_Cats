using System.Numerics;

namespace Operation_Cats
{
    internal class Wall
    {
        public float x, y, z;

        public float width, height;

        public Wall(float x, float y, float z, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.width = width;
            this.height = height;
        }

        public void Draw(Draw draw)
        {
            Vector3 pos1 = new Vector3(x, y, z);
            Vector3 pos2 = new Vector3(x + width, y, z);
            Vector3 pos3 = new Vector3(x + width, y + height, z);
            Vector3 pos4 = new Vector3(x, y + height, z);

            Vector2 pos1_ = draw.ToScreenPos(draw.Project(pos1, out bool draw1));
            Vector2 pos2_ = draw.ToScreenPos(draw.Project(pos2, out bool draw2));
            Vector2 pos3_ = draw.ToScreenPos(draw.Project(pos3, out bool draw3));
            Vector2 pos4_ = draw.ToScreenPos(draw.Project(pos4, out bool draw4));

            if (!(draw1 && draw2 && draw3 && draw4))
                return;

            draw.DrawTriangle(pos1_, pos2_, pos3_, 'W');
            draw.DrawTriangle(pos3_, pos4_, pos1_, 'W');
        }
    }
}
