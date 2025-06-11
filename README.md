# Unity Contracts
Generic class contracts used in my Unity projects. This repository is intended for use as a submodule to another Git repository housing a Unity project, placed in the /Assets/Contracts directory.

### Omnibus
The Omnibus is the singleton contract for a game. It provides access to all Managers via their interfaces, for the purpose of cross-feature calls.

### Manager
A Manager is the top-level contract for a feature, which persists across the lifespan of a game. They hold state and data which persists across scenes, and provide the entry point for cross-feature calls.

### Entity
An Entity is the contract for any multi-instanced game object. They hold state and data relevant to the game object, and provide the entry point for cross-feature calls. Every Entity is expected to belong to at least one Manager collection.

### Trait
A Trait is the contract for any logic or functionality which does not provide any entry points to external features. Manager Traits provide feature-wide behavior, while Entity Traits provide behavior scoped to a game object.

### API
An API is the contract for a Trait which also provides an entry point to external features. APIs are exposed on their parent Manager or Entity as an interface for the purpose of cross-feature calls.

### Ex Classes
The Ex contracts provide simple data types with built-in events occuring on state change. This allows game logic to be event driven rather than polling each frame, providing a performance benefit when triggering events are relatively sparse.

The ExValue contract provides events for a generically-typed value. The ExBool contract provides additional events for a boolean value. The ExCollection contract provides a collection of Entity contracts.

## Dependencies
[unity-scripttemplates](https://github.com/ocreeva/unity-scripttemplates):
The script template processor only provides value when processing script templates utilizing its tokens.
