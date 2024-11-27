#!/bin/bash

# Install .NET SDK and Runtime for .NET 8.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0

# Add .NET to the PATH
export PATH="$HOME/.dotnet:$PATH"

# Verify installation
dotnet --version

# Publish the application
dotnet publish -c Release -o ./publish

# List contents of the publish folder to verify the output
echo "Contents of publish folder:"
ls -al ./publish

# Start the application
dotnet ./publish/YourApp.dll
