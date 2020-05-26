using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;


namespace Inicjator
{
    /// <summary>
    /// 'DnD Initiator'
    /// Is an application used to determine 
    /// the order of movement 
    /// during the encounter. 
    /// </summary>



    public partial class MainWindow : Window
    {

        List<Hero> heroList = new List<Hero>();
        List<Hero> enemyList = new List<Hero>();
        List<Hero> battleList = new List<Hero>();

        //current hero index:
        int currentIndex = 0;
        //previous hero index:
        int prevIndex = 0;
        //next hero index:
        int nextIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            NextButton.IsEnabled = false;
            enemyListBox.IsReadOnly = true;
            heroListBox.IsReadOnly = true;
        }

        //AddToListButton
        private void AddingButton_Click(object sender, RoutedEventArgs e)
        {

            //Check If Data Is Present
            if (type_name.Text.Equals("") || type_initiative.Text.Equals(""))
            {
                printStatus("No Data To Save");
                return;
            }

            try
            {
                //Create Hero
                Hero heroToAdd = new Hero();
                string heroName = type_name.Text;
                int heroInit = int.Parse(type_initiative.Text);

                heroToAdd.Name = heroName;
                heroToAdd.Init = heroInit;

                //Add Hero To List
                printStatus("Adding: " + heroToAdd);

                if (heroRadioButton.IsChecked == true)
                {
                    heroList.Add(heroToAdd);
                }
                else
                {
                    enemyList.Add(heroToAdd);
                }

                //Refresh Lists
                refreshList();

                //Clear Text Boxes
                clearTextBoxes();

            }
            catch (Exception)
            {
                printStatus("Error");
                clearTextBoxes();
                return;
            }

        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //Hero Radio Button Switch

            if (enemyRadioButton.IsChecked == true)
            {
                enemyRadioButton.IsChecked = false;

            }

        }

        private void enemyRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //Enemy Radio Button
            if (heroRadioButton.IsChecked == true)
            {
                heroRadioButton.IsChecked = false;

            }
        }

        private void clearAllButton_Click(object sender, RoutedEventArgs e)
        {
            //Clear and sets default values.

            heroList.Clear();
            enemyList.Clear();
            battleList.Clear();
            heroListBox.Clear();
            enemyListBox.Clear();

            startEncounterButton.IsEnabled = true;

            currentFighter.Text = "-";
            previousFighter.Text = "-";
            nextFighter.Text = "-";

        }

        private void clearTextBoxes()
        {
            type_initiative.Clear();
            type_name.Clear();
        }

        private void refreshList()
        {
            //Display heroes/enemies on lists:

            StringBuilder sb = new StringBuilder();

            foreach (Hero hero in heroList)
            {

                sb.Append(hero.Name).Append("\t");
                sb.Append(hero.Init).Append("\n");

            }
            heroListBox.Text = sb.ToString();

            //Clear And Update Counter
            sb.Clear();

            foreach (Hero hero in enemyList)
            {

                sb.Append(hero.Name).Append("\t");
                sb.Append(hero.Init).Append("\n");

            }
            enemyListBox.Text = sb.ToString();

        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            ExitWindow exit = new ExitWindow();
            exit.Owner = this;
            exit.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //This Method Sets up The Battle

            //Check if there is more than 1 hero in each List<Hero>.
            if (heroList.Count < 1 || enemyList.Count < 1)
            {
                printStatus("Not Enough Participants!");
                return;
            }

            //Add Heroes to battleList.
            battleList.AddRange(heroList);
            battleList.AddRange(enemyList);

            //Sort It.
            battleList.Sort((x, y) => y.Init.CompareTo(x.Init));

            foreach (Hero hero in battleList)
            {
                Debug.Print(hero.ToString());
            }

            //Display default setup.
            displayFighters();

            //Turn off encounter button:
            startEncounterButton.IsEnabled = false;
            NextButton.IsEnabled = true;
        }


        private void displayFighters()
        {
            //starting setup of encounter

            prevIndex = battleList.Count - 1;
            nextIndex = currentIndex + 1;

            currentFighter.Text = battleList[currentIndex].Name;
            previousFighter.Text = battleList[prevIndex].Name;
            nextFighter.Text = battleList[nextIndex].Name;

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

            if(battleList.Count() == 0)
            {
                printStatus("forbidden action");
                return;
            }

            //Display current fighter.
            currentIndex++;
            if (currentIndex > battleList.Count - 1)
            {
                currentIndex = 0;
            }
            currentFighter.Text = battleList[currentIndex].Name;

            //Display next fighter.
            nextIndex = currentIndex + 1;
            if (nextIndex > battleList.Count - 1)
            {
                nextIndex = 0;
            }
            nextFighter.Text = battleList[nextIndex].Name;

            //Display previous fighter.
            prevIndex = currentIndex - 1;
            if (prevIndex < 0)
            {
                prevIndex = battleList.Count - 1;
            }
            previousFighter.Text = battleList[prevIndex].Name;
        }
        private void printStatus(string message)
        {
            status_log.Text = message;
            status_log.Foreground = Brushes.Red;
        }

    }

}

class Hero
{
    private string _name;
    private int _initiative;

    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value;
        }
    }

    public int Init
    {
        get
        {
            return _initiative;
        }

        set
        {
            _initiative = value;
        }
    }

    public override string ToString()
    {
        return Name + " " + Init;
    }

}



