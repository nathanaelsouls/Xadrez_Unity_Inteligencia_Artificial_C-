using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public Movement savedMovement;
    public Movement queenMovement;
    public Movement knightMovement;
    public Movement bishopMovement;
    public Movement rookMovement;
    protected override void Start(){
        base.Start();
        movement = savedMovement = new PawnMovement(maxTeam);
        queenMovement = new QueenMovement(maxTeam);
        knightMovement = new KnightMovement(maxTeam);
        bishopMovement = new BishopMovement(maxTeam);
        rookMovement = new RookMovement(maxTeam);
    }
    public override AffectedPiece CreateAffected(){
        AffectedPawn aff = new AffectedPawn();
        aff.wasMoved = wasMoved;
        return aff;
    }    
}