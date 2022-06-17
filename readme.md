# Woven

Woven is a dotnet tool that you can install to perform the following tasks:

- Pull a complete list of repositories into a folder - the branch/tag can be also be specified.
- Perform a dotnet build process on each repository.

This is a perfect way to grab an entire workspace of repositories locally, perform the build process in a particular order and generate the output you need.

# Installation

```
dotnet tool install --global woven --version 1.0.1
```

# Uninstallation

```
dotnet tool uninstall woven --global
```

# Usage

## Scenario 1: Running the tool from a folder that has a weave file:

```
woven -o %PATH%
```

Substituting the %PATH% with your own.

## Scenario 2: Running the tool on a specified weave file:

```
woven -i c:\dev\weave.xml -o %PATH%
```

Substituting the %PATH% with your own.

# The weave file

The weave file (weave.xml) should contain something like the following:

```
<?xml version="1.0" encoding="utf-8"?>
<project>
	<repository>
		<name>MY_REPO</name>
		<url>https://github.com/MY_REPO</url>
        <branch>branchname</branch>
	</repository>
</project>
```

- The branch element is optional and will pull the default branch.
- Repeat the repository section for each repository you want to pull down.
