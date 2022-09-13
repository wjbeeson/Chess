using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Bishop : Piece
    {
        //Constructor
        public Bishop(string coords, bool isWhite) : base (coords, isWhite)
        {
            Type = "Bishop";
            Value = 3;
        }
        //Methods
        // It's possible to make the movement itself more compact, but since I wanted to check
        // each vector for pins and attacks this seemed to be the best way to do it
        public override void UpdateCoverage(Dictionary<string,Piece> pieces)
        {
            // Get array coords from piece coords
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
            //PlusPlus Block Start
            Stack<string> tempVector = new Stack<string>();
            bool clearVector = true;
            for (int i = iLocation + 1; i < ReferenceArray.GetLength(0); i++)
            {
                for (var j = jLocation + 1; j < ReferenceArray.GetLength(1); j++)
                {
                    if ((iLocation - jLocation) == (i - j))
                    {
                        tempVector.Push(ReferenceArray[i, j]);
                        if (clearVector) { TotalCoverage.Add(ReferenceArray[i, j]); }
                        if (pieces.ContainsKey(ReferenceArray[i, j]))
                        {
                            clearVector = false;
                        }
                    }
                }
            }
            this.CalcPinsAndThreats(tempVector, pieces);
            //PlusMinus Block Start
            tempVector.Clear();
            clearVector = true;
            for (int i = iLocation + 1; i < ReferenceArray.GetLength(0); i++)
            {
                for (var j = jLocation - 1; j >= 0; j--)
                {
                    if (i + j == iLocation + jLocation)
                    {
                        tempVector.Push(ReferenceArray[i, j]);
                        if (clearVector) { TotalCoverage.Add(ReferenceArray[i, j]); }
                        if (pieces.ContainsKey(ReferenceArray[i, j]))
                        {
                            clearVector = false;
                        }
                    }
                }
            }
            this.CalcPinsAndThreats(tempVector, pieces);
            //MinusMinus Block start
            tempVector.Clear();
            clearVector = true;
            for (int i = iLocation - 1; i >= 0; i--)
            {
                for (var j = jLocation - 1; j >= 0; j--)
                {
                    if ((iLocation - jLocation) == (i - j))
                    {
                        tempVector.Push(ReferenceArray[i, j]);
                        if (clearVector) { TotalCoverage.Add(ReferenceArray[i, j]); }
                        if (pieces.ContainsKey(ReferenceArray[i, j]))
                        {
                            clearVector = false;
                        }
                    }
                }
            }
            this.CalcPinsAndThreats(tempVector, pieces);
            //MinusPlus Block Start
            tempVector.Clear();
            clearVector = true;
            for (int i = iLocation - 1; i >= 0; i--)
            {
                for (var j = jLocation + 1; j < ReferenceArray.GetLength(1); j++)
                {
                    if (i + j == iLocation + jLocation)
                    {
                        tempVector.Push(ReferenceArray[i, j]);
                        if (clearVector) { TotalCoverage.Add(ReferenceArray[i, j]); }
                        if (pieces.ContainsKey(ReferenceArray[i, j]))
                        {
                            clearVector = false;
                        }
                    }
                }
            }
            this.CalcPinsAndThreats(tempVector, pieces);
        }
    }
}
