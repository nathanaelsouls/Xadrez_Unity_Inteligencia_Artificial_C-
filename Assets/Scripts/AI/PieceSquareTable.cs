using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSquareTable
{
    public Dictionary<Vector2Int, int> pawnGold = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> pawnGreen = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> knightGold = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> knightGreen = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> bishopGold = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> bishopGreen = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> rookGold = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> rookGreen = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> queenGold = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> queenGreen = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> kingGold = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> kingGreen = new Dictionary<Vector2Int, int>();
    public void SetDictionaries(){
        List<int> pawnValues = new List<int>(){
            0,  0,  0,  0,  0,  0,  0,  0,
            50, 50, 50, 50, 50, 50, 50, 50,
            10, 10, 20, 30, 30, 20, 10, 10,
            5,  5, 10, 25, 25, 10,  5,  5,
            0,  0,  0, 20, 20,  0,  0,  0,
            5, -5,-10,  0,  0,-10, -5,  5,
            5, 10, 10,-20,-20, 10, 10,  5,
            0,  0,  0,  0,  0,  0,  0,  0
        };
        SetDictionary(pawnGold, pawnGreen, pawnValues);

        List<int> knightValues = new List<int>(){
            -50,-40,-30,-30,-30,-30,-40,-50,
            -40,-20,  0,  0,  0,  0,-20,-40,
            -30,  0, 10, 15, 15, 10,  0,-30,
            -30,  5, 15, 20, 20, 15,  5,-30,
            -30,  0, 15, 20, 20, 15,  0,-30,
            -30,  5, 10, 15, 15, 10,  5,-30,
            -40,-20,  0,  5,  5,  0,-20,-40,
            -50,-40,-30,-30,-30,-30,-40,-50
        };
        SetDictionary(knightGold, knightGreen, knightValues);

        List<int> bishopValues = new List<int>(){
            -20,-10,-10,-10,-10,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  0,  5, 10, 10,  5,  0,-10,
            -10,  5,  5, 10, 10,  5,  5,-10,
            -10,  0, 10, 10, 10, 10,  0,-10,
            -10, 10, 10, 10, 10, 10, 10,-10,
            -10,  5,  0,  0,  0,  0,  5,-10,
            -20,-10,-10,-10,-10,-10,-10,-20
        };
        SetDictionary(bishopGold, bishopGreen, bishopValues);

        List<int> rookValues = new List<int>(){
             0,  0,  0,  0,  0,  0,  0,  0,
             5, 10, 10, 10, 10, 10, 10,  5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
             0,  0,  0,  5,  5,  0,  0,  0
        };
        SetDictionary(rookGold, rookGreen, rookValues);

        List<int> queenValues = new List<int>(){
            -20,-10,-10, -5, -5,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  0,  5,  5,  5,  5,  0,-10,
             -5,  0,  5,  5,  5,  5,  0, -5,
              0,  0,  5,  5,  5,  5,  0, -5,
            -10,  5,  5,  5,  5,  5,  0,-10,
            -10,  0,  5,  0,  0,  0,  0,-10,
            -20,-10,-10, -5, -5,-10,-10,-20
        };
        SetDictionary(queenGold, queenGreen, queenValues);

        List<int> kingValues = new List<int>(){
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -20,-30,-30,-40,-40,-30,-30,-20,
            -10,-20,-20,-20,-20,-20,-20,-10,
             20, 20,  0,  0,  0,  0, 20, 20,
             20, 30, 10,  0,  0, 10, 30, 20
        };
        SetDictionary(kingGold, kingGreen, kingValues);
    }

    void SetDictionary(Dictionary<Vector2Int, int> gold, Dictionary<Vector2Int, int> green, List<int> values){
        int i=0;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                gold.Add(new Vector2Int(x, y), values[i]);
                green.Add(new Vector2Int(7-x, 7-y), values[i++]);
            }
        }
    }
}
