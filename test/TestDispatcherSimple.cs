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

            var hasBeenCalled = false;
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                hasBeenCalled = true;
            });

            dispatcher.Raise (new TestEventTypeSimple () {
                id = 1337,
                name = "Großherzug Hans von Wurst"
            });

            Assert.IsFalse (hasBeenCalled,
                "Delegate should not have been called at this point!"
            );

            dispatcher.DispatchAllRaisedEvents ();

            Assert.IsTrue (hasBeenCalled,
                "Delegate callback has not been called!"
            );
            Assert.AreEqual (0, dispatcher.CurrentlyRaisedEvents);
        }

        [Test]
        public void SubscribeAndFire () {
            var dispatcher =
                new EventDispatcher<TestEventTypeSimple> ();

            var hasBeenCalled = false;
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                hasBeenCalled = true;
            });

            dispatcher.Fire (new TestEventTypeSimple () {
                id = 1337,
                name = "Knurpselwums"
            });

            Assert.IsTrue (hasBeenCalled,
                "Delegate callback has not been called!"
            );
            Assert.AreEqual (0, dispatcher.CurrentlyRaisedEvents);
        }

        [Test]
        public void RaisedEventRaisesNewEvent () {
            var dispatcher =
                new EventDispatcher<TestEventTypeSimple> ();

            var hasBeenCalled = false;
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                dispatcher.Raise (new TestEventTypeSimple () {
                    id = 42,
                    name = "Knurpselwums"
                });
            });
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                if (42 == e.id && "Knurpselwums" == e.name) {
                    hasBeenCalled = true;
                }
            });

            dispatcher.Raise (new TestEventTypeSimple () {
                id = 1337,
                name = "Großherzog Hans Hubertus von Wurst"
            });

            Assert.IsFalse (hasBeenCalled,
                "Delegate should not have been called at this point!"
            );

            dispatcher.DispatchAllRaisedEvents ();

            Assert.IsFalse (hasBeenCalled,
                "This should not happen :/"
            );

            dispatcher.DispatchAllRaisedEvents ();

            Assert.IsTrue (hasBeenCalled,
                "Delegate callback has not been called!"
            );

            Assert.AreEqual (1, dispatcher.CurrentlyRaisedEvents,
                "There should be one event left in the queue!"
            );
        }

        [Test]
        public void FiredEventRaisesNewEvent () {
            var dispatcher =
                new EventDispatcher<TestEventTypeSimple> ();

            var hasBeenCalled = false;
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                dispatcher.Raise (new TestEventTypeSimple () {
                    id = 42,
                    name = "Knurpselwums"
                });
            });
            dispatcher.Subscribe ((TestEventTypeSimple e) => {
                if (42 == e.id && "Knurpselwums" == e.name) {
                    hasBeenCalled = true;
                }
            });

            dispatcher.Fire (new TestEventTypeSimple () {
                id = 1337,
                name = "Großherzog Hans Hubertus von Wurst"
            });

            Assert.IsFalse (hasBeenCalled,
                "Delegate should not have been called at this point!"
            );

            dispatcher.DispatchAllRaisedEvents ();

            Assert.IsTrue (hasBeenCalled,
                "Delegate callback has not been called!"
            );

            Assert.AreEqual (1, dispatcher.CurrentlyRaisedEvents,
                "There should be one event left in the queue!"
            );
        }

    }

}
