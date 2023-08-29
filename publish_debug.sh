#!/bin/bash

DEBUGDIR=ChebsAmplifiedStations/bin/Debug
DLL=$DEBUGDIR/ChebsAmplifiedStations.dll
#PLUGINS=/home/$USER/.local/share/Steam/steamapps/common/Valheim/BepInEx/plugins
PLUGINS=/home/$USER/.config/r2modmanPlus-local/Valheim/profiles/cheb-development/BepInEx/plugins/ChebGonaz-ChebsAmplifiedStations

# Check that source files exist and are readable
if [ ! -f "$DLL" ]; then
    echo "Error: $DLL does not exist or is not readable."
    exit 1
fi

# Check that target directory exists and is writable
if [ ! -d "$PLUGINS" ]; then
    echo "Error: $PLUGINS directory does not exist."
    exit 1
fi

if [ ! -w "$PLUGINS" ]; then
    echo "Error: $PLUGINS directory is not writable."
    exit 1
fi


cp -f "$DLL" "$PLUGINS" || { echo "Error: Failed to copy $DLL"; exit 1; }
