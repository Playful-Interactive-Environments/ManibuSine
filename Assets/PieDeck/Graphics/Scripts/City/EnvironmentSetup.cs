using UnityEngine;
using System.Collections;

public class EnvironmentSetup : MonoBehaviour {

    public int MainBuildingWidth = 1920;
    public int MainBuildingDepth = 1080;
    public int MainBuildingHeight = 1920;
    public int StreetWidth = 500;
    public Color[] PieColors;

    public int BuildingDistanceX
    {
        get { return MainBuildingWidth + StreetWidth; }
    }

    public int BuildingDistanceY
    {
        get { return MainBuildingDepth + StreetWidth; }
    }

    public int MainBuildingCenterX
    {
        get { return MainBuildingWidth / 2; }
    }

    public int MainBuildingCenterY
    {
        get { return MainBuildingDepth / 2; }
    }

    public int GetXStreetCenter(int streetNo)
    {
        return streetNo * BuildingDistanceX - StreetWidth / 2;
    }

    public int GetYStreetCenter(int streetNo)
    {
        return streetNo * BuildingDistanceY - StreetWidth / 2;
    }
}
