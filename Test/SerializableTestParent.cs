using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete ("결론 : System.Serializable 해도 Monobehaviour에서는 안 뜸.")]
public class SerializableTestParent : MonoBehaviour
{
    [SerializeField] SerializableTest serializableTestScript;
}
