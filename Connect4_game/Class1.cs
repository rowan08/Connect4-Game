using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4_game
{
    /// <summary>
    /// This class is designed to represent a point in an A x B 2D Array (specifically the 
    /// columns array in the main script). It will be used to keep move hoizonally, vertically
    /// and diagonally, representing moves along a board. In the Connect4 app, it will be used
    /// to check fot wins
    /// </summary>
    class ArrayPoint
    {
        private MarkType[][] referenceArray;
        private int[] startPosition;
        private int[] currentPosition;

        public ArrayPoint(MarkType[][] columnsArray, int columnNumber, int rowNumber)
        {
            referenceArray = columnsArray;
            startPosition = [columnNumber, rowNumber];
            currentPosition = [columnNumber, rowNumber];
        }

        /// <summary>
        /// Will change the value of currentPosition, based on directions given
        /// </summary>
        /// <param name="directions"></param>
        /// <returns>0 if successful, else 1</returns>
        public int ChangePosition(string directions)
        {
            string legalMoves = "UDLR";

            // First, make sure that the move combination does not go beyond  
            // the boundaries of the grid
            foreach (char c in directions)
            {
                // Make sure legal move is specified
                if (!legalMoves.Contains(c))
                {
                    throw new System.ArgumentException("Invalid direction specified", "c");
                }

                // Eventually use a dictionary-like structure to create one if-statement
                // for this process
                if (c == 'U')
                {
                    if (currentPosition[0] == 0)
                    {
                        return 1;
                    }
                    continue;
                }
                else if (c == 'D')
                {
                    if (currentPosition[0] == 7)
                    {
                        return 1;
                    }
                    continue;
                }
                else if (c == 'L')
                {
                    if (currentPosition[1] == 0)
                    {
                        return 1;
                    }
                    continue;
                }
                else if (c == 'R')
                {
                    if (currentPosition[1] == 7)
                    {
                        return 1;
                    }
                    continue;
                }
            }

            // Move currentPosition based on direction(s)
            foreach (char c in directions)
            {
                switch (c)
                {
                    case 'U':
                        currentPosition[0] -= 1;
                        break;
                    case 'D':
                        currentPosition[0] += 1;
                        break;
                    case 'L':
                        currentPosition[1] -= 1;
                        break;
                    case 'R':
                        currentPosition[1] += 1;
                        break;
                }
            }
            return 0;
        }

        public MarkType getCurrentValue()
        {
            int columnNumber = currentPosition[0];
            int rowNumber = currentPosition[1];

            return referenceArray[columnNumber][rowNumber];
        }

        public void ResetPosition()
        {
            currentPosition = startPosition.ToArray();
        }
    }    
}
