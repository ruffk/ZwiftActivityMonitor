To publish:

1) Copy previous ZwiftActivityMonitor.1.0.0.nupkg file to a new file with next version #.

2) Open the file (double-click) with NuGet package explorer and update the internal version to match.  Add any new files to lib\net45 if necessary.  
	If unsure if there are any new files, delete the net45 folder, add it back, and re-select all files in the bin\release folder.

3) Run the following command from package manager console: (replace with new version #)
	squirrel --releasify ZwiftActivityMonitor.1.0.0.nupkg

4) From GitHub repository, create a new release.  Set the tag and enter any comments.  Click the binaries box to select all the files in the releases folder.

5) Publish release