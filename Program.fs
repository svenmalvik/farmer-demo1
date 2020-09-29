open Farmer
open Farmer.Builders
open System

let demoMaps = maps {
    name "demo_maps"
    sku Maps.S0
}

let deployment = arm {
    // Our location we set previously
    location Location.WestEurope
    add_resource demoMaps
}

module Config =
    // getEnv is only needed inside this module
    let private getEnv name =
        match Environment.GetEnvironmentVariable name with
        | null -> None
        | name -> Some name
    // Read rg from env or set default value
    let resourceGroupName =
        getEnv "RESOURCE_GROUP_NAME" |> Option.defaultValue "farmer-ci-deploy"

// Execute andf deploy
let response =
    deployment
    |> Deploy.tryExecute Config.resourceGroupName Deploy.NoParameters
    |> function
    | Ok outputs -> sprintf "Success! Outputs: %A" outputs
    | Error error -> sprintf "Rejected! %A" error

printfn "Deployment finished with result: %s" response
