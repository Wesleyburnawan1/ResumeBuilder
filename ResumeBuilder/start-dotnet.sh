#!/bin/bash

# Install .NET SDK and Runtime for .NET 8.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0

# Add .NET to the PATH
export PATH="$HOME/.dotnet:$PATH"

# Start the application
dotnet ./publish/ResumeBuilder.dll
