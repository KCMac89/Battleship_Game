using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; // allows us to create a debug log in the outputs window

namespace Battleship
{
    public partial class Form1 : Form
    {
        List<Button> playerPosition; // create a list for all the player position buttons
        List<Button> enemyPosition; // create a list for all the enemy position buttons
        Random rand = new Random(); // create a new instance for the random class called rand
        int totalShips = 3; // number of total player ships
        int totalenemy = 3; // number of total enemy ships
        int rounds = 10; // total rounds to play each will play 5 rounds
        int playerTotalScore = 0; // default player score
        int enemyTotalScore = 0; // default enemy score
        Help instructions = new Help(); // instance of Help form
        public Form1()
        {
            InitializeComponent();
            loadbuttons(); // load the buttons for enemy and player to the system
            attackButton.Enabled = false; // disable the player attack button
            enemyLocationList.Text = null; // nullify the enemy location drop down box
        }

        private void loadbuttons()
        {
            // this function will load all the buttons into the lists we declared above
            // we load all of the player and enemy buttons first
            playerPosition = new List<Button> { w1, w2, w3, w4, x1, x2, x3, x4, y1, y2, y3, y4, z1, z2, z3, z4 };
            enemyPosition = new List<Button> { a1, a2, a3, a4, b1, b2, b3, b4, c1, c2, c3, c4, d1, d2, d3, d4 };

            // this loop will go through each of the enemy position button
            // then it will add them to the enemy location drop down list for us
            // it will also remove all Tags from the enemy location buttons
            for (int i = 0; i < enemyPosition.Count; i++)
            {
                enemyPosition[i].Tag = null;
                enemyLocationList.Items.Add(enemyPosition[i].Text);
            }
        }

        private void playerPicksPosition(object sender, EventArgs e)
        {
            //this function will let the player pick 3 positions on the map
            // in the beginning of the game this is how we allow the player to pick 3 positions

            if (totalShips > 0)
            {
                // if total ships is more than 0 then we check
                var button = (Button)sender;
                // which button was clicked
                button.Enabled = false;
                // disable that button
                button.Tag = "playerShip";
                // put a tag on it called playership
                button.BackColor = System.Drawing.Color.Blue;
                // change the colour to blue
                totalShips--;
                // decrease the total ships by 1
            }
            if (totalShips == 0)
            {
                // if the player has picked their all 3 ships 
                // then we do the following
                attackButton.Enabled = true;
                // activate the attack button
                attackButton.BackColor = System.Drawing.Color.Red;
                // give it a background colour of red
                helpText.Top = 10;
                // move the help text to top 10
                helpText.Left = 310;
                // move the help text to left 310
                helpText.Text = "2) Now pick an attack position from the drop down";
                // change the help text to above
            }
        }

        private void attackEnemyPosition(object sender, EventArgs e)
        {
            // this function will allow the player to make the moves on the enemy location
            // we need to check if the player can choose a location from the drop down list
            if (enemyLocationList.Text != "")
            {
                // if the location is appropriately picked then we do the following
                var attackPos = enemyLocationList.Text;
                // create a variable called attack pos and give it the value of the text selected from the drop down menu
                attackPos = attackPos.ToLower();
                // change the string to a lower case to match the button name
                int index = enemyPosition.FindIndex(a => a.Name == attackPos);
                // in this int we will run the index of the enemy location and search for the string the player picked
                // once its found it will be saved inside the index local variable


                // in the if statement below we will link that index number to the enemy position list
                // and we need to check if we have more rounds to player
                // if so we do the following
                if (enemyPosition[index].Enabled && rounds > 0)
                {

                    rounds--;
                    //reduce 1 from the rounds
                    roundsText.Text = "Rounds " + rounds;
                    // update the rounds text

                    if (enemyPosition[index].Tag == "enemyship")
                    {
                        // if that location the player picked has a enemyship tag in it then we do the following
                        enemyPosition[index].Enabled = false;
                        // disable that button
                        enemyPosition[index].BackgroundImage = Properties.Resources.fireIcon;
                        //change the background image to the fire icon
                        enemyPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                        // change the background colour to dark blue
                        playerTotalScore++;
                        // increase 1 to the player score 
                        playerScore.Text = "" + playerTotalScore;
                        // update the player score on the player label
                        enemyPlayTimer.Start();
                        // start the cpu timer so the enemy can make its move
                    }
                    else
                    {
                        // if player picks a location that isn't where the enemy ship is
                        // then do the following
                        enemyPosition[index].Enabled = false;
                        // we disable that  button
                        enemyPosition[index].BackgroundImage = Properties.Resources.missIcon;
                        // change the background image to miss icon
                        enemyPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                        // change the background to dark blue
                        enemyPlayTimer.Start();
                        // start the cpu time so the enemy can make its move
                    }

                }


            }
            else
            {
                // if player doesn't pick a location from the drop down list, alert them to do so
                MessageBox.Show("Choose a location from the drop down list. ");
            }
        }

        private void enemyAttackPlayer(object sender, EventArgs e)
        {
            // this function is for the CPU to make a move on the player

            // if the player position is more than 0 and there are more rounds to play
            // then we will do the following inside this if statement
            if (playerPosition.Count > 0 && rounds > 0)
            {

                rounds--; // reduce a round from the total
                roundsText.Text = "Rounds " + rounds; // show the updated number to the rounds label

                int index = rand.Next(playerPosition.Count); // create a new int index and place a random player button

                if (playerPosition[index].Tag == "playerShip") 
                {
                    // if the index has a tag of playership then we do the following

                    playerPosition[index].BackgroundImage = Properties.Resources.fireIcon;
                    // change its icon to the fire icon


                    enemyMoves.Text = "" + playerPosition[index].Text;
                    // show which button was attacked
                    playerPosition[index].Enabled = false;
                    //disable the button
                    playerPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                    //change the background colour to dark blue
                    playerPosition.RemoveAt(index);
                    // remove this button from the player position list so it won't get attacked again by the CPU
                    enemyTotalScore++;
                    // add 1 to the enemy score
                    enemyScore.Text = "" + enemyTotalScore;
                    // show the enemy score on the label
                    enemyPlayTimer.Stop();
                    //stop the time for the CPU
                }
                else
                {
                    // if the player tag isn't of playership 
                    // then we do the following
                    playerPosition[index].BackgroundImage = Properties.Resources.missIcon;
                    //show the miss icon on the button
                    enemyMoves.Text = "" + playerPosition[index].Text;
                    // update the enemy attack location on label 2
                    playerPosition[index].Enabled = false;
                    // diable the button cpu attacked
                    playerPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                    //change the background colour to dark blue
                    playerPosition.RemoveAt(index);
                    // remove this button from the list so it wont get attacked again by the cpu
                    enemyPlayTimer.Stop();
                    // stop the cpu time. 
                }

            }

            // below is the if statement thats checking whether we won, drew or lost the game

            // if rounds are less than 1 OR player score is more than 2 OR enemy score is more than 2

            if (rounds < 1 || playerTotalScore > 2 || enemyTotalScore > 2)
            {
                if (playerTotalScore > enemyTotalScore)
                {
                    // if player score is more than enemy score player wins
                    MessageBox.Show("You Win", "Winning");
                }
                if (playerTotalScore == enemyTotalScore)
                {
                    // if player and enemy scores the the same its a draw
                    MessageBox.Show("No one wins this", "Draw");
                }
                if (enemyTotalScore > playerTotalScore)
                {
                    // if enemy score is more than player then enemy winds
                    MessageBox.Show("Haha! I Sunk Your Battle Ship", "Lost");
                }
            }
        }

        private void enemyPicksPositions(object sender, EventArgs e)
        {
            // this function will allow the CPU to pick 3 positions on the MAP
            // we need to make sure that the enemy CPU will pick 3 positions on the map

            int index = rand.Next(enemyPosition.Count);
            // create a local variable called index and choose a random button from the enemy position list

            if (enemyPosition[index].Enabled == true && enemyPosition[index].Tag == null)
            {
                // when we find the buttons we need to check if they are enabled and they dont have a tag yet
                enemyPosition[index].Tag = "enemyship";
                // now add a tag called enemy ship to the button
                totalenemy--;
                // and we will reduce the total enemy by 1

                Debug.WriteLine("Enemy Position  " + enemyPosition[index].Text);
                // the line above will show us inthe debug window which buttons the enemy chose
                // this can help us figure out if the game is working as intended
            }
            else
            {
                // if the top condition dont match then we will run it again to select the 3 positions and tags
                index = rand.Next(enemyPosition.Count);
            }
            if (totalenemy < 1)
            {
                // if the cpu has selected the 3 positions then we can stop the timer
                enemyPositionPicker.Stop();
            }
        }

        private void Rounds_lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Rounds_lb.SelectedIndex == 0) // 10 rounds
            {
                rounds = 10;
            }
            if (Rounds_lb.SelectedIndex == 1) // 16
            {
                rounds = 16;
            }
            if (Rounds_lb.SelectedIndex == 2) // 20
            {
                rounds = 20;
            }
            if (Rounds_lb.SelectedIndex == 3) // 24
            {
                rounds = 24;
            }


        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instructions.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Reset_btn_Click(object sender, EventArgs e)
        {
            totalShips = 3;
            totalenemy = 3;
            playerTotalScore = 0;
            enemyTotalScore = 0;
            loadbuttons();
            foreach (var Button in enemyPosition)
            {
                Button.BackgroundImage = null;
                Button.BackColor = System.Drawing.SystemColors.Control;
                Button.Enabled = true;
               
            }

            foreach (var Button in playerPosition)
            {
                Button.BackgroundImage = null;
                Button.BackColor = System.Drawing.SystemColors.Control;
                Button.Enabled = true;

            }

            
            attackButton.Enabled = false; // disable the player attack button
            enemyLocationList.Text = null; // nullify the enemy location drop down box
            playerScore.Text = "00";
            enemyScore.Text = "00";
        }
    }
}
