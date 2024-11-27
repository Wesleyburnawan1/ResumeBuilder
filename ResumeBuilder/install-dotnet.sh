#!/bin/bash

# Download and install .NET runtime
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 7.0

# Add .NET to the PATH
export PATH="$HOME/.dotnet:$PATH"

# Verify installation
dotnet --version

# Publish the application
dotnet publish -c Release -o ./publish
