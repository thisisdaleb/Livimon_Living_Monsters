using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Threading;
using Button = System.Windows.Controls.Button;

namespace LivimonTestWPF
{
    public partial class MainWindow : Window
    {
        private bool currentlyUpdatingGUI = false;
        GameLogic gameLogic;
        GameController controller;

        //ThreadExampleViewModel viewModel = new ThreadExampleViewModel();
        //https://arcanecode.com/2007/09/07/adding-wpf-controls-progrrammatically/ look at this
        //https://www.c-sharpcorner.com/UploadFile/mahesh/openfiledialog-in-wpf/ and this
        //consider https://www.codeproject.com/Articles/165368/WPF-MVVM-Quick-Start-Tutorial so you have good code practices?
        
         /*
           _____ _    _ _____    ________      ________ _   _ _______    _    _          _   _ _____  _      ______ _____   _____ 
          / ____| |  | |_   _|  |  ____\ \    / /  ____| \ | |__   __|  | |  | |   /\   | \ | |  __ \| |    |  ____|  __ \ / ____|
         | |  __| |  | | | |    | |__   \ \  / /| |__  |  \| |  | |     | |__| |  /  \  |  \| | |  | | |    | |__  | |__) | (___  
         | | |_ | |  | | | |    |  __|   \ \/ / |  __| | . ` |  | |     |  __  | / /\ \ | . ` | |  | | |    |  __| |  _  / \___ \ 
         | |__| | |__| |_| |_   | |____   \  /  | |____| |\  |  | |     | |  | |/ ____ \| |\  | |__| | |____| |____| | \ \ ____) |
          \_____|\____/|_____|  |______|   \/   |______|_| \_|  |_|     |_|  |_/_/    \_\_| \_|_____/|______|______|_|  \_\_____/ 
                                                                                                                          
        */

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeSystem();
            textBlock.Text = "This will contain a description of your surroundings, actions performed by your livimon, dialog from NPCs, combat events, queries asking you what to do next, or general story text.";

            Thread logicThread = new Thread(runLogicUpdate);
            logicThread.IsBackground = true;
            logicThread.Start();

            Thread thread = new Thread(runGuiUpdate);
            thread.IsBackground = true;
            thread.Start();
        }

        /*
          _____ _   _ _____ _______ _____          _      _____ ____________     _____          __  __ ______ 
         |_   _| \ | |_   _|__   __|_   _|   /\   | |    |_   _|___  /  ____|   / ____|   /\   |  \/  |  ____|
           | | |  \| | | |    | |    | |    /  \  | |      | |    / /| |__     | |  __   /  \  | \  / | |__   
           | | | . ` | | |    | |    | |   / /\ \ | |      | |   / / |  __|    | | |_ | / /\ \ | |\/| |  __|  
          _| |_| |\  |_| |_   | |   _| |_ / ____ \| |____ _| |_ / /__| |____   | |__| |/ ____ \| |  | | |____ 
         |_____|_| \_|_____|  |_|  |_____/_/    \_\______|_____/_____|______|   \_____/_/    \_\_|  |_|______|
          _____ _   _ _____ _______ _____          _      _____ ____________     _____ ____  _   _ _______ _____   ____  _      _      ______ _____  
         |_   _| \ | |_   _|__   __|_   _|   /\   | |    |_   _|___  /  ____|   / ____/ __ \| \ | |__   __|  __ \ / __ \| |    | |    |  ____|  __ \ 
           | | |  \| | | |    | |    | |    /  \  | |      | |    / /| |__     | |   | |  | |  \| |  | |  | |__) | |  | | |    | |    | |__  | |__) |
           | | | . ` | | |    | |    | |   / /\ \ | |      | |   / / |  __|    | |   | |  | | . ` |  | |  |  _  /| |  | | |    | |    |  __| |  _  / 
          _| |_| |\  |_| |_   | |   _| |_ / ____ \| |____ _| |_ / /__| |____   | |___| |__| | |\  |  | |  | | \ \| |__| | |____| |____| |____| | \ \ 
         |_____|_| \_|_____|  |_|  |_____/_/    \_\______|_____/_____|______|   \_____\____/|_| \_|  |_|  |_|  \_\\____/|______|______|______|_|  \_\
                                                                                                                                             
        */

        private void InitializeSystem()
        {
            //not on background thread, make sure it's quick
            gameLogic = new GameLogic();
            controller = new GameController(gameLogic);
        }

        /*
          _      ____   _____ _____ _____   _    _ _____  _____       _______ ______   ______ _    _ _   _  _____ _______ _____ ____  _   _  _____ 
         | |    / __ \ / ____|_   _/ ____| | |  | |  __ \|  __ \   /\|__   __|  ____| |  ____| |  | | \ | |/ ____|__   __|_   _/ __ \| \ | |/ ____|
         | |   | |  | | |  __  | || |      | |  | | |__) | |  | | /  \  | |  | |__    | |__  | |  | |  \| | |       | |    | || |  | |  \| | (___  
         | |   | |  | | | |_ | | || |      | |  | |  ___/| |  | |/ /\ \ | |  |  __|   |  __| | |  | | . ` | |       | |    | || |  | | . ` |\___ \ 
         | |___| |__| | |__| |_| || |____  | |__| | |    | |__| / ____ \| |  | |____  | |    | |__| | |\  | |____   | |   _| || |__| | |\  |____) |
         |______\____/ \_____|_____\_____|  \____/|_|    |_____/_/    \_\_|  |______| |_|     \____/|_| \_|\_____|  |_|  |_____\____/|_| \_|_____/ 
                                                                                                                                           
        */

        //10ms = 100 updates per seconds
        private void runLogicUpdate()
        {
            gameLogic.runInitialLogic();
            while (true)
            {
                Thread.Sleep(10);
                if(!currentlyUpdatingGUI) gameLogic.runLogicUpdate(); //This may not be needed, except as per the idea of updating text over time
            }
        }
        /*
           _____ _    _ _____    ________      ________ _   _ _______    _    _          _   _ _____  _      ______ _____   _____ 
          / ____| |  | |_   _|  |  ____\ \    / /  ____| \ | |__   __|  | |  | |   /\   | \ | |  __ \| |    |  ____|  __ \ / ____|
         | |  __| |  | | | |    | |__   \ \  / /| |__  |  \| |  | |     | |__| |  /  \  |  \| | |  | | |    | |__  | |__) | (___  
         | | |_ | |  | | | |    |  __|   \ \/ / |  __| | . ` |  | |     |  __  | / /\ \ | . ` | |  | | |    |  __| |  _  / \___ \ 
         | |__| | |__| |_| |_   | |____   \  /  | |____| |\  |  | |     | |  | |/ ____ \| |\  | |__| | |____| |____| | \ \ ____) |
          \_____|\____/|_____|  |______|   \/   |______|_| \_|  |_|     |_|  |_/_/    \_\_| \_|_____/|______|______|_|  \_\_____/ 
                                                                                                                          
        */

        private void click_buttonPressed(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            System.Diagnostics.Debug.WriteLine(clicked.Name);
        }

        /*
           _____ _    _ _____    _____  _____       __          __   ______ _    _ _   _  _____ _______ _____ ____  _   _  _____ 
          / ____| |  | |_   _|  |  __ \|  __ \     /\ \        / /  |  ____| |  | | \ | |/ ____|__   __|_   _/ __ \| \ | |/ ____|
         | |  __| |  | | | |    | |  | | |__) |   /  \ \  /\  / /   | |__  | |  | |  \| | |       | |    | || |  | |  \| | (___  
         | | |_ | |  | | | |    | |  | |  _  /   / /\ \ \/  \/ /    |  __| | |  | | . ` | |       | |    | || |  | | . ` |\___ \ 
         | |__| | |__| |_| |_   | |__| | | \ \  / ____ \  /\  /     | |    | |__| | |\  | |____   | |   _| || |__| | |\  |____) |
          \_____|\____/|_____|  |_____/|_|  \_\/_/    \_\/  \/      |_|     \____/|_| \_|\_____|  |_|  |_____\____/|_| \_|_____/ 

        */
        //consider rewriting this to use onPropertyChanged(?) so people don't laugh at bad code
        //33ms = 30 frames per second
        private void runGuiUpdate()
        {
            while (true)
            {
                Thread.Sleep(33); 
                Dispatcher.Invoke(() =>
                {
                    if (GUIHandler.isReadyToUpdate())
                    {
                        updateGUI();
                    }
                });
            }
        }

        private void updateGUI()
        {
            currentlyUpdatingGUI = true;

            updateDescription();
            updateTitle();

            currentlyUpdatingGUI = false;
        }

        private void updateDescription()
        {
            if (GUIHandler.descriptionChanged()) textBlock.Text = GUIHandler.getDescription();
        }

        private void updateTitle()
        {
            if (GUIHandler.titleChanged()) TitleText.Text = GUIHandler.getTitle();
        }
    }
}
