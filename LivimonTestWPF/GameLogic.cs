﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LivimonTestWPF
{
    class GameLogic
    {
        // constructor
        public GameLogic()
        {
            
        }

        public void runInitialLogic()
        {
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
