# Unity Contracts
Generic class contracts used in my Unity projects. This repository is intended for use as a submodule to another Git repository housing a Unity project, placed in the /Assets/Contracts directory.

### Omnibus
The Omnibus is the singleton contract for a game. It provides access to all Managers via their interfaces, for the purpose of cross-feature calls.

### Manager
A Manager is the top-level contract for a feature, which persists across the lifespan of a game. They hold state and data which persists across scenes, and provide the entry point for cross-feature calls.

## Dependencies
[unity-scripttemplates](https://github.com/ocreeva/unity-scripttemplates):
The script template processor only provides value when processing script templates utilizing its tokens.
