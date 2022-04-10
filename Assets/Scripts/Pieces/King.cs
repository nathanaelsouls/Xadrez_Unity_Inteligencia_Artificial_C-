using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    protected override void Start(){
        base.Start();
        movement = new KingMovement(maxTeam);
    }
    public override AffectedPiece CreateAffected(){
        AffectedKingRook aff = new AffectedKingRook();
        aff.wasMoved = wasMoved;
        return aff;
    }
}
