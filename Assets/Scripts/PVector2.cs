using UnityEngine;
using System.Collections;

public class PVector2 {

	public static PVector2 zero = new PVector2(Vector2.zero);
	public static PVector2 up = new PVector2(Vector2.up);
	public static PVector2 right = new PVector2(Vector2.right);

	public static PVector2 operator +(PVector2 a, PVector2 b) => new PVector2(a.v + b.v);
	public static PVector2 operator -(PVector2 a, PVector2 b) => new PVector2(a.v - b.v);
	public static PVector2 operator *(PVector2 pv, float x) => new PVector2(pv.v * x);

	public Vector2 v;

	public PVector2(Vector2 v) { this.v = v; }
	public PVector2(float x, float y) { v = new Vector2(x, y); }

	public PVector2 ProjectOn(PVector2 pv) {
		if (pv == PVector2.zero)
			return PVector2.zero;

		// Esta é a fórmula que encontrei, funciona mas (acho que) é computacionalmente caro
		//return b * Mathf.Cos(Mathf.Atan2(a.y, a.x) - Mathf.Atan2(b.y, b.x));

		// Esta é a fórmula matemática de projeção de vetores
		Vector2 projection = pv.v * ((v.x * pv.v.x + v.y * pv.v.y) / pv.v.sqrMagnitude);
		return new PVector2(projection);
	}

	public float ScaleOfProjectionOn(PVector2 pv) {
		if (pv == PVector2.zero)
			return 0f;

		return (v.x * pv.v.x + v.y * pv.v.y) / pv.v.sqrMagnitude;
	}

}
