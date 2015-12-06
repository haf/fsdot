#if INTERACTIVE
#else
namespace FsDot
#endif

open System
open Microsoft.FSharp.Reflection
open System.Collections.Generic
open System.Drawing

// TODO eliminate duplication
[<AutoOpen>]
module Utils =
   let getUnionCaseName (x:'a) = 
      match FSharpValue.GetUnionFields(x, typeof<'a>) with
      | case, _ -> case.Name  
   let isNumber (s : string) =
      // Commas are allowed by Double.TryParse, so sabotage that:
      match Double.TryParse(s.Replace(',', 'x')) with
      | true, _ -> true
      | _ -> false
   let isBool s =
      s = "true" || s = "false"
   let quote s =
      sprintf "\"%s\"" s
   // TODO different from Graph one at the mo
   let dictToAttrList2 (dict : Dictionary<string, string>) =
      let pairs = 
         if dict.Count > 0 then
            dict
            |> Seq.map (fun kvp -> 
               let valueStr =
                  if kvp.Value |> isNumber || kvp.Value |> isBool then
                     kvp.Value
                  else
                     kvp.Value |> quote
               sprintf "%s=%s" kvp.Key valueStr)
            |> Array.ofSeq
         else
            [||]
      if pairs.Length > 0 then
         sprintf "[%s]" (String.Join(";", pairs))
      else
         ""

   let initials s =
      let chars =
        s
        |> Seq.filter (fun s -> s.ToString().ToUpperInvariant() = s.ToString())
        |> Array.ofSeq
      String(chars)

   let colorToString (c : Color) =
      if c.IsNamedColor then
         c.Name
      else
         sprintf "#%02x%02x%02x%02x" c.A c.R c.G c.B

// TODO needs unit tests

module GraphNode =
    // TODO many more shapes
  type Shape =
    | Box
    | Polygon
    | Ellipse
    | Oval
    | Circle
    | Point
    | Egg
    | Triangle
    | PlainText
    | Plain
    | Diamond
    | Trapezium
    | Parallelogram
    | House
    | Pentagon
    | Hexagon
    | Septagon
    | Octagon
    | DoubleCircle
    | DoubleOctagon
    | TripleOctagon
    | InvTriangle
    | InvTrapezium
    | InvHouse
    | MDiamond
    | MSquare
    | MCircle
    | Rect
    | Rectangle
    | Square
    | Star
    | None
    | Underline
    | Note
    | Tab
    | Folder
    | Box3d
    | Component
    | Promotor 
    | Cds
    | Terminator
    | Utr
    | PrimerSite
    | RestrictionSite
    | FivePOverhang
    | ThreePOverhang
    | NOverhang
    | Assembly
    | Signature
    | Insulator
    | RiboSite
    | RnaStab
    | LArrow
    | LPromotor
  with
    override this.ToString() =
      (getUnionCaseName this).ToLowerInvariant()

  type GraphNode(id : Id) =
    let defaultShape = Shape.Ellipse
    member __.Id = id
    member val Shape = defaultShape with get, set
    member private this.NodeAttributes =
      // TODO eliminate duplication with Graph
      let dict = Dictionary<string, string>()
      let addIf v dv name =
        if v <> dv then
          dict.[name] <- v.ToString()
      let addIfS v name =
        match v with
        | Some x -> dict.[name] <- x.ToString()
        | _ -> ()
      let addIfB v dv name =
        if v <> dv then
          dict.[name] <- v.ToString().ToLowerInvariant()
      addIf this.Shape defaultShape "shape"
      dict |> dictToAttrList2

    override this.ToString() =
      (sprintf "\"%O\" %O" this.Id this.NodeAttributes).Trim()
