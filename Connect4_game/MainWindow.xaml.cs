﻿using System;
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

namespace Connect4_game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private members
        private MarkType[][] columns;
        private MarkType[] newRow;
        private bool isPlayer1Turn;
        private bool isGameOver;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            SetUpArray();   
            NewGame();
        }

        private void NewGame()
        {
            // Reset all values in new array
            for (int i = 0; i < columns.Length; i++)
            {
                var newRow = columns[i];
                for (int k = 0; k < newRow.Length; k++)
                {
                    newRow[k] = MarkType.Free;
                    //Also reset text box values
                    ResetTextBox(i, k);
                }
            }
            // set player 1 turn to true and gameover to false
            isPlayer1Turn = true;
            isGameOver = false;
        }

        private void SetUpArray()
        {
            // Set up new array, columns
            columns = new MarkType[8][];
            for (int i = 0; i < columns.Length; i++)
            {
                newRow = new MarkType[8];
                for (int k = 0; k < newRow.Length; k++)
                {
                    newRow[k] = MarkType.Free;
                    AddTextBox(i, k);
                }
                columns[i] = newRow;
            }
        }

        private void AddTextBox(int i, int k)
        {
            // Add TextBlock object to position (i,k) in the Grid
            // Corresponding to position (i,k) in the columns array
            TextBlock tblock = new TextBlock()
            {
                //Text = "X",
                Name = "TextBlock" + i.ToString() + k.ToString(),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 60
            };
            Grid.SetColumn(tblock, i);
            Grid.SetRow(tblock, k);
            Container.Children.Add(tblock);
        }

        private void ResetTextBox(int i, int k)
        {
            // Reset the value of TextBlock at position (i,k) in the Grid
            TextBlock selectedTB = FindChild<TextBlock>(Container,
                "TextBlock" + i.ToString() + k.ToString());
            selectedTB.Text = String.Empty;
            selectedTB.Foreground = Brushes.Black;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isGameOver)
            {
                NewGame();
                return;
            }
            
            //cast the sender to a button
            var button = (Button)sender;

            //find the button's position in the array
            var columnIndex = Grid.GetColumn(button);

            // Get index of 'lowest' empty cell
            var columnRows = columns[columnIndex];
            int rowIndex;
            if (!columnRows.Contains(MarkType.Free))
            {
                return;
            }
            else
            {
                rowIndex = Array.LastIndexOf(columnRows, MarkType.Free);
            }

            // Fill grid with 'X/O' and column array with corresponding MarkType
            MarkType playerMarkType = isPlayer1Turn ? MarkType.Cross : MarkType.Nought;
            String playerMarkstring = isPlayer1Turn ? "X" : "O";
            TextBlock selectedTB = FindChild<TextBlock>(Container, 
                "TextBlock" + columnIndex.ToString() + rowIndex);
            selectedTB.Text = playerMarkstring;

            // Set font color to Red on Player 1's turn
            if (isPlayer1Turn)
            {
                selectedTB.Foreground = Brushes.Red;
            }
            columnRows[rowIndex] = playerMarkType;

            // Check if game has ended
            CheckIfGamoOver();

            // Alternate player - I actually quite like this approach
            isPlayer1Turn = !isPlayer1Turn; 
        }

        private void CheckIfGamoOver()
        {
            if (IsDraw())
            {
                isGameOver = true;
            }
        }

        private bool IsDraw()
        {
            // Check if any more moves can be made
            // Only need to check the first/highest row
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i][0] == MarkType.Free)
                {
                    return false;
                }
            }
            return true;
        }

        // The Below FindChild method is from the following site:
        // https://stackoverflow.com/questions/636383/how-can-i-find-wpf-controls-by-name-or-type

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
