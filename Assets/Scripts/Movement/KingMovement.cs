using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovement : Movement
{
    public KingMovement(bool maxTeam){
        value = 100000;
        if(maxTeam) 
            positionValue = AIController.instance.squareTable.kingGold;
        else
            positionValue = AIController.instance.squareTable.kingGreen;
    }
    public override List<AvailableMove> GetValidMoves(){
        List<AvailableMove> moves = new List<AvailableMove>();
        UntilBlockedPath(moves, new Vector2Int( 1, 0), true, 1);
        UntilBlockedPath(moves, new Vector2Int(-1, 0), true, 1);
        UntilBlockedPath(moves, new Vector2Int(0,  1), true, 1);
        UntilBlockedPath(moves, new Vector2Int(0, -1), true, 1);
        UntilBlockedPath(moves, new Vector2Int( 1,  1), true, 1);
        UntilBlockedPath(moves, new Vector2Int( 1, -1), true, 1);
        UntilBlockedPath(moves, new Vector2Int(-1, -1), true, 1);
        UntilBlockedPath(moves, new Vector2Int(-1,  1), true, 1);

        Castling(moves);
        return moves;
    }

    void Castling(List<AvailableMove> moves){         
        if (Board.instance.selectedPiece.wasMoved)
            return;

        Tile temp = CheckRook(new Vector2Int(1, 0));
        if(temp!= null){            
            moves.Add(new AvailableMove(temp.pos, MoveType.Castling));
        }
            
        temp = CheckRook(new Vector2Int(-1, 0));
        if(temp!= null){            
            moves.Add(new AvailableMove(temp.pos, MoveType.Castling));
        }            

        return;
    }

    Tile CheckRook(Vector2Int direction){
        Rook rook;
        Tile currentTile = GetTile(Board.instance.selectedPiece.tile.pos + direction);

        while (currentTile != null){
            if (currentTile.content != null)
                break;
            currentTile = GetTile(currentTile.pos + direction);
        }
        if(currentTile == null)
            return null;
        rook = currentTile.content as Rook;
        if(rook == null || rook.wasMoved)
            return null;
        return rook.tile;
    }
}
