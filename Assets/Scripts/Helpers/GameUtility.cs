using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtility
{
    public static Vector2Int IsometricToCarestian(float x, float y, int MapSideSize)
    {
        float cartX = ((2 * y + x) / 2) + 0.25f;
        float cartY = ((2 * y - x) / 2) + 0.25f;
        Vector2 CartesianafterOffset = new Vector2(cartX - MapSideSize / 2, cartY);
        int tileIdX = Mathf.CeilToInt(CartesianafterOffset.x) * 2 - (Mathf.Abs((int)CartesianafterOffset.x - CartesianafterOffset.x) > 0.5 ? 1 : 2);
        int tileIdY = Mathf.CeilToInt(CartesianafterOffset.y) * 2 - (Mathf.Abs((int)CartesianafterOffset.y - CartesianafterOffset.y) > 0.5f ? 1 : 2);
        return new Vector2Int(tileIdX, tileIdY);
    }
    //нужно передавать длину поля для офсета!
    public static Vector2Int GetTileIDUnderMousePosition()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(IsometricToCarestian(mousePos.x, mousePos.y, MapGenerator.SideSize));
        return IsometricToCarestian(mousePos.x, mousePos.y, MapGenerator.SideSize);
    }

    public static Tile GetTileUnderMousePosition()
    {
        var tileID = GetTileIDUnderMousePosition();
        if (CheckIDIfValid(tileID))
        {
            return MapGenerator.Map[tileID.x, tileID.y];
        }
        else
            return null;
    }
    public static bool GetTileUnderMousePosition(out Tile tile)
    {
        tile = GetTileUnderMousePosition();
        if (tile == null) return false;
        else return true;
    }
    public static bool CheckIDIfValid(Vector2Int ID)
    {
        if (ID.x >= -1 && ID.y >= -1 && ID.x <= MapGenerator.SideSize - 1 && ID.y <= MapGenerator.SideSize - 1)
            return true;
        return false;
    }
    public static Road GetRoadByID(Vector2Int ID)
    {
        var roadTile = MapGenerator.Map[ID.x, ID.y] as TileWithBuilding;
        return roadTile.Building as Road;
    }

}
