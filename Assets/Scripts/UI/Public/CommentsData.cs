using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//コメントのテキストデータ管理スクリプタブルオブジェクト雛形
[CreateAssetMenu(
  fileName = "CommentsData",
  menuName = "ScriptableObject/CommentsData",
  order = 0)
]
public class CommentsData : ScriptableObject
{
    public List<Comment> Comments = new List<Comment>();
    public float DistanceL,DistanceU; //距離上限・下限
    public float HeightL, HeightU; //高さ上限・下限

}

[System.Serializable]
public class Comment
{
    public string Value="";

}
