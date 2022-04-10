using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    protected override void Start(){
        base.Start();
        movement = new QueenMovement(maxTeam);
    }
}
