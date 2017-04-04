#r "packages/fake/tools/FakeLib.dll"
#r "System.Management.Automation"

open Fake
open Fake.Testing.NUnit3
open System
open System.Management.Automation

let logo = "FluentWindsor: "
let solution = "fluentwindsor.sln"
let projectFiles = "src/**/*.csproj"

Target "Build" <| fun _ ->
    tracef "%sC# Build\r\n" logo
    let setParams defaults =
            { defaults with
                Verbosity = Some(Quiet)
                Targets = ["Build"]
                Properties =
                    [
                        "Optimize", "True"
                        "DebugSymbols", "False"
                        "Configuration", "Debug"
                    ]
             }
    build setParams solution
          |> DoNothing

RunTargetOrDefault "Build"
