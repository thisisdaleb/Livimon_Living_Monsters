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

namespace LivimonWPF
{
    public partial class MainWindow : Window
    {
        //ThreadExampleViewModel viewModel = new ThreadExampleViewModel();
        //https://arcanecode.com/2007/09/07/adding-wpf-controls-progrrammatically/ look at this
        //https://www.c-sharpcorner.com/UploadFile/mahesh/openfiledialog-in-wpf/ and this
        //consider https://www.codeproject.com/Articles/165368/WPF-MVVM-Quick-Start-Tutorial so you have good code practices?
        //http://patorjk.com/software/taag/#p=display&f=Big&t=Type%20Something%20 for text

        private bool currentlyUpdatingGUI = false;
        GameLogic gameLogic;
        GameController controller;
        Rectangle[,] mapCellGrid;

        /*
          _____ _______       _____ _______ _    _ _____  
         / ____|__   __|/\   |  __ \__   __| |  | |  __ \ 
        | (___    | |  /  \  | |__) | | |  | |  | | |__) |
         \___ \   | | / /\ \ |  _  /  | |  | |  | |  ___/ 
         ____) |  | |/ ____ \| | \ \  | |  | |__| | |     
        |_____/   |_/_/    \_\_|  \_\ |_|   \____/|_|     

        */

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeSystem();
            //MainText.Text = "This will contain a description of your surroundings, actions performed by your livimon, dialog from NPCs, combat events, queries asking you what to do next, or general story text.";

            Thread logicThread = new Thread(runLogicUpdate);
            logicThread.IsBackground = true;
            logicThread.Start();

            Thread thread = new Thread(runGuiUpdate);
            thread.IsBackground = true;
            thread.Start();
        }

        /*
          _____ _   _ _____ _______ _____          _      _____ ____________  
         |_   _| \ | |_   _|__   __|_   _|   /\   | |    |_   _|___  /  ____| 
           | | |  \| | | |    | |    | |    /  \  | |      | |    / /| |__    
           | | | . ` | | |    | |    | |   / /\ \ | |      | |   / / |  __|   
          _| |_| |\  |_| |_   | |   _| |_ / ____ \| |____ _| |_ / /__| |____  
         |_____|_| \_|_____|  |_|  |_____/_/    \_\______|_____/_____|______| 
        
        */

        private void InitializeSystem()
        {
            putAllElementsIntoMapCellGrid();
            gameLogic = new GameLogic();
            controller = new GameController(gameLogic);
        }

        private void putAllElementsIntoMapCellGrid()
        {
            //If the rectangles were dynamically generated, we wouldn't have this mess or the one in the xaml, but maybe that could introduce other problems
            mapCellGrid = new Rectangle[,]
                {
                    {MapCellRow0Col0, MapCellRow0Col1, MapCellRow0Col2, MapCellRow0Col3, MapCellRow0Col4, MapCellRow0Col5, MapCellRow0Col6, MapCellRow0Col7, MapCellRow0Col8},
                    {MapCellRow1Col0, MapCellRow1Col1, MapCellRow1Col2, MapCellRow1Col3, MapCellRow1Col4, MapCellRow1Col5, MapCellRow1Col6, MapCellRow1Col7, MapCellRow1Col8},
                    {MapCellRow2Col0, MapCellRow2Col1, MapCellRow2Col2, MapCellRow2Col3, MapCellRow2Col4, MapCellRow2Col5, MapCellRow2Col6, MapCellRow2Col7, MapCellRow2Col8},
                    {MapCellRow3Col0, MapCellRow3Col1, MapCellRow3Col2, MapCellRow3Col3, MapCellRow3Col4, MapCellRow3Col5, MapCellRow3Col6, MapCellRow3Col7, MapCellRow3Col8},
                    {MapCellRow4Col0, MapCellRow4Col1, MapCellRow4Col2, MapCellRow4Col3, MapCellRow4Col4, MapCellRow4Col5, MapCellRow4Col6, MapCellRow4Col7, MapCellRow4Col8},
                    {MapCellRow5Col0, MapCellRow5Col1, MapCellRow5Col2, MapCellRow5Col3, MapCellRow5Col4, MapCellRow5Col5, MapCellRow5Col6, MapCellRow5Col7, MapCellRow5Col8},
                    {MapCellRow6Col0, MapCellRow6Col1, MapCellRow6Col2, MapCellRow6Col3, MapCellRow6Col4, MapCellRow6Col5, MapCellRow6Col6, MapCellRow6Col7, MapCellRow6Col8}
                };
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
            Console.log(clicked.Name);
        }

        private void MouseUp_PlayerCell(object sender, RoutedEventArgs e)
        {
        }
        private void MouseUp_LeftCell(object sender, RoutedEventArgs e)
        {
            controller.clickedLeft();
        }
        private void MouseUp_RightCell(object sender, RoutedEventArgs e)
        {
            controller.clickedRight();
        }
        private void MouseUp_UpCell(object sender, RoutedEventArgs e)
        {
            controller.clickedUp();
        }
        private void MouseUp_DownCell(object sender, RoutedEventArgs e)
        {
            controller.clickedDown();
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
        //15ms = 60 frames per second, but don't expect that to actually be 60, it could take an extra frame to update
        private void runGuiUpdate()
        {
            while (true)
            {
                Thread.Sleep(5); 
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
            updateMapText();
            updateMapGrid();
            updateFullMap();

            currentlyUpdatingGUI = false;
        }

        private void updateDescription()
        {
            if (GUIHandler.descriptionChanged()) MainText.Text = GUIHandler.getDescription();
        }

        private void updateTitle()
        {
            if (GUIHandler.titleChanged()) TitleText.Text = GUIHandler.getTitle();
        }

        private void updateMapText()
        {
            if (GUIHandler.mapTextChanged()) MapText.Text = GUIHandler.getMapText();
        }

        /*
      __  __          _____     _____ _____  _____ _____  
     |  \/  |   /\   |  __ \   / ____|  __ \|_   _|  __ \ 
     | \  / |  /  \  | |__) | | |  __| |__) | | | | |  | |
     | |\/| | / /\ \ |  ___/  | | |_ |  _  /  | | | |  | |
     | |  | |/ ____ \| |      | |__| | | \ \ _| |_| |__| |
     |_|  |_/_/    \_\_|       \_____|_|  \_\_____|_____/ 
                                                      
        */

        private void updateMapGrid()
        {
            if (GUIHandler.mapGridChanged())
            {
                RectangleUpdate[,] newRects = GUIHandler.getMapGrid();
                if (newRects.GetLength(0) != mapCellGrid.GetLength(0) || newRects.GetLength(1) != mapCellGrid.GetLength(1))
                {
                    throw new System.ArgumentException("BAD MATRIX SIZED FOR MAP");
                }
                for (int row = 0; row < mapCellGrid.GetLength(0); row++)
                {
                    for (int col = 0; col < mapCellGrid.GetLength(1); col++)
                    {
                        Rectangle currentRectangle = mapCellGrid[row, col];
                        RectangleUpdate currentUpdate = newRects[row, col];
                        if(currentUpdate == null)
                        {
                            throw new System.ArgumentException("updateMapGrid failed at row " + row + " col " + col);
                        }

                        if (currentUpdate.turnOffRectangle)
                        {
                            if (currentRectangle.StrokeThickness > 0)
                            {
                                if (currentRectangle.Stroke != null)
                                {
                                    removeClickableRectsClick(currentRectangle);
                                }
                            }

                            if (currentUpdate.color != null) currentRectangle.Fill = currentUpdate.color;
                            else currentRectangle.Fill = Brushes.Black;
                            if( !string.IsNullOrEmpty(currentUpdate.tooltip) ) currentRectangle.ToolTip = currentUpdate.tooltip;
                        }
                        else
                        {
                            if (currentRectangle.StrokeThickness > 0)
                            {
                                if (currentRectangle.Stroke == null)
                                {
                                    addClickableRectsClick(currentRectangle);
                                }
                            }

                            currentRectangle.Fill = currentUpdate.color;
                            if (!string.IsNullOrEmpty( currentUpdate.tooltip )) currentRectangle.ToolTip = currentUpdate.tooltip;
                        }
                    }
                }
            }
        }

        private void removeClickableRectsClick(Rectangle _currentRectangle)
        {
            if (_currentRectangle.Name == "MapCellRow2Col4")
            {
                _currentRectangle.MouseUp -= new MouseButtonEventHandler(MouseUp_UpCell);
                _currentRectangle.Stroke = null;
                MapCellRow2Col4Background.Fill = Brushes.Black;
            }
            if (_currentRectangle.Name == "MapCellRow3Col3")
            {
                _currentRectangle.MouseUp -= new MouseButtonEventHandler(MouseUp_LeftCell);
                _currentRectangle.Stroke = null;
                MapCellRow3Col3Background.Fill = Brushes.Black;
            }
            if (_currentRectangle.Name == "MapCellRow3Col5")
            {
                _currentRectangle.MouseUp -= new MouseButtonEventHandler(MouseUp_RightCell);
                _currentRectangle.Stroke = null;
                MapCellRow3Col5Background.Fill = Brushes.Black;
            }
            if (_currentRectangle.Name == "MapCellRow4Col4")
            {
                _currentRectangle.MouseUp -= new MouseButtonEventHandler(MouseUp_DownCell);
                _currentRectangle.Stroke = null;
                MapCellRow4Col4Background.Fill = Brushes.Black;
            }
            //if (_currentRectangle.Name == "MapCellRow3Col4") _currentRectangle.MouseUp += new MouseButtonEventHandler(MouseUp_PlayerCell);
        }

        private void addClickableRectsClick(Rectangle _currentRectangle)
        {
            if (_currentRectangle.Name == "MapCellRow2Col4")
            {
                _currentRectangle.MouseUp += new MouseButtonEventHandler(MouseUp_UpCell);
                _currentRectangle.Stroke = Brushes.Black;
                MapCellRow2Col4Background.Fill = Brushes.White;
            }
            if (_currentRectangle.Name == "MapCellRow3Col3"){
                _currentRectangle.MouseUp += new MouseButtonEventHandler(MouseUp_LeftCell);
                _currentRectangle.Stroke = Brushes.Black;
                MapCellRow3Col3Background.Fill = Brushes.White;
            }
            if (_currentRectangle.Name == "MapCellRow3Col5"){
                _currentRectangle.MouseUp += new MouseButtonEventHandler(MouseUp_RightCell);
                _currentRectangle.Stroke = Brushes.Black;
                MapCellRow3Col5Background.Fill = Brushes.White;
            }
            if (_currentRectangle.Name == "MapCellRow4Col4"){
                _currentRectangle.MouseUp += new MouseButtonEventHandler(MouseUp_DownCell);
                _currentRectangle.Stroke = Brushes.Black;
                MapCellRow4Col4Background.Fill = Brushes.White;
            }
            //if (_currentRectangle.Name == "MapCellRow3Col4") _currentRectangle.MouseUp += new MouseButtonEventHandler(MouseUp_PlayerCell);
            //currentRectangle.Stroke = new SolidColorBrush(Color.FromRgb(0xE2, 0xE2, 0xE2));
        }

        private void updateFullMap()
        {
            if (GUIHandler.fullMapChanged())
            {
                //remove previous full map
                FullMapGrid.Children.Clear();
                RenderOptions.SetEdgeMode(FullMapGrid, EdgeMode.Aliased);

                RectangleUpdate[,] fullMap = GUIHandler.getFullMap();

                //repopulate completely
                for (int row = 0; row < fullMap.GetLength(0); row++)
                {
                    for (int col = 0; col < fullMap.GetLength(1); col++)
                    {
                        Rectangle newRect = new Rectangle()
                        {
                            Width = 1,
                            Height = 1,
                            Fill = fullMap[row, col].color,
                            Stroke = fullMap[row, col].color,
                            StrokeThickness = 0,
                            Margin = new Thickness(col,row,0,0),
                            HorizontalAlignment = HorizontalContentAlignment,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        FullMapGrid.Children.Add(newRect);
                    }
                }
            }
        }
    }
}
