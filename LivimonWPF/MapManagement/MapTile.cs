using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LivimonWPF
{

    class MapTile
    {
        private readonly string typeName;
        private string areaName;
        private SolidColorBrush tileColor;
        private bool isUnpassable = false;

        public MapTile(MapTileType _tileDef)
        {
            typeName = _tileDef.typeName;
            tileColor = _tileDef.primaryColor;
            isUnpassable = _tileDef.unpassable;
        }

        public void setAreaName(string _areaName)
        {
            areaName = _areaName;
        }

        public string getTypeName()
        {
            return typeName;
        }

        public SolidColorBrush getColor()
        {
            return tileColor;
        }

        public bool tileUnpassable()
        {
            return isUnpassable;
        }
    }

    class MapTileType
    {
        public string typeName;
        public SolidColorBrush primaryColor;
        public bool unpassable = false;

        public MapTileType(string _name, SolidColorBrush _color)
        {
            typeName = _name;
            primaryColor = _color;
        }

        public void makeUnpassable()
        {
            unpassable = true;
        }
    }
}
