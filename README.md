# BeatSaber_OWO

This mod hooks into a few functions in Beat Saber and provides matching haptic feedback in the OwO skin. Feedback currently includes:
* Slicing blocks (left and right)
* Exploding bombs
* Missed blocks
* Hitting an obstacle

## Compilation / installation

The mod uses BSIPA to hook into Beat Saber methods, so you will need to have BSIPA installed on your Beat Saber installation. This is done most easily via [ModAssistant](https://github.com/Assistant/ModAssistant). And you should start up Beat Saber once after the installation.

Then you will need to unzip the [latest release](https://github.com/floh-bhaptics/BeatSaber_OWO/releases/latest/) into your Beat Saber game directory (the game root directory should contain the folders "Beat Saber_Data", "Libs", and "Plugins"). The ZIP file contains the OWO SDK (will be unzipped into the "Libs" folder) and the actual Mod DLL (will be unzipped into the "Plugins" folder).

You can connect with a manually set IP by placing a file called "OWO_Manual_IP.txt" into the "UserData" folder and writing the IP in there.
