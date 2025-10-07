#!/usr/bin/env python3

import sys
import subprocess


def print_help():
    print("[Python CMD] Available commands are: build and devrun")


def main():
    inputs = sys.argv

    if len(inputs) < 2 :
        print("[Python CMD] No commands provided")
        print_help()
        sys.exit()

    command = inputs[1]

    try:
        if (command == "help"):
            print_help()

        elif (command == "build"):
            print("[Python CMD]  Project is building.")
            subprocess.run(["dotnet", "build", "Todos.sln"])

        elif (command == "devrun"):
            print("[Python CMD]  Project is running on Development ENV.")
            subprocess.run(["dotnet", "run", "--project", "src/api/Api.csproj", "--verbosity", "m"])

        # elif (command == "test"):
        #     print("[Python CMD]  Project is now being tested.")
        #     subprocess.run(["dotnet", "test", "src/Tests"])

        else:
            print("[Python CMD] Sorry. Didn't Match Anything.")
            print_help()

    except KeyboardInterrupt:
        print("[Python CMD] Program interrupted.")


main()
