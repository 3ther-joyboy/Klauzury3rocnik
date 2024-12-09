using System;
using Godot;
public class MathCustom 
{
	static Random rand;

	private static void RNG(){
		if(rand == null){
			rand = new Random(game_manager.instance.seed);
		}
	}

	public static Vector3 RandomPositionOnPoligon3D(Vector3[] poligon){
		RNG();
		Vector3 output = new Vector3();
//			Generating for X
//			not actually random, just around the x and z
		Vector2 inPlane = RandomPositionOnPoligon2D(
			new Vector2[]{
				new Vector2(poligon[0].X,poligon[0].Z),
				new Vector2(poligon[1].X,poligon[1].Z),
				new Vector2(poligon[2].X,poligon[2].Z),
			});
		output.X = inPlane.X;
		output.Z = inPlane.Y;
		output.Y = poligon[0].Y;

		return output;
	}
	public static Vector2 RandomPositionOnPoligon2D(Vector2[] poligon){
		RNG();
		Vector2 output = new Vector2();
//   		[Random Point in Poligon](https://stackoverflow.com/questions/47410054/generate-random-locations-within-a-triangular-domain)
// 	  			qwp
		double r1 = rand.NextDouble();
		double r2 = rand.NextDouble();
		double s1 = Math.Sqrt(r1);

		output.X = (float)(poligon[0].X * (1.0 - s1) + poligon[1].X * (1.0 - r2) * s1 + poligon[2].X * r2 * s1);
		output.Y = (float)(poligon[0].Y * (1.0 - s1) + poligon[1].X * (1.0 - r2) * s1 + poligon[2].Y * r2 * s1);

		return output;
	}

}
