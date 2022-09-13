using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class King : Piece
    {
        // Counts the number of times the king is in check to simplify checking for mate
        protected int checkCounter = 0;
        //Properties
        // Tracks the squares in the attack vector to prevent the king moving backwards into an
        // an attack
        public List<string> ThreatVector = new List<string>();
        // List of moves that will interupt the attack on the king if a piece is able to move there
        public List<string> BlockingMoves = new List<string>();
        public int CheckCounter
        {
            get { return checkCounter; }
            set
            {
                if (value >= 0)
                {
                    checkCounter = value;
                }
            }
        }
        //Constructor
        public King(string coords, bool isWhite) : base(coords, isWhite)
        {
            Type = "King";
            // King cannot be pinned
            IsPinned = false;
            Value = 0;
        }
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
            // King Movement
            for (int i = 0; i < ReferenceArray.GetLength(0); i++)
            {
                for (int j = 0; j < ReferenceArray.GetLength(1); j++)
                {
                    if (Math.Abs(jLocation - j) <= 1 && Math.Abs(iLocation - i) <= 1)
                    {
                        if (iLocation == i && jLocation == j)
                        {
                            continue;
                        }
                        TotalCoverage.Add(ReferenceArray[i, j]);
                    }
                }
            }
        }
        public override void CalcMoves(Dictionary<string, Piece> pieces)
        {
            base.CalcMoves(pieces);
            // Removes all possible king moves that are shared with opposite color piece
            // coverage moves
            foreach (Piece piece in pieces.Values)
            {
                // Ensures opposite color
                if (piece.IsWhite != this.IsWhite)
                {
                    foreach (string move in piece.TotalCoverage)
                    {
                        if (this.NetMoves.Contains(move))
                        {
                            this.NetMoves.Remove(move);
                        }
                    }
                }
            }
            // Removes all possible king moves in the threat vector
            foreach (string move in this.ThreatVector)
            {
                if (this.NetMoves.Contains(move))
                {
                    this.NetMoves.Remove(move);
                }
            }
        }
        public void CheckCheckmate(Dictionary<string, Chess.Piece> pieces)
        {
            bool checkMate = true;
            // If no king moves left and checked more than once, blocking is impossible:
            // therefore checkmate
            if (this.NetMoves.Count == 0 && checkCounter > 1)
            {
                checkMate = true;
            }
            // If the king has moves and is checked more than once, removes all Possible moves
            // for every other piece since blocking is impossible
            if (this.NetMoves.Count > 0 && checkCounter > 1)
            {
                checkMate = false;
                foreach (Piece piece in pieces.Values)
                {
                    if(piece is not King)
                    {
                        piece.NetMoves.Clear();
                    }
                }
            }
            //If Checked once, updates possible piece moves of same color to only blocking moves
            else if (this.NetMoves.Count >= 0 && checkCounter == 1)
            {
                // If the king has moves it is not checkmate. If it doesn't, it has to be proven
                // that it isn't checkmate
                if (this.NetMoves.Count > 0) { checkMate = false; }
                foreach (Piece piece in pieces.Values)
                {
                    // If the piece is not a king and is the same color as this king, find all
                    // moves that get inbetween the attacking piece and the king or take the
                    // attacking piece
                    List<string> defendingMoves = new List<string>();
                    if (piece is not King && piece.IsWhite == this.IsWhite)
                    {
                        foreach (string move in this.BlockingMoves)
                        {
                            if (piece.NetMoves.Contains(move))
                            {
                                checkMate = false;
                                defendingMoves.Add(move);
                            }
                        }
                        // Update net moves to only moves that defend the king
                        piece.NetMoves = defendingMoves;
                    }
                }
            }
            else
            {
                checkMate = false;
            }
            // Not implmented yet, could easily add a scoreboard that writes into a file, but that
            // wasn't really the point of the project
            if (checkMate)
            {
                throw new Exception("Checkmate");
            }
        }
        // Some additional things need to be reset
        public override void ResetPositions()
        {
            base.ResetPositions();
            ThreatVector.Clear();
            BlockingMoves.Clear();
            checkCounter = 0;
        }
        // Some additional info is useful
        public override void GetInfo()
        {
            Console.WriteLine($"Coordinates: {Coords}, IsWhite: {IsWhite}, Type: " +
                $"{Type}, In Check: {CheckCounter} times");
            if (this.CheckCounter > 0)
            {
                Console.Write($"Threat Vector: ");
                foreach (string attackSquare in ThreatVector)
                {
                    Console.Write($" ({attackSquare}) ");
                }
                Console.WriteLine();
                Console.Write($"Blocking Moves: ");
                foreach (string attackSquare in BlockingMoves)
                {
                    Console.Write($" ({attackSquare}) ");
                }
                Console.WriteLine();
            }
            Console.Write("NetMoves: ");
            foreach (string move in NetMoves)
            {
                Console.Write($"({move}) ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
