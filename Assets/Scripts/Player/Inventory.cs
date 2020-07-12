using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //set public for debug purpose will change later
    public int _throwables = 0;
    public int _ghosts = 0;
    public int _dummies = 0;
    public int _usbs = 0;

    public enum Item {
        THROWABLE,
        DUMMY,
        GHOST,
        USB,
        NOTHING
    }


    public void IncreaseItemInInventory(Item item){
        switch (item) {
            case Item.GHOST:
                _ghosts++;
                break;
            case Item.DUMMY:
                _dummies++;
                break;
            case Item.THROWABLE:
                _throwables++;
                break;
            case Item.USB:
                _usbs++;
                break;
            default:
                break;
        }
    }

    public void DecreaseItemInInventory(Item item)
    {
        switch (item)
        {
            case Item.GHOST:
                if (_ghosts > 0) _ghosts--;
                break;
            case Item.DUMMY:
                if (_dummies > 0) _dummies--;
                break;
            case Item.THROWABLE:
                if (_throwables > 0) _throwables--;
                break;
            case Item.USB:
                if (_usbs > 0) _usbs--;
                break;
            default:
                break;
        }
    }

    public int GetThrowablesNumber()
    {
        return _throwables;
    }

    public int GetDummiesNumber()
    {
        return _dummies;
    }

    public int GetGhostsNumber()
    {
        return _ghosts;
    }

    public int GetUsbsNumber()
    {
        return _usbs;
    }

}
