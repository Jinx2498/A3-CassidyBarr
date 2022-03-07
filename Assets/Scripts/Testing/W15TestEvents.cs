using System.Collections;
using System.ComponentModel;
using GameBrains.Entities;
using GameBrains.EventSystem;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.GUI;
using UnityEngine;

namespace Testing
{
	[AddComponentMenu("Scripts/Testing/W15 Test Events")]
	public class W15TestEvents : ExtendedMonoBehaviour
	{
		public MessageViewer messageViewer;

		public bool testSubscribeAndUnsubscribe;
		public bool testWithDelegate;
		public bool testFireAndEnqueueMessage;
		public bool testWithMessageWithDelegate;

		public delegate void OnTestDelegate();

		public static OnTestDelegate testDelegate;

		public override void Update()
		{
			base.Update();
			
			if (testSubscribeAndUnsubscribe)
			{
				testSubscribeAndUnsubscribe = false;

				StartCoroutine(TestSubscribeAndUnsubscribe());
			}

			if (testWithDelegate)
			{
				testWithDelegate = false;

				TestFireAndEnqueueWithDelegate();
			}

			if (testFireAndEnqueueMessage)
			{
				testFireAndEnqueueMessage = false;

				TestFireAndEnqueueMessage();
			}

			if (testWithMessageWithDelegate)
			{
				testWithMessageWithDelegate = false;

				TestMessageWithDelegate();
			}
		}

		IEnumerator TestSubscribeAndUnsubscribe()
		{
			// Subscribe to receive MyEvents
			EventManager.Instance.Subscribe<float>(Events.MyEvent, OnTest);

			// Queued for start of next cycle
			// Note: Either specify the type explicitly or it will be inferred.
			//       Without <float> it would infer the type is int unless we add f.
			EventManager.Instance.Enqueue<float>(Events.MyEvent, 0);

			// Queued for 1 second
			EventManager.Instance.Enqueue(Events.MyEvent, 1f, 1f);

			yield return new WaitForSeconds(0.5f);

			// Add another subscriber
			// Note: We are subscribing 0.5f seconds after events enqueued. We'll miss the
			//       first one but catch the second.
			EventManager.Instance.Subscribe<float>(Events.MyEvent, OnOtherTest);

			// Queued for 3 seconds, but we Unsubscribe after 2 seconds, so not received by us.
			EventManager.Instance.Enqueue(Events.MyEvent, 3f, 2f);

			// Do immediately
			EventManager.Instance.Fire(Events.MyEvent, 3f);

			// Do immediately
			EventManager.Instance.Fire(Events.MyEvent, 4f);

			yield return new WaitForSeconds(2);

			// Unsubscribe OnTest (no longer receive) MyEvents
			EventManager.Instance.Unsubscribe<float>(Events.MyEvent, OnTest);

			// Do immediately, but we are no longer subscribed, so not received by us.
			EventManager.Instance.Fire(Events.MyEvent, 5f);

			// Unsubscribe OnOtherTest (no longer receive) MyEvents
			EventManager.Instance.Unsubscribe<float>(Events.MyEvent, OnOtherTest);
		}

		void TestFireAndEnqueueWithDelegate()
		{
			EventManager.Instance.Enqueue(Events.MyEvent, 3f, OnUnsubscribedTest, 7f);

			// Do immediately
			EventManager.Instance.Fire(Events.MyEvent, OnUnsubscribedTest, 8f);
		}

		void TestFireAndEnqueueMessage()
		{
			// Get an entity named Receiver to send messages to.
			// Don't really need to look it up since it should be in receiver field
			Entity lookedUpReceiver = EntityManager.Find<Entity>("Receiver");

			// Send immediately
			EventManager.Instance.Fire(Events.Message, lookedUpReceiver.ID, "Hello");

			// Queued for 2 seconds
			EventManager.Instance.Enqueue(Events.Message, 2f, lookedUpReceiver.ID, "Hello again");
		}

		void TestMessageWithDelegate()
		{
			// Send immediately
			EventManager.Instance.Fire(Events.Message,  OnMessageTest, "Hello");

			// Queued for 2 seconds
			EventManager.Instance.Enqueue(Events.Message, 2f,  OnMessageTest, "Hello again");
		}

		void OnTest(Event<float> eventArguments)
		{
			if (VerbosityDebug)
			{
				Debug.Log($"OnTest: {eventArguments}");
			}
		}

		void OnOtherTest(Event<float> eventArguments)
		{

			if (VerbosityDebug)
			{
				Debug.Log($"OnOtherTest: {eventArguments}");
			}
		}

		// We won't subscribe for events with this one, but we can still use it.
		void OnUnsubscribedTest(Event<float> eventArguments)
		{
			if (VerbosityDebug)
			{
				Debug.Log($"OnUnsubscribedTest: {eventArguments}");
			}
		}

		void OnMessageTest(Event<string> eventArguments)
		{
			if (VerbosityDebug)
			{
				Debug.Log($"OnMessageTest: {eventArguments}");
			}

			messageViewer.messageQueue.AddMessage(eventArguments.EventData);
		}
	}
}

namespace GameBrains.EventSystem
{
	// Add our own event types
	public static partial class Events
	{
		[Description("My Event")]
		public static readonly EventType MyEvent = (EventType) Count++;
	}
}