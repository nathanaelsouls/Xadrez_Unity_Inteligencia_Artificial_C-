using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AIController : MonoBehaviour
{
    public static AIController instance;    
    public HighlightClick AIhighlight;
    int calculationCount;
    public int objectivePlyDepth = 2;
    float lastInterval;
    public AvailableMove enPassantFlagSaved;
    public PieceSquareTable squareTable = new PieceSquareTable();
    Ply maxPly;
    Ply minPly;
    void Awake(){
        instance = this;
        maxPly = new Ply();
        maxPly.score = 999999;
        minPly = new Ply();
        minPly.score = -999999;
        squareTable.SetDictionaries();
    }
    [ContextMenu("Calculate Plays")]
    public async Task<Ply> CalculatePlays(){
        lastInterval = Time.realtimeSinceStartup;
        int minimaxDirection;
        if(StateMachineController.instance.currentlyPlaying == StateMachineController.instance.player1)
            minimaxDirection = 1;
        else
            minimaxDirection = -1;

        enPassantFlagSaved = PieceMovementState.enPassantFlag;
        Ply currentPly = new Ply();    
        calculationCount = 0;
        
        currentPly.originPly = null;
        int currentPlyDepth = 0;
        currentPly.changes = new List<AffectedPiece>();        
        
        Debug.Log("Começo");
        Task<Ply> calculation = CalculatePly(currentPly,
            -1000000, 1000000,
            currentPlyDepth,
            minimaxDirection);
        await calculation;

        Debug.Log("Calculations: "+calculationCount);
        Debug.Log("Time: "+(Time.realtimeSinceStartup-lastInterval));
        PrintBestPly(currentPly.bestFuture);
        PieceMovementState.enPassantFlag = enPassantFlagSaved;
        return currentPly.bestFuture;
    }

    async Task<Ply> CalculatePly(Ply parentPly, int alpha, int beta, int currentPlyDepth, int minimaxDirection){
        currentPlyDepth++;
        if (currentPlyDepth > objectivePlyDepth){
            
            EvaluateBoard(parentPly);
            // Task evaluationTask = Task.Run(()=> EvaluateBoard(parentPly));
            // await evaluationTask;
            return parentPly;
        }
        List<Piece> team;
        if(minimaxDirection == 1){
            team = Board.instance.goldPieces;
            parentPly.bestFuture = minPly;
        } else {
            team = Board.instance.greenPieces;
            parentPly.bestFuture = maxPly;
        }

        for (int i = 0; i < team.Count; i++){
            Board.instance.selectedPiece = team[i];
            foreach (AvailableMove move in team[i].movement.GetValidMoves()){
                calculationCount++;                
                Board.instance.selectedPiece = team[i];
                Board.instance.selectedMove = move;
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                PieceMovementState.MovePiece(tcs, true, move.moveType);

                await tcs.Task;
                Ply newPly = new Ply();
                newPly.changes = PieceMovementState.changes;  
                newPly.enPassantFlag = PieceMovementState.enPassantFlag;

                Task<Ply> calculation = CalculatePly(newPly,
                    alpha, beta,
                    currentPlyDepth, minimaxDirection*-1);
                await calculation;

                parentPly.bestFuture = IsBest(parentPly.bestFuture, minimaxDirection, calculation.Result,
                    ref alpha, ref beta);                
                newPly.originPly = parentPly;

                PieceMovementState.enPassantFlag = parentPly.enPassantFlag;
                ResetBoardBackwards(newPly);
                if(beta <= alpha){
                    return parentPly.bestFuture;
                }
            }
        }
        return parentPly.bestFuture;
    }

    Ply IsBest(Ply ply, int minimaxDirection, Ply potentialBest, ref int alpha, ref int beta){
        Ply best = ply;
        if (minimaxDirection == 1){
            if (potentialBest.score > ply.score)
                best = potentialBest;
            alpha = Mathf.Max(alpha, best.score);
        } else {
            if (potentialBest.score < ply.score)
                best = potentialBest;
            beta = Mathf.Min(beta, best.score);
        }
        return best;
    }
    
    void EvaluateBoard(Ply ply){
        foreach(Piece piece in Board.instance.goldPieces){
            EvaluatePiece(piece, ply, 1);
        }
        foreach(Piece piece in Board.instance.greenPieces){
            EvaluatePiece(piece, ply, -1);
        }
        //Debug.Log("Board score: "+ply.score);
    }
    void EvaluatePiece(Piece eva, Ply ply, int scoreDirection){
        int positionValue = eva.movement.positionValue[eva.tile.pos];
        ply.score += (eva.movement.value+positionValue)*scoreDirection;
    }
    void ResetBoardBackwards(Ply ply){
        foreach (AffectedPiece p in ply.changes){
            p.Undo();
        }
    }

    void PrintBestPly(Ply finalPly){
        Ply currentPly = finalPly;
        Debug.Log("Melhor jogada:");
        while (currentPly.originPly != null){
            Debug.LogFormat("{0}-{1}->{2}",
                currentPly.changes[0].piece.transform.parent.name,
                currentPly.changes[0].piece.name,
                currentPly.changes[0].to.pos);

            currentPly = currentPly.originPly;
        }
    }

}
