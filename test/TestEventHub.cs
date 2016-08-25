namespace BlurryRoots.Happening.Test {

    using NUnit.Framework;
    using BlurryRoots.Happening;

    [TestFixture]
    public class TestEventHub {

        [Test]
        public void SubscribeRaiseAndDispatch () {
            var eventHub = new EventHub ();
            
            var hasBeenCalled = 0;
            EventCallback<TestEventTypeSimple> callback = 
                (TestEventTypeSimple e) => {
                    ++hasBeenCalled;
                };

            eventHub.Subscribe (callback);

            eventHub.Raise (new TestEventTypeSimple () {
                id = 1337,
                name = "Großherzug Hans von Wurst"
            });

            Assert.AreEqual (0, hasBeenCalled,
                "Delegate should not have been called at this point!"
            );

            eventHub.DispatchAllRaisedEvents ();

            Assert.AreEqual (1, hasBeenCalled,
                "Delegate callback has not been called!"
            );
        }

        [Test]
        public void SubscribeAndFire () {
            var eventHub = new EventHub ();

            var hasBeenCalled = 0;
            EventCallback<TestEventTypeSimple> callback =
                (TestEventTypeSimple e) => {
                    ++hasBeenCalled;
                };

            eventHub.Subscribe (callback);

            eventHub.Fire (new TestEventTypeSimple () {
                id = 1337,
                name = "Großherzug Hans von Wurst"
            });

            Assert.AreEqual (1, hasBeenCalled,
                "Delegate callback has not been called!"
            );
        }

        [Test]
        public void SubscribeRaiseFireAndUnsubscribe () {
            var eventHub = new EventHub ();

            var hasBeenCalled = 0;
            EventCallback<TestEventTypeSimple> callback =
                (TestEventTypeSimple e) => {
                    ++hasBeenCalled;
                };

            eventHub.Subscribe (callback);

            eventHub.Raise (new TestEventTypeSimple () {
                id = 1337,
                name = "Großherzug Hans von Wurst"
            });

            Assert.AreEqual (0, hasBeenCalled,
                "Delegate should not have been called at this point!"
            );

            eventHub.DispatchAllRaisedEvents ();

            Assert.AreEqual (1, hasBeenCalled,
                "Delegate callback has not been called!"
            );

            eventHub.Fire (new TestEventTypeSimple () {
                id = 1337,
                name = "Großherzug Hans von Wurst"
            });

            Assert.AreEqual (2, hasBeenCalled,
                "Delegate callback has not been called!"
            );

            eventHub.Unsubscribe (callback);

            eventHub.Fire (new TestEventTypeSimple () {
                id = 1337,
                name = "Großherzug Hans von Wurst"
            });

            Assert.AreEqual (2, hasBeenCalled,
                "Delegate callback has not been called!"
            );

        }

    }

}
