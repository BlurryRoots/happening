# Happening

A generic deffered event system.

## Details

Happending supports raising and reacting to events based on class types.

### Subscribing

Let's assume you want to react to an object entering a trigger volume. When an object enters you want the function ```OnTrapTriggerEntered (TriggerEntered e)``` to be called.

```c#
eventHub.Subscribe<TriggerEntered> (this.OnTrapTriggerEntered);
```

### Raising an event

In another part of your application, you would then react to the physic calculations and raise the custom event ```TriggerEntered```.

```c#
eventHub.Raise (new TriggerEntered () {
    trigger = this,
    collider = other,
});
```

### Processing events

Raised events are not immeditatly processed. You may choose the appropriate time and circumstances and tell the event hub to dispatch all raised events.

```c#
events.DispatchAllRaisedEvents ();
```

### Immediate processing

Sometimes you may want to instantly process an event.

```c#
eventHub.Fire (new TriggerEntered () {
    trigger = this,
    collider = other,
});
```


## Documentation

Head over to [docify](https://www.docify.net/Doc/happening) for API documentation.

## Demo

Head over to the [unity demo project](https://github.com/BlurryRoots/happening-unity-demo) to see *happening* in action.