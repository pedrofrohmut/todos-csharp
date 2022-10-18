#! /usr/bin/env bash

case "$1" in
    build) dotnet build ;;
    run) dotnet run --project src/api ;;
    wrun) dotnet watch run --project src/api ;;
    *)
        echo " Didn't match anything"
esac
