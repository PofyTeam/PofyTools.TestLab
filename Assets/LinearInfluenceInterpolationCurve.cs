using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearInfluenceInterpolationCurve
{
	public LinkedList<Vector3> data;
	public LinkedList<Vector3> interpolatedData;

	public int maxDataCount = 10;
	public int segmentResolution = 100;

	void AddData (Vector3 newData)
	{
		this.data.AddLast (newData);
		if (this.data.Count > this.maxDataCount) {
			this.data.RemoveFirst ();
		}

		RebuildInterpolatedCurve ();
	}

	void RebuildInterpolatedCurve ()
	{
		this.interpolatedData.Clear ();
		foreach (var DataPoint in this.data) {
			
		}
	}

	public void Draw ()
	{
		
	}
}
