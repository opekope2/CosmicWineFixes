#!/bin/bash
if [ -z "$1" ]; then
    exit 1
fi

echo Parameters: $@

SRC=$(dirname "$1")

TARGET=../../../../Bin64/Plugins/Local
mkdir -p "$TARGET" >/dev/null 2>&1

echo
echo Deploying CLIENT plugin binary:
echo
while true; do
    sleep 2
    echo From "$1" to "$TARGET/"
    cp "$1" "$TARGET/"
    if [ $? -ne 0 ]; then
        continue
    fi
    echo Copying "$SRC/0Harmony.dll" into "$TARGET/"
    cp "$SRC/0Harmony.dll" "$TARGET/"
    echo Done
    echo
    exit 0
done
