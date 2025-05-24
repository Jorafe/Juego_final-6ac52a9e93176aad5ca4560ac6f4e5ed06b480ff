using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryFlow", menuName = "Game/Story Flow")]
public class StoryFlow : ScriptableObject
{
    public List<StoryStep> steps;
}