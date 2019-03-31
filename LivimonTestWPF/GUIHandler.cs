using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

//handler is probably the incorrect name
//This allows us to replace the GUI whenever we want
namespace LivimonTestWPF
{
    //the Game will hand off info to this class, which will call the appropriate functions inside of the GUI class to pass it back in
    static class GUIHandler
    {
        /////////////////////////
        //use to find out whether to delete or not
        /////////////////////////
        private static bool screenUpdateReady = false;
        public static bool isReadyToUpdate()
        {
            if (screenUpdateReady)
            {
                screenUpdateReady = false;
                return true;
            }
            return false;
        }
        public static void screenUpdatePrepared()
        {
            screenUpdateReady = true;
        }

        /////////////////////////
        //modify main description
        /////////////////////////
        static string description = "";
        static bool hasNewDescription = false;
        public static bool descriptionChanged()
        {
            return hasNewDescription;
        }
        public static string getDescription()
        {
            hasNewDescription = false;
            return description;
        }
        public static void setDescription(string _newValue)
        {
            if (description != _newValue)
            {
                hasNewDescription = true;
                description = _newValue;
            }
        }

        /////////////////////////
        //modify title
        /////////////////////////
        static string title = "";
        static bool hasNewTitle = false;
        public static bool titleChanged()
        {
            return hasNewTitle;
        }
        public static string getTitle()
        {
            hasNewTitle = false;
            return title;
        }
        public static void setTitle(string _newValue)
        {
            if (title != _newValue)
            {
                hasNewTitle = true;
                title = _newValue;
            }
        }

        /////////////////////////
        //modify map text
        /////////////////////////
        static string mapText = "";
        static bool hasMapText = false;
        public static bool mapTextChanged()
        {
            return hasMapText;
        }
        public static string getMapText()
        {
            hasMapText = false;
            return mapText;
        }
        public static void setMapText(string _newValue)
        {
            if (mapText != _newValue)
            {
                hasMapText = true;
                mapText = _newValue;
            }
        }

        /////////////////////////
        //modify map grid
        /////////////////////////
        static RectangleUpdate[,] mapGrid;
        static bool hasMapGrid = false;
        public static bool mapGridChanged()
        {
            return hasMapGrid;
        }
        public static RectangleUpdate[,] getMapGrid()
        {
            hasMapGrid = false;
            return mapGrid;
        }
        //must assume that if game logic is trying to set the grid, it's changed. It's too complex to put that check in GUIHandler.
        public static void setMapGrid(RectangleUpdate[,] _newValue)
        {
            hasMapGrid = true;
            mapGrid = _newValue;
        }
    }
    
    public class RectangleUpdate
    {
        public Brush color = null;
        public string tooltip = null;
        public bool turnOffRectangle = false;

        public RectangleUpdate(Brush _color, string _tooltip = null, bool _turnOff = false)
        {
            color = _color;
            tooltip = _tooltip;
            turnOffRectangle = _turnOff;
        }

        public RectangleUpdate()
        {
            turnOffRectangle = true;
        }
    }
}
