//handler is probably the incorrect name
//This allows us to replace the GUI whenever we want
using System;

namespace LivimonTestWPF
{
    //the GUI will hand off info to this class, which will call the appropriate functions inside of the game logic class to pass it back in
    class GameController
    {
        private GameLogic gameLogic;

        public GameController(GameLogic gameLogic)
        {
            this.gameLogic = gameLogic;
        }

        public void clickedLeft()
        {
            gameLogic.playerRequestsMoveLeft();
        }

        public void clickedRight()
        {
            gameLogic.playerRequestsMoveRight();
        }

        public void clickedUp()
        {
            gameLogic.playerRequestsMoveUp();
        }

        public void clickedDown()
        {
            gameLogic.playerRequestsMoveDown();
        }
    }
}
