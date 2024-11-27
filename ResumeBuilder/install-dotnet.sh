#!/bin/bash

# Install .NET 8.0 SDK
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0

# Add .NET to the PATH
export PATH="$HOME/.dotnet:$PATH"

# Verify installation
dotnet --version

# Publish the application
dotnet publish -c Release -o ./publish
