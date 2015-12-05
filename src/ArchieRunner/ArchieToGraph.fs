module ArchieToGraph

open System.IO
open Archie
open GraphVizWrapper

let ComponentKindToShape = function
| Client -> GraphNode.Shape.Oval
| Store -> GraphNode.Shape.Folder
| Processor -> GraphNode.Shape.Box 
| UserInterface -> GraphNode.Shape.MSquare
| Queue -> GraphNode.Shape.Cds

let ArchitectureToGraph (architecture : Architecture) : CommandResult =

    let graph =
        Graph(Id "id", Strictness.NonStrict, GraphKind.Digraph)

    let nodes =
        architecture.Components
        |> Seq.map (fun c -> GraphNode.GraphNode(Id c.Name, Shape = ComponentKindToShape c.Kind))
        |> Seq.map NodeStatement

    let graph =
        (graph, nodes) ||>
        Seq.fold (fun g ns -> g.WithStatement(ns))

    let edges =
        architecture
        |> Find.Edges
        |> Seq.map (fun (user, activity, provider) ->
            let n1 = GraphNode.GraphNode(Id user.Name)
            let n2 = GraphNode.GraphNode(Id provider.Name)
            EdgeStatement(activity.Name, n1, n2, Directionality.Directed)
        )

    let graph =
        (graph, edges) ||>
        Seq.fold (fun g es -> g.WithStatement(es))

    // TODO somehow make sure all attributes copied on With...
    // but for now set this after adding statements
    //graph.Size <- Size.DesiredHW(5., 5.) |> Some
    //graph.Size <- Size.MaxHW(6.,6.) |> Some
    //graph.Page <- Page(7., 7.) |> Some
    graph.FontSize <- FontSize(14.)
    //graph.Splines <- Splines.Line |> Some
    graph.Overlap <- Overlap.False |> Some
    // graph.Mode <- Mode.Hier
    //graph.FontName <- "ARIALUNI.TTF"
    //graph.BgColor <- GraphColor.SingleColor(System.Drawing.Color.AliceBlue) |> Some
    let temp = graph.ToString()
    GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Png, graph.ToString())

let MakeTempFile extension = 
    let tempFileName = Path.GetTempFileName()
    let tempFileSvg = Path.ChangeExtension(tempFileName, extension)
    File.Move(tempFileName, tempFileSvg)
    tempFileSvg

let private GraphToFile(commandResult : CommandResult) =
    match commandResult with
    | CommandResult.SuccessText t ->
        let tempFileSvg = MakeTempFile "svg"
        File.WriteAllText(tempFileSvg, t)
        tempFileSvg
    | CommandResult.SuccessBinary b ->
        let tempFileJpg = MakeTempFile "jpg"
        File.WriteAllBytes(tempFileJpg, b)
        tempFileJpg
    | CommandResult.Failure m ->
        failwithf "Error generating graph: %s" m

/// Use the system default view to view the file by
/// shelling using its filename as the argument.
let ViewGraph (commandResult : CommandResult) =
    let graphFileName = GraphToFile commandResult
    let si =
        new System.Diagnostics.ProcessStartInfo(graphFileName, "",
            UseShellExecute = true,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            RedirectStandardInput = false)
    use p = new System.Diagnostics.Process()
    p.StartInfo <- si
    p.Start() |> ignore