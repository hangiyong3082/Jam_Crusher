using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ScriptableObjectTest", menuName = "ScriptableObject Test")]
public class ScriptableObjectTest : ScriptableObject
{
    [Header("스킨 그룹에 있는 차 모델들의 순서에 맞춰서 할당")]
    [SerializeField] public string skinName;
    [SerializeField] public GameObject skinGroup;
    
    [SerializeField] public List<Material> colors;
    [SerializeField] public List<int> nums;
}
