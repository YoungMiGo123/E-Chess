using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using E_Chess.Models;
using E_Chess.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace E_Chess.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public Board board { get; set; }
        [BindProperty]
        public string GameState { get; set; }
        [BindProperty]
        public string Pieces { get; set; }
        [BindProperty]
        public List<Tile> PieceNeighbours { get; set; }

        public List<TrackMove> TrackMoves { get; set; }
        public List<Check> TrackChecks { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

        }
        [BindProperty]

        public string DataField1 { get; set; }

        [BindProperty]
        public string DataField2 { get; set; }
        [BindProperty]
        public string ColorPlayingNow { get; set; }
        public bool isFirstMove { get; set; }
        public bool isGameOver { get; set; }
        public bool hasMoved { get;set; }
        [BindProperty]
        public string Message { get; set; }

        #region OnMethods
        public void OnGet()
        {
            if (string.IsNullOrEmpty(DataField1) || string.IsNullOrEmpty(DataField2))
            {
                board = new Board();
                var description = "ABCDEFGH";

                int count = 0;
                int indx = 0;
                int ticket = 1;
                for (int i = 0; i < board.Rows; i++)
                {
                    for (int j = 0; j < board.Columns; j++)
                    {

                        var tile = new Tile()
                        {
                            Id = count,
                            Description = description[indx] + $"{ticket}",
                            IsEmpty = true,
                            Piece = null,
                            Xpos = i,
                            Ypos = j,
                            isUnderAttackWhite = false,
                            isUnderAttackBlack = false


                        };

                        board.Tiles.Add(count, tile);
                        count += 1;
                        indx += 1;
                    }
                    ticket += 1;
                    indx = 0;
                }
                TrackMoves = new List<TrackMove>();
                TrackChecks = new List<Check>();
                //  SetWhitePawns();
                // 
                // SetWhiteBoardPieces();
                // SetBlackPawns();
                //  SetBlackBoardPieces();
                //SetBlackHorses();
                //SetWhiteKing();
                //SetBlackKing();
                 SetWhiteKingNQueen();
                SetBlackKingNQueen();
                SetBlackRooks();
                SetWhiteRooks();
                //SetWhiteBishops();
                //SetBlackBishops();
            }
    


            GameState = JsonConvert.SerializeObject(board);
            WriteToDb(GameState);

            Pieces = JsonConvert.SerializeObject(TrackMoves);
            WriteMovesToDb(Pieces);


            var checks = JsonConvert.SerializeObject(TrackChecks);
            WriteChecksToDb(checks);

            ColorPlayingNow = Color.White == 0 ? "W" : "B";
       
  
        }
        public void OnPost()
        {
            var res = Request.Form["DataField"];
            hasMoved = false;
            ReadMovesFromDb();
            if(TrackMoves != null && TrackMoves.Count > 0)
            {
                ColorPlayingNow = TrackMoves.LastOrDefault().Color == Color.White ? "B" : "W"; 
            }
            ReadFromDb();

            ReadChecksFromDb();
            board = PieceManager.UpdateAttackedSpots(ColorPlayingNow != "W" ? Color.White : Color.Black, board);
            //  var neighbours = obj.Tiles.Where(x => x.Value.Description == DataField);
            var movefrom = board.GetTile(DataField1);
            var moveto = board.GetTile(DataField2);
            var color = ColorPlayingNow == "W" ? Color.White : Color.Black;
            var validation = PieceManager.ValidateMove(color, movefrom, moveto, board);
            if (!validation)
            {
                Message = $"Invalid move {color}";
                return;
            }
            var lastCheck = TrackChecks.LastOrDefault();
            if (!movefrom.IsEmpty)
            {
              
                if (lastCheck != null && lastCheck.isChecked && lastCheck.isActive)
                {
                    var kingInCheck = Getkingincheck(ColorPlayingNow == "W" ? Color.White : Color.Black, board);
                    var moves = kingInCheck.Piece.GetNeighbours(board);
                    var EndageredSquared = PieceManager.GetSquaresUnderThreat(ColorPlayingNow == "W" ? Color.White : Color.Black, board);
                    moves.AddRange(EndageredSquared);
                    var canMovePieces = new List<Tile>();
                    if (ColorPlayingNow == "W" )
                    {
                        var currColor = lastCheck.ColorInCheck == "B" ? "Black" : "White";
                        //white attacking black, check what squares black is controlling/attacking
                        if (moves.Any(x => x.isUnderAttackWhite))
                        {
                            var squareUnderAttack = moves.Where(x => x.isUnderAttackWhite || (x.Piece != null && x.Piece.Color != Color.Black));
                            if (!hasMoved)
                            {


                                if (movefrom.Piece.GetName() != "king")
                                {
                                    var currentPlayersPieces = PieceManager.GetPlayersPieces(board, Color.Black);

                                    var currentMoves = currentPlayersPieces.Select(x => x.Piece);
                                    foreach (var piece in currentMoves)
                                    {
                                        foreach (var sq  in squareUnderAttack)
                                        {
                                            var canMove = piece.GetNeighbours(board).Any(x => x.Xpos == sq.Xpos && x.Ypos == sq.Ypos);
                                            if (canMove)
                                            {
                                                canMovePieces.Add(currentPlayersPieces.FirstOrDefault(x => x.Piece.GetName() == piece.GetName()));
                                            }
                                        }
                               
                                    }

                                    if (canMovePieces.Any(x => x.Piece.GetName() == movefrom.Piece.GetName()))
                                    {
                                        // Validate move, then Process the move if it passes validation, else return false
                                        var flag = PieceManager.ValidateMove(ColorPlayingNow == "W" ? Color.White : Color.Black, movefrom, moveto, board);
                                        if (!flag)
                                        {
                                            var tmpColor = lastCheck.ColorInCheck == "B" ? "Black" : "White";
                                            Message = $"{tmpColor} - Your king is currently under threat (Check)<br/> Please protect your king to continue";
                                            return;
                                        }
                                        var currentMove = new TrackMove(ColorPlayingNow == "W" ? Color.White : Color.Black, movefrom.Piece.TypeName, movefrom.Description);
                                        TrackMoves.Add(currentMove);
                                        PieceManager.ProcessMove(board, movefrom, moveto, TrackMoves);
                                        PieceManager.UpdateAttackedSpots(Color.Black, board);
                                        lastCheck.isActive = false;
                                        hasMoved = true;
                                        TrackChecks.Add(lastCheck);
                                        ProcessCheck();
                                        ProcessCheckMate();
                                        ChangePlayers();
                                        CompleteWrites();
                                        return;
                                    }
                                    else
                                    {
                                    
                                        Message = $"{currColor} - Your king is currently under threat (Check)<br/> Please move your king to continue";
                                        return;
                                    }

                                }
                            }
                            if (!hasMoved)
                            {
                                if (movefrom.Piece.GetName() == "king")
                                {
                                    // Validate can the king move there.
                                    if (moveto.isUnderAttackWhite)
                                    {
                                        Message = $"{currColor} - You cannot move to {moveto.Description}, king is currently under threat (Check)<br/> Please move your king to continue";
                                        return;
                                    }
                                    lastCheck.isActive = false;
                                    var currentMove = new TrackMove(ColorPlayingNow == "W" ? Color.White : Color.Black, movefrom.Piece.TypeName, movefrom.Description);
                                    TrackMoves.Add(currentMove);
                                    PieceManager.ProcessMove(board, movefrom, moveto, TrackMoves);
                                    PieceManager.UpdateAttackedSpots(Color.Black, board);
                                    hasMoved = true;
                                    TrackChecks.Add(lastCheck);
                                    ProcessCheck();
                                    ProcessCheckMate();
                                    ChangePlayers();
                                    CompleteWrites();
                                    return;
                                }
                            }
                        

                        }
                    }
                   if(ColorPlayingNow == "B")
                    {
                        //Black attacking white, check what squares black is controlling/attacking
                        if (moves.Any(x => x.isUnderAttackWhite))
                        {

                            var squareUnderAttack = moves.Where(x => x.isUnderAttackWhite || (x.Piece != null && x.Piece.Color != Color.White));
                            if (movefrom.Piece.GetName() != "king")
                            {

                                var currentPlayersPieces = PieceManager.GetPlayersPieces(board, Color.White);

                                var currentMoves = currentPlayersPieces.Select(x => x.Piece);
                                foreach (var piece in currentMoves)
                                {
                                    foreach (var sq in squareUnderAttack)
                                    {
                                        var canMove = piece.GetNeighbours(board).Any(x => x.Xpos == sq.Xpos && x.Ypos == sq.Ypos);
                                        if (canMove)
                                        {
                                            canMovePieces.Add(currentPlayersPieces.FirstOrDefault(x => x.Piece.GetName() == piece.GetName()));
                                        }
                                    }

                                }

                                if (canMovePieces.Any(x => x.Piece.GetName() == movefrom.Piece.GetName()))
                                {
                                    var flag = PieceManager.ValidateMove(ColorPlayingNow == "W" ? Color.White : Color.Black, movefrom, moveto, board);
                                    if (!flag)
                                    {
                                        var currColor = lastCheck.ColorInCheck == "B" ? "Black" : "White";
                                        Message = $"{currColor} - Your king is currently under threat (Check)<br/> Please protect your king to continue";
                                        return;
                                    }
                                    var currentMove = new TrackMove(ColorPlayingNow == "W" ? Color.White : Color.Black, movefrom.Piece.TypeName, movefrom.Description);
                                    TrackMoves.Add(currentMove);
                                    PieceManager.ProcessMove(board, movefrom, moveto, TrackMoves);
                                    PieceManager.UpdateAttackedSpots(Color.White, board);
                                    hasMoved = true;
                                    lastCheck.isActive = false;
                                    TrackChecks.Add(lastCheck);
                                    ProcessCheck();
                                    ProcessCheckMate();
                                    ChangePlayers();
                                    CompleteWrites();
                                    return;
                                }
                                else
                                {
                                    var currColor = lastCheck.ColorInCheck == "B" ? "Black" : "White";
                                    Message = $"{currColor} - Your king is currently under threat (Check)<br/> Please move your king to continue";
                                    return;
                                }

                            }
                            if (movefrom.Piece.GetName() == "king")
                            {
                                // Validate can the king move there.
                                if (moveto.isUnderAttackWhite)
                                {
                                    var currColor = lastCheck.ColorInCheck == "B" ? "Black" : "White";
                                    Message = $"{currColor} - You cannot move to {moveto.Description}, king is currently under threat (Check)<br/> Please move your king to continue";
                                    return;
                                }
                                lastCheck.isActive = false;
                                var currentMove = new TrackMove(ColorPlayingNow == "W" ? Color.White : Color.Black, movefrom.Piece.TypeName, movefrom.Description);
                                TrackMoves.Add(currentMove);
                                PieceManager.ProcessMove(board, movefrom, moveto, TrackMoves);
                                PieceManager.UpdateAttackedSpots(Color.White, board);
                                hasMoved = true;
                                TrackChecks.Add(lastCheck);
                                ProcessCheck();
                                ProcessCheckMate();
                                ChangePlayers();
                                CompleteWrites();
                                return;
                            }
                        }
                    }
                 
                }
              
                if (TrackMoves.Count == 0 && movefrom.Piece.Color != Color.White)
                {
                    ColorPlayingNow = "W";
                    return;
                }
        


                if (!hasMoved)
                {
                    PieceNeighbours = movefrom.Piece.GetNeighbours(board);
                    if (PieceNeighbours.Any(x => x.Description == moveto.Description))
                    {
                      
                        var currentMove = new TrackMove(ColorPlayingNow == "W" ? Color.White : Color.Black, movefrom.Piece.TypeName, movefrom.Description);
                        TrackMoves.Add(currentMove);
                        PieceManager.ProcessMove(board, movefrom, moveto, TrackMoves);
                        hasMoved = true;
                     
                        ProcessCheck();
                        ProcessCheckMate();
                        if (movefrom.Piece != null && movefrom.Piece.GetName() == "king")
                        {
                            lastCheck.isActive = false;

                        }
                        ChangePlayers();
                        CompleteWrites();
                        return;
                    }
               
                }
            }
          
           
        
        
        }
        private void CompleteWrites()
        {
            GameState = JsonConvert.SerializeObject(board);
            WriteToDb(GameState);

            Pieces = JsonConvert.SerializeObject(TrackMoves);
            WriteMovesToDb(Pieces);

            var checks = JsonConvert.SerializeObject(TrackChecks);
            WriteChecksToDb(checks);

        }
        private void ProcessCheckMate()
        {
            var isCheckMate = PieceManager.isCheckMate(ColorPlayingNow == "W" ? Color.White : Color.Black, board);
            if (isCheckMate)
            {
                var ColorInCheckMate = ColorPlayingNow == "B" ? "W" : "B";
                if (ColorInCheckMate == "B")
                {
                    Message = $"The game is over Black Has lost! <br/> Congratulations White You Won!";
                    isGameOver = true;
                }
                else
                {
                    Message = $"The game is over White Has lost! <br/> Congratulations  White You Won!";
                    isGameOver = true;

                }
                return;

            }
        }
        private void ProcessCheck()
        {
            var isCheck = PieceManager.isCheck(ColorPlayingNow == "W" ? Color.White : Color.Black, board);
            if (isCheck)
            {
                var ColorInCheck = ColorPlayingNow == "B" ? "W" : "B";
                var kingInCheck = Getkingincheck(ColorPlayingNow == "W" ? Color.White : Color.Black, board);

                var newCheck = new Check()
                {
                    isChecked = true,
                    isActive = true,
                    ColorInCheck = ColorInCheck,
                    Xpos = kingInCheck.Xpos,
                    Ypos = kingInCheck.Ypos,
                    CheckID = -1
                };
                TrackChecks.Add(newCheck);


                Message = ColorPlayingNow == "B" ? "White your king is under threat by black (Check)" : "Black your king is under threat by white (Check)";

            }
        }
       private void ChangePlayers()
        {
            if (TrackMoves.Count >= 1)
            {
                var lastMovePlayed = TrackMoves.LastOrDefault();

                if (lastMovePlayed.Color == Color.White)
                {
                    ColorPlayingNow = "B";
                }
                else
                {
                    ColorPlayingNow = "W";
                }

            }
        }
        private Tile Getkingincheck(Color currentColor, Board board)
        {
            var oppenentsKing = board.Tiles.FirstOrDefault(x => x.Value.Piece != null && x.Value.Piece.GetName() == "king" && x.Value.Piece.Color != currentColor);//(x.Piece.Color != currentColor) x.Piece.TypeName = 'K')
            return oppenentsKing.Value;
        }
        #endregion

        #region WriteDBMethods
        public void WriteToDb(string data)
        {
            // Write file using StreamWriter  
            string path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\" + "gamedata.json"}";
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(data);
                writer.Close();
            }

        }
        public void WriteMovesToDb(string data)
        {
            // Write file using StreamWriter  
            string path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\" + "MovesMade.json"}";
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(data);
                writer.Close();
            }

        }
        public void WriteChecksToDb(string data)
        {
            // Write file using StreamWriter  
            string path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\" + "checks.json"}";
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(data);
                writer.Close();
            }

        }
        #endregion

        #region ReadFromDbMethods
        public void ReadMovesFromDb()
        {
            // Write file using StreamWriter  
            string path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\" + "MovesMade.json"}";
            using (StreamReader r = new StreamReader(path))
            {

                var movesMade = r.ReadToEnd();
                //   RebuiltPiecesMoved(movesMade);
                TrackMoves = JsonConvert.DeserializeObject<List<TrackMove>>(movesMade);
                if (TrackMoves == null)
                {
                    TrackMoves = new List<TrackMove>();
                }
                r.Close();

            }
        }
        public void ReadChecksFromDb()
        {
            // Write file using StreamWriter  
            string path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\" + "checks.json"}";
            using (StreamReader r = new StreamReader(path))
            {

                var checksMade = r.ReadToEnd();
                //   RebuiltPiecesMoved(movesMade);
                TrackChecks = JsonConvert.DeserializeObject<List<Check>>(checksMade);
                if (TrackChecks == null)
                {
                    TrackChecks = new List<Check>();
                }
                r.Close();

            }
        }
        public void ReadFromDb()
        {
            // Write file using StreamWriter  
            string path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\" + "gamedata.json"}";
            using (StreamReader r = new StreamReader(path))
            {
                GameState = r.ReadToEnd();
                RebuiltBoard();
                r.Close();
            }
        }
        #endregion

        #region SetupBoard
        public void RebuiltPiecesMoved(string data)
        {

            var jsonDoc = JsonDocument.Parse(data);
            var lists = jsonDoc.RootElement.EnumerateArray();
            var datalist = new List<Tile>();
            var superList = new List<List<Tile>>();
            foreach (var list in lists)
            {
                var list2 = list.EnumerateArray();
                foreach (var pieceX in list2)
                {
                    JsonElement test;
                    var element = pieceX.TryGetProperty("Piece",out test);
                    var flag = test.GetRawText();
                    if (element && !string.IsNullOrEmpty(flag) && flag != "null")
                    {
                        var Tile = new Tile()
                        {
                            Description = pieceX.GetProperty("Description").GetString(),
                            Id = pieceX.GetProperty("Id").GetInt32(),
                            IsEmpty = false,
                            Piece = null,
                            Xpos = pieceX.GetProperty("Xpos").GetInt32(),
                            Ypos = pieceX.GetProperty("Ypos").GetInt32()
                        };

                        var color = test.GetProperty("Color").GetInt32() == 0 ? Color.White : Color.Black;
                        var currentPos = test.GetProperty("CurrentPos");
                        var img = currentPos.GetProperty("Image").GetString();
                        var newPIeceData = new PieceData()
                        {
                            Description = currentPos.GetProperty("Description").GetString(),
                            PieceName = currentPos.GetProperty("PieceName").GetString(),
                            Xpos = currentPos.GetProperty("Xpos").GetInt32(),
                            Ypos = currentPos.GetProperty("Ypos").GetInt32(),
                            Image = img

                        };
                        var moveCount = test.GetProperty("MoveCount").GetInt32();
                        switch (newPIeceData.PieceName)
                        {
                            case "rook":
                                var rook = new Rook(color, newPIeceData);
                                rook.MoveCount = moveCount;
                                Tile.Piece = rook;
                                break;
                            case "pawn":
                                var pawn = new Pawn(color, newPIeceData);
                                pawn.MoveCount = moveCount;
                                Tile.Piece = pawn;
                                break;
                            case "bishop":
                                var bishop = new Bishop(color, newPIeceData);
                                bishop.MoveCount = moveCount;
                                Tile.Piece = bishop;
                                break;
                            case "king":
                                var king = new King(color, newPIeceData);
                                king.MoveCount = moveCount;
                                Tile.Piece = king;
                                break;
                            case "queen":
                                var queen = new Queen(color, newPIeceData);
                                queen.MoveCount = moveCount;
                                Tile.Piece = queen;
                                break;
                            case "horse":
                                var horse = new Horse(color, newPIeceData);
                                horse.MoveCount = moveCount;
                                Tile.Piece = horse;
                                break;
                        }
                        datalist.Add(Tile);
                    }

                }

                superList.Add(datalist);

            }


        }
        public void RebuiltBoard()
        {

            var jsonDoc = JsonDocument.Parse(GameState);
            board = JsonConvert.DeserializeObject<Board>(GameState);
            var myString = jsonDoc.RootElement.GetProperty("Tiles");   // Get a string from a JsonElement
            var list = myString.EnumerateObject();
            List<Tile> tiles = new List<Tile>();
            var dict = new Dictionary<int, Tile>();
            foreach (var item in list)
            {
                var keyID = Convert.ToInt32(item.Name);
                var empty = item.Value.GetProperty("IsEmpty").GetBoolean();
                var Tile = new Tile()
                {
                    Description = item.Value.GetProperty("Description").GetString(),
                    Id = item.Value.GetProperty("Id").GetInt32(),
                    IsEmpty = empty,
                    Piece = null,
                    Xpos = item.Value.GetProperty("Xpos").GetInt32(),
                    Ypos = item.Value.GetProperty("Ypos").GetInt32()
                };
                if (!empty)
                {
                    var pieceX = item.Value.GetProperty("Piece");
                    var color = pieceX.GetProperty("Color").GetInt32() == 0 ? Color.White : Color.Black;
                    var currentPos = pieceX.GetProperty("CurrentPos");
                    var img = currentPos.GetProperty("Image").GetString();
                    var newPIeceData = new PieceData()
                    {
                        Description = currentPos.GetProperty("Description").GetString(),
                        PieceName = currentPos.GetProperty("PieceName").GetString(),
                        Xpos = currentPos.GetProperty("Xpos").GetInt32(),
                        Ypos = currentPos.GetProperty("Ypos").GetInt32(),
                        Image = img

                    };
                    // var isPawnfistMove = newPIeceData.PieceName == "pawn" ? pieceX.GetProperty("isFirstMove").GetBoolean() : pieceX.GetProperty("isFirstMove").GetBoolean();
                    var moveCount = pieceX.GetProperty("MoveCount").GetInt32();
                    switch (newPIeceData.PieceName)
                    {
                        case "rook":
                            var rook = new Rook(color, newPIeceData);
                            Tile.Piece = rook;
                            Tile.Piece.MoveCount = moveCount;
                            break;
                        case "pawn":
                            var pawn = new Pawn(color, newPIeceData);
                            Tile.Piece = pawn;
                            Tile.Piece.MoveCount = moveCount;
                            break;
                        case "bishop":
                            var bishop = new Bishop(color, newPIeceData);
                            Tile.Piece = bishop;
                            Tile.Piece.MoveCount = moveCount;
                            break;
                        case "king":
                            var king = new King(color, newPIeceData);
                            Tile.Piece = king;
                            Tile.Piece.MoveCount = moveCount;
                            break;
                        case "queen":
                            var queen = new Queen(color, newPIeceData);
                            Tile.Piece = queen;
                            Tile.Piece.MoveCount = moveCount;
                            break;
                        case "horse":
                            var horse = new Horse(color, newPIeceData);
                            Tile.Piece = horse;
                            Tile.Piece.MoveCount = moveCount;
                            break;
                    }

                }
                board.Tiles[keyID] = Tile;


            }


        }
        public void SetWhitePawns()
        {
            int size = board.Rows;

            var list = new string[] { "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2" };
            for (int i = 0; i < size; i++)
            {
                var tile = board.GetTile(list[i]);
                tile.IsEmpty = false;
                var piece = new Pawn(Color.White, new PieceData()
                {
                    Description = tile.Description,
                    Xpos = tile.Xpos,
                    Ypos = tile.Ypos
                });
                piece.MoveCount = 0;
                tile.Piece = piece;

            }

        }
        public void SetBlackPawns()
        {
            int size = board.Rows;
            var list = new string[] { "A7", "B7", "C7", "D7", "E7", "F7", "G7", "H7" };
            for (int i = 0; i < size; i++)
            {
                var tile = board.GetTile(list[i]);
                tile.IsEmpty = false;
                var piece = new Pawn(Color.Black, new PieceData()
                {
                    Description = tile.Description,
                    Xpos = tile.Xpos,
                    Ypos = tile.Ypos
                });
                piece.MoveCount = 0;
                tile.Piece = piece;


            }

        }

        private void SetWhiteHorses()
        {
            var list = new string[] { "B1", "G1" };
            foreach (var horse in list)
            {
                var tile = board.GetTile(horse);
                tile.IsEmpty = false;
                var piece = new Horse(Color.White, new PieceData()
                {
                    Description = tile.Description,
                    Xpos = tile.Xpos,
                    Ypos = tile.Ypos
                });
                piece.MoveCount = 0;
                tile.Piece = piece;
                
            }
        }

        private void SetBlackHorses()
        {
            var list = new string[] { "B8", "G8" };
            foreach (var horse in list)
            {
                var tile = board.GetTile(horse);
                tile.IsEmpty = false;
                var piece = new Horse(Color.Black, new PieceData()
                {
                    Description = tile.Description,
                    Xpos = tile.Xpos,
                    Ypos = tile.Ypos
                });
                piece.MoveCount = 0;
                tile.Piece = piece;
            }
        }

        private void SetWhiteRooks()
        {
            var list = new string[] { "A1", "H1" };
            foreach (var horse in list)
            {
                var tile = board.GetTile(horse);
                tile.IsEmpty = false;
                var piece = new Rook(Color.White, new PieceData()
                {
                    Description = tile.Description,
                    Xpos = tile.Xpos,
                    Ypos = tile.Ypos
                });
                piece.MoveCount = 0;
                tile.Piece = piece;
            }
        }

        private void SetBlackRooks()
        {
            var list = new string[] { "A8", "H8" };
            foreach (var horse in list)
            {
                var tile = board.GetTile(horse);
                tile.IsEmpty = false;
                var piece = new Rook(Color.Black, new PieceData()
                {
                    Description = tile.Description,
                    Xpos = tile.Xpos,
                    Ypos = tile.Ypos
                });
                piece.MoveCount = 0;
                tile.Piece = piece;
            }
        }

        private void SetWhiteKingNQueen()
        {
            var list = new string[] { "D1", "E1" };
            int count = 0;
            foreach (var kq in list)
            {
                var tile = board.GetTile(kq);
                tile.IsEmpty = false;
                if (count == 0)
                {
                    var piece = new Queen(Color.White, new PieceData()
                    {
                        Description = tile.Description,
                        Xpos = tile.Xpos,
                        Ypos = tile.Ypos
                    });
                    piece.MoveCount = 0;
                    tile.Piece = piece;
                 
                }
                else
                {
                    var piece = new King(Color.White, new PieceData()
                    {
                        Description = tile.Description,
                        Xpos = tile.Xpos,
                        Ypos = tile.Ypos
                    });
                    piece.MoveCount = 0;
                    tile.Piece = piece;
                }
                count++;
            }
        }

        private void SetBlackKing()
        {
            var tile = board.GetTile("E8");
            var piece = new King(Color.Black, new PieceData()
            {
                Description = tile.Description,
                Xpos = tile.Xpos,
                Ypos = tile.Ypos
            });
            piece.MoveCount = 0;
            tile.Piece = piece;
            board.SetTile(tile);
        }
        private void SetWhiteKing()
        {
            var tile = board.GetTile("E1");
            var piece = new King(Color.White, new PieceData()
            {
                Description = tile.Description,
                Xpos = tile.Xpos,
                Ypos = tile.Ypos
            });
            piece.MoveCount = 0;
            tile.Piece = piece;
            board.SetTile(tile);
        }

        private void SetBlackKingNQueen()
        {
            var list = new string[] { "D8", "E8" };
            int count = 0;
            foreach (var kq in list)
            {
                var tile = board.GetTile(kq);
                tile.IsEmpty = false;
                if (count == 0)
                {
                    var piece = new Queen(Color.Black, new PieceData()
                    {
                        Description = tile.Description,
                        Xpos = tile.Xpos,
                        Ypos = tile.Ypos
                    });
                    piece.MoveCount = 0;
                    tile.Piece = piece;
                }
                else
                {
                    var piece = new King(Color.Black, new PieceData()
                    {
                        Description = tile.Description,
                        Xpos = tile.Xpos,
                        Ypos = tile.Ypos
                    });
                    piece.MoveCount = 0;
                    tile.Piece = piece;
                }
                count++;
            }
        }

        public void SetWhiteBishops()
        {
            var list = new string[] { "C1", "F1" };
            foreach (var bishop in list)
            {
                var tile = board.GetTile(bishop);
                tile.IsEmpty = false;
                var piece = new Bishop(Color.White, new PieceData()
                {
                    Description = tile.Description,
                    Xpos = tile.Xpos,
                    Ypos = tile.Ypos
                });
                piece.MoveCount = 0;
                tile.Piece = piece;
            }
        }
        public void SetBlackBishops()
        {
            var list = new string[] { "C8", "F8" };
            foreach (var bishop in list)
            {
                var tile = board.GetTile(bishop);
                tile.IsEmpty = false;
                var piece = new Bishop(Color.Black, new PieceData()
                {
                    Description = tile.Description,
                    Xpos = tile.Xpos,
                    Ypos = tile.Ypos
                });
                piece.MoveCount = 0;
                tile.Piece = piece;
            }

        }
        public void SetBlackBoardPieces()
        {
            //Set Black pawns
            SetBlackPawns();
            //Set Black horse
            SetBlackHorses();
            SetBlackBishops();
            //Set Black Queen and king
            SetBlackKingNQueen();
            //Set Rooks
            SetBlackRooks();
        }

        public void SetWhiteBoardPieces()
        {
            //Set Black pawns
            SetWhitePawns();
            //Set Black horse
            SetWhiteHorses();
            SetWhiteBishops();
            //Set Black Queen and king
            SetWhiteKingNQueen();
            //Set Rooks
            SetWhiteRooks();
        }

        #endregion

    }

}
