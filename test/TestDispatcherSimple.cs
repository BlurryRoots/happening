namespace BlurryRoots.Happening.Test {

    using NUnit.Framework;

    [TestFixture]
    public class TestDispatcher {

        [Test]
        public void CreateAndCallEmptyDispatch () {
            Assert.DoesNotThrow (
                () => {
                    var dispatcher = 
                        new EventDispatcher<TestEventTypeSimple> ();
                },
                "Could not create dispatcher for simple test event data!"
            );
        }

        [Test]
        public void SubscribeRaiseAndDispatch () {
            var dispatcher =
                new EventDispatcher<TestEventTypeSimple> ();

            var hasBeenCalled = 0;
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                ++hasBeenCalled;
            });

            dispatcher.Raise (new TestEventTypeSimple () {
                id = 1337,
                name = "Großherzug Hans von Wurst"
            });

            Assert.AreEqual (0, hasBeenCalled,
                "Delegate should not have been called at this point!"
            );

            dispatcher.DispatchAllRaisedEvents ();

            Assert.AreEqual (1, hasBeenCalled,
                "Delegate callback has not been called!"
            );
            Assert.AreEqual (0, dispatcher.CurrentlyActiveEvents);
        }

        [Test]
        public void SubscribeAndFire () {
            var dispatcher =
                new EventDispatcher<TestEventTypeSimple> ();

            var hasBeenCalled = 0;
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                ++hasBeenCalled;
            });

            dispatcher.Fire (new TestEventTypeSimple () {
                id = 1337,
                name = "Knurpselwums"
            });

            Assert.AreEqual (1, hasBeenCalled,
                "Delegate callback has not been called!"
            );
            Assert.AreEqual (0, dispatcher.CurrentlyActiveEvents);
        }

        [Test]
        public void RaisedEventRaisesNewEvent () {
            var dispatcher =
                new EventDispatcher<TestEventTypeSimple> ();

            var hasBeenCalled = 0;
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                dispatcher.Raise (new TestEventTypeSimple () {
                    id = 42,
                    name = "Knurpselwums"
                });
            });
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                if (42 == e.id && "Knurpselwums" == e.name) {
                    ++hasBeenCalled;
                }
            });

            dispatcher.Raise (new TestEventTypeSimple () {
                id = 1337,
                name = "Großherzog Hans Hubertus von Wurst"
            });

            Assert.AreEqual (0, hasBeenCalled,
                "Delegate should not have been called at this point!"
            );

            dispatcher.DispatchAllRaisedEvents ();

            Assert.AreEqual (0, hasBeenCalled,
                "First event has already triggered the second one?!"
            );

            dispatcher.DispatchAllRaisedEvents ();

            Assert.AreEqual (1, hasBeenCalled,
                "Delegate callback has not been called!"
            );

            Assert.AreEqual (1, dispatcher.CurrentlyActiveEvents,
                "There should be one event left in the queue!"
            );
        }

        [Test]
        public void FiredEventRaisesNewEvent () {
            var dispatcher =
                new EventDispatcher<TestEventTypeSimple> ();

            var hasBeenCalled = 0;
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                dispatcher.Raise (new TestEventTypeSimple () {
                    id = 42,
                    name = "Knurpselwums"
                });
            });
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                if (42 == e.id && "Knurpselwums" == e.name) {
                    ++hasBeenCalled;
                }
            });

            dispatcher.Fire (new TestEventTypeSimple () {
                id = 1337,
                name = "Großherzog Hans Hubertus von Wurst"
            });

            Assert.AreEqual (0, hasBeenCalled,
                "Delegate should not have been called at this point!"
            );

            dispatcher.DispatchAllRaisedEvents ();

            Assert.AreEqual (1, hasBeenCalled,
                "Delegate callback has not been called!"
            );

            Assert.AreEqual (1, dispatcher.CurrentlyActiveEvents,
                "There should be one event left in the queue!"
            );
        }

    }

}
