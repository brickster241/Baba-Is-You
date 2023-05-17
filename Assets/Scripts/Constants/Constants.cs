using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

/*
    Constants to keep track of all the constants.
*/
public class Constants : MonoBehaviour
{
    public static Vector2[] directions = {Vector2.left, Vector2.up, Vector2.right, Vector2.down};
    public static NounType[] nounTypes = {NounType.NONE, NounType.BABA, NounType.FLAG, NounType.ROCK, NounType.WALL};
    public static PropertyType[] propertyTypes = {PropertyType.YOU, PropertyType.STOP, PropertyType.PUSH, PropertyType.WIN, PropertyType.NONE};
    public static BlockType[] blockTypes = {BlockType.OPERATOR, BlockType.PROPERTY_TEXT, BlockType.NOUN_TEXT, BlockType.NOUN};
    public const int GRID_MAX_X = 16;
    public const int GRID_MAX_Y = 8;
}
