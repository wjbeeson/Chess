using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Pawn : Piece
    {
        private int promoteRow;
        public override List<string> GrossMoves { get; set;} = new List<string>();
        public Pawn(string coords, bool isWhite) : base(coords, isWhite)
        {
            Type = "Pawn";
            Value = 1;
            if (isWhite)
            {
                promoteRow = 0;
            }
            else
            {
                promoteRow = 7;
            }
        }
        public override void UpdateCoverage(Dictionary<string, Piece> pieces)
        {
            Stack<string> tempVector = new Stack<string>();
            int increment = 0;
            switch (this.IsWhite)
            {
                case true:
                    increment = -1;
                    break;
                case false:
                    increment = 1;
                    break;
            }
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
            // Coverage Calculator
            int iDestination = iLocation + increment;
            for (int i = -1; i < 2; i += 2)
            {
                int jDestination = jLocation + i;
                if (iDestination >= 0 && iDestination < ReferenceArray.GetLength(0)
                    && jDestination >= 0 && jDestination < ReferenceArray.GetLength(1))
                {
                    tempVector.Clear();
                    tempVector.Push(ReferenceArray[iDestination, jDestination]);
                    this.TotalCoverage.Add(ReferenceArray[iDestination, jDestination]);
                    this.CalcPinsAndThreats(tempVector, pieces);
                }
            }
            // Move calculator
            // Allows for forward movement if there is no piece in front
            if (iDestination >= 0 && iDestination < ReferenceArray.GetLength(0))
            {
                if (!pieces.ContainsKey(ReferenceArray[iDestination, jLocation]))
                {
                    this.GrossMoves.Add(ReferenceArray[iDestination, jLocation]);
                    // Allows for two space movement on first move if no piece in square
                    if (iDestination + increment >= 0 && iDestination + increment
                        < ReferenceArray.GetLength(0))
                    {
                        if (!pieces.ContainsKey(ReferenceArray[iDestination += increment, jLocation])
                            && this.MoveCounter == 0)
                        {
                            this.GrossMoves.Add(ReferenceArray[iDestination, jLocation]);
                        }
                    }
                }
            }
            //Allows Diagonal moves if there is a piece of the opposite color on the square
            foreach (string move in this.TotalCoverage)
            {
                if (pieces.ContainsKey(move))
                {
                    Piece piece = pieces[move];
                    if (piece.IsWhite != this.IsWhite)
                    {
                        this.GrossMoves.Add(move);
                    }
                }
            }
        }
        // This method is identical to the base method except for that there is no need to validate
        // friendly fire since it was already done above because of the nature of pawn moves
        public override void CalcMoves(Dictionary<string, Piece> pieces)
        {
            // Removes all pinned piece moves not on the same vector as the pin
            if (this.IsPinned == true)
            {
                this.NetMoves.Clear();
                foreach (string move in this.PinnedMoves)
                {
                    if (this.GrossMoves.Contains(move))
                    {
                        this.NetMoves.Add(move);
                    }
                }
            }
            // If not pinned, then allmoves are possible
            else
            {
                this.NetMoves = this.GrossMoves;
            }
        }
    }
}
