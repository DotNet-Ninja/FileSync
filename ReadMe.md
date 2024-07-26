# FileSync

FileSync is a simple commandline utility that can sync files between two directories, typically on different devices.  

The primary use case is to set up a scheduled task to sync files to an external hard fisk giving you a simple, raw file copy type backup.

Need to back up multiple directories?  No problem, simply add an additional sync target for each source directory.  When a `filesync run` command is executed it runs a sync on each sync target sequentially.


## Installation

Install FileSync in 3 easy steps.

### Clone Repository

```shell
git clone https://github.com/DotNet-Ninja/FileSync.git
```

### Build & Install

From root of repository run:
```shell
.\build\install.ps1
```

### Set Up Path

Add path {USERPROFILE}\\.filesync\bin to your path.

## Usage

### Add Sync Target

This command will add a new sync target to the configuration file.

```
filesync add-target -n {NameOfTarget} -s {SourceDirectoryPath} -d {DestinationDirectoryPath}
```

### Remove Sync Target

This command will remove a sync target from the configuration file.

```
filesync remove-target -n {NameOfTarget}
```

### Update Sync Target

This command will update the values for an existing sync target.

```
filesync add-target -n {NameOfTarget} -s {SourceDirectoryPath} -d {DestinationDirectoryPath} -c {NewTargetName}
```
> Note: For Updates to targets only the name is a required parameter.  Any other parameters not supplied will not be updated.

### List Sync Targets

This command will list all targets that have been added to the configuration file.

```
filesync list-targets
```

### Run Sync Operation

This command will run a sync operation on each existing target.

```
filesync run
```