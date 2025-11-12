#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"

export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$DOTNET_ROOT:$PATH"

if ! command -v dotnet >/dev/null 2>&1; then
  echo "Installing .NET SDK 10.0 locally to $DOTNET_ROOT ..."
  curl -sSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
  chmod +x /tmp/dotnet-install.sh
  /tmp/dotnet-install.sh --channel 10.0 --install-dir "$DOTNET_ROOT"
fi

echo "Restoring..."
dotnet restore "$ROOT_DIR/Rc41.sln"

echo "Building Release..."
dotnet build "$ROOT_DIR/Rc41.sln" -c Release

echo "Publishing Windows x64 self-contained..."
mkdir -p "$ROOT_DIR/Dist/win-x64"
dotnet publish "$ROOT_DIR/Rc41/Rc41.csproj" -c Release -r win-x64 --self-contained true \
  -p:PublishSingleFile=true -p:PublishTrimmed=true -p:EnableCompressionInSingleFile=true -o "$ROOT_DIR/Dist/win-x64"

echo "Done. Output: $ROOT_DIR/Dist/win-x64/Rc41.exe"
