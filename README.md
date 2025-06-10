# Unity Contracts
Generic class contracts used in my Unity projects. This repository is intended for use as a submodule to another Git repository housing a Unity project, placed in the /Assets/Contracts directory.

### Manager
Managers are the top-level contract for a feature. They hold state and data which persists across scenes, and provide the entry point for cross-feature calls.

## Dependencies
[unity-scripttemplates](https://github.com/ocreeva/unity-scripttemplates):
The script template processor only provides value when processing script templates utilizing its tokens.
