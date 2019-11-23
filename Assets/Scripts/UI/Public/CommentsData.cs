using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
  fileName = "CommentsData",
  menuName = "ScriptableObject/CommentsData",
  order = 0)
]
public class CommentsData : ScriptableObject
{
    public List<Comment> Comments = new List<Comment>();
    public float DistanceL,DistanceU;
    public float HeightL, HeightU;

}

[System.Serializable]
public class Comment
{
    public string Value="";

}
