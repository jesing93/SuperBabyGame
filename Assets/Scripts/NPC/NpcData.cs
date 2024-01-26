using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcType
{
    guard,
    shopkeeper,
    elder,
    client
}
[CreateAssetMenu(fileName = "New NPC Data", menuName = "NPC/Data")]

public class NpcData : ScriptableObject
{
    [SerializeField] private string NpcName;
    [SerializeField] private NpcType npcType;
    [SerializeField] private float speed;
    [SerializeField] private bool isStuneable ;

    public string NpcName1 { get => NpcName; set => NpcName = value; }
    public NpcType NpcType { get => npcType; set => npcType = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool IsStuneable { get => isStuneable; set => isStuneable = value; }
}
