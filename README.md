# bhapticsFunctional

This mod hooks into a few functions in Beat Saber and provides matching haptic feedback in the bHaptics vest, arms, and hands. Feedback currently includes:
* Slicing blocks (left and right)
* Exploding bombs
* Missed blocks
* Hitting an obstacle
* Celebration or fail effect when finishing a level

I made a second *"bHapticsMusical"* mod that will provide feedback matching to the music, hence the "Functional" in the name.

You can see a short demo of the effects provided to both mods here:
[https://www.youtube.com/watch?v=X15WuW8BiaM](https://www.youtube.com/watch?v=X15WuW8BiaM)

## Compilation / installation

The mod uses BSIPA to hook into Beat Saber methods via Harmony, and uses the bHaptics library to interface the HTTP/TLS interface of the bHaptics Player.
So BSIPA and the bHaptics library have to be installed on Beat Saber. It does not touch the game variables or functions and only
hooks in to trigger feedback.

The mod is built with Visual Studio 2019 and should just compile if the BSIPA modding tools are installed correctly.
