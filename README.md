# Zwift Activity Monitor
This project allows Zwift users to monitor their power and heartrate average in real-time.

## Prerequisites

Before you begin, ensure you have met the following requirements:
* You have installed the latest version of Npcap: Packet capture library for Windows. (https://nmap.org/npcap/#download)

## Installing Zwift Activity Monitor

To install Zwift Activity Monitor, follow these steps:

Windows:

<install_command>

## Using Zwift Activity Monitor

<p><b>Steps for preparing to use Zwift Activity Monitor:</b></p>

<ol>
    <li>Find the name of your network adapter</li>
    <ol type="i">
        <li>Open a command prompt by opening the Windows start menu and entering the command "cmd".</li> 
        <li>In the command prompt window, enter the command "ipconfig /all"</li>
        <li>Scroll through the results to find your network adapter name.  You're looking for the adapter with an IP addressed assigned.  It must be the same network that you run Zwift on.</li>
    </ol>
    <br>Examples:
    <ul>
        <li>Ethernet adapter Ethernet: (in this case the name is "Ethernet")</li>
        <li>Wireless Lan adapter Wi-Fi: (in this case the name is "Wi-Fi")</li>
        <li>There may be some others in the list.</li>
    </ul><br>
    <li>Using a text editor (like notepad.exe) find and open the file appsettings.Production.json.  It will be in the same directory as the executable files,
	which by default is <b>C:\Program Files (x86)\Zwift Activity Monitor</b></li>
	<ol type="i">
    	<li>Section ZwiftPacketMonitor</li>
    	<ul>
        	<li>Modify the value associated with the "Network" key to the network name you found in step one.</li>
    	</ul>
    	<li>Section ZwiftActivityMonitor</li>
    	<ol type="a">
        <li>Modify the value associated with the "Weight" key to your weight.  You can enter it in pounds (ie. 175) or kilograms (75.4).</li>
        <li>Modify the value associated with the "UnitOfMeasure" key to be either lbs or kgs, according to the units you entered your weight in.</li>
        <li>Modify the value associated with the "ThresholdPower" key to your threshold power number (in watts).  This is not your FTP, this is the value you would multiply by .95 to get your FTP.  It is used to calculate IF (intensity factor).</li>
        </ol>
    </ol><br>
	<li>Select default Moving Average collectors.  This is optional as 1 min, 5 min, and 20 min collectors are already setup for you.</li>
    <ol type="i">
    	<li>Determine the three collectors you would like to see on application start-up. (5 sec, 1 min, 5 min, etc.)</li>
    	<li>Modify the value associated with the "Display" key to be either true (if you'd like to see it) or false (if you don't).</li>
    	<li>Optionally, you can change how units for average power, maximum power, and FTP are displayed.  This can be either in watts or wkg.  You can even specify none if you don't want to see a value.</li>
    </ol><br>
<li>Save the configuration file.</li>
</ol>

<p><b>Steps for running the Zwift Activity Monitor:</b></p>

<ol>
	<li>Launch the application from the desktop icon or start menu</li>
	<li>When the Advanced Options dialog appears, click Start</li>
	<ul>
		<li>The Manual Operation Status should switch to Running.  If so, click Close and you're ready to monitor!</li>
		<li>If you get an error regarding your network, verify that you've setup the ZwiftPacketMonitor:Network key correctly using the steps above. </li>
	</ul>
	<li>Launch the Zwift application if not already running.</li>
</ol>

<p><b>Quick start guidance to get monitoring:</b></p>

<p>
	The Zwift Activity Monitor main window is transparent and is designed to be moved on top of the Zwift main window.
	Because it is transparent, you will still be able to see Zwift screen activities behind it.  If you have chosen (up to three)
	default collectors in the setup procedure above, you should see those items on the main window.
</p>

<ol>
	<li>Analyze menu</li>
	<ul>
		<li>Start - This will start collecting power and heartrate from Zwift as you ride.  Those results will be displayed on the main window.</li>
		<li>Stop - This will stop collection but will leave the latest values on the screen for your analysis.</li>
		<li>Timer</li>
		<ul type="i">
			<li>Setup Timer dialog - The timer is handy when waiting in the pen for an event to start.  You can syncronize the time remaining to the Zwift
			event clock and click Start.  On the main window you will see the countdown occur and when it hits zero the monitor will automatically begin.
			This is also useful for TTTs with a delayed start.  If you will have a four minute delay after banner drop, add four minutes to the Zwift event clock
			and set the timer.  Now you will know exactly when to Go! Go! Go!</li>
			<li>Stop Timer - Stops a running timer</li>
		</ul>
	</ul>
</ol>

## Contributing to <project_name>

1. Fork this repository.
2. Create a branch: `git checkout -b <branch_name>`.
3. Make your changes and commit them: `git commit -m '<commit_message>'`
4. Push to the original branch: `git push origin <project_name>/<location>`
5. Create the pull request.

Alternatively see the GitHub documentation on [creating a pull request] (https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request).

## Credits

Brad Walker for development of the ZwiftPacketMonitor library (https://github.com/braddwalker/ZwiftPacketMonitor) and giving me great ideas.

<div>Icons made by <a href="" title="photo3idea_studio">photo3idea_studio</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a></div>

## Contact

If you want to contact me you can reach me at <mailto:ruff.kevin@outlook.com>.

## License

This project uses the following license: [MIT License] (https://github.com/ruffk/ZwiftActivityMonitor/blob/master/LICENSE).
