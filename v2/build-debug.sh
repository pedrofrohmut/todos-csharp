#! /usr/bin/env bash

dotnet build -c Debug -p:NoWarn=CS1998

if ! [[ $? -eq 0 ]]; then
    echo "[ERROR] Build failed. Exiting..."
    exit 1
fi

if [[ $* == *--run* ]]; then
    dotnet run --project ./src/WebApi/WebApi.csproj
fi
