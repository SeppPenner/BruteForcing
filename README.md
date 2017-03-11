# Bruteforcing
Bruteforcing implementation in C#
This project was written and tested in .Net 4.6.2.

[![Build status](https://ci.appveyor.com/api/projects/status/gyev94dkfeka6csq?svg=true)](https://ci.appveyor.com/project/SeppPenner/bruteforcing-8op89)


## Basic usage:
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

Change history
--------------

* **Version 1.0.0.1 (2017-03-11)** : Switched to .Net 4.6.2. Reformatted code.
* **Version 1.0.0.0 (2016-12-03)** : Added basic usage to Readme.
* **Version 1.0.0.0 (2016-11-17)** : 1.0 release.