# TeamsStatusLight

Control an RGB LED by sending serial messages to an ATmega microcontroller based on your current Microsoft Teams status.

See https://github.com/wesleytabaka/RGBStatusLight for the code for the microcontroller.

# Video Demo

<a href="https://www.youtube.com/watch?feature=player_embedded&v=qYAO10NIxuU" target="_blank">
    <img src="https://img.youtube.com/vi/qYAO10NIxuU/0.jpg" alt="IMAGE ALT TEXT HERE"><br>View on YouTube
</a>

# Public API

## (Enum) Effect
- SOLID: LED is steady on the primary color.
- BLINK: LED is steady on the primary color then steady on the secondary color.
- FLASH: LED fades between the primary and secondary color.
- PULSE: LED is fully on primary color then fades to secondary color.
- CYCLE: LED cycles through all the colors.

## (Enum) [Transition](./Transition.cs)
- NOW: Change color immediately
- FADE: Change color gradually.

## (Class) [Indicator](./Indicator.cs) : [IIndicator](./IIndicator.cs)
Handles writing to and reading from SerialPort.

- ### (Constructor) Indicator(string ***portName***)
	Creates an Indicator instance representing the LED driver (Indicator) using serial port named ***portname***.
- ### (Method) void SetIndicator(IIndicatorInstruction ***instruction***)
	Instructs the LED driver to change the RGB LED to the new color/effect/transition, ***instruction***.

## (Class) [IndicatorInstruction](./IndicatorInstruction.cs) : [IIndicatorInstruction](./IIndicatorInstruction.cs)
- ### (Constructor, overload) IndicatorInstruction(int r, int g, int b, int? r2, int? g2, int? b2, Effect effect, int effectRate, Transition transition, int transitionDuration)
	Creates a new IndicatorInstruction instance with the primary color, secondary color, effect, and transition settings above.
- ### (Constructor, overload) IndicatorInstruction(int r, int g, int b, Effect effect, int effectRate, Transition transition, int transitionDuration)
	Creates a new IndicatorInstruction instance with the color, effect, and transition settings above.
- ### (Constructor, overload) IndicatorInstruction(int r, int g, int b)
	Creates a new IndicatorInstruction instance with the color settings above, SOLID Effect, NOW Transition. 
- ### (Constructor, overload) IndicatorInstruction()
	Creates a new IndicatorInstruction instance with all LEDs off, SOLID Effect, NOW Transition.
- ### (Method, static) Dictionary<string, IndicatorInstruction> DeserializeIndicatorInstructionsFromConfig(IConfigurationSection config)
	Helper method to deserialize the IndicatorInstructionMapping from the appsettings.json file.

## (Class) [Presence](./Presence.cs) : [IPresence](./IPresence.cs)
- ### (Constructor) Presence()
	Creates a new Presence instance, representing the state of your Teams status.  Two instances of this class track your previous status and current status.  When the presence changes, we call the Equals method below to see if the Presence is different.
- ### (Method, overload) void setPresence(string activity, string availability)
	Updates the presence instance.
- ### (Method, overload) void setPresence(IPresence presence)
	Updates the presence instance.
- ### (Method) Presence getPresence()
	Returns this object.
- ### (Method) bool Equals(IPresence other)
	Compares the activity and availability fields to another Presence instance, returning true if the same, false if different.

## (Entry point) [Program](./Program.cs)

## (Class) [TeamsStatus](./TeamsStatus.cs) : [ITeamsStatus](./ITeamsStatus.cs)
- ### (Constructor) TeamsStatus()
	Creates new TeamsStatus instance representing your Teams status based on your Teams log files.
- ### (Method) IPresence getPresence()
	Determines your Teams presence based on log files.  Gets the latest log file based on the timestamp in the filename and gets the latest status matching a regular expression pattern.

# Configuration
See [appsettings.json](./appsettings.json)

## COMPort
Set this to the COM port to which the microcontroller is attached.

## CheckPresenceIntervalMS
How often to check your Microsoft Teams status in milliseconds.

## IndicatorInstructionMapping
Customize the color, secondary color, transition, and effect for each Teams status.  This section of [appsettings.json](./appsettings.json) is deserialized into instances of [IndicatorInstruction](./IndicatorInstruction.cs). See [IndicatorInstruction.cs](./IndicatorInstruction.cs)

If a secondary color is not defined, it will be defaulted to off/0.

The following IndicatorInstructions are defined for Teams statuses:
- Unknown
	- Not an official Teams status.  Is used when an exception is encountered getting your Teams status.  If you'd rather the LED just turn off in this case, change all color components to 0 and set effect to SOLID in the "Unkown" section of IndicatorInstructionMapping in appsettings.json.
- Off
	- Not an official Teams status.  For future use if we need to just turn everything off.
- Offline
- Available
- Busy
- DoNotDisturb
- BeRightBack
- Away

# Contributing
Fork this repo and create a pull request.  Thank you for your interest!
