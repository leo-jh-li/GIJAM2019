using System;
using System.Collections;

public static class Utilities {
	public static float sigmoid(float x, float x_offset) {
		return (float)(1 / (1 + Math.Exp((double)(-x + x_offset))));
	}
}