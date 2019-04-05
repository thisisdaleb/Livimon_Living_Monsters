using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LivimonTestWPF
{
    class GameLogic
    {
        private MapGridSystem MapSystem;

        // constructor happens on the main, make it quick!
        public GameLogic()
        {
            MapSystem = new MapGridSystem();
        }

        public void runInitialLogic()
        {
            MapSystem.initializeWorldMap();
            GUIHandler.setFullMap(MapSystem.getFullMapListView());
            runFullGUIUpdate();
        }

        public void runLogicUpdate()
        {
            //this is code to run every 10 ms

        }

        public void runFullGUIUpdate()
        {
            GUIHandler.setDescription("You are wandering the world... ");
            GUIHandler.setTitle("The World");
            GUIHandler.setMapText(MapSystem.getTileName());
            GUIHandler.setMapGrid(MapSystem.getCurrentMapListView());
            if(MapSystem.getFullMapListNeedsUpdate()) GUIHandler.setFullMap(MapSystem.getFullMapListView());

            GUIHandler.screenUpdatePrepared();
        }

        /*
       _____ ____  _   _ _______ _____   ____  _       _____ 
      / ____/ __ \| \ | |__   __|  __ \ / __ \| |     / ____|
     | |   | |  | |  \| |  | |  | |__) | |  | | |    | (___  
     | |   | |  | | . ` |  | |  |  _  /| |  | | |     \___ \ 
     | |___| |__| | |\  |  | |  | | \ \| |__| | |____ ____) |
      \_____\____/|_| \_|  |_|  |_|  \_\\____/|______|_____/ 
                                                         
         */

        //I put the request move in here because they will not just call mapManager but also modify the player eventually

        public void playerRequestsMoveLeft()
        {
            MapSystem.moveLeft();
            runFullGUIUpdate();
        }

        public void playerRequestsMoveRight()
        {
            MapSystem.moveRight();
            runFullGUIUpdate();
        }

        public void playerRequestsMoveUp()
        {
            MapSystem.moveUp();
            runFullGUIUpdate();
        }

        public void playerRequestsMoveDown()
        {
            MapSystem.moveDown();
            runFullGUIUpdate();
        }

    }
}
