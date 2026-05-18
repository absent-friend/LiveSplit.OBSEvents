# LiveSplit.OBSEvents

This component watches for certain events during a run and sends commands to OBS when those events occur.

For the moment, it only does the following:
- When you start a run, it tells OBS to start the replay buffer.
- When you get a best segment, it tells OBS to save the replay buffer.
  - If OBS is running locally, it also renames the file to `<segment_name>-<segment_time>.(mkv|mp4)`
  - e.g. a 3:21.234 time in a segment named "Sunny Villa" becomes `Sunny Villa-3m21s234ms.mkv` (if you use `.mkv`)
 
Planned additions:
- Per-game configuration of the replay buffer length.
- Chapter markers after a run starts/ends and after every split. (Will require use of the Hybrid MP4/MOV format.)

## Building

Building this component requires the .NET 9 SDK to be installed: `winget install Microsoft.DotNet.SDK.9`.  

* Run `dotnet build -c <Debug|Release>` to build
* Copy the resulting `.dll` (`artifacts/bin/LiveSplit.OBSEvents/<debug|release>/LiveSplit.OBSEvents.dll`) to your `LiveSplit/Components` directory to test
