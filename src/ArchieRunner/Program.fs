open ArchieToGraph
open Archie.ResourceManager2

[<EntryPoint>]
let main argv = 
    resourceManager
    |> ArchitectureToGraph
    |> ViewGraph
    0 
