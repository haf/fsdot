#if INTERACTIVE
#else
namespace GraphVizWrapper
#endif

open System
open Microsoft.FSharp.Reflection
open System.Collections.Generic
open System.Drawing

[<AutoOpen>]
module __ =
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
   let dictToAttrList (dict : Dictionary<string, string>) =
      let pairs = 
         if dict.Count > 0 then
            dict
            |> Seq.map (fun kvp -> 
               let valueStr =
                  if kvp.Value |> isNumber || kvp.Value |> isBool then
                     kvp.Value
                  else
                     kvp.Value |> quote
               sprintf "%s = %s" kvp.Key valueStr)
            |> Array.ofSeq
         else
            [||]
      if pairs.Length > 0 then
         sprintf "  %s;\r\n" (String.Join(";\r\n  ", pairs))
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

type Strictness =
| Strict
| NonStrict
   override this.ToString() =
      match this with
      | Strict -> "strict"
      | NonStrict -> ""

type GraphKind =
| Graph
| Digraph
   override this.ToString() =
      (getUnionCaseName this).ToLowerInvariant()

// TODO Should it really be possible to add directed edges to graphs
// and undirected edges to digraphs?
type Directionality =
| Directed
| Undirected
   override this.ToString() =
      match this with
      | Directed -> "->"
      | Undirected -> "--"

type Label = string

type Statement =
| NodeStatement of GraphNode.GraphNode
| EdgeStatement of Label * GraphNode.GraphNode * GraphNode.GraphNode * Directionality
   override this.ToString() =
      match this with
      | NodeStatement n -> 
         //sprintf "\"%O\" [shape=box] " n.Id
         sprintf "%O" n
      // TODO unit test for labelling etc
      | EdgeStatement(l, n1, n2, d) ->
         // For Archie: sprintf "%O %O %O [label=\"%s\"; fontsize=12] " n1 d n2 l
         sprintf "%O %O %O" n1 d n2

type Statements(statements : Statement list) =
   member __.Statements = statements
   member this.WithStatement(statement : Statement) =
      // TODO this is inefficient!
      let statements' = this.Statements @ [statement]
      Statements(statements')
   override __.ToString() =
      if statements.IsEmpty then 
         ""
      else
         sprintf "%s;\r\n" 
            (String.Join (";\r\n", 
               [| for statement in statements -> 
                     sprintf "  %O" statement |]))

type AttributeStatementType =
| GraphStatementType
| NodeStatementType
| EdgeStatementType
   override this.ToString() =
      (getUnionCaseName this).ToLowerInvariant().Replace("statementtype", "")

type Attribute (key : Id, value : Id) =
   member __.Key = key
   member __.Value = value
   override this.ToString() =
      sprintf "\"%O\" = \"%O\"" this.Key this.Value

type Attributes(statementType : AttributeStatementType, attributes : Attribute list) =
   member __.StatementType = statementType
   member __.Attributes = attributes
   member this.WithAttribute(attribute : Attribute) =
      Attributes(this.StatementType, attribute::this.Attributes)
   override this.ToString() =
      if attributes.IsEmpty then 
         ""
      else
         sprintf "  %O [ %s ]\r\n"
            this.StatementType
            (String.Join (";", [| for attribute in attributes -> attribute.ToString() |]))

type GraphVizRect = 
   {
      llx : double
      lly : double
      urx : double
      ury : double
   }
      override this.ToString() =
         sprintf "%g,%g,%g,%g" this.llx this.lly this.urx this.ury

type WeightedColor =
   {
      WColor : Color
      Weight : float option
   }
      override this.ToString() =
         sprintf "%s%s" 
            (this.WColor |> colorToString) 
            (match this.Weight with | Some w -> sprintf ";%g" w | None -> "")

type WeightedColorList(colors : WeightedColor list) =
   let totalWeight items =
      items |> List.sumBy (fun wc -> match wc.Weight with | Some w -> w | _ -> 0.)
   let checkTotal clist = 
      if clist |> totalWeight > 1.0 then
         raise (ArgumentException("Color weights cannot add up to more than 1.0"))
   do checkTotal colors

   let mutable list : WeightedColor list = colors

   member this.Add(weightedColor : WeightedColor) =
      let newList = weightedColor :: list |> List.rev
      checkTotal newList 
      list <- newList
      this
   override __.ToString() =
      let items = 
         list
         |> List.map (fun wc -> wc.ToString())
         |> Array.ofList
      String.Join(":", items)

type FontColor(color : Color) =
    member __.Color = color
    override __.ToString() =
        color |> colorToString

type GraphColor =
| SingleColor of Color
| ColorList of WeightedColorList
   override this.ToString() =
      match this with
      | SingleColor c -> c |> colorToString
      | ColorList cl -> cl.ToString()

type Dimension(n : int) =
   do
      if n < 2 || n > 10 then
         raise (ArgumentOutOfRangeException("Dimension value must be between 2 and 10 inclusive"))
   member __.N = n
   override this.ToString() =
      n.ToString()

type DirEdgeConstraints =
| False
| True
| Hier
   override this.ToString() =
      (getUnionCaseName this).ToLowerInvariant()

type Separation(h : double, w : double, additive : bool) =
   new (p : double, additive) =
      Separation(p, p, additive)
   member __.H = h
   member __.W = w
   member __.Additive = additive
   override __.ToString() =
      sprintf "%s%s"
         (if additive then "+" else "")
         (if h = w then sprintf "%g" h else sprintf "%g,%g" h w)

type FontNames =
| Undefined
| Svg
| Ps
| Gd
   override this.ToString() =
      (getUnionCaseName this).ToLowerInvariant()

type FontSize(points : double) =
   do
      if points < 1.0 then
         raise (ArgumentOutOfRangeException("Font size must be at least 1.0"))
   member __.Points = points
   override __.ToString() =
      sprintf "%g" points

type ImagePath(elements : string[]) =
   let onWindows =
      // Ugh! Really?
      Environment.OSVersion.Platform.ToString().StartsWith("Win")
   member __.Elements = elements
   override this.ToString() =
      let sep = if onWindows then ";" else ":"
      String.Join(sep, this.Elements)

type LabelScheme =
| NoScheme
| PenaltyCenter
| PenaltyCenterOld
| TwoStep
   override this.ToString() =
      match this with
      | NoScheme -> "0"
      | PenaltyCenter -> "1"
      | PenaltyCenterOld -> "2"
      | TwoStep -> "3"

type LabelJust =
| Center
| Right
| Left
   override this.ToString() =
      match this with
      | Center -> "c"
      | Right -> "r"
      | Left -> "l"

type LabelLoc =
| Top
| Center
| Bottom
   override this.ToString() =
      match this with
      | Top -> "t"
      | Center -> "c"
      | Bottom -> "b"

type Layer(name : string, selected : bool) =
   member val Name = name with get
   member val Selected = selected with get

type Layers(layers : Layer[], layerSep : string) =
   static member DefaultLayerSep = ":"
   member __.Layers = layers
   member val LayerSep = layerSep with get, set
   new (layers : Layer[]) =
      Layers(layers, Layers.DefaultLayerSep)
   override __.ToString() =
      let names = layers |> Array.map (fun layer -> layer.Name)
      String.Join(layerSep, names)
   member this.AllSelected =
      this.Layers 
      |> Array.exists (fun l -> l.Selected |> not) 
      |> not
   member this.LayerSelect(sep) =
      let layerIndices =
         this.Layers
         |> Array.mapi (fun i l -> i+1, l)
         |> Array.filter (fun (_, l) -> l.Selected)
         |> Array.map fst
         |> Array.map (fun n -> n.ToString())
      String.Join(sep, layerIndices)

type GraphPoint(x : float, y : float) =
   member __.X = x
   member __.Y = y
   override __.ToString() =
      sprintf "%g,%g" x y

type Margin(x : float, y : float) =
   new (n : float) =
      Margin(n, n)
   override __.ToString() =
      if x = y then
         sprintf "%g" x
      else
         sprintf "%g,%g" x y

type Mode =
| Major
| KK
| Hier
| IpSep
| Spring
| MaxEnt
   override this.ToString() =
      match this with
      | KK -> "KK"
      | _ -> (getUnionCaseName this).ToLowerInvariant()

type Model =
| ShortPath
| Circuit
| Subset
| Mds
   override this.ToString() = 
      (getUnionCaseName this).ToLowerInvariant()

type NodeSep(inches : double) =
   do
      if inches < 0.02 then
         raise (ArgumentOutOfRangeException("Node separation must be at least 0.02 inches"))
   member __.Inches = inches
   override __.ToString() =
      sprintf "%g" inches

type Normalize =
| Bool of bool
| Degrees of float
   override this.ToString() =
      match this with
      | Bool b -> b.ToString().ToLowerInvariant()
      | Degrees d -> sprintf "%g" d

type Ordering =
| Out
| In
   override this.ToString() =
      (getUnionCaseName this).ToLowerInvariant()

type OutputOrder =
| BreadthFirst
| NodesFirst
| EdgesFirst
   override this.ToString() =
      (getUnionCaseName this).ToLowerInvariant()

type Overlap =
| True  
| False  
| OverlapScale // Renamed
| Prism  
| PrismN of suffix:int  
| Voronoi  
| ScaleXY  
| Compress  
| Vpsc  
| Ortho  
| OrthoXY  
| OrthoYX  
| Ortho_YX  
| POrtho  
| POrthoXY  
| POrthoYX  
| POrtho_YX  
| IpSep  
   member this.ToString(prefix : int) =
      let s = 
         match this with
         | PrismN n -> sprintf "prism%i" n
         | OverlapScale -> sprintf "scale"
         | _ -> (getUnionCaseName this).ToLowerInvariant()
      if prefix > 0 then
         sprintf "%i:%s" prefix s
      else
         s

type Pack =
| True of margin:int option
| False
   override this.ToString() =
      match this with
      | True(Some m) -> sprintf "%i" m
      | True(None) -> sprintf "true"
      | False -> sprintf "false"

type PackModeOrdering =
| Default
| ColumnMajor
| ColumnMajorN of int
   override this.ToString() =
      match this with
      | Default -> ""
      | ColumnMajor -> "c"
      | ColumnMajorN n -> sprintf "c%i" n

type PackModeAlignment =
| Default
| Top 
| Bottom
| Left
| Right
   override this.ToString() =
      match this with
      | Default -> ""
      | _ -> (getUnionCaseName this).ToLowerInvariant().[0].ToString()

type PackModeInsertionOrder =
| Default
| User
   override this.ToString() =
      match this with
      | Default -> ""
      | _ -> (getUnionCaseName this).ToLowerInvariant().[0].ToString()

type PackMode =
| Node
| Clust
| Graph
| Array of ordering:PackModeOrdering * alignment:PackModeAlignment * insertionOrder:PackModeInsertionOrder
   override this.ToString() =
      match this with
      | Array(ordering, alignment, insertionOrder) ->
         let flags = sprintf "%s%s%s" (ordering.ToString()) (alignment.ToString()) (insertionOrder.ToString())
         if flags.Length > 0 then sprintf "array_%s" flags else "array"
      | _ -> (getUnionCaseName this).ToLowerInvariant()

type Pad(x : float, y : float) =
   new (n : float) =
      Pad(n, n)
   override __.ToString() =
      if x = y then
         sprintf "%g" x
      else
         sprintf "%g,%g" x y

type Page(x : float, y : float) =
   new (n : float) =
      Page(n, n)
   override __.ToString() =
      if x = y then
         sprintf "%g" x
      else
         sprintf "%g,%g" x y

type PageDir =
| BottomLeft
| BottomRight
| TopLeft
| TopRight
| RightBottom
| RightTop
| LeftBottom
| LeftTop
   override this.ToString() =
      this
      |> getUnionCaseName
      |> initials


type Quadtree =
| Normal
| Fast
| None_ 
   override this.ToString() =
      match this with
      | Normal | Fast -> (getUnionCaseName this).ToLowerInvariant()
      | None_ -> "none"

type RankDir =
| TopBottom
| LeftRight
| BottomTop
| RightLeft
   override this.ToString() =
      this
      |> getUnionCaseName
      |> initials

type DoubleList(list : double[]) =
   override __.ToString() =
      let items = 
         list
         |> Array.map (fun wc -> wc.ToString())
      String.Join(":", items)

type RankSep =
| Single of float
| List of DoubleList
| Equally of float
    override this.ToString() =
        match this with
        | Single f -> f.ToString()
        | List l -> l.ToString()
        | Equally f -> sprintf "%g equally" f

type Ratio =
| Numeric of float
| Fill
| Compress
| Expand
| Auto
    override this.ToString() =
        match this with
        | Numeric f -> f.ToString()
        | _ -> (getUnionCaseName this).ToLowerInvariant()

type Rotate =
| Portrait
| Landscape

type Scale(x : float, y : float) =
   new (n : float) =
      Scale(n, n)
   override __.ToString() =
      if x = y then
         sprintf "%g" x
      else
         sprintf "%g,%g" x y
         
type Sep =
| SepAdd of float
| SepAddHW of float * float
| SepScale of float
| SepScaleHW of float * float
   override this.ToString() =
      match this with
      | SepAdd x -> 
         sprintf "+%g" x
      | SepAddHW (h,w) ->
         if h = w then
            Sep.SepAdd(h).ToString()
         else
            sprintf "+%g,%g" h w
      | SepScale h ->
         sprintf "%g" h
      | SepScaleHW (h,w) ->
         if h = w then
            Sep.SepScale(h).ToString()
         else
            sprintf "%g,%g" h w

type ShowBoxes =
| Off
| Beginning
| End
   override this.ToString() =
      match this with
      | Off -> "0"
      | Beginning -> "1"
      | End -> "2"  

type Size =
| Max of float
| MaxHW of float * float
| Desired of float
| DesiredHW of float * float
   override this.ToString() =
      match this with
      | Max n -> sprintf "%g" n
      | MaxHW(h, w) -> sprintf "%g,%g" h w
      | Desired n -> sprintf "%g!" n
      | DesiredHW(h, w) -> sprintf "%g,%g!" h w

type Smoothing =
| AverageDistance
| GraphDistance
| PowerDistance
| Random
| Spring
| Triangle
   override this.ToString() =
      match this with
      | AverageDistance -> "avg_dist"
      | GraphDistance -> "graph_dist"
      | PowerDistance -> "power_dist"
      | Random -> "rng"
      | Spring -> "spring"
      | Triangle -> "triangle"

type Splines =
| Curved
| Compound
| Line
| NoEdges
| Ortho
| PolyLine
| Spline
   override this.ToString() =
      match this with
      | NoEdges -> "none"
      | _ -> (getUnionCaseName this).ToLowerInvariant()

type Graph
   (
      id : Id, 
      strictness: Strictness, 
      kind : GraphKind, 
      statements : Statements
//      graphAttributes : Attributes,
//      nodeAttributes : Attributes,
//      edgeAttributes : Attributes
   ) =
   let defaultDamping = 0.99
   let defaultK = 0.3
   let defaultUrl = ""
   let defaultBackground = ""
   let defaultBb : GraphVizRect option = None
   // TODO colorlist
   let defaultBgColor : GraphColor option = None
   let defaultCenter = false
   let defaultCharSet = "UTF-8"
   let defaultClusterRank = "local"
   let defaultColor : GraphColor = GraphColor.SingleColor(Color.Black)
   let defaultColorScheme = ""
   let defaultComment = ""
   let defaultCompound = false
   let defaultConcentrate = false
   let defaultDefaultDistance : float option = None
   let defaultDim = Dimension(2)
   let defaultDimen = Dimension(2)
   let defaultDirEdgeConstraints = DirEdgeConstraints.False
   let defaultDpi = 96.
   let defaultEpsilon : float option = None
   let defaultESep : Separation option = None
   let defaultFontColor = FontColor(Color.Black)
   let defaultFontName = "Times-Roman"
   let defaultFontNames = FontNames.Undefined
   let defaultFontPath = ""
   let defaultFontSize = FontSize(14.0)
   let defaultForceLabels = true
   let defaultGradientAngle = 0
   let defaultIdAttribute = ""
   let defaultImagePath = ImagePath([||])
   let defaultInputScale = 72.0
   let defaultLabel = ""
   let defaultLabelScheme = LabelScheme.NoScheme
   let defaultLabelJust = LabelJust.Center
   let defaultLabelLoc : LabelLoc option = None
   let defaultRotation = 0
   let defaultLayerListSep = ","
   let defaultLayers = Layers([||])
   let defaultLayout = ""
   let defaultLevels = Int32.MaxValue
   let defaultLevelsGap = 0.0
   let defaultLHeight = 0.0
   let defaultLp : GraphPoint option = None
   let defaultLWidth = 0.0
   let defaultMargin = Margin(0.0)
   let defaultMaxIter = 0
   let defaultMcLimit = 1.0
   let defaultMinDist = 1.0
   let defaultMode = Mode.Major
   let defaultModel = Model.ShortPath
   let defaultMosek = false
   let defaultNodeSep = NodeSep(0.25)
   let defaultNoJustify = false
   let defaultNormalize = Normalize.Bool(false)
   let defaultNoTranslate = false
   let defaultNsLimit = 0.0
   let defaultNsLimit1 = 0.0
   let defaultOrdering : Ordering option = None
   // TODO Orientation for graph is a string
   let defaultOrientation = 0.0
   let defaultOutputOrder = OutputOrder.BreadthFirst
   let defaultOverlap : Overlap option = None
   let defaultOverlapPrefix = 0
   let defaultOverlapScaling = -4.0
   let defaultOverlapShrink = true
   let defaultPack = Pack.False
   let defaultPackMode = PackMode.Node
   let defaultPad = Pad(0.0555)
   let defaultPage : Page option = None
   let defaultPageDir = PageDir.BottomLeft
   let defaultPenColor = GraphColor.SingleColor(Color.Black)
   let defaultQuadtree = Quadtree.Normal
   let defaultQuantum = 0.0
   let defaultRankDir = RankDir.TopBottom
   let defaultRankSep : RankSep option = None
   let defaultRatio : Ratio option = None
   let defaultReMinCross = true
   let defaultRepulsiveForce = 1.0
   let defaultRoot = ""
   let defaultScale : Scale option = None
   let defaultSearchSize : int = 30
   let defaultSep = Sep.SepAdd(4.)
   let defaultShowBoxes = ShowBoxes.Off
   let defaultSize : Size option = None
   let defaultSmoothing : Smoothing option = None
   let defaultSplines : Splines option = None
   new (id : Id, strictness: Strictness, kind : GraphKind) =
      Graph(id, strictness, kind, Statements([])) 
//         Attributes(AttributeStatementType.Graph, []),
//         Attributes(AttributeStatementType.Node, []),
//         Attributes(AttributeStatementType.Edge, []))
   member __.Id = id
   member __.Strictness = strictness
   member __.Kind = kind
   member __.Statements = statements
   member val Damping = defaultDamping with get, set
   member val K = defaultK with get, set
   member val Url = defaultUrl with get, set
   member val Background = defaultBackground with get, set
   member val Bb = defaultBb with get, set
   member val BgColor = defaultBgColor with get, set
   member val Center = defaultCenter with get, set
   member val Charset = defaultCharSet with get, set
   member val ClusterRank = defaultClusterRank with get, set
   member val Color = defaultColor with get, set
   member val ColorScheme = defaultColorScheme with get, set
   member val Comment = defaultComment with get, set
   member val Compound = defaultCompound with get, set
   member val Concentrate = defaultConcentrate with get, set
   member val DefaultDistance = defaultDefaultDistance with get, set
   member val Dim = defaultDim with get, set
   member val Dimen = defaultDimen with get, set
   member val DirEdgeConstraints = defaultDirEdgeConstraints with get, set
   member val Dpi = defaultDpi with get, set
   member val Epsilon = defaultEpsilon with get, set
   member val ESep = defaultESep with get, set
   member val FontColor = defaultFontColor with get, set
   member val FontName = defaultFontName with get, set
   member val FontNames = defaultFontNames with get, set
   member val FontPath = defaultFontPath with get, set
   member val FontSize = defaultFontSize with get, set
   member val ForceLabels = defaultForceLabels with get, set
   member val GradientAngle = defaultGradientAngle with get, set
   // Href is a synonym for url:
   member this.Href 
      with get() = this.Url
      and set(value) = this.Url <- value
   /// Rendered as 'id' in the attribute string but called 'IdAttribute' here
   /// to avoid a naming clash with the Graph Id.
   member val IdAttribute = defaultIdAttribute with get, set
   member val ImagePath = defaultImagePath with get, set
   member val InputScale = defaultInputScale with get, set
   member val Label = defaultLabel with get, set
   member val LabelScheme = defaultLabelScheme with get, set
   member val LabelJust = defaultLabelJust with get, set
   member val LabelLoc = defaultLabelLoc with get, set
   member this.Landscape
      with get() = this.Rotation = 90
      and set(value) = 
         if value then this.Rotation <- 90
         else if this.Rotation = 90 then this.Rotation <- 0
   member val LayerListSep = defaultLayerListSep with get, set
   member val Layers = defaultLayers with get, set
   //member this.LayerSep with get() = this.Layers.LayerSep and set(value) = this.Layers.LayerSep <- value
   member val Layout = defaultLayout with get, set
   member val Levels = defaultLevels with get, set
   member val LevelsGap = defaultLevelsGap with get, set
   member val LHeight = defaultLHeight with get, set
   member val Lp = defaultLp with get, set
   member val LWidth = defaultLWidth with get, set
   member val Margin = defaultMargin with get, set
   member val MaxIter = defaultMaxIter with get, set
   member val McLimit = defaultMcLimit with get, set
   member val MinDist = defaultMinDist with get, set
   member val Mode = defaultMode with get, set
   member val Model = defaultModel with get, set
   member val Mosek = defaultMosek with get, set
   member val NodeSep = defaultNodeSep with get, set
   member val NoJustify = defaultNoJustify with get, set
   member val Normalize = defaultNormalize with get, set
   member val NoTranslate = defaultNoTranslate with get, set
   member val NsLimit = defaultNsLimit with get, set
   member val NsLimit1 = defaultNsLimit1 with get, set
   member val Ordering = defaultOrdering with get, set
   // TODO wrap over 360?
   member val Orientation = defaultOrientation with get, set 
   // TODO wrap over 360?
   member val Rotation = defaultRotation with get, set
   member val OutputOrder = defaultOutputOrder with get, set
   member val Overlap = defaultOverlap with get, set
   member val OverlapPrefix = defaultOverlapPrefix with get, set
   member val OverlapScaling = defaultOverlapScaling with get, set
   member val OverlapShrink = defaultOverlapShrink with get, set
   member val Pack = defaultPack with get, set
   member val PackMode = defaultPackMode with get, set
   member val Pad = defaultPad with get, set
   member val Page = defaultPage with get, set
   member val PageDir = defaultPageDir with get, set
   member val PenColor = defaultPenColor with get, set
   member val Quadtree = defaultQuadtree with get, set
   member val Quantum = defaultQuantum with get, set
   member val RankDir = defaultRankDir with get, set
   member val RankSep = defaultRankSep with get, set
   member val Ratio = defaultRatio with get, set
   member val ReMinCross = defaultReMinCross with get, set
   member val RepulsiveForce = defaultRepulsiveForce with get, set
   member this.Resolution
      with get() = this.Dpi
      and set(value) = this.Dpi <- value
   member val Root = defaultRoot with get, set
   member this.Rotate 
      with get() = 
         if this.Landscape then 
            Rotate.Landscape
         else
            Rotate.Portrait
      and set(value) =
         match value with
         | Portrait -> this.Landscape <- false
         | Landscape -> this.Landscape <- true
   member val Scale = defaultScale with get, set     
   member val SearchSize = defaultSearchSize with get, set 
   member val Sep = defaultSep with get, set
   member val ShowBoxes = defaultShowBoxes with get, set
   member val Size = defaultSize with get, set
   member val Smoothing = defaultSmoothing with get, set
   member val Splines = defaultSplines with get, set
   member private this.GraphAttributes =
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
      // TODO could consider putting an attribute on the relevant members
      // and iterating over them using reflection
      addIf this.Damping defaultDamping "Damping"
      addIf this.K defaultK "K"
      addIf this.Url defaultUrl "URL"
      addIf this.Background defaultBackground "_background"
      addIfS this.Bb "bb"
      addIfS this.BgColor "bgcolor"
      addIfB this.Center defaultCenter "center"
      addIf this.Charset defaultCharSet "charset"
      addIf this.ClusterRank defaultClusterRank "clusterrank"
      addIf this.Color defaultColor "color"
      addIf this.ColorScheme defaultColorScheme "colorscheme"
      addIf this.Comment defaultComment "comment"
      addIfB this.Compound defaultCompound "compound"
      addIfB this.Concentrate defaultConcentrate "concentrate"
      addIfS this.DefaultDistance "defaultdist"
      addIf this.Dim defaultDim "dim"
      addIf this.Dimen defaultDimen "dimen"
      addIf this.DirEdgeConstraints defaultDirEdgeConstraints "diredgeconstraints"
      addIf this.Dpi defaultDpi "dpi"
      addIfS this.Epsilon "epsilon"
      addIfS this.ESep "esep"
      addIf this.FontColor defaultFontColor "fontcolor"
      addIf this.FontName defaultFontName "fontname"
      addIf this.FontNames defaultFontNames "fontnames"
      addIf this.FontPath defaultFontPath "fontpath"
      addIf this.FontSize defaultFontSize "fontsize"
      addIfB this.ForceLabels defaultForceLabels "forcelabels"
      addIf this.GradientAngle defaultGradientAngle "gradientangle"
      addIf this.IdAttribute defaultIdAttribute "id"
      addIf this.ImagePath defaultImagePath "imagepath"
      addIf this.InputScale defaultInputScale "inputscale"
      addIf this.Label defaultLabel "label"
      addIf this.LabelScheme defaultLabelScheme "labelscheme"
      addIf this.LabelJust defaultLabelJust "labeljust"
      addIfS this.LabelLoc "labelloc"
      addIf this.Rotation defaultRotation "rotation"
      addIf this.LayerListSep defaultLayerListSep "layerlistsep"
      addIf this.Layers defaultLayers "layers"
      addIf this.Layers.LayerSep Layers.DefaultLayerSep "layersep"
      if this.Layers.AllSelected |> not then
         dict.["layerselect"] <- this.Layers.LayerSelect(this.LayerListSep)
      addIf this.Layout defaultLayout "layout"
      addIf this.Levels defaultLevels "levels"
      addIf this.LevelsGap defaultLevelsGap "levelsgap"
      addIf this.LHeight defaultLHeight "lheight"
      addIfS this.Lp "lp"
      addIf this.LWidth defaultLWidth "lwidth"
      addIf this.Margin defaultMargin "margin"
      addIf this.MaxIter defaultMaxIter "maxiter"
      addIf this.McLimit defaultMcLimit "mclimit"
      addIf this.MinDist defaultMinDist "mindist"
      addIf this.Mode defaultMode "mode"
      addIf this.Model defaultModel "model"
      addIfB this.Mosek defaultMosek "mosek"
      addIf this.NodeSep defaultNodeSep "nodesep"
      addIfB this.NoJustify defaultNoJustify "nojustify"
      addIf this.Normalize defaultNormalize "normalize"
      addIfB this.NoTranslate defaultNoTranslate "notranslate"
      addIf this.NsLimit defaultNsLimit "nslimit"
      addIf this.NsLimit1 defaultNsLimit1 "nslimit1"
      addIfS this.Ordering "ordering"
      addIf this.Orientation defaultOrientation "orientation"
      addIf this.OutputOrder defaultOutputOrder "outputorder"
      match this.Overlap with
      | Some o -> dict.["overlap"] <- o.ToString(this.OverlapPrefix)
      | None -> ()
      addIf this.OverlapScaling defaultOverlapScaling "overlap_scaling"
      addIfB this.OverlapShrink defaultOverlapShrink "overlap_shrink"
      addIf this.Pack defaultPack "pack"
      addIf this.PackMode defaultPackMode "packmode"
      addIf this.Pad defaultPad "pad"
      addIfS this.Page "page"
      addIf this.PageDir defaultPageDir "pagedir"
      addIf this.PenColor defaultPenColor "pencolor"
      addIf this.Quadtree defaultQuadtree "quadtree"
      addIf this.Quantum defaultQuantum "quantum"
      addIf this.RankDir defaultRankDir "rankdir"
      addIfS this.RankSep "ranksep"
      addIfS this.Ratio "ratio"
      addIfB this.ReMinCross defaultReMinCross "remincross"
      addIf this.RepulsiveForce defaultRepulsiveForce "repulsiveforce"
      addIf this.Root defaultRoot "root"
      addIfS this.Scale "scale"
      addIf this.SearchSize defaultSearchSize "searchsize"
      addIf this.Sep defaultSep "sep"
      addIf this.ShowBoxes defaultShowBoxes "showboxes"
      addIfS this.Size "size"
      addIfS this.Smoothing "smoothing"
      // TODO add start attribute
      addIfS this.Splines "splines"

      dict |> dictToAttrList
//   member __.GraphAttributes = graphAttributes
//   member __.NodeAttributes = nodeAttributes
//   member __.EdgeAttributes = edgeAttributes
   member this.WithStatement(statement : Statement) =
      let statements = this.Statements.WithStatement statement
      Graph(this.Id, this.Strictness, this.Kind, statements)
      //, this.GraphAttributes, this.NodeAttributes, this.EdgeAttributes)
//   member this.WithGraphAttribute(attribute : Attribute) =
//      Graph(this.Id, this.Strictness, this.Kind, this.Statements, this.GraphAttributes.WithAttribute attribute, this.NodeAttributes, this.EdgeAttributes)
//   member this.WithNodeAttribute(attribute : Attribute) =
//      Graph(this.Id, this.Strictness, this.Kind, this.Statements, this.GraphAttributes, this.NodeAttributes.WithAttribute attribute, this.EdgeAttributes)
//   member this.WithEdgeAttribute(attribute : Attribute) =
//      Graph(this.Id, this.Strictness, this.Kind, this.Statements, this.GraphAttributes, this.NodeAttributes, this.EdgeAttributes.WithAttribute attribute)
   override this.ToString() =
      (
         sprintf "\
            %O %O \"%O\"\r\n\
            {\r\n\
               %O%O\r\n\
            }\
         " 
            this.Strictness
            this.Kind
            this.Id
//            this.NodeAttributes
            this.GraphAttributes
//            this.EdgeAttributes
            this.Statements
      ).Trim()
