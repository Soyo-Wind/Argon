#!/bin/bash
dotnet run > log.txt
echo "dotnet ok"
g++ main.cpp
echo "g++ ok"