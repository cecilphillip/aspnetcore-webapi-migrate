This repository contains sample projects that show the same API built with both ASP .NET WebAPI 2 and ASP .NET Core. 


Getting Setup
-------------

####ASP .NET Core RC 2
To get the latest tools for developing .NET core apps (which includes ASP .NET Core) head over to [dot.net](https://dot.net). There you can select the version of the .NET Core SDK that
works for your environment. For Visual Studio users, you'll get the both VS tools and as well as the dotnet CLI added to your path.


####ASP .NET Web API 2
If you have the Visual Studio 2015, you already haev all that you need for building Web API 2 applications.

Covered in the samples
-------------

* Moving from Web API Controllers to MVC Core Controllers
* Routing templates and Attribute Routing
* Setting up CORS
* Adding Swagger documentation
* Moving from Delegating Handlers to Middleware
* Wiring up basic Model Validation attributes
* Setting up Dependecy Injection


Upgrading to RC2
--------------
There were a few minor tasks for the RC2 upgrade.

* The [logging levels](https://docs.asp.net/en/latest/migration/rc1-to-rc2.html#logging) have slightly changed between RC1 and RC2. Verbose is no longer a supported level.
* The framework namespaces were updated from Microsoft.AspNet.* to Microsoft.AspNetCore.*
* The frameworks section in project.json was updated to use the new [.NET Standard](https://github.com/dotnet/corefx/blob/master/Documentation/architecture/net-platform-standard.md) TFMs
* In the launchSettings.json, the Hosting:Environment environment variable was changed to ASPNETCORE_ENVIRONMENT.
* web.config no longer lives in the wwwroot folder.
* The IIS and Kestrel middleware have been moved to the Main entry point.
* DNX build targets were removed from xproj
* The Swashbuckle configuration API has slightly changed. Also note the package versions changed from 6.0.0-rc1-final to 6.0.0-beta9. 

Now you should be all set. Happy Coding

