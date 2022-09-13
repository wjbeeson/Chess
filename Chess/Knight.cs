using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Knight : Piece
    {
        //Constructor
        public Knight(string coords, bool isWhite) : base(coords, isWhite)
        {
            Type = "Knight";
            Value = 3;
        }
        //Methods
        public override void UpdateCoverage(Dictionary<string, Piece> pieces)
        {
            // Get array coords from piece coords
            Stack<string> tempVector = new Stack<string>();
            int iLocation = -1;
            int jLocation = -1;
            for (int i = 0; i < ReferenceArray.GetLength(0); i++)
            {
                for (int j = 0; j < ReferenceArray.GetLength(1); j++)
                {
                    if (ReferenceArray[i, j] == this.Coords)
                    {
                        iLocation = i;
                        jLocation = j;
                    }
                }
            }
            int iSaver = iLocation;
            int jSaver = jLocation;
            // Since each vector was just once square it was possible to make the movement very
            // compact
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    if (Math.Abs(i) + Math.Abs(j) == 3) 
                    {
                        int iDestination = iSaver + i;
                        int jDestination = jSaver + j;
                        if (iDestination >= 0 && iDestination < ReferenceArray.GetLength(0) 
                            && jDestination >= 0 && jDestination < ReferenceArray.GetLength(1))
                        {
                            tempVector.Clear();
                            tempVector.Push(ReferenceArray[iDestination, jDestination]);
                            this.TotalCoverage.Add(ReferenceArray[iDestination, jDestination]);
                            this.CalcPinsAndThreats(tempVector, pieces);
                        }
                    }
                }
            }
            {

            }
        }
    }
}
