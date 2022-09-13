Dictionary<string, Chess.Piece> pieces = new Dictionary<string, Chess.Piece>();
pieces.Add("a1", new Chess.Rook("a1", true));
pieces.Add("b1", new Chess.Knight("b1", true));
pieces.Add("c1", new Chess.Bishop("c1", true));
pieces.Add("d1", new Chess.Queen("d1", true));
pieces.Add("e1", new Chess.King("e1", true));
pieces.Add("f1", new Chess.Bishop("f1", true));
pieces.Add("g1", new Chess.Knight("g1", true));
pieces.Add("h1", new Chess.Rook("h1", true));
pieces.Add("a2", new Chess.Pawn("a2", true));
pieces.Add("b2", new Chess.Pawn("b2", true));
pieces.Add("c2", new Chess.Pawn("c2", true));
pieces.Add("d2", new Chess.Pawn("d2", true));
pieces.Add("e2", new Chess.Pawn("e2", true));
pieces.Add("f2", new Chess.Pawn("f2", true));
pieces.Add("g2", new Chess.Pawn("g2", true));
pieces.Add("h2", new Chess.Pawn("h2", true));

pieces.Add("a8", new Chess.Rook("a8", false));
pieces.Add("b8", new Chess.Knight("b8", false));
pieces.Add("c8", new Chess.Bishop("c8", false));
pieces.Add("d8", new Chess.Queen("d8", false));
pieces.Add("e8", new Chess.King("e8", false));
pieces.Add("f8", new Chess.Bishop("f8", false));
pieces.Add("g8", new Chess.Knight("g8", false));
pieces.Add("h8", new Chess.Rook("h8", false));
pieces.Add("a7", new Chess.Pawn("a7", false));
pieces.Add("b7", new Chess.Pawn("b7", false));
pieces.Add("c7", new Chess.Pawn("c7", false));
pieces.Add("d7", new Chess.Pawn("d7", false));
pieces.Add("e7", new Chess.Pawn("e7", false));
pieces.Add("f7", new Chess.Pawn("f7", false));
pieces.Add("g7", new Chess.Pawn("g7", false));
pieces.Add("h7", new Chess.Pawn("h7", false));

int turnCounter = 0;
while (true)
{
    printBoard(pieces);
    turnCounter++;
    bool isWhiteTurn = turnCounter % 2 == 1 ? true : false;
    string playerTurn = turnCounter % 2 == 1 ? "white" : "black";
    foreach (Chess.Piece piece in pieces.Values)
    {
        piece.ResetPositions();
    }
    foreach (Chess.Piece piece in pieces.Values)
    {
        piece.UpdateCoverage(pieces);
    }
    foreach (Chess.Piece piece in pieces.Values)
    {
        piece.CalcMoves(pieces);
    }
    foreach (Chess.Piece piece in pieces.Values)
    {
        if (piece is Chess.King)
        {
            Chess.King king = (Chess.King)piece;
            king.CheckCheckmate(pieces);
        }
    }
    // Uncomment for debug
    //foreach (Chess.Piece piece in pieces.Values)
    //{
    //    piece.GetInfo();
    //}
    Chess.Piece selectedPiece = new Chess.Bishop("i9",true);
    bool valid = false;
    while (!valid)
    {
        Console.WriteLine($"{playerTurn}, choose a piece:");
        try
        {
            selectedPiece = GetPieceFromReference(pieces, Console.ReadLine()
                .Trim().ToLower(), isWhiteTurn);
            valid = true;
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid Piece Selection");
        }
    }
    valid = false;
    while (!valid)
    {
        Console.WriteLine($"{playerTurn}, choose a destination:");
        try
        {
            string destinationSquare = Console.ReadLine().Trim().ToLower();
            selectedPiece.MovePiece(pieces, destinationSquare);
            valid = true;
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid Destination");
        }
    }
    Console.Clear();
}


//Methods

Chess.Piece GetPieceFromReference(Dictionary<string, Chess.Piece> pieces, string reference, bool isWhiteTurn)
{
    if (pieces.ContainsKey(reference))
    {
        Chess.Piece refPiece = pieces[reference];
        if (isWhiteTurn == refPiece.IsWhite)
        {
            return refPiece;
        }
        else
        {
             throw new Exception("Cannot move the other player's pieces.");
        }
    }
    else
    {
        throw new Exception("Invalid reference");
    }
}
void printBoard(Dictionary<string, Chess.Piece> pieces) 
{
    Chess.Piece refArray = new Chess.Bishop("a1", false);
    string[,] referenceArray = refArray.ReferenceArray;
    for (int i = 0; i < referenceArray.GetLength(0); i++)
    {
        for (int j = 0; j < referenceArray.GetLength(1); j++)
        {
            string icon = "";
            if( pieces.ContainsKey(referenceArray[i,j])) 
            {
                Chess.Piece piece = pieces[referenceArray[i,j]];
                if (piece.IsWhite)
                {
                    icon += "| W";
                }
                else
                {
                    icon += "| B";
                }
                switch (piece)
                {
                    case Chess.Bishop:
                        icon += "B ";
                        break;
                    case Chess.King:
                        icon += "K ";
                        break;
                    case Chess.Rook:
                        icon += "R ";
                        break;
                    case Chess.Queen:
                        icon += "Q ";
                        break;
                    case Chess.Knight:
                        icon += "H ";
                        break;
                    case Chess.Pawn:
                        icon += "P ";
                        break;
                }
                Console.Write(icon);
            }
            else
            {
                Console.Write("|    ");
            }
        }
        Console.WriteLine();
        Console.WriteLine("|____|____|____|____|____|____|____|____|");
    }
}



