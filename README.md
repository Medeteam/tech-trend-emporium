# tech-trend-emporium
E-Commerce App using FAKESTORE API

## Actions' pipeline status

**Main branch status**

![main branch status](https://github.com/Medeteam/tech-trend-emporium/actions/workflows/pipeline.yml/badge.svg)

## Rulest
This repository uses the trunk-based strategy, so a Pull request is required with 2 aprovals before any merge, no force pushes allowed.

## Clone project
For cloning the project, use `git clone` command. Then in the local repository execute the instruction:

```dotnet restore ./emporium/emporium.sln```

> **Note:** If you don't have the emporium.sln file in that route, change the route in the command before runing it.

## Pipeline CI/CD
The pipeline contains integration with SonarCloud to scan the code in order to check the code coverage for tests.
