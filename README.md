# Smooth Splitter for Dyson Sphere Program

**DSP Smooth Splitter** is a mod for the Unity game Dyson Sphere Program developed by Youthcat Studio and published by Gamera Game.  The game is available on [here](https://store.steampowered.com/app/1366540/Dyson_Sphere_Program/).

Smooths out the priority output of the Splitter.  Have you ever had a two inputs and two outputs where the input was greater than the priority output, and the priority output belt wasn't full?  Do you have a 3-in-1-out splitter which is intermitantly backing while intermitantly outputting gaps?  If you have, and this bothers you, this mod is for you.

The issues in the default game's splitter happen because the game's splitter contains no buffer, and just outputs when there's space at the front of the belt.  This mod fixes that by shoving items into the output belt when that's possible.  So you might see little gaps coming out of your splitters, and then closing up.

This mod does not change your save file format.  So it is safe to uninstall later if you wish without losing your save.

If you like this mod, please click the thumbs up at the [top of the page](https://dsp.thunderstore.io/package/GreyHak/DSP_Smooth_Splitter/) (next to the Total rating).  That would be a nice thank you for me, and help other people to find a mod you enjoy.

If you have issues with this mod, please report them on [GitHub](https://github.com/GreyHak/dsp-smooth-splitter/issues).  I try to respond within 12 hours.    You can also contact me at GreyHak#2995 on the [DSP Modding](https://discord.gg/XxhyTNte) Discord #tech-support channel..

## Installation
This mod uses the BepInEx mod patch framework.  So BepInEx must be installed to use this mod.  Find details for installing BepInEx [in their user guide](https://bepinex.github.io/bepinex_docs/master/articles/user_guide/installation/index.html#installing-bepinex-1).  This mod was tested with BepInEx x64 5.4.5.0 and Dyson Sphere Program 0.6.16.5831 on Windows 10.

To manually install this mod, add the `DSPSmoothSplitter.dll` to your `%PROGRAMFILES(X86)%\Steam\steamapps\common\Dyson Sphere Program\BepInEx\patchers\` folder.

This mod can also be installed using ebkr's [r2modman](https://dsp.thunderstore.io/package/ebkr/r2modman/) mod manager by clicking "Install with Mod Manager" on the [DSP Modding](https://dsp.thunderstore.io/package/GreyHak/DSP_Smooth_Splitter/) site.

## Open Source
The source code for this mod is available for download, review and forking on GitHub [here](https://github.com/GreyHak/dsp-smooth-splitter) under the BSD 3 clause license.

## Change Log
### v1.0.0
 - Initial release.
