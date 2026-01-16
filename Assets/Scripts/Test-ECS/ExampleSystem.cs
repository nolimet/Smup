using Unity.Entities;
using UnityEngine;

namespace Test_ECS
{
	public partial struct ExampleSystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
			var entity = state.EntityManager.CreateEntity();
			// Initialize and add a HelloComponent component to the entity.
			state.EntityManager.AddComponentData(entity, new HelloComponent
				{Message = "Hello ECS World"});
			// Set the name of the entity to make it easier to identify it.
			// Note: the entity Name property only exists in the Editor.
			state.EntityManager.SetName(entity, "Hello World Entity");
		}

		public void OnUpdate(ref SystemState state)
		{
			// The query retrieves all entities with a HelloComponent component.
			foreach (var message in
			         SystemAPI.Query<RefRO<HelloComponent>>())
			{
				Debug.Log(message.ValueRO.Message);
			}
		}
	}
}
