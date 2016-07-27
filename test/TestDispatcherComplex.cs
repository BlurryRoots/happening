namespace BlurryRoots.Happening.Test {

    using NUnit.Framework;

    [TestFixture]
    public class TestDispatcherComplex {

        [Test]
        public void CreateAndCallEmptyDispatch () {
            Assert.DoesNotThrow (
                () => {
                    var dispatcher =
                        new EventDispatcher<TestEventTypeComplex> ();
                },
                "Could not create dispatcher for simple test event data!"
            );
        }

        [Test]
        public void SubscribeRaiseAndDispatch () {
            var dispatcher =
                new EventDispatcher<TestEventTypeComplex> ();

            var hasBeenCalled = false;
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                hasBeenCalled = true;
            });

            dispatcher.Raise (new TestEventTypeComplex (08, 15));

            Assert.IsFalse (hasBeenCalled,
                "Delegate should not have been called at this point!"
            );

            dispatcher.DispatchAllRaisedEvents ();

            Assert.IsTrue (hasBeenCalled,
                "Delegate callback has not been called!"
            );
            Assert.AreEqual (0, dispatcher.CurrentlyActiveEvents);
        }

        [Test]
        public void SubscribeAndFire () {
            var dispatcher =
                new EventDispatcher<TestEventTypeComplex> ();

            var hasBeenCalled = false;
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                hasBeenCalled = true;
            });

            dispatcher.Fire (new TestEventTypeComplex (08, 15));

            Assert.IsTrue (hasBeenCalled,
                "Delegate callback has not been called!"
            );
            Assert.AreEqual (0, dispatcher.CurrentlyActiveEvents);
        }

        [Test]
        public void RaisedEventRaisesNewEvent () {
            var dispatcher =
                new EventDispatcher<TestEventTypeComplex> ();

            var hasBeenCalled = false;
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                dispatcher.Raise (new TestEventTypeComplex (27, 12));
            });
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                if (27 == e.Dates[0] && 12 == e.Dates[1]) {
                    hasBeenCalled = true;
                }
            });

            dispatcher.Raise (new TestEventTypeComplex (08, 15));

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

            Assert.AreEqual (1, dispatcher.CurrentlyActiveEvents,
                "There should be one event left in the queue!"
            );
        }

        [Test]
        public void FiredEventRaisesNewEvent () {
            var dispatcher =
                 new EventDispatcher<TestEventTypeComplex> ();

            var hasBeenCalled = false;
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                dispatcher.Raise (new TestEventTypeComplex (27, 12));
            });
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                if (27 == e.Dates[0] && 12 == e.Dates[1]) {
                    hasBeenCalled = true;
                }
            });

            dispatcher.Fire (new TestEventTypeComplex (08, 15));

            Assert.IsFalse (hasBeenCalled,
                "Delegate should not have been called at this point!"
            );

            dispatcher.DispatchAllRaisedEvents ();

            Assert.IsTrue (hasBeenCalled,
                "Delegate callback has not been called!"
            );

            Assert.AreEqual (1, dispatcher.CurrentlyActiveEvents,
                "There should be one event left in the queue!"
            );
        }

    }

}
