using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Rook : Piece
    {
        //Constructor
        public Rook(string coords, bool isWhite) : base(coords, isWhite)
        {
            Type = "Rook";
            Value = 5;
        }
        //Methods
        // Exact same process as bishop
        public override void UpdateCoverage(Dictionary<string, Piece> pieces)
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
            //Down Block Start
            Stack<string> tempVector = new Stack<string>();
            bool clearVector = true;
            for (int i = iLocation + 1; i < ReferenceArray.GetLength(0); i++)
            {
                for (var j = jLocation; j < ReferenceArray.GetLength(1); j++)
                {
                    if (jLocation == j)
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
            //Left Block Start
            tempVector.Clear();
            clearVector = true;
            for (int i = iLocation; i < ReferenceArray.GetLength(0); i++)
            {
                for (var j = jLocation - 1; j >= 0; j--)
                {
                    if (iLocation == i)
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
            //Up Block start
            tempVector.Clear();
            clearVector = true;
            for (int i = iLocation - 1; i >= 0; i--)
            {
                for (var j = jLocation; j < ReferenceArray.GetLength(1); j++)
                {
                    if (jLocation == j)
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
            //Right Block Start
            tempVector.Clear();
            clearVector = true;
            for (int i = iLocation; i < ReferenceArray.GetLength(0); i++)
            {
                for (var j = jLocation + 1; j < ReferenceArray.GetLength(1); j++)
                {
                    if (iLocation == i)
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
