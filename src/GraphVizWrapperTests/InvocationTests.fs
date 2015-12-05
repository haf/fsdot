module InvocationTests

open System.IO
open NUnit.Framework
open FsUnit
open GraphVizWrapper

[<AutoOpen>]
module __ =
   [<Literal>]
   let emptyGraph = "graph {}"
   [<Literal>]
   let emptyDigraph = "digraph {}"
   [<Literal>]
   let oneNodeGraph = "graph { a; }"
   [<Literal>]
   let oneNodeDigraph = "digraph { a; }"

[<TestFixture>]
type InvocationTests() =

   // Invalid input:

   [<Test>]
   member __.``Invoking the dot command with empty input causes an 'Empty input' error``() =
      let expected = CommandResult.Failure "Empty input"
      let actual = GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Svg, "")
      actual |> should equal expected

   [<Test>]
   member __.``Invoking the dot command with null input causes an 'Null input' error``() =
      let expected = CommandResult.Failure "Null input"
      let actual = GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Svg, null)
      actual |> should equal expected

   [<Test; Ignore("Hangs when reading stderror")>]
   member __.``Invoking the dot command with invalid input causes a 'Invalid input file content' error``() =
      let expected = "Invalid input content: "
      let actual = 
         match GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Svg, "invalid") with
         | SuccessText _ -> ""
         | SuccessBinary _ -> ""
         | Failure message -> message
      actual |> should startWith expected

   // Dot for svg:

   [<TestCase(emptyGraph)>]
   [<TestCase(emptyDigraph)>]
   member __.``Invoking the dot for svg command with a file containing a valid but empty graph generates an SVG file representing a blank page``(dotContent) =
      let expected = File.ReadAllText(@"..\..\TestFiles\valid-dot-no-content.svg")
      let actual = 
         match GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Svg, dotContent) with
         | SuccessText content -> content
         | SuccessBinary _ -> ""
         | Failure _ -> ""
      actual |> should equal expected

   [<TestCase(oneNodeGraph)>]
   [<TestCase(oneNodeDigraph)>]
   member __.``Invoking the dot for svg command with a file containing a single node generates an SVG file representing that node``(dotContent) =
      let expected = File.ReadAllText(@"..\..\TestFiles\valid-dot-one-node.svg")
      let actual = 
         match GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Svg, dotContent) with
         | SuccessText content -> content
         | SuccessBinary _ -> ""
         | Failure _ -> ""
      actual |> should equal expected

   // Neato for svg:

   [<TestCase(emptyGraph)>]
   [<TestCase(emptyDigraph)>]
   member __.``Invoking the neato for svg command with a file containing a valid but empty graph generates an SVG file representing a blank page``(dotContent) =
      let expected = File.ReadAllText(@"..\..\TestFiles\valid-neato-no-content.svg")
      let actual = 
         match GraphVizWrapper.Invocation.Call(Algo.Neato, OutputType.Svg, dotContent) with
         | SuccessText content -> content
         | SuccessBinary _ -> ""
         | Failure _ -> ""
      actual |> should equal expected

   [<TestCase(oneNodeGraph)>]
   [<TestCase(oneNodeDigraph)>]
   member __.``Invoking the neato for svg command with a file containing a single node generates an SVG file representing that node``(dotContent) =
      let expected = File.ReadAllText(@"..\..\TestFiles\valid-neato-one-node.svg")
      let actual = 
         match GraphVizWrapper.Invocation.Call(Algo.Neato, OutputType.Svg, dotContent) with
         | SuccessText content -> content
         | SuccessBinary _ -> ""
         | Failure _ -> ""
      actual |> should equal expected

   // Dot for gif:

   [<TestCase(emptyGraph)>]
   [<TestCase(emptyDigraph)>]
   member __.``Invoking the dot for gif command with a file containing a valid but empty graph generates a GIF file representing a blank page``(dotContent) =
      let expected = File.ReadAllBytes(@"..\..\TestFiles\valid-dot-no-content.gif")
      let actual = 
         match GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Gif, dotContent) with
         | SuccessText _ -> [||]
         | SuccessBinary content -> content
         | Failure _ -> [||]
      actual |> should equal expected

   [<TestCase(oneNodeGraph)>]
   [<TestCase(oneNodeDigraph)>]
   member __.``Invoking the dot for gif command with a file containing a single node generates a GIF file representing that node``(dotContent) =
      let expected = File.ReadAllBytes(@"..\..\TestFiles\valid-dot-one-node.gif")
      let actual = 
         match GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Gif, dotContent) with
         | SuccessText _ -> [||]
         | SuccessBinary content -> content
         | Failure _ -> [||]
      actual |> should equal expected

   // Dot for png:

   [<TestCase(emptyGraph)>]
   [<TestCase(emptyDigraph)>]
   member __.``Invoking the dot for png command with a file containing a valid but empty graph generates a PNG file representing a blank page``(dotContent) =
      let expected = File.ReadAllBytes(@"..\..\TestFiles\valid-dot-no-content.png")
      let actual = 
         match GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Png, dotContent) with
         | SuccessText _ -> [||]
         | SuccessBinary content -> content
         | Failure _ -> [||]
      actual |> should equal expected

   [<TestCase(oneNodeGraph)>]
   [<TestCase(oneNodeDigraph)>]
   member __.``Invoking the dot for png command with a file containing a single node generates a PNG file representing that node``(dotContent) =
      let expected = File.ReadAllBytes(@"..\..\TestFiles\valid-dot-one-node.png")
      let actual = 
         match GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Png, dotContent) with
         | SuccessText _ -> [||]
         | SuccessBinary content -> content
         | Failure _ -> [||]
      actual |> should equal expected

   // Dot for jpg:

   [<TestCase(emptyGraph)>]
   [<TestCase(emptyDigraph)>]
   member __.``Invoking the dot for jpg command with a file containing a valid but empty graph generates a JPG file representing a blank page``(dotContent) =
      let expected = File.ReadAllBytes(@"..\..\TestFiles\valid-dot-no-content.jpg")
      let actual = 
         match GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Jpg, dotContent) with
         | SuccessText _ -> [||]
         | SuccessBinary content -> content
         | Failure _ -> [||]
      //File.WriteAllBytes(@"C:\temp\valid-dot-no-content.jpg", actual)
      actual |> should equal expected

   [<TestCase(oneNodeGraph)>]
   [<TestCase(oneNodeDigraph)>]
   member __.``Invoking the dot for jpg command with a file containing a single node generates a JPG file representing that node``(dotContent) =
      let expected = File.ReadAllBytes(@"..\..\TestFiles\valid-dot-one-node.jpg")
      let actual = 
         match GraphVizWrapper.Invocation.Call(Algo.Dot, OutputType.Jpg, dotContent) with
         | SuccessText _ -> [||]
         | SuccessBinary content -> content
         | Failure _ -> [||]
      //File.WriteAllBytes(@"C:\temp\valid-dot-one-node.jpg", actual)
      actual |> should equal expected