# LiveSplit.OBSEvents

This component watches for certain events during a run and sends commands to OBS when those events occur.

For the moment, its main function is to save the replay buffer when you get a new best segment.
 
Planned additions:
- Automatically set the replay buffer duration to fit the longest segment in the run.
- Automatically start recording when you open LiveSplit.
- Chapter markers after a run starts/ends and after every split. (Will require use of the Hybrid MP4/MOV format.)
- More generic event configuration (e.g. show source for X seconds after Y event)

## Building

Building this component requires the .NET 9 SDK to be installed: `winget install Microsoft.DotNet.SDK.9`.  

* Run `dotnet build -c <Debug|Release>` to build
* Copy the resulting `.dll` (`artifacts/bin/LiveSplit.OBSEvents/<debug|release>/LiveSplit.OBSEvents.dll`) to your `LiveSplit/Components` directory to test
