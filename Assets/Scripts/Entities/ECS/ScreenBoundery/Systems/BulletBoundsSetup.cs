using Entities.ECS.ScreenBoundery.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using PresentationSystemGroup = Unity.Entities.PresentationSystemGroup;
using SystemAPI = Unity.Entities.SystemAPI;
using SystemBase = Unity.Entities.SystemBase;

namespace Entities.ECS.ScreenBoundery.Systems
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class BulletBoundsSetup : SystemBase
	{
		protected override void OnCreate() { }

		protected override void OnUpdate()
		{
			// If singleton exists, we're done.
			if (SystemAPI.HasSingleton<ViewBounds>())
			{
				return;
			}

			var cam = Camera.main;
			if (cam == null)
			{
				return;
			}

			// NOTE: For 2D gameplay you usually want a plane depth, not nearClipPlane.
			// If your game is on Z=0 plane, use distance from camera to z=0.
			var zPlane = 0f;
			var distance = Mathf.Abs(cam.transform.position.z - zPlane);

			var bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, distance));
			var topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, distance));

			const float buffer = 1.0f;
			var bounds = new float4(
				bottomLeft.x - buffer,
				topRight.x + buffer,
				bottomLeft.y - buffer,
				topRight.y + buffer
			);

			var e = EntityManager.CreateEntity(typeof(ViewBounds));
			EntityManager.SetComponentData(e, new ViewBounds {Value = bounds});

			Enabled = false;
		}
	}
}
