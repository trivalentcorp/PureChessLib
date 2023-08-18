﻿namespace PureChess
{
    public class Engine
    {
        public bool[] uciValidations = {false,false,false, false};

        public bool ValidateMove(int initialSquare, int targetSquare)
        {
            Piece piece = Game.board.squares[initialSquare].piece;

            int difference = targetSquare - initialSquare;


            Square[] moveSquares = { Game.board.squares[initialSquare], Game.board.squares[targetSquare] };

            switch (piece.type)
            {
                case "None":
                    return false;
                case "Pawn":
                    return Pawn(moveSquares[0], moveSquares[1], piece);
                case "King":
                    return King(moveSquares[0], moveSquares[1], piece); ;
                default:
                    return true;
            }
        }

        private bool Pawn(Square initial, Square final, Piece piece)
        {
            int d = final.index - initial.index;

            // Pawn moving forward
            if (d == 8 && piece.color == 0 || d == -8 && piece.color == 1)
            {
                return final.piece.GetPieceType("None");
            }

            // Pawn moving forward 2x
            if (d == 16 && piece.color == 0 && initial.x == 1 || d == -16 && piece.color == 1 && initial.x == 6)
            {
                return final.piece.GetPieceType("None");
            }

            if ((d == 9 || d == 8) && piece.color == 0 || (d == -8 || d == -9) && piece.color == 1)
            {
                return !final.piece.GetPieceType("None");
            }

            return false;
        }

        private bool King(Square initial, Square final, Piece piece)
        {
            int d = final.index - initial.index;

            // Pawn moving forward
            if (d == 8 || d == -8 || d == 1 || d == -1 || d == 7 || d == -7 || d == 9 || d == -9)
            {
                return final.piece.GetPieceType("None") || (!final.piece.GetPieceType("None") && final.piece.color != piece.color);
            }

            return false;
        }

        public bool MakeMove(Square initial, Square final)
        {
            bool isValid = ValidateMove(initial.index, final.index);

            if (isValid)
            {
                Piece defaultPiece = initial.piece;

                initial.piece = final.piece;

                final.piece = defaultPiece;
                Game.playerTurn = Game.playerTurn == 0 ? 1 : 0;

                Game.settings.DebugMessage($"§aMove {Game.ConvertToCoordinate(initial.index)}-{Game.ConvertToCoordinate(final.index)} is valid!");

                Game.board.DrawCurrentPosition();


                return true;
            }

            Game.settings.DebugMessage($"§cMove {Game.ConvertToCoordinate(initial.index)}-{Game.ConvertToCoordinate(final.index)} is not valid!");
            return false;

        }

        public void ForceMove(Square initialSquare, Square targetSquare)
        {
            Piece defaultPiece = initialSquare.piece;

            initialSquare.piece = targetSquare.piece;

            targetSquare.piece = defaultPiece;

            Game.settings.DebugMessage($"Making move {defaultPiece.GetPieceSymbol()}{initialSquare.index} - {targetSquare.index}");

            Game.board.DrawCurrentPosition();
        }
    }
}
