using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AIPlayingState : State
{
    public async override void Enter(){
        Task<Ply> task = AIController.instance.CalculatePlays();
        await task;
        Ply bestResult = task.Result;
        MakeBestPlay(bestResult);
    }
    async void MakeBestPlay(Ply ply){
        Ply currentPly = ply;

        for (int i = 1; i < AIController.instance.objectivePlyDepth; i++){
            currentPly = currentPly.originPly;            
        }
        Board.instance.selectedPiece = currentPly.changes[0].piece;
        Debug.Log(currentPly.changes[0].piece.name);
        Board.instance.selectedMove = GetMoveType(currentPly);
        Debug.Log(Board.instance.selectedMove);
        await Task.Delay(100);
        machine.ChangeTo<PieceMovementState>();
    }
    AvailableMove GetMoveType(Ply ply){
        List<AvailableMove> moves = Board.instance.selectedPiece.movement.GetValidMoves();
        foreach (AvailableMove m in moves)
        {
            if(m.pos == ply.changes[0].to.pos)
                return m;
        }
        return new AvailableMove();
    }
}
