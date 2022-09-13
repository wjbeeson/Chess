using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Queen : Piece
    {
        public Queen(string coords, bool isWhite) : base(coords, isWhite)
        {
            Type = "Queen";
            Value = 9;
        }
        // Easiest piece since it's just a bishop and a rook
        public override void UpdateCoverage(Dictionary<string, Piece> pieces)
        {

            Bishop bishop = new Bishop(this.Coords, this.IsWhite);
            Rook rook = new Rook(this.Coords, this.IsWhite);
            bishop.UpdateCoverage(pieces);
            rook.UpdateCoverage(pieces);
            foreach (string move in bishop.TotalCoverage)
            {
                this.TotalCoverage.Add(move);
            }
            foreach (string move in rook.TotalCoverage)
            {
                this.TotalCoverage.Add(move);
            }
        }
    }
}
