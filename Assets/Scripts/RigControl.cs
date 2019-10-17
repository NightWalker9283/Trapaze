using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class RigControl : MonoBehaviour
{
	
	int bodyCount = 6;
	int jointCount = 25;
	

	public GameObject humanoid;
	public bool mirror = true;
	public bool move = true;
	CharacterSkeleton skeleton;

	void Start()
	{
		
		skeleton = new CharacterSkeleton(humanoid);
	}
	void Update()
	{
		float[] data = new float[bodyCount * jointCount * 3];
		int[] state = new int[bodyCount * jointCount];
		int[] id = new int[bodyCount];
		
		
		{
			skeleton.set(data, state, 0, mirror, move);
		}
	}
}