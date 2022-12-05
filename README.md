# 1. Table of content
- [1. Table of content](#1-table-of-content)
- [2. What is this Plugin?](#2-what-is-this-plugin)
- [3. Setup](#3-setup)
- [4. How to use it?](#5-how-to-use-it)
- [5. Support / Feedback](#4-support--feedback)
- [6. How to contribute?](#6-how-to-contribute)
- [7. Sponsor me!](#7-how-to-sponsor)

# 2. What is this Plugin?
This Loupedeck Plugin allows you to control your openHAB System with a Loupedeck (https://openhab.org)
Right now it´s macOS only.
You can pick Items out of a list - send commands to Items - get the State of an Item etc.
It is possible to use the touch buttons - dials - and show the state of an Item on the Main Screen or the Dial Screen.


# 3. Setup
Install via .lplug4 Plugin File or directly via VS Code.

create a openhab.json like this 

```json
{
    "user": "openHAB-username",
    "password": "openHAB-password",
    "url": "openhab-url:8080/rest/items/",
    "entries": [
     {
       "service": "switch",
       "items": [
         "Itemyouwanttocontrol",
         "Itemyouwanttocontrol"
         ]
     },
     {
       "service": "lights",
       "items": [
         "Itemyouwanttocontrol",
         "Itemyouwanttocontrol"
         ]
     },
     
     {
       "service": "termostat",
       "items": [
         "Itemyouwanttocontrol",
         "Itemyouwanttocontrol"
         ]
     },
     {
       "service": "LED",
       "items": [
         "Itemyouwanttocontrol"
         ]
     }
    ]
 }
```

replace the fields with your values.


place the file in `%userprofile%\.local/share/Loupedeck/Plugins/OpenHAB/` as `openhab.json`

# 4. How to use it?

1. Install the Plugin
2. Create a openhab.json config with your Items
3. Add Actions to Loupedeck
4. Have fun controlling your openHAB System



# 5. Support / Feedback
Any bugs? Feature request? Message me [here](https://github.com/bangertech) or click on the "Issues" tab here on the GitHub repository!

# 6. How to contribute?

Fork the repository and create PR's.

# 7 How to sponsor?

[Paypal](https://www.paypal.com/donate/?hosted_button_id=FD26FHKRWS3US)
