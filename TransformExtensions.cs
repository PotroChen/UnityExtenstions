using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static string GetAbsolutePath(this Transform self)
    {
		string path = self.gameObject.name;
		Transform parent = self.parent;
		while (parent != null)
		{
			path = parent.name + "/" + path;
			parent = parent.parent;
		}
		return path;
	}

    	public static string GetChildPath(this Transform self, Transform child)
    	{
		string path = child.gameObject.name;
		Transform parent = child.parent;
		while (parent != self)
		{
		    if (parent == null)
		    {
			if (self == child)
			    return "";
			else
			{
			    throw new Exception($"{child.name} is not child of {self.name}");
			}
		    }
		    path = parent.name + "/" + path;
		    parent = parent.parent;
		}
		return path;
    	}

	public static Transform RecursiveFindChild(this Transform self, string childName)
	{
		Transform child = null;
		for (int i = 0; i < self.childCount; i++)
		{
			child = self.GetChild(i);
			if (child.name == childName)
			{
				break;
			}
			else
			{
				child = RecursiveFindChild(child, childName);
				if (child != null)
				{
					break;
				}
			}
		}

		return child;
	}
}
