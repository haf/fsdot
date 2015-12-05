namespace Archie

type Name = string

type ConnectionKind =
| InProcess
| Http
| AzureServiceBus
| Sql

type ComponentKind =
| Client
| Store
| Processor
| UserInterface
| Queue

type ActivityKind =
| Command
| Query
| CommandQuery

type Activity =
    {
        Name : Name
        Kind : ActivityKind
        ConnectionKind : ConnectionKind
    }
    with
        static member New(name : Name, kind : ActivityKind, connectionKind : ConnectionKind) =
            { Name = name; Kind = kind; ConnectionKind = connectionKind }

type Ordinality =
| Singleton
| Multiple
| HorizontallyScaled

type Component private (name : Name, kind : ComponentKind, ordinality : Ordinality, provided : Activity list, used : Activity list) =
    new (name : Name, kind : ComponentKind, ordinality : Ordinality) =
        Component(name, kind, ordinality, [], [])
    member __.Name = name
    member __.Kind = kind
    member __.Ordinality = ordinality
    member __.Provided = provided
    member __.Used = used
    member this.Provides(activity : Activity) =
        Component(this.Name, this.Kind, this.Ordinality, activity::this.Provided, this.Used)
    member this.Uses(activity : Activity) =
        Component(this.Name, this.Kind, this.Ordinality, this.Provided, activity::this.Used)

type Architecture =
    {
        Name : Name
        Components : Component list
    }

module Find =
    /// Find the components which provide the given activity across the
    /// given architecture
    let Providers(activity : Activity) (architecture : Architecture) =
        architecture.Components
        |> Seq.filter (fun comp -> 
            (comp.Provided |> Set.ofList).Contains activity
        )

    /// Find all the linkages between components in the architecture
    let Edges (architecture : Architecture) =
        seq {
            for user in architecture.Components do
                for ua in user.Used do
                    for provider in (Providers ua architecture) do
                        yield user, ua, provider
        }

    let AllProvided (architecture : Architecture) =
        seq {
            for provider in architecture.Components do
                for pa in provider.Provided do
                    yield pa
        } |> Set.ofSeq

    let AllUsed (architecture : Architecture) =
        seq {
            for user in architecture.Components do
                for ua in user.Used do
                    yield ua
        } |> Set.ofSeq
        
    let Validate (architecture : Architecture) =
        let p = architecture |> AllProvided
        let u = architecture |> AllUsed
        printf "Provided but not used:"
        for activity in p - u do
            printfn "%A" activity
        printf "Used but not provided:"
        for activity in u - p do
            printfn "%A" activity
        
