open Farmer
open Farmer.Builders
open System

// Create ARM resources here e.g. webApp { } or storageAccount { } etc.
// See https://compositionalit.github.io/farmer/api-overview/resources/ for more details.

// Add resources to the ARM deployment using the add_resource keyword.
// See https://compositionalit.github.io/farmer/api-overview/resources/arm/ for more details.
let deployment = arm {
    location Location.WestEurope
}

module Config =
    let private getEnv name =
        match Environment.GetEnvironmentVariable name with
        | null -> None
        | name -> Some name
    let resourceGroupName =
        getEnv "RESOURCE_GROUP_NAME" |> Option.defaultValue "farmer-ci-deploy"

let response =
    deployment
    |> Deploy.tryExecute Config.resourceGroupName Deploy.NoParameters
    |> function
    | Ok outputs -> sprintf "Success! Outputs: %A" outputs
    | Error error -> sprintf "Rejected! %A" error

printfn "Deployment finished with result: %s" response 