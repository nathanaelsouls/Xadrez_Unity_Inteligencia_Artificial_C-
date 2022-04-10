using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PieceMovementState : State
{
    public static List<AffectedPiece> changes;
    public static AvailableMove enPassantFlag;

    public override async void Enter()
    {
        Debug.Log("PieceMovementState:");
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        MovePiece(tcs, false, Board.instance.selectedMove.moveType);

        await tcs.Task;
        machine.ChangeTo<TurnEndState>();
    }
    public static void MovePiece(TaskCompletionSource<bool> tcs, bool skipMovements, MoveType moveType)
    {
        changes = new List<AffectedPiece>(); 
        enPassantFlag = new AvailableMove();       

        switch (moveType)
        {
            case MoveType.Normal:
                NormalMove(tcs, skipMovements);
                break;
            case MoveType.Castling:
                Castling(tcs, skipMovements);
                break;
            case MoveType.PawnDoubleMove:
                PawnDoubleMove(tcs, skipMovements);
                break;
            case MoveType.EnPassant:
                EnPassant(tcs, skipMovements);
                break;
            case MoveType.Promotion:
                Promotion(tcs, skipMovements);
                break;
        }
    }
    static void NormalMove(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece piece = Board.instance.selectedPiece;
        AffectedPiece pieceMoving = piece.CreateAffected();
        pieceMoving.piece = piece;
        pieceMoving.from = piece.tile;
        pieceMoving.to = Board.instance.tiles[Board.instance.selectedMove.pos];        
        changes.Insert(0, pieceMoving);

        piece.tile.content = null;
        piece.tile = pieceMoving.to;

        if (piece.tile.content != null)
        {
            Piece deadPiece = piece.tile.content;
            AffectedEnemy pieceKilled = new AffectedEnemy();
            pieceKilled.piece = deadPiece;
            pieceKilled.from = pieceKilled.to = piece.tile;
            changes.Add(pieceKilled);            
            deadPiece.gameObject.SetActive(false);
            pieceKilled.index = deadPiece.team.IndexOf(deadPiece);
            deadPiece.team.RemoveAt(pieceKilled.index);
        }
        piece.tile.content = piece;
        piece.wasMoved = true;        
        
        if (skipMovements)
        {
            piece.wasMoved = true;
            //Vector3 v3Pos = new Vector3(Board.instance.selectedMove.pos.x, Board.instance.selectedMove.pos.y, 0);
            //piece.transform.position = v3Pos;
            tcs.SetResult(true);
        }
        else
        {
            Vector3 v3Pos = new Vector3(Board.instance.selectedMove.pos.x, Board.instance.selectedMove.pos.y, 0);
            float timing = Vector3.Distance
                (piece.transform.position, v3Pos) * 0.5f;

            LeanTween.move(piece.gameObject, v3Pos, timing).
                setOnComplete(() =>
                {
                    tcs.SetResult(true);
                });
        }
    }
    static void Castling(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece king = Board.instance.selectedPiece;
        AffectedKingRook affectedKing = new AffectedKingRook();
        affectedKing.from = king.tile;
        king.tile.content = null;        
        affectedKing.piece = king;
        
        Piece rook = Board.instance.tiles[Board.instance.selectedMove.pos].content;
        AffectedKingRook affectedRook = new AffectedKingRook();
        affectedRook.from = rook.tile;        
        rook.tile.content = null;        
        affectedRook.piece = rook;

        Vector2Int direction = rook.tile.pos - king.tile.pos;
        if (direction.x > 0)
        {
            king.tile = Board.instance.tiles[new Vector2Int(king.tile.pos.x + 2, king.tile.pos.y)];
            rook.tile = Board.instance.tiles[new Vector2Int(king.tile.pos.x - 1, king.tile.pos.y)];
        }
        else
        {
            king.tile = Board.instance.tiles[new Vector2Int(king.tile.pos.x - 2, king.tile.pos.y)];
            rook.tile = Board.instance.tiles[new Vector2Int(king.tile.pos.x + 1, king.tile.pos.y)];
        }
        king.tile.content = king;
        affectedKing.to = king.tile;
        changes.Add(affectedKing);        
        rook.tile.content = rook;
        affectedRook.to = rook.tile;
        changes.Add(affectedRook);


        king.wasMoved = true;
        rook.wasMoved = true;

        if(skipMovements){
            tcs.SetResult(true);
        } else {
            LeanTween.move(king.gameObject, new Vector3(king.tile.pos.x, king.tile.pos.y, 0), 1.5f).
            setOnComplete(() => {
                tcs.SetResult(true);
            });
            LeanTween.move(rook.gameObject, new Vector3(rook.tile.pos.x, rook.tile.pos.y, 0), 1.4f);
        }
    }
    
    static void PawnDoubleMove(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece pawn = Board.instance.selectedPiece;
        Vector2Int direction = pawn.maxTeam ?
            new Vector2Int(0, 1) :
            new Vector2Int(0, -1);
        
        enPassantFlag = new AvailableMove(pawn.tile.pos+direction, MoveType.EnPassant);
        NormalMove(tcs, skipMovements);
    }

    static void EnPassant(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece pawn = Board.instance.selectedPiece;
        Vector2Int direction = pawn.maxTeam ?
            new Vector2Int(0, -1) :
            new Vector2Int(0, 1);

        
        Tile enemy = Board.instance.tiles[Board.instance.selectedMove.pos+direction];
        AffectedEnemy affectedEnemy = new AffectedEnemy();
        affectedEnemy.from = affectedEnemy.to = enemy;
        affectedEnemy.piece = enemy.content;
        affectedEnemy.index = affectedEnemy.piece.team.IndexOf(affectedEnemy.piece);
        affectedEnemy.piece.team.RemoveAt(affectedEnemy.index);
        changes.Add(affectedEnemy);
        enemy.content.gameObject.SetActive(false);
        enemy.content = null;

        NormalMove(tcs, skipMovements);
    }

    static async void Promotion(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        TaskCompletionSource<bool> movementTCS = new TaskCompletionSource<bool>();
        NormalMove(movementTCS, skipMovements);
        await movementTCS.Task;
        //Debug.Log("Promoveu");
        Pawn pawn = Board.instance.selectedPiece as Pawn;

        if(!skipMovements){
            StateMachineController.instance.taskHold = new TaskCompletionSource<object>();
            StateMachineController.instance.promotionPanel.SetActive(true);

            await StateMachineController.instance.taskHold.Task;

            string result = StateMachineController.instance.taskHold.Task.Result as string;
            switch (result)
            {
                case "Queen":
                    Board.instance.selectedPiece.movement = pawn.queenMovement;
                    break;
                case "Knight":
                    Board.instance.selectedPiece.movement = pawn.knightMovement;
                    break;
                case "Bishop":
                    Board.instance.selectedPiece.movement = pawn.bishopMovement;
                    break;
                case "Rook":
                    Board.instance.selectedPiece.movement = pawn.rookMovement;
                    break;
            } 
            StateMachineController.instance.promotionPanel.SetActive(false);
        } else {
            AffectedPawn affectedPawn = new AffectedPawn();
            affectedPawn.wasMoved = true;
            affectedPawn.resetMovement = true;
            affectedPawn.from = changes[0].from;
            affectedPawn.to = changes[0].to;
            affectedPawn.piece = pawn;

            changes[0] = affectedPawn;
            pawn.movement = pawn.queenMovement;
        }
        
        tcs.SetResult(true);
    }
}
