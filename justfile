#!/usr/bin/env just --justfile


serve:
  docfx docfx/docfx.json --serve

docs:
    docfx docfx/docfx.json

install:
  dotnet tool update -g docfx
  
gen:
  docfx init --quiet