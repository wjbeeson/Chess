using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal abstract class Piece
    {
        //Fields
        protected string coords = "";
        protected int moveCounter = 0;
        //Properties
        public int MoveCounter
        {
            get { return moveCounter; }
            set { moveCounter = value; }
        }
        public string[,] ReferenceArray { get; } =
        {
            {"a8","b8","c8","d8","e8","f8","g8","h8"},
            {"a7","b7","c7","d7","e7","f7","g7","h7"},
            {"a6","b6","c6","d6","e6","f6","g6","h6"},
            {"a5","b5","c5","d5","e5","f5","g5","h5"},
            {"a4","b4","c4","d4","e4","f4","g4","h4"},
            {"a3","b3","c3","d3","e3","f3","g3","h3"},
            {"a2","b2","c2","d2","e2","f2","g2","h2"},
            {"a1","b1","c1","d1","e1","f1","g1","h1"}
        };
        public string Coords
        {
            get
            {
                return coords;
            }
            set
            {
                if (value.Length == 2 && Char.IsLetter(char.Parse(value.Substring(0, 1)))
                    && int.TryParse(value.Substring(1, 1), out int number))
                {
                    coords = value.ToLower();
                }
                else
                {
                    throw new Exception("Invalid Coordinate");
                }
            }
        }
        // true = white; false = black
        public bool IsWhite { get; set; }
        // Unused
        public string Type { get; set; } = "Not Assigned";
        // Unused
        public int Value { get; set; } = -1;
        // Pinned pieces cannot move because they would expose their king to an attack
        public bool IsPinned { get; set; } = false;
        // PinnedMoves is the attacking vector starting from the attacking piece and ending on the
        // square before the king
        public List<string> PinnedMoves { get; set; } = new List<string>();
        // Total coverage is the total amount of sqaures that are being attacked by a certain piece
        public List<string> TotalCoverage = new List<string>();
        // Defending moves are moves that can come inbetween an attacking piece and the friendly king
        // without exposing it to another attack
        public virtual List<string> GrossMoves { get; set; } = new List<string>();
        // These are the actual moves that can be performed and is what the MovePiece method checks
        // when attempting to move a piece
        public List<string> NetMoves { get; set; } = new List<string>();

        //Constructor
        public Piece(string coords, bool isWhite)
        {
            Coords = coords;
            IsWhite = isWhite;
        }
        //Methods
        // Abstract method beacuse every piece is unique. Controls the direction that a piece can
        // move and adds each move to TotalCoverage for each piece.
        public abstract void UpdateCoverage(Dictionary<string, Piece> pieces);

        // This coverts TotalCoverage to GrossMoves by removing friendly fire. Kings and Pawns
        // are weird so they have an override method. After this it calculates NetMoves by
        // filtering out pieces
        public virtual void CalcMoves(Dictionary<string, Piece> pieces)
        {
            foreach (string move in this.TotalCoverage)
            {
                if (pieces.ContainsKey(move))
                {
                    Piece temp = pieces[move];
                    if (temp.IsWhite == this.IsWhite)
                    {
                        continue;
                    }
                }
                this.GrossMoves.Add(move);
            }
            // Removes all pinned piece moves not on the same vector as the pin
            if (this.IsPinned == true)
            {
                foreach (string move in this.PinnedMoves)
                {
                    if (this.GrossMoves.Contains(move))
                    {
                        this.NetMoves.Add(move);
                    }
                }
            }
            // If not pinned, then all moves are possible
            else
            {
                this.NetMoves = this.GrossMoves;
            }

        }
        // Moves piece by removing it from dictionary and readding it under new coords.
        // User-chosen move must be on the NetMoves list
        public void MovePiece(Dictionary<string, Piece> pieces, string endCoord)
        {
            Piece startPiece = (Piece)pieces[Coords];
            if (startPiece.NetMoves.Contains(endCoord))
            {
                pieces.Remove(Coords);
                if (pieces.ContainsKey(endCoord))
                {
                    Piece endPiece = (Piece)pieces[endCoord];
                    if (endPiece.IsWhite == startPiece.IsWhite)
                    {
                        throw new Exception("A piece cannot take a piece of the same color");
                    }
                    pieces.Remove(endCoord);
                }
                pieces.Add(endCoord, startPiece);
                Coords = endCoord;
                this.MoveCounter++;
            }
            else
            {
                throw new Exception("Invalid Move");
            }
        }
        // This method takes an attack vector as a stack and determines if a king is underattack
        // or if a piece in pinned within the vector. It starts at the end of the attack vector
        // and checks until it finds a king, and then saves the space between the attacking piece
        // and the king inclusive of the attacking piece to blockingMoves to check if the attack 
        // can be blocked. It also saves the entire attack vector to prevent the king from moving
        // backwards into an attack
        public void CalcPinsAndThreats(Stack<string> vector, Dictionary<string, Piece> pieces)
        {
            int pieceCounter = 0;
            bool oppositeKingOnVector = false;
            // Every square in the vector to the end of the board not including the king or the piece
            List<string> pThreatVector = new List<string>();
            // Every sqaure inbetween the king and the attacking piece including the piece and not
            // including the king
            List<string> pBlockingMoves = new List<string>();
            Piece topPieceInVector = new Bishop("i9",true);
            //Starting from the back of the attack vector
            for (int i = 0; i < vector.Count;)
            {
                // If the last board ref in the stack has a piece on it...
                string vectorEnd = vector.Pop();
                if (pieces.ContainsKey(vectorEnd))
                {
                    // Track the number of pieces in between the king and the attacking piece
                    if(oppositeKingOnVector) pieceCounter++;
                    topPieceInVector = (Piece)pieces[vectorEnd];
                    // If the piece is a king, add the coords of the attacking piece to the blocking
                    // moves of the enemy king, flip the bool, and update the piece counter
                    if (topPieceInVector is King && topPieceInVector.IsWhite != IsWhite)
                    {
                        pieceCounter++;
                        oppositeKingOnVector = true;
                        King king = (King)topPieceInVector;
                        pBlockingMoves.Add(this.Coords);
                        // continue to avoid adding the king's coords to blocking moves
                        continue;
                    }
                }
                // Adding to threat vector regardless of king on vector to also remove the sqaures 
                // behind the king as move options if there is a king on the vector
                pThreatVector.Add(vectorEnd); 
                // Gets the blocking moves between the king and the threat inclusive of the threat
                if(oppositeKingOnVector) { pBlockingMoves.Add(vectorEnd); }
            }
            if (oppositeKingOnVector)
            {
                switch (pieceCounter)
                {
                    // If there are no pieces inbetween the king and the attacking piece, pieceCounter
                    // will be a 1 which means a direct check
                    case 1:
                        if (topPieceInVector.IsWhite != this.IsWhite)
                        {
                            King king = (King)topPieceInVector;
                            king.CheckCounter++;
                            king.ThreatVector = pThreatVector;
                            king.BlockingMoves = pBlockingMoves;
                        }
                        break;
                    // If there is a single piece in between, the piece is pinned and cannot move
                    // anywhere else except for the other squares in the vector.
                    case 2:
                        if (topPieceInVector.IsWhite != this.IsWhite) 
                        { 
                            // Pin the piece, add every sqaure in the vector between the king and the 
                            // piece to the property PinnedMoves of the pinned piece for later
                            topPieceInVector.IsPinned = true; 
                            topPieceInVector.PinnedMoves = pBlockingMoves;
                            topPieceInVector.PinnedMoves.Remove(topPieceInVector.Coords);
                        }
                        break;
                }
            }
        }
        // Resets every property before each turn
        public virtual void ResetPositions()
        {
            IsPinned = false;
            PinnedMoves.Clear();
            TotalCoverage.Clear();
            NetMoves.Clear();
            GrossMoves.Clear();
        }
        // Debug get info
        public virtual void GetInfo()
        {
            Console.WriteLine($"Coordinates: {Coords}, IsWhite: {IsWhite}, Type: {Type}, IsPinned: {IsPinned}");
            Console.Write("TotalCoverage: ");
            foreach (string move in this.TotalCoverage)
            {
                Console.Write($"({move}) ");
            }
            Console.WriteLine();
            Console.Write("NetMoves: ");
            foreach (string move in this.NetMoves)
            {
                Console.Write($"({move}) ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
