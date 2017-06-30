Title: Cake Bootstrapper Script
---

If your using *Cake.Paket.Module* you should use the bootstrappers we discuss below. If your **only** using the addin *Cake.Paket* and NOT using the module, then you can use the cake teams default bootstrappers [build.ps1](https://github.com/cake-build/example/blob/master/build.ps1) and/or [build.sh](https://github.com/cake-build/example/blob/master/build.sh).

The Cake team states

> "The Cake Bootstrapper that you can get directly from cakebuild.net is intended as a starting point for what can be done. It is the developer's discretion to extend the bootstrapper to solve for your own requirements."

With this in mind, we created a cake bootstrapper script similar to the one provided by the cake team, but it uses paket instead of nuget.

- On Windows use PowerShell and run [build.ps1](https://github.com/larzw/Cake.Paket/blob/master/build.ps1). if it errors out due to an execution policy, take a look at [changing the execution policy](https://technet.microsoft.com/en-us/library/ee176961.aspx).
- On Linux or OS X use the terminal and run [build.sh](https://github.com/larzw/Cake.Paket/blob/master/build.sh). You may need to change the permissions `chmod +x build.sh`.
