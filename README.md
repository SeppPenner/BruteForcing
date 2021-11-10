Bruteforcing
====================================

Bruteforcing implementation in C#.
This project was written and tested in .Net 6.0.

[![Build status](https://ci.appveyor.com/api/projects/status/gyev94dkfeka6csq?svg=true)](https://ci.appveyor.com/project/SeppPenner/bruteforcing-8op89)
[![GitHub issues](https://img.shields.io/github/issues/SeppPenner/Bruteforcing.svg)](https://github.com/SeppPenner/Bruteforcing/issues)
[![GitHub forks](https://img.shields.io/github/forks/SeppPenner/Bruteforcing.svg)](https://github.com/SeppPenner/Bruteforcing/network)
[![GitHub stars](https://img.shields.io/github/stars/SeppPenner/Bruteforcing.svg)](https://github.com/SeppPenner/Bruteforcing/stargazers)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://raw.githubusercontent.com/SeppPenner/Bruteforcing/master/License.txt)
[![Nuget](https://img.shields.io/badge/Bruteforcing-Nuget-brightgreen.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.Bruteforcing/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/HaemmerElectronics.SeppPenner.Bruteforcing.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.Bruteforcing/)
[![Known Vulnerabilities](https://snyk.io/test/github/SeppPenner/Bruteforcing/badge.svg)](https://snyk.io/test/github/SeppPenner/Bruteforcing)
[![Gitter](https://badges.gitter.im/CSharpBruteforcing/community.svg)](https://gitter.im/CSharpBruteforcing/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

## Available for
* NetFramework 4.6
* NetFramework 4.6.2
* NetFramework 4.7
* NetFramework 4.7.2
* NetFramework 4.8
* NetStandard 2.0
* NetStandard 2.1
* NetCore 3.1
* Net 5.0
* Net 6.0

## Net Core and Net Framework latest and LTS versions
* https://dotnet.microsoft.com/download/dotnet-framework
* https://dotnet.microsoft.com/download/dotnet-core
* https://dotnet.microsoft.com/download/dotnet

## Basic usage
```csharp
public void Bruteforce()
{
    IBruteforcing bruteforce = new Bruteforcing("abcdefghijklmnopqrstuvwxyz0123456789", 1, 3);

    foreach (string result in bruteforce)
    {
        var text = result;
    }
}
```

The project can be found on [nuget](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.Bruteforcing/).

## Install

```bash
dotnet add package HaemmerElectronics.SeppPenner.Bruteforcing
```

Change history
--------------

See the [Changelog](https://github.com/SeppPenner/Bruteforcing/blob/master/Changelog.md).