using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board instance;
    public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    public Transform goldHolder {get{return StateMachineController.instance.player1.transform;}}
    public Transform greenHolder {get{return StateMachineController.instance.player2.transform;}}
    public List<Piece> goldPieces = new List<Piece>();
    public List<Piece> greenPieces = new List<Piece>();
    
    public Piece selectedPiece;
    public AvailableMove selectedMove;

    void Awake(){
        instance = this;        
    }

    public async Task Load(){
        GetTeams();

        await Task.Run(() => CreateBoard());
    }

    [ContextMenu("Reset Board")]
    public void ResetBoard(){
        foreach(Tile t in tiles.Values){
            t.content = null;
        }
        foreach(Piece p in goldPieces){
            ResetPiece(p);
        }
        foreach(Piece p in greenPieces){
            ResetPiece(p);
        }
    }

    void ResetPiece(Piece piece){
        if(!piece.gameObject.activeSelf)
            return;
        Vector2Int pos = new Vector2Int((int)piece.transform.position.x, (int)piece.transform.position.y);
        tiles.TryGetValue(pos, out piece.tile);
        piece.tile.content = piece;
    }

    void GetTeams(){
        goldPieces.AddRange(goldHolder.GetComponentsInChildren<Piece>());
        greenPieces.AddRange(greenHolder.GetComponentsInChildren<Piece>());
    }

    public void AddPiece(string team, Piece piece){
        Vector2 v2Pos = piece.transform.position;
        Vector2Int pos = new Vector2Int((int)v2Pos.x, (int)v2Pos.y);
        piece.tile = tiles[pos];
        piece.tile.content = piece;
    }

    public void CreateBoard(){
        for(int i=0; i<8; i++){
            for(int j=0; j<8; j++){
                CreateTile(i, j);
            }
        }
    }

    void CreateTile(int i, int j){
        Tile tile = new Tile();
        tile.pos = new Vector2Int(i, j);
        tiles.Add(tile.pos, tile);
    }
}
