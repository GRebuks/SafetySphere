using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointsContainer<T> where T : Point
{
	public List<T> points;
}

[System.Serializable]
public class  ObjectController<T> : MonoBehaviour
{
	public T objectInfo;
}