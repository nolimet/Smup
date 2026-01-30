using Unity.Entities;
using Unity.Mathematics;

namespace Entities.ECS.ScreenBoundery.Components
{
	public struct ViewBounds : IComponentData
	{
		public float4 Value;
	}
}
