using Unity.Entities;

namespace Entities.ECS.ScreenBoundery.Components
{
	public struct ViewBoundComponent : IComponentData
	{
		public bool WasVisible;
		public readonly bool DestroyWhenOutOfView;

		public ViewBoundComponent(bool destroyWhenOutOfView)
		{
			DestroyWhenOutOfView = destroyWhenOutOfView;
			WasVisible = false;
		}
	}
}
