using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LivimonTestWPF
{

    class MapTile
    {
        private readonly string typeName;
        private string areaName;
        private SolidColorBrush tileColor;

        public MapTile(MapTileDef _tileDef)
        {
            typeName = _tileDef.typeName;
            tileColor = _tileDef.primaryColor;
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
    }

    class MapTileDef
    {
        public string typeName;
        public SolidColorBrush primaryColor;
        public MapTileDef(string _name, SolidColorBrush _color)
        {
            typeName = _name;
            primaryColor = _color;
        }
    }
}
