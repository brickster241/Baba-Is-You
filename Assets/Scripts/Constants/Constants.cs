using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Constants : MonoBehaviour
{
    public static Vector2[] directions = {Vector2.left, Vector2.up, Vector2.right, Vector2.down};
    public static NounType[] nounTypes = {NounType.NONE, NounType.BABA, NounType.FLAG, NounType.ROCK, NounType.WALL};
    public static PropertyType[] propertyTypes = {PropertyType.YOU, PropertyType.STOP, PropertyType.PUSH, PropertyType.WIN, PropertyType.NONE};
    public static BlockType[] blockTypes = {BlockType.OPERATOR, BlockType.PROPERTY_TEXT, BlockType.NOUN_TEXT, BlockType.NOUN};
}
