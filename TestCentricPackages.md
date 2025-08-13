# Latest Versions of TestCentric Packages

Package Name                      | Latest<br>Release | Latest<br>Dev Release | Next<br>Release
--------------------------------- | :---------------: | :-------------------: | :-------------:
**TestCentric.Cake.Recipe**       | 1.3.3
**TestCentric.GuiRunner**         | 2.0.0-beta7       | 2.0.0-dev00680        | 2.0.0-beta8
**TestCentric.Engine**            | 2.0.0-beta7       | 2.0.0-dev00011        | 2.0.0-beta8
**TestCentric.Agent.Core**        | 2.1.1             | 2.2.0-dev00011        | 2.2.0
**TestCentric.Engine.Api**        | 2.0.0-beta7       | 2.0.0-dev00009        | 2.0.0-beta8
**TestCentric.Extensibility**     | 3.2.0             | 4.0.0-dev00004        | 4.0.0
**TestCentric.Extensibility.Api** |   "               |   "                   |   "
**TestCentric.Metadata**          | 3.0.4
**TestCentric.InternalTrace**     | 1.2.1
**Net80PluggableAgent**           | 2.5.3             | 2.5.4-dev00002        | 2.5.4
**Net70PluggableAgent**           | 2.5.1
**Net60PluggableAgent**           | 2.5.2             | 2.5.3-dev00004        | 2.5.3
**Net50PluggableAgent**           | 2.2.1             | 2.2.2-dev00001
**NetCore31PluggableAgent**       | 2.2.0             | 2.2.1-dev00003
**NetCore21PluggableAgent** (1)   | 2.2.0             | 2.2.1-dev00009
**Net462PluggableAgent**          | 2.5.2             | 2.6.0-dev00011        | 2.6.0
**Net40PluggableAgent**           | 1.1.0             | 1.1.1-dev00001
**Net20PluggableAgent**           | 2.2.0             | 2.2.1-dev00001

## Dependencies

| <br>Depends On                  | <br>GUI | <br>Engine | Dependent<br>Engine Api | Package<br>Agent Core | <br>Extensibility
--------------------------------- | :-----: | :--------: | :---------------------: | :-------------------: | :---------:
**TestCentric.Engine**            |    X    |            |                         |                       |
**TestCentric.Engine.Api**        |    X    |     X      |                         |         X             |
**TestCentric.Agent.Core**        |         |            |                         |                       |
**TestCentric.Extensibility**     |    X    |     X      |                         |         X             |
**TestCentric.Extensibility.Api** |         |            |          X              |                       |
**TestCentric.Metadata**          |         |     X      |                         |         X             |      X
**TestCentric.InternalTrace**     |    X    |     X      |          X              |         X             |      X

## Notes

1. The .NET Core 2.1 agent must be built and published manually from local machine with proper keys.
 
