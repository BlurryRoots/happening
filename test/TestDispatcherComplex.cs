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

            var hasBeenCalled = 0;
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                ++hasBeenCalled;
            });

            dispatcher.Raise (new TestEventTypeComplex (08, 15));

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
                new EventDispatcher<TestEventTypeComplex> ();

            var hasBeenCalled = 0;
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                ++hasBeenCalled;
            });

            dispatcher.Fire (new TestEventTypeComplex (08, 15));

            Assert.AreEqual (1, hasBeenCalled,
                "Delegate callback has not been called!"
            );
            Assert.AreEqual (0, dispatcher.CurrentlyActiveEvents);
        }

        [Test]
        public void RaisedEventRaisesNewEvent () {
            var dispatcher =
                new EventDispatcher<TestEventTypeComplex> ();

            var hasBeenCalled = 0;
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                dispatcher.Raise (new TestEventTypeComplex (27, 12));
            });
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                if (27 == e.Dates[0] && 12 == e.Dates[1]) {
                    ++hasBeenCalled;
                }
            });

            dispatcher.Raise (new TestEventTypeComplex (08, 15));

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
                 new EventDispatcher<TestEventTypeComplex> ();

            var hasBeenCalled = 0;
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                dispatcher.Raise (new TestEventTypeComplex (27, 12));
            });
            dispatcher.Subscribe ((TestEventTypeComplex e) => {
                if (27 == e.Dates[0] && 12 == e.Dates[1]) {
                    ++hasBeenCalled;
                }
            });

            dispatcher.Fire (new TestEventTypeComplex (08, 15));

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
