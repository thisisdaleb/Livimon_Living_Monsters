using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        static String description = "";
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
        public static void setDescription(String _newValue)
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
        static String title = "";
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
        public static void setTitle(String _newValue)
        {
            if (title != _newValue)
            {
                hasNewTitle = true;
                title = _newValue;
            }
        }
    }
}
