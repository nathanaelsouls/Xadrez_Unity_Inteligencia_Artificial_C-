using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : Movement
{
    public KnightMovement(bool maxTeam){
        value = 300;
        if(maxTeam) 
            positionValue = AIController.instance.squareTable.knightGold;
        else
            positionValue = AIController.instance.squareTable.knightGreen;
    }
    public override List<AvailableMove> GetValidMoves(){
        List<AvailableMove> moves = new List<AvailableMove>();

        moves.AddRange(GetMovement(new Vector2Int(  0,  1)));
        moves.AddRange(GetMovement(new Vector2Int(  0, -1)));
        moves.AddRange(GetMovement(new Vector2Int(  1, 0)));
        moves.AddRange(GetMovement(new Vector2Int( -1, 0)));
        return moves;
    }

    List<AvailableMove> GetMovement(Vector2Int direction){
        List<AvailableMove> moves = new List<AvailableMove>();
        Tile current = Board.instance.selectedPiece.tile;
        Tile temp = GetTile(current.pos+direction*2);
        if(temp != null){
            moves.AddRange(GetCurvedPart(temp.pos, new Vector2Int(direction.y, direction.x)));
        }
        return moves;
    }

    List<AvailableMove> GetCurvedPart(Vector2Int pos, Vector2Int direction){
        List<AvailableMove> availableMoves = new List<AvailableMove>();
        Tile tile1 = GetTile(pos+direction);
        Tile tile2 = GetTile(pos-direction);
        if(tile1 != null && (tile1.content == null || IsEnemy(tile1)))
            availableMoves.Add(new AvailableMove(tile1.pos));
        if(tile2 != null && (tile2.content == null || IsEnemy(tile2)))
            availableMoves.Add(new AvailableMove(tile2.pos));
        return availableMoves;
    }
}
