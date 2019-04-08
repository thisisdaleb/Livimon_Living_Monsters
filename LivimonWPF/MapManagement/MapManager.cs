namespace LivimonWPF
{
    abstract class MapManager
    {
        protected Map map;
        public MapManager()
        {
            map = makeMap();
        }

        protected abstract Map makeMap();

        public Map getMap()
        {
            return map;
        }

    }

    class Map
    {
        public MapTile[,] map;

        public Map(int _y, int _x)
        {
            map = new MapTile[_y, _x];
        }

    }
}
