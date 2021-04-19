using E_Chess.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace E_Chess.Utilities
{
    public static class PieceManager
    {
        public static string GetImage(string PieceName, Color Color)
        {

            string path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\images"}";
            var files = Directory.GetFiles(path);
            if (Color == Color.White)
            {
                //White
                var path1 = files.FirstOrDefault(x => x.Contains(PieceName));
                if (path1 != null)
                {
                    var result = path1.Split("\\");
                    return result.LastOrDefault();
                }

            }
            else
            {
                //Black
                var path2 = files.FirstOrDefault(x => x.Contains(PieceName));
                if (path2 != null)
                {
                    var result = path2.Split("\\");
                    return result.LastOrDefault();
                }
            }
            return String.Empty;
        }
        public static bool ValidateMove(Color currentColor, Tile movefrom, Tile moveto, Board board)
        {
            //Check whether the move to and move from can happen
            //Check whether there's something in the path of where the piece is from to where its going eg. a rook can't eat a queen if there's something infront of it
            //Check whether the place the king is moving too isn't a danger square
            // check if they moving from and to the same place
            //eating the same color piece
            var pieceonMoveFrom = movefrom.Piece;
            var possibleMoves = pieceonMoveFrom.GetNeighbours(board);
            var allPieces = GetAllPlayersPieces(board, Color.White);
            allPieces.AddRange(GetAllPlayersPieces(board, Color.Black));
            //king case
            if(movefrom.Description == moveto.Description) { return false; }
            if(!movefrom.IsEmpty && !moveto.IsEmpty)
            {
                if(movefrom.Piece.Color == moveto.Piece.Color)
                {
                    return false;
                }
            }
            if(pieceonMoveFrom != null && pieceonMoveFrom.GetName() == "king")
            {
                var test = ValidKing(currentColor, movefrom, moveto, board);
                //var flag = possibleMoves.Any(x => x.Description == moveto.Description);
                if (!test) return false;
           
            }
            if (pieceonMoveFrom != null && pieceonMoveFrom.GetName() == "bishop")
            {
                var test = ValidBishop(currentColor, movefrom, moveto, board);
                if (!test) return false;
            }
            if (pieceonMoveFrom != null && pieceonMoveFrom.GetName() == "rook")
            {
                var test = ValidRook(currentColor, movefrom, moveto, board);
                if (!test) return false;
            }
         

            if (pieceonMoveFrom != null && pieceonMoveFrom.GetName() == "queen")
            {
                var test = ValidQueen(currentColor, movefrom, moveto, board);
                if (!test) return false;
            }
            if (pieceonMoveFrom != null && pieceonMoveFrom.GetName() == "knight")
            {
                var flag = possibleMoves.Any(x => x.Description == moveto.Description && !x.IsEmpty);
                return false;
            }
      
            return true;
        }

      
        private static List<Tile> GetRow(int x, Board board)
        {
            var list = board.Tiles.Values.Where(y => y.Xpos == x).ToList();
            return list;
        }
        private static List<Tile> GetColumn(int y, Board board)
        {
            var list = board.Tiles.Values.Where(x => x.Ypos == y).ToList();
            return list;
        }

        private static List<Tile> GetDiagonal(Color currentColor,Tile CurrentPos, Board board)
        {
            int row = CurrentPos.Xpos, col = CurrentPos.Ypos;

            var list = new List<Tile>();

            if (Color.Black == currentColor)
            {
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 1));

                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != currentColor)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row -= 1;
                    col -= 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 1));
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != currentColor)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row += 1;
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1));
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != currentColor)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row -= 1;
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 1));
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != currentColor)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row += 1;
                    col -= 1;
                }
            }
            else
            {
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 1));
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != currentColor)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row -= 1;
                    col -= 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 1));
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != currentColor)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row += 1;
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1));
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != currentColor)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row -= 1;
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 1));
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != currentColor)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row += 1;
                    col -= 1;
                }
            }
            return list;
        }
        
        private static bool ValidRook(Color currentColor, Tile movefrom, Tile moveto, Board board)
        {
      
            var test = (moveto.Xpos - movefrom.Xpos == 0) || (moveto.Ypos - movefrom.Ypos == 0);
            if (!test) return false;
            var rows = GetRow(movefrom.Xpos,board);
            var columns = GetColumn(movefrom.Ypos, board);
            if(rows.Any(x => !x.IsEmpty && x.Ypos != moveto.Ypos && x.Xpos != moveto.Xpos && x.Piece.Color != currentColor)) { return false; }
            if(columns.Any(x => !x.IsEmpty && x.Ypos != moveto.Ypos && x.Xpos == moveto.Xpos && x.Piece.Color != currentColor)) { return false; }
            

            return true;
        }
        private static bool ValidBishop(Color currentColor, Tile movefrom, Tile moveto, Board board)
        {
           
            var test = (moveto.Xpos - movefrom.Xpos) == (moveto.Ypos - movefrom.Ypos);
            if (!test) return false;
         
            return true;
        }
        private static bool ValidQueen(Color currentColor, Tile movefrom, Tile moveto, Board board)
        {
            var validateRook = ValidRook(currentColor, movefrom, moveto, board);
            var validateBishop = ValidBishop(currentColor, movefrom, moveto, board);
            return validateRook || validateBishop;
        }
        private static bool ValidKing(Color currentColor, Tile movefrom, Tile moveto, Board board)
        {
            var rows = GetRow(movefrom.Xpos, board);
            var columns = GetColumn(movefrom.Ypos, board);
            var diagonals = GetDiagonal(currentColor, movefrom, board);

            var isColumnUnderThreat = columns.Any(x => x.isUnderAttackBlack && (x.Xpos == moveto.Xpos && x.Ypos == moveto.Ypos));
            var isRowUnderThreat = rows.Any(x => x.isUnderAttackBlack && (x.Xpos == moveto.Xpos && x.Ypos == moveto.Ypos));
            var isDiagonalUnderThreat = diagonals.Any(x => x.isUnderAttackBlack && (x.Xpos == moveto.Xpos && x.Ypos == moveto.Ypos));

            if (isColumnUnderThreat || isRowUnderThreat || isDiagonalUnderThreat)
            {
                return false;
            }

            return true;
        }
        private static bool ValidKnight(Color currentColor, Tile movefrom, Tile moveto, Board board)
        {
            return false;
        }
        private static bool ValidPawn(Color currentColor, Tile movefrom, Tile moveto, Board board)
        {
            return false;
        }
        public static Board UpdateAttackedSpots(Color currentColor, Board board)
        {
            List<Tile> masterPieces = board.Tiles.Values.Where(x => x.Piece != null && x.Piece.Color == currentColor).ToList();
            List<Tile> remainingSpots = board.Tiles.Values.Where(x => !masterPieces.Any(y => x.Xpos == y.Xpos && x.Ypos == y.Ypos)).ToList();
            foreach(var item in remainingSpots)
            {
                item.isUnderAttackBlack = false;
                item.isUnderAttackWhite = false;
                board.SetTile(item);
            }
            foreach (var tile in masterPieces)
            {
                var moves = tile.Piece.GetNeighbours(board);
                //var currPiece = board.Tiles.Values.FirstOrDefault(x => x.Xpos == tile.Xpos && x.Ypos == tile.Ypos);
                //moves.Add(currPiece);
                foreach (var move in moves)
                {
                   // move.isUnderAttack = true;
                    if(currentColor == Color.White)
                    {
                        move.isUnderAttackBlack = true;
                    }
                    else
                    {
                        move.isUnderAttackWhite = true;
                    }
                    board.SetTile(move);
                }

            }
            return board;
        }
        public static List<Tile> GetSquaresUnderThreat (Color currentColor, Board board)
        {
            if (Color.White == currentColor)
            {
                var resultData = board.Tiles.Values.Where(x => x.isUnderAttackBlack).ToList();
                return resultData;
            }
        
            var data = board.Tiles.Values.Where(x => x.isUnderAttackWhite).ToList();
            return data;

        }
        public static bool isCheck(Color currentColor, Board board)
        {
            board = UpdateAttackedSpots(currentColor, board);
            ///List<Tile> masterPieces = board.Tiles.Values.Where(x => x.Piece != null && x.Piece.Color == currentColor).ToList();
            var oppenentsKing = board.Tiles.FirstOrDefault(x => x.Value.Piece != null &&  x.Value.Piece.GetName() == "king" && x.Value.Piece.Color != currentColor);
            if(currentColor == Color.White)
            {
                if (oppenentsKing.Value != null && oppenentsKing.Value.isUnderAttackBlack) return true;
            }
            else
            {
                if (oppenentsKing.Value != null && oppenentsKing.Value.isUnderAttackWhite) return true;
            }
            
         
            return false;

        }
        public static bool isCheckMate(Color currentColor, Board board)
        {
            var oppenentsKing = board.Tiles.FirstOrDefault(x => x.Value.Piece != null && x.Value.Piece.GetName() == "king" && x.Value.Piece.Color != currentColor);
            var isInCheck = isCheck(currentColor, board);
            if (isInCheck)
            {
                var kingMoves = oppenentsKing.Value.Piece.GetNeighbours(board);

                if (currentColor == Color.White)
                {
                    if (kingMoves.All(x => x.isUnderAttackBlack))
                    {
                        return true;
                    }
                }
                else
                {
                    if (kingMoves.All(x => x.isUnderAttackWhite))
                    {
                        return true;
                    }

                }
             
            }
            return false;
        }
        public static void ProcessMove(Board board, Tile movefrom, Tile moveto, List<TrackMove> TrackMoves)
        {
            if (!moveto.IsEmpty && (moveto.Piece.Color != movefrom.Piece.Color))
            {
                if (moveto.Piece.Color == Color.White)
                {
                    var currentPieceOnMoveTo = moveto.Piece.GetNeighbours(board);
                    foreach (var move in currentPieceOnMoveTo)
                    {
                        move.isUnderAttackBlack = false;
                        board.SetTile(move);
                    }
                }
                else
                {
                    var currentPieceOnMoveTo = moveto.Piece.GetNeighbours(board);
                    foreach (var move in currentPieceOnMoveTo)
                    {
                        move.isUnderAttackWhite = false;
                        board.SetTile(move);
                    }

                }

            }

            var item = board.Tiles.Values.FirstOrDefault(x => x.Description == moveto.Description);
            item.Piece = movefrom.Piece;
            item.Piece.CurrentPos.Description = moveto.Description;
            item.Piece.CurrentPos.Xpos = moveto.Xpos;
            item.Piece.CurrentPos.Ypos = moveto.Ypos;
            item.Piece.MoveCount += 1;
            item.isUnderAttackBlack = movefrom.isUnderAttackBlack;
            item.isUnderAttackWhite = movefrom.isUnderAttackWhite;

  

            board.SetTile(item);
            //var moveTrack = new TrackMove(item.Piece.Color, item.Piece.TypeName, item.Description);
            //TrackMoves.Add(moveTrack);
            movefrom.Piece = null;
            movefrom.IsEmpty = true;

            board.SetTile(moveto);
        }
        public static List<Tile> GetPlayersPieces(Board board, Color currentColor)
        {
            List<Tile> masterPieces = board.Tiles.Values.Where(x => x.Piece != null && x.Piece.Color == currentColor && x.Piece.GetName() != "king").ToList();
            return masterPieces;
        }
        public static List<Tile> GetAllPlayersPieces(Board board, Color currentColor)
        {
            List<Tile> masterPieces = board.Tiles.Values.Where(x => x.Piece != null && x.Piece.Color == currentColor).ToList();
            return masterPieces;
        }
        public static List<Tile> GetNeighbours(Board board, Tile tile)
        {
            //Check left side
            int row = tile.Xpos, col = tile.Ypos;
            var list = new List<Tile>();
            //Assumes a piece is within the middle to centre of a board.
            if ( (row > 0 && row < board.Rows)  && (col > 0 && col < board.Columns))
            {
              
                var left = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (x.Xpos - 1));
                var right = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (x.Xpos + 1));
                var up = board.Tiles.Values.FirstOrDefault(x => x.Ypos == (x.Ypos + 1));
                var down = board.Tiles.Values.FirstOrDefault(x => x.Ypos == (x.Ypos - 1));
                var leftup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (x.Xpos - 1) && x.Ypos == (x.Ypos + 1));
                var leftdown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (x.Xpos - 1) && x.Ypos == (x.Ypos - 1));
                var rightup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (x.Xpos + 1) && x.Ypos == (x.Ypos + 1));
                var rightDown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (x.Xpos + 1) && x.Ypos == (x.Ypos - 1));
                list.Add(left);
                list.Add(right);
                list.Add(down);
                list.Add(up);
                list.Add(leftup);
                list.Add(rightup);
                list.Add(leftdown);
                list.Add(rightDown);
                return list.Where(x => x.IsEmpty).ToList();
            }
            //Assumes a piece is on the left side 
            if( row == 0 && col >= 0)
            {

            }
            return null;
            //Check right side
            //Check below 
            //Check above 
            // Check left,up
            //Check right,up
            //Check left, down
            //Check right, below
        }

    }
}
