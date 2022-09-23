using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cell : MonoBehaviour
{
    public Vector2 position;
    public bool isAlife;
    private GameObject obj;
    public cell(float x, float y, bool isAlife, GameObject cellPref)
    {
        position = new Vector2(x, y);
        this.isAlife = isAlife;
        obj =  Instantiate(cellPref, position, Quaternion.identity);
    }
    public void changeState(bool isAlife)
    {
        this.isAlife = isAlife;
        if (this.isAlife == true) obj.GetComponent<SpriteRenderer>().color = Color.black;
        else obj.GetComponent<SpriteRenderer>().color = Color.white;
    }
    private void OnMouseDown()
    {
        cell cellt = gameLife.field[Convert.ToInt32(GetComponent<Transform>().position.x),Convert.ToInt32(GetComponent<Transform>().position.y)];
        if (gameLife.gameStart == false) cellt.changeState(!cellt.isAlife);
    }
}
