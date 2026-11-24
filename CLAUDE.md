# Project rules for Claude

## What this is

Bruteforcing is a small .NET class library that enumerates every string over a given character set
whose length lies between a minimum and a maximum. It is a plain generator, it does not check
passwords or talk to anything, the caller decides what to do with each produced string. It is
published on NuGet as
[HaemmerElectronics.SeppPenner.Bruteforcing](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.Bruteforcing/),
so every public API change is a breaking change for consumers.

One solution `src/Bruteforcing.sln` with exactly two projects:

- `src/Bruteforcing/Bruteforcing.csproj`, `OutputType` `Library`, the actual code. Assembly and
  root namespace `Bruteforcing`, package id `HaemmerElectronics.SeppPenner.Bruteforcing`. This is
  the only project that produces the package.
- `src/Bruteforcing.Tests/Bruteforcing.Tests.csproj`, MSTest, added in version 1.0.14.0,
  `IsPackable` `false`.

There is no example or demo project, the Readme sample is the only usage documentation.

Layout inside `src/Bruteforcing` (flat, no subfolders):

- `Bruteforcing.cs`: the public class `Bruteforcing`, which implements both `IBruteforcing` and
  `IEnumerable`. The constructor takes `charset`, `minimum` and `maximum`, exposes them as the
  properties `Charset`, `Minimum` and `Maximum`, and yields the combinations from `GetEnumerator`.
  The private helper `Factoradic` builds one string, the private fields `length` and `stringBuilder`
  are its scratch state.
- `IBruteforcing.cs`: the public contract, a single `GetEnumerator` method. Consumers program
  against it (the Readme sample types the variable as `IBruteforcing`).
- `GlobalUsings.cs`: all usings of the project, inside the `#pragma warning disable IDE0065` block.

Layout inside `src/Bruteforcing.Tests`:

- `BruteforcingTests.cs`: enumeration behaviour. Full coverage of a small range, distinct results,
  every result inside the length range, every character from the charset, the single character
  case, the property round trips and the empty range. The `Enumerate` helper uses the same
  `foreach (string result in bruteforce)` loop as the Readme.
- `GlobalUsings.cs`: all usings of the test project.

Repository root: `README.md` (the only user documentation, with the usage sample and the badges),
`Changelog.md`, `Updating.md` (five short release lines, the complete process is under
`## Releasing` below), `License.txt` (MIT), `Bruteforcing.png` (the package icon),
`BuildAndPushPackage.bat`, `Delete-BIN-OBJ-Folders.bat`, `.editorconfig` (under `src`),
`.gitattributes` and `.gitignore`.

## Build

```powershell
dotnet build src/Bruteforcing.sln
```

```powershell
dotnet test src/Bruteforcing.sln
```

- The library multi targets `net10.0;net11.0`, so building needs the .NET 11 SDK. The test project
  targets `net11.0` only, keep it on the newest framework the library targets. Nothing in the code
  is Windows specific, but the helper batch files are.
- `GeneratePackageOnBuild` is on in the library, so **every** build packs: a `.nupkg` and a
  `.snupkg` appear in `src/Bruteforcing/bin/<configuration>`, in Debug too. The version comes from
  GitVersion, for example `1.0.14-1` for the first commit after tag `1.0.13`. The test project has
  `IsPackable` `false` and produces no package.
- `TreatWarningsAsErrors` is enabled in both projects, so every warning breaks the build, NuGet
  warnings (`NU****`) from restore and pack included. A clean build reports zero warnings, keep it
  that way.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list. `NuGetAudit` and `NuGetAuditMode=all` are on in the test
  project, so a vulnerable transitive package fails the build too.
- All build properties live directly in the two `.csproj` files. There is **no**
  `Directory.Build.props` in this repository, unlike some sibling repositories.
- Versions come from GitVersion.MsBuild out of the git tags, never edit a version property or an
  assembly version by hand.
- Restore needs nuget.org. If a private feed is configured globally on the machine and answers 404
  for public packages, restore fails with `NU1301`. Then build with an explicit source:
  `dotnet build src/Bruteforcing.sln --source https://api.nuget.org/v3/index.json`.
- Tests are MSTest, in the single test project `src/Bruteforcing.Tests`, with the package set the
  sibling repositories use: `Microsoft.NET.Test.Sdk`, `MSTest.TestAdapter`, `MSTest.TestFramework`,
  `coverlet.collector` and `GitVersion.MsBuild`. `dotnet test` runs 9 tests, they need no network
  and no fixture outside the repository. Never claim a test run happened without running it.

## Code conventions

Follow the surrounding code, it is consistent throughout every file:

- File header comment block with `<copyright file="..." company="Hämmer Electronics">` and a
  `<summary>`, then the file-scoped namespace.
- XML doc comments on every type and every member, private members included, no exceptions.
  Implementations of an interface member additionally carry `<inheritdoc cref="..."/>` and
  `<seealso cref="..."/>` pointing at that interface.
- `Nullable`, `ImplicitUsings` and `LangVersion latest` are enabled.
- New `using` directives go into the `GlobalUsings.cs` of the respective project, inside the
  existing `#pragma warning disable IDE0065` block, never at the top of a file. The editorconfig
  requires usings inside the namespace (`csharp_using_directive_placement=inside_namespace:warning`),
  which global usings cannot satisfy, that is what the pragma is for. Do not add other pragmas. The
  comment text in that block is German because Visual Studio generated it, leave it alone.
- Fields, properties, methods and events are always accessed with `this.` qualification
  (`dotnet_style_qualification_for_*` at severity `warning`).
- `src/.editorconfig` also enforces braces everywhere, no multiple blank lines, four spaces, CRLF,
  UTF-8, file scoped namespaces, `System` usings sorted first and `IDE0005` as warning. Analyzer
  warnings are fixed, not silenced.

## Known quirks

Do not silently "clean up" these, they are existing behaviour:

- **The namespace is spelled `BruteForcing`, everything else `Bruteforcing`.** The code namespace
  is `BruteForcing` with a capital F, while the assembly name, root namespace, package id, project
  folder and solution use the lower case `Bruteforcing`. Consumers therefore write
  `using BruteForcing;`. Changing the namespace is a breaking change for every consumer, leave it.
  The test project deliberately uses the namespace `BruteForcing.Tests` so that the `Bruteforcing`
  class sits in an enclosing namespace and needs no `using`, and so that the identifier
  `Bruteforcing` resolves to the class and not to a namespace.
- **Both `IBruteforcing` and `IEnumerable` declare `GetEnumerator`.** One public `GetEnumerator`
  method on the class satisfies both. `foreach` uses it through the enumerator pattern.
- **The enumerator reuses instance state.** `GetEnumerator` is a `yield` iterator that writes into
  the shared private `length` and `stringBuilder` fields. A single `foreach`, which is the Readme
  usage, is fine. Enumerating the same instance from two interleaved loops at once would corrupt the
  shared `StringBuilder`. This is existing behaviour, not a bug to fix.
- **Little-endian result order.** The first character varies fastest, so charset `"ab"` over the
  range one to two yields `a, b, aa, ba, ab, bb`. The produced set is complete and free of
  duplicates, only the order looks unusual.
- **`Math.Pow` counts the combinations.** The total per length is `(ulong)Math.Pow(charset.Length,
  length)`. This is a `double` cast to `ulong`, very large charset and length combinations lose
  precision or overflow. This is an example library, that case is not guarded.
- **Edge inputs are silent, not exceptions.** A minimum larger than the maximum yields nothing, a
  minimum of zero yields one empty string, an empty charset yields nothing (the zero total keeps the
  `% length` in `Factoradic` from ever running, so there is no division by zero).
- **AppVeyor badge without CI in the repository.** `README.md` links an AppVeyor build that is
  configured outside of this repository. There is no `.github` folder and no pipeline file here.
- **`src/Bruteforcing.sln.DotSettings`** is tracked and holds nothing but a ReSharper user
  dictionary (`Bruteforcing`, `factoradic`, `H_00E4mmer`). Leave it alone.
- **`.gitattributes` sets `* text=auto`**, every rule of the Visual Studio template below it is
  commented out. There is no binary fixture in this repository, `Bruteforcing.png` is treated as
  binary by git's own heuristic. Any future binary file that must not be normalized needs its own
  rule.

## Releasing

**Every new version is released completely.** Changelog, README, nuget.org, GitHub Packages and a
GitHub release belong to every version, none of these steps is optional and none of them waits for
a separate request.

1. Make the change.
2. Update `PackageReleaseNotes` in `src/Bruteforcing/Bruteforcing.csproj` to the new version line,
   `Version 1.0.14.0 (2026-08-11): Short description.` It is the only place in the build files that
   names a version, and it is duplicated from the changelog by hand.
3. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.14.0 (2026-08-11)** : Short description.`
4. Update `README.md` for every version. The `## Available for` list has to match the target
   frameworks, the usage sample has to compile against the current API, and nothing there may
   describe a state the package has left behind.
5. Commit those.
6. Tag the commit with the plain version number, no `v` prefix (`1.0.13`, `1.0.12`, ...). The
   existing tags are lightweight tags, create new ones the same way.
7. Push the commits and the tag.
8. Only then build the package, and build it while the tag is the newest commit. GitVersion takes
   the version from the tag plus the distance to it, so a build before the tag or after another
   commit produces a prerelease version like `1.0.15-3+Branch.master.Sha...`, and that version
   would end up on nuget.org. `git describe --tags` has to answer with the plain tag.
9. Publish to nuget.org with `BuildAndPushPackage.bat`, started from the repository root, its first
   line is `cd .\src`. It deletes every `bin` and `obj` below `src`, builds `-c Release` and pushes
   the `.nupkg` and the `.snupkg` to nuget.org with `%NUGET_API_KEY%` and to the NuGet source named
   `github` with `%GITHUB_API_KEY%`. It ends with `PAUSE`, so it wants a console. Call it with its
   full path: the Claude Code shell sets `NoDefaultCurrentDirectoryInExePath=1`, so cmd does not
   find a bare `BuildAndPushPackage.bat` in the current directory. Because it always rebuilds, it
   is only correct as long as no commit sits on top of the tag. If one does, push the package that
   was built from the tagged state instead, by its file name and not with the wildcard, because
   `GeneratePackageOnBuild` leaves a `.nupkg` of every intermediate build in `bin`:
   `dotnet nuget push HaemmerElectronics.SeppPenner.Bruteforcing.1.0.14.nupkg -s "nuget.org" --skip-duplicate -k %NUGET_API_KEY%`
   in `src/Bruteforcing/bin/Release`. That command takes the matching `.snupkg` with it. A push to
   nuget.org cannot be taken back, the version is burned even after unlisting. nuget.org needs a
   few minutes to validate a new version before it is listed.
10. Publish the same `.nupkg` to GitHub Packages,
    `https://nuget.pkg.github.com/SeppPenner/index.json`. The script only reaches it if a NuGet
    source named `github` is registered on the machine, `dotnet nuget list source` shows that. On
    2026-11-24 only nuget.org was registered here, so the two `github` lines of the script fail and
    the push has to be done by hand in `src/Bruteforcing/bin/Release`:
    `dotnet nuget push HaemmerElectronics.SeppPenner.Bruteforcing.1.0.14.nupkg -s https://nuget.pkg.github.com/SeppPenner/index.json --skip-duplicate --api-key <token>`.
    The API key is a GitHub token with the scope `write:packages`, for example from
    `gh auth token`. If `gh auth status` lists no `write:packages`, the user has to run
    `gh auth refresh -h github.com -s write:packages,read:packages`, it needs a browser login.
    GitHub Packages takes no symbol packages, push the `.nupkg` only.
11. Create the GitHub release:
    `gh release create 1.0.14 -R SeppPenner/BruteForcing --verify-tag --latest --title "Version 1.0.14.0 (2026-08-11)" --notes-file <file>`.
    The notes are the changelog sentence plus the line
    `NuGet: https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.Bruteforcing/1.0.14`. Write
    credits as `` `@name` `` in backticks, a bare `@name` notifies that person again.

The version in the `Changelog.md` and in `PackageReleaseNotes` has four parts (`1.0.14.0`), the tag
has three (`1.0.14`). GitVersion turns the tag into the assembly and package version, so an untagged
commit produces something like `1.0.14-1+Branch.master.Sha...`.

nuget.org and GitHub Packages hold 0.1.0, which has no tag and therefore no GitHub release, and
every tagged version from 1.0.2 to 1.0.15. Every tag has a GitHub release. The missing versions on GitHub Packages and the releases were added on 2026-11-24.

## Git

- Include the full GitHub pull request or issue URL in the body of each related commit on a `Refs:`
  line. Use the [GitHub issue skill](.claude/skills/github-issue/SKILL.md) for issue work and closeout.
- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed.
- Code comments and comments in project files such as `.csproj` are **always English**, regardless
  of the language used in the conversation.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies) always use real umlauts and ß, never ASCII
  transliterations such as `ae`, `oe`, `ue` or `ss`. Identifiers, file names and configuration keys
  stay unchanged where umlauts are technically undesirable.
