# ZwiftActivityMonitor
This project allows Zwift users to monitor their power and heartrate average in real-time.

## Prerequisites

Before you begin, ensure you have met the following requirements:
* You have installed the latest version of Npcap: Packet capture library for Windows. (https://nmap.org/npcap/#download)

## Installing ZwiftActivityMonitor

To install ZwiftActivityMonitor, follow these steps:

Windows:

<install_command>

## Using ZwiftActivityMonitor

<p>Steps for preparing to use ZwiftActivityMonitor:</p>

<ol>
    <li>Find the name of your network adapter</li>
    <ol>
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
    <li>Using a text editor (like notepad.exe) find and open the file appsettings.Production.json.  It will be in the same directory as the executable files.</li>
	<ul>
    	<li>Section ZwiftPacketMonitor</li>
    	<ul>
        	<li>Modify the value associated with the "Network" key to the network name you found in step one.</li>
    	</ul>
    	<li>Section ZwiftActivityMonitor</li>
    	<ul>
        <li>Modify the value associated with the "Weight" key to your weight.  You can enter it in pounds (ie. 175) or kilograms (75.4).</li>
        <li>Modify the value associated with the "UnitOfMeasure" key to be either lbs or kgs, according to the units you entered your weight in.</li>
        <li>Modify the value associated with the "ThresholdPower" key to your threshold power number (in watts).  This is not your FTP, this is the value you would multiply by .95 to get your FTP.  It is used to calculate IF (intensity factor).</li>
        </ul>
    </ul><br>
	<li>Select default Moving Average collectors.  This is optional as 1 min, 5 min, and 20 min collectors are already setup for you.</li>
    <ul>
    	<li>Determine the three collectors you would like to see on application start-up. (5 sec, 1 min, 5 min, etc.)</li>
    	<li>Modify the value associated with the "Display" key to be either true (if you'd like to see it) or false (if you don't).</li>
    	<li>Optionally, you can change how units for average power, maximum power, and FTP are displayed.  This can be either in watts or wkg.  You can even specify none if you don't want to see a value.</li>
    </ul><br>
<li>Save the configuration file.</li>
</ol>



Steps for running ZwiftActivityMonitor:


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
