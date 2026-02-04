# Development environment for .NET 9.0 application
FROM mcr.microsoft.com/dotnet/sdk:9.0

# Install basic development tools
RUN apt-get update && \
    apt-get install -y \
    git \
    curl \
    vim \
    nano \
    && rm -rf /var/lib/apt/lists/*

# Set working directory
WORKDIR /workspace

# Copy project files
COPY . /workspace/

# Restore NuGet packages for the solution
# This ensures all dependencies are available in the development environment
RUN dotnet restore FitnessTracker.sln || true

# Default to bash shell for development
CMD ["/bin/bash"]