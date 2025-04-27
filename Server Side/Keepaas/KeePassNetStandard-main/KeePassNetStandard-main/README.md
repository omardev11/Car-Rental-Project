# KeePassNetStandard

A port of the KeePassLib code of [KeePass](https://keepass.info) (v2.57) to .Net Standard.

Tags for previous versions of the port: v2.47, v2.48, v2.48.1, v2.49

#### Table of contents
[Why?](#h01)<br>
[GPL](#h02)<br>
[How to use it](#h03)<br>
[How it was ported, what's missing?](#h04)<br>
[Links](#h05)<br>
[Donation](#h06)<br>

## Why?
<a name="h01" />

I wanted to be able to read passwords from a KeePass database from a .Net Core app.
The author/maintainer of KeePass is not interested in moving his codebase in that direction.
I found [another port](https://github.com/Strangelovian/KeePass2Core) but it was of the code from a few years ago.

The alternative I've seen is to call a powershell script from C#, where the script does a command-line invocation of KeePass.exe.
I wanted someting less hacky than that.

## GPL
<a name="h02" />

The KeePass source code is [licensed under GPL v2](https://keepass.info/help/v2/license.html), so this code must be also.

Per [this FAQ](https://www.gnu.org/licenses/gpl-faq.html#InternalDistribution),
this source can be used for internal applications that will not be distributed.

And [this FAQ](https://www.gnu.org/licenses/gpl-faq.html#UnreleasedMods)
would apply to using the code on the backend of a web site.

I still would avoid putting any of this code into an internal source control, though.

## How to use it
<a name="h03" />

Get the source, build the KeePassNetStandard and KeePassLib project and use in your code.

If you just want to read passwords from a password database secured by a master password,
I recommend using the methods in KeePassNetStandard.KeePassUtilities:

```
using KeePassNetStandard;

static void Main()
{
    var db = KeePassUtilities.OpenPasswordDatabase(pathToKdbx, masterPassword);
    var somePassword = db.GetPassword("SomeGroupName", "TheEntryTitle", "TheUserName");
}
```

See the QuickTest project in the solution for more info.

Otherwise, the functionality of KeePassLib is available.

## How it was ported, what's missing?
<a name="h04" />

In general, the changes are minimal to make it easier to update as the main KeePass source changes.

I followed the same methods as Strangelovian in [KeePass2Core](https://github.com/Strangelovian/KeePass2Core).
All code changes can be found by searching for `NETSTANDARD2_0`.
Also see KeePassLib.csproj for files removed from compilation.

Some things have been `#if`-ed out, notably UI code down in the core library. 
(The sort of things that are okay in a one-man project, but wouldn't pass code review in a team setting.)

Note that the assembly in this project is not signed.

The code I added in KeePassNetStandard.KeePassUtilities has XML documentation to make it easier to use.

Please let me know if something is broken or if there are some other tests I should add to the QuickTest project.

## Links
<a name="h05" />

- Thanks to [KeePass](https://keepass.info) for making something useful enough that people want to use it beyond your vision.
- Thanks to Strangelovian for showing it was possible to port in [KeePass2Core](https://github.com/Strangelovian/KeePass2Core).

## Donation
<a name="h06" />

If this project helped you, you can help me :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donate_SM.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=XE5JR3FR458ZE&currency_code=USD)
