using System.ComponentModel;
using GameBrains.Entities;
using GameBrains.EventSystem;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace Testing
{
	[AddComponentMenu("Scripts/Testing/W15 Test Entity Event Handling")]
	public class W15TestEntityEventHandling : ExtendedMonoBehaviour
	{
		public bool testEntityEventHandling;

		public override void Update()
		{
			base.Update();
			
			if (testEntityEventHandling)
			{
				testEntityEventHandling = false;

				TestEntityEventHandling();
			}
		}

		void TestEntityEventHandling()
		{
			Entity receiver = EntityManager.Find<Entity>("Receiver");

			EventManager.Instance.Enqueue(Events.MyEntityEvent, receiver.ID, "Hello");

			// Do immediately
			EventManager.Instance.Fire(Events.MyEntityEvent, receiver.ID, 8);
		}
	}
}

namespace GameBrains.EventSystem
{
	// Add our own event types
	public static partial class Events
	{
		[Description("My Entity Event")]
		public static readonly EventType MyEntityEvent = (EventType) Count++;
		
		[Description("My Other Event")]
		public static readonly EventType MyOtherEvent = (EventType) Count++;
	}
}