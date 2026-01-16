using Unity.Collections;
using Unity.Entities;

namespace Test_ECS
{
	// This is an example of an unmanaged ECS component.
	public struct HelloComponent : IComponentData
	{
		// FixedString32Bytes is used instead of string, because
		// struct IComponentData can only contain unmanaged types.
		public FixedString32Bytes Message;
	}
}
