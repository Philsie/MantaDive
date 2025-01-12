using System;
using TMPro;
using UnityEngine;

public class LeaderboardElement : MonoBehaviour
{
    [SerializeField]
    private TMP_Text username;
    [SerializeField]
    private TMP_Text depth;
    internal void UpdateText(string userName, string maxDepth)
    {
        username.text = userName;
        depth.text = maxDepth;
    }
}
