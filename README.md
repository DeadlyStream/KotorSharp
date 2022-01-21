# KotorSharp
C# Implementation of Kotor File Patching and Management

## Overview
KotorSharp is divided into separate projects part of a larger solution.

* AppToolbox: A collection of Managers, Controllers and helpful classes/extensions 
* AuroraIO: Reading/Writing common Aurora File formats like 2da and gff. Also includes key, biff, and mod/rim/erf/sav
* KotorManifest: Loads up a Kotor directory and acts as an interface for accessing common files and directories
* KPatcherApp/KPatcherBase: An attempt at implementing a TSLPatcher-like file patcher

## Setup
KotorSharp is developed using [Visual Studio](https://visualstudio.microsoft.com/vs/). If you know what you're doing, I'm sure you could get it working with your favorite IDE.