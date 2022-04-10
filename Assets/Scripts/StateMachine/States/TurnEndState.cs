using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndState : State
{
    public override async void Enter(){
        Debug.Log("Em TurnEndState: ");
        bool gameFinished = CheckConditions();
        await Task.Delay(100);
        if(gameFinished){
            machine.ChangeTo<GameEndState>();
        } else {
            machine.ChangeTo<TurnBeginState>();
        }
    }

    bool CheckConditions(){
        if(CheckTeams() || CheckKing())
            return true;
        return false;
    }

    bool CheckTeams(){
        Piece goldPiece = Board.instance.goldPieces.Find((x) => x.gameObject.activeSelf == true)        ;
        if(goldPiece == null){
            Debug.Log("Lado verde ganhou");
            return true;
        }
        Piece greenPiece = Board.instance.greenPieces.Find((x) => x.gameObject.activeSelf == true)        ;
        if(greenPiece == null){
            Debug.Log("Lado dourado ganhou");
            return true;
        }
        return false;
    }

    bool CheckKing(){
        King king = Board.instance.goldHolder.GetComponentInChildren<King>();
        if (king == null){
            Debug.Log("Lado verde ganhou");
            return true;
        }
        king = Board.instance.greenHolder.GetComponentInChildren<King>();
        if (king == null){
            Debug.Log("Lado dourado ganhou");
            return true;
        }
        return false;
    }
}
