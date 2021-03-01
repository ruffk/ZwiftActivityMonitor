To publish:

1) Copy previous ZwiftActivityMonitor.1.0.0.nupkg file to a new file with next version #.

2) Open the file (double-click) with NuGet package explorer and update the internal version to match.  Add any new files to lib\net45 if necessary.

3) Run the following command from package manager console: (replace with new version #)
	squirrel --releasify ZwiftActivityMonitor.1.0.0.nupkg
