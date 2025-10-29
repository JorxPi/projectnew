using UnityEngine;

[CreateAssetMenu(fileName = "LevelBoundsMinMax2D", menuName = "Game/Level Bounds 2D (MinMax)")]
public class LevelBound2D : ScriptableObject
{
    public float minX = -15f;
    public float maxX = 15f;
    public float minY = -5f;
    public float maxY = 10f;

    public bool allowHorizontal = true;
    public bool allowVertical = true;
}
