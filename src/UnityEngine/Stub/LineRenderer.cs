
namespace UnityEngine
{

	public class LineRenderer : Renderer
	{

		// Set the line width at the start and at the end.
		public void SetWidth(float start, float end) {}

		// Set the line color at the start and at the end.
		public void SetColors(Color start, Color end) {}

		// Set the number of line segments.
		public void SetVertexCount(int count) {}

		// Set the position of the vertex in the line.
		public void SetPosition(int index, Vector3 position) {}

		// If enabled, the lines are defined in world space.
		public bool useWorldSpace;

	}

}
