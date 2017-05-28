#!/bin/bash

EXPORT_PATH="/Users/ken.chou/projects/NewTetrisAndroid/Update"
ASSET_SRC="/Users/ken.chou/projects/NewTetrisAndroid/Update/NewTetrisUnity/src/main/assets"
ASSET_DEST="/Users/ken.chou/projects/NewTetrisAndroid/NewTetrisUnity/src/main/"
BUILD_LOG="build.log"

# clean up
rm -rf $EXPORT_PATH

# build
/Applications/Unity/Unity.app/Contents/MacOS/Unity -quit -batchmode -username "kuanyingchou@gmail.com" -password "565656" -logFile "$BUILD_LOG" -executeMethod "Exporter.export" "$EXPORT_PATH"

if [ $? -eq 0 ]; then
  # import assets to android project
  rm -rf "$ASSET_DEST/assets"
  cp -r "$ASSET_SRC" "$ASSET_DEST"
  echo "SUCCESS!"
else
  echo "Oops!"
  grep "error" "$BUILD_LOG"
fi

