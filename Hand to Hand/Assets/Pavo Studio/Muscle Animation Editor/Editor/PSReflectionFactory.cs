using UnityEngine;
using System.Collections;

public class PSReflectionFactory
{
	private static PSReflectionFactory instance;

	public static PSReflectionFactory GetInstance ()
	{
		if (instance == null)
			instance = new PSReflectionFactory ();
		return instance;
	}

	public PSBaseReflection GetReflection ()
	{
		int i = compareVersion (Application.unityVersion, "5.1.0");

		if (i < 0)
			return new PSReflectionForUnity4 ();
		else
			return new PSReflectionForUnity510 ();
	}

	public int compareVersion (string v1, string v2)
	{
		return v1.Substring (0, 5).CompareTo (v2.Substring (0, 5));
	}

}
