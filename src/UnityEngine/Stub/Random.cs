using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngineInternal;

namespace UnityEngine
{

// Class for generating random data.
public class Random
{
	static System.Random random = new System.Random(45734);
	public static int Range (int min, int max)
	{
		if (min == max) return min;
		return random.Next() % (max - min) + min;
	}

	/*
	// Sets the seed for the random number generator.
	CUSTOM_PROP static int seed { return GetScriptingRand().GetSeed(); } { GetScriptingRand().SetSeed(value); }

	// Returns a random float number between and /min/ [inclusive] and /max/ [inclusive] (RO).
	CUSTOM static float Range (float min, float max)  { return RangedRandom (GetScriptingRand(), min, max); }

	// Returns a random integer number between /min/ [inclusive] and /max/ [exclusive] (RO).
	CSRAW public static int Range (int min, int max) { return RandomRangeInt (min, max); }

	CUSTOM private static int RandomRangeInt (int min, int max) { return RangedRandom (GetScriptingRand(), min, max); }

	// Returns a random number between 0.0 [inclusive] and 1.0 [inclusive] (RO).
	CUSTOM_PROP static float value { return Random01 (GetScriptingRand()); }

	// Returns a random point inside a sphere with radius 1 (RO).
	CUSTOM_PROP static Vector3 insideUnitSphere { return RandomPointInsideUnitSphere (GetScriptingRand()); }

	// Workaround for gcc/msvc where passing small mono structures by value does not work
	CUSTOM private static void GetRandomUnitCircle (out Vector2 output)
	{
		*output = RandomPointInsideUnitCircle (GetScriptingRand());
	}

	// Returns a random point inside a circle with radius 1 (RO).
	CSRAW public static Vector2 insideUnitCircle { get { Vector2 r; GetRandomUnitCircle(out r); return r; } }

	// Returns a random point on the surface of a sphere with radius 1 (RO).
	CUSTOM_PROP static Vector3 onUnitSphere { return RandomUnitVector (GetScriptingRand()); }
	
	// Returns a random rotation (RO).
	CUSTOM_PROP static Quaternion rotation { return RandomQuaternion (GetScriptingRand()); }

	// Returns a random rotation with uniform distribution(RO).
	CUSTOM_PROP static Quaternion rotationUniform { return RandomQuaternionUniformDistribution (GetScriptingRand()); }


	OBSOLETE warning Use Random.Range instead
	CSRAW public static float RandomRange (float min, float max)  { return Range (min, max); }
	OBSOLETE warning Use Random.Range instead
	CSRAW public static int RandomRange (int min, int max) { return Range (min, max); }
	*/
}


}
