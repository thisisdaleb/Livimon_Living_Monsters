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
        private MapGridSystem mapManager;

        // constructor happens on the main, make it quick!
        public GameLogic()
        {
            mapManager = new MapGridSystem();
        }

        public void runInitialLogic()
        {
            mapManager.initializeWorldMap();
            GUIHandler.setMapGrid(mapManager.getCurrentMapListView());
            GUIHandler.screenUpdatePrepared();

            Thread.Sleep(3000);

            GUIHandler.setDescription("THE GAME LOGIC HAPPENED BUT ACTUALLY 4 REAL");
            GUIHandler.setTitle("AAAAAH");
            GUIHandler.screenUpdatePrepared();
        }

        public void runLogicUpdate()
        {
            //this is code to run every 10 ms

        }
    }
}
