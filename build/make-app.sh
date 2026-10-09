#!/bin/bash
# Phase 4 groundwork: turn the self-contained osx-arm64 publish output into a
# runnable SphereIQ.app. Run this ON THE MAC.
#
# Usage:
#   ./build/make-app.sh [publish-dir] [output-dir]
#
# Defaults:
#   publish-dir = artifacts/publish/osx-arm64
#   output-dir  = artifacts/app
#
# Not signing here on purpose - signing/notarization is Phase 5 and needs the
# Apple Developer certificate. An unsigned .app runs fine locally; Gatekeeper
# only complains for downloaded apps.

set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PUBLISH="${1:-$ROOT/artifacts/publish/osx-arm64}"
OUT="${2:-$ROOT/artifacts/app}"
APP="$OUT/SphereIQ.app"

VERSION="$(grep -oP '(?<=<VersionPrefix>)[^<]+' "$ROOT/src/SphereIQ.App/SphereIQ.App.csproj" || true)"
if [ -z "${VERSION:-}" ]; then VERSION="1.0.0"; fi

if [ ! -f "$PUBLISH/SphereIQ" ]; then
  echo "ERROR: $PUBLISH/SphereIQ not found."
  echo "Run on Windows first:"
  echo "  dotnet publish src/SphereIQ.App/SphereIQ.App.csproj -r osx-arm64 --self-contained true -c Release -o artifacts/publish/osx-arm64"
  exit 1
fi

echo "==> Version: $VERSION"
echo "==> Cleaning $APP"
rm -rf "$APP"
mkdir -p "$APP/Contents/MacOS" "$APP/Contents/Resources"

echo "==> Copying payload"
cp -R "$PUBLISH"/. "$APP/Contents/MacOS/"

echo "==> Writing Info.plist"
sed "s/__VERSION__/$VERSION/g" "$ROOT/build/Info.plist" > "$APP/Contents/Info.plist"

# PkgInfo is required by LaunchServices; without it the bundle is not treated
# as an application.
echo -n "APPL????" > "$APP/Contents/PkgInfo"

# Apphost must be executable or double-click silently does nothing.
chmod +x "$APP/Contents/MacOS/SphereIQ"

# Native libs need the exec bit too, otherwise dlopen fails at startup.
find "$APP/Contents/MacOS" -name '*.dylib' -exec chmod +x {} \;

echo "==> Stripping extended attributes (quarantine etc.)"
xattr -cr "$APP" 2>/dev/null || true

echo
echo "Built: $APP"
echo
echo "Run from Terminal to see logs live:"
echo "  \"$APP/Contents/MacOS/SphereIQ\""
echo
echo "Then read the crash log:"
echo "  cat ~/Library/Application\\ Support/SphereIQ/Logs/sphereiq.log"
