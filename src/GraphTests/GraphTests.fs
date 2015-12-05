module GraphTests

open System.Drawing
open NUnit.Framework
open FsUnit
open GraphVizWrapper

// TODO
// Graph 'start' attribute is missing
// Added graph 'splines' attribute - needs unit tests

[<AutoOpen>]
module __ =
   let emptyGraph = 
      "graph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let emptyDigraph =
      "digraph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let emptyStrictGraph =
      "strict graph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let oneNodeGraph =
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20\"node1\";\r\n\r\n\
      }"
   let twoNodeGraph =
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20\"node1\";\r\n\
      \x20\x20\"node2\";\r\n\r\n\
      }"
   let oneEdgeGraph =
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20\"node1\" -- \"node2\";\r\n\r\n\
      }"
   let twoEdgeGraph =
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20\"node1\" -- \"node2\";\r\n\
      \x20\x20\"node3\" -- \"node4\";\r\n\r\n\
      }"
   let twoEdgeDigraph =
      "digraph \"id\"\r\n\
      {\r\n\
      \x20\x20\"node1\" -> \"node2\";\r\n\
      \x20\x20\"node3\" -> \"node4\";\r\n\r\n\
      }"
   let emptyGraphWithDamping = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20Damping = 0.5;\r\n\r\n\
      }"
   let emptyGraphWithK = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20K = 1.1;\r\n\r\n\
      }"
   let emptyGraphWithDampingAndK = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20Damping = 0.5;\r\n\x20\x20K = 1.1;\r\n\r\n\
      }"
   let emptyGraphWithUrl = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20URL = \"www.kiteason.com\";\r\n\r\n\
      }"
   let emptyGraphWithBackground = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20_background = \"background xdot\";\r\n\r\n\
      }"
   let emptyGraphWithBb = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20bb = \"0,1.11,2.222,3.3333\";\r\n\r\n\
      }"
   let emptyGraphWithBgColorNamed = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20bgcolor = \"PeachPuff\";\r\n\r\n\
      }"
   let emptyGraphWithBgColorArgb = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20bgcolor = \"#01020304\";\r\n\r\n\
      }"
   let emptyGraphWithBgColorList = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20bgcolor = \"PeachPuff;0.4:#01020304;0.6\";\r\n\r\n\
      }"
   let emptyGraphWithBgColorListNoWeighting = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20bgcolor = \"PeachPuff:#01020304\";\r\n\r\n\
      }"
   let emptyGraphWithCenter = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20center = true;\r\n\r\n\
      }"
   let emptyGraphWithCharset = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20charset = \"UTF-16\";\r\n\r\n\
      }"
   let emptyGraphWithClusterRank = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20clusterrank = \"global\";\r\n\r\n\
      }"
   let emptyGraphWithColorNamed = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20color = \"PeachPuff\";\r\n\r\n\
      }"
   let emptyGraphWithColorArgb = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20color = \"#01020304\";\r\n\r\n\
      }"
   let emptyGraphWithColorList = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20color = \"PeachPuff;0.4:#01020304;0.6\";\r\n\r\n\
      }"
   let emptyGraphWithColorScheme = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20colorscheme = \"color scheme\";\r\n\r\n\
      }"
   let emptyGraphWithComment = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20comment = \"this is a comment\";\r\n\r\n\
      }"
   let emptyGraphWithCompound = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20compound = true;\r\n\r\n\
      }"
   let emptyGraphWithConcentrate = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20concentrate = true;\r\n\r\n\
      }"
   let emptyGraphWithDefaultDistance = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20defaultdist = 3.142;\r\n\r\n\
      }"
   let emptyGraphWithDim = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20dim = 3;\r\n\r\n\
      }"
   let emptyGraphWithDimen = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20dimen = 10;\r\n\r\n\
      }"
   let emptyGraphWithDirEdgeConstraintsTrue = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20diredgeconstraints = true;\r\n\r\n\
      }"
   let emptyGraphWithDirEdgeConstraintsHier = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20diredgeconstraints = \"hier\";\r\n\r\n\
      }"
   let emptyGraphWithDpi = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20dpi = 123.4;\r\n\r\n\
      }"
   let emptyGraphWithEpsilon = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20epsilon = 1.2;\r\n\r\n\
      }"
   let emptyGraphWithESepSimpleNonAdditive = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20esep = 1.2;\r\n\r\n\
      }"
   let emptyGraphWithESepPointNonAdditive = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20esep = \"1.2,3.4\";\r\n\r\n\
      }"
   let emptyGraphWithESepSimpleAdditive = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20esep = +1.2;\r\n\r\n\
      }"
   let emptyGraphWithESepPointAdditive = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20esep = \"+1.2,3.4\";\r\n\r\n\
      }"
   let emptyGraphWithFontColor = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20fontcolor = \"PeachPuff\";\r\n\r\n\
      }"
   let emptyGraphWithFontName = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20fontname = \"Helvetica\";\r\n\r\n\
      }"
   let emptyGraphWithFontNames = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20fontnames = \"svg\";\r\n\r\n\
      }"
   let emptyGraphWithFontPath = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20fontpath = \"c:\\fonts\";\r\n\r\n\
      }"
   let emptyGraphWithFontSize = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20fontsize = 11.5;\r\n\r\n\
      }"
   let emptyGraphWithForceLabels = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20forcelabels = false;\r\n\r\n\
      }"
   let emptyGraphWithGradientAngle = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20gradientangle = 180;\r\n\r\n\
      }"
   let emptyGraphWithId = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20id = \"id attribute\";\r\n\r\n\
      }"
   let emptyGraphWithImagePath = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20imagepath = \"element1;element2\";\r\n\r\n\
      }"
   let emptyGraphWithInputScale = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20inputscale = 36;\r\n\r\n\
      }"
   let emptyGraphWithLabel = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20label = \"graph label\";\r\n\r\n\
      }"
   let emptyGraphWithLabelSchemePenaltyCenter = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20labelscheme = 1;\r\n\r\n\
      }"
   let emptyGraphWithLabelSchemePenaltyOldCenter = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20labelscheme = 2;\r\n\r\n\
      }"
   let emptyGraphWithLabelSchemeTwoStep = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20labelscheme = 3;\r\n\r\n\
      }"
   let emptyGraphWithLabelJustRight = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20labeljust = \"r\";\r\n\r\n\
      }"
   let emptyGraphWithLabelJustLeft = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20labeljust = \"l\";\r\n\r\n\
      }"
   let emptyGraphWithLabelLocTop = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20labelloc = \"t\";\r\n\r\n\
      }"
   let emptyGraphWithLabelLocCenter = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20labelloc = \"c\";\r\n\r\n\
      }"
   let emptyGraphWithLabelLocBottom = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20labelloc = \"b\";\r\n\r\n\
      }"
   // Landscape = true is a synonym for rotation = 90
   let emptyGraphWithLandscape = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20rotation = 90;\r\n\r\n\
      }"
   let emptyGraphWithLayerListSep = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20layerlistsep = \";\";\r\n\r\n\
      }"
   let emptyGraphWithLayerSep = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20layersep = \"|\";\r\n\r\n\
      }"
   let emptyGraphWithLayersUsingDefaultSep = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20layers = \"layer1:layer2\";\r\n\r\n\
      }"
   let emptyGraphWithLayersUsingNonDefaultSep = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20layers = \"layer1|layer2\";\r\n\x20\x20layersep = \"|\";\r\n\r\n\
      }"
   let emptyGraphWithLayerSelectSome = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20layers = \"layer1:layer2:layer3\";\r\n\
      \x20\x20layerselect = \"1,3\";\r\n\r\n\
      }"
   let emptyGraphWithLayerSelectSomeNonDefaultLayerListSep = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20layerlistsep = \"|\";\r\n\
      \x20\x20layers = \"layer1:layer2:layer3\";\r\n\
      \x20\x20layerselect = \"1|3\";\r\n\r\n\
      }"
   let emptyGraphWithLayout = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20layout = \"neato\";\r\n\r\n\
      }"
   let emptyGraphWithLevels = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20levels = 99;\r\n\r\n\
      }"
   let emptyGraphWithLevelsGap = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20levelsgap = 0.1;\r\n\r\n\
      }"
   let emptyGraphWithLHeight = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20lheight = 1.2;\r\n\r\n\
      }"
   let emptyGraphWithLp = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20lp = \"1.2,3.4\";\r\n\r\n\
      }"
   let emptyGraphWithLWidth = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20lwidth = 5.6;\r\n\r\n\
      }"
   let emptyGraphWithSingleMargin = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20margin = 7.8;\r\n\r\n\
      }"
   let emptyGraphWithXYMargin = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20margin = \"7.8,9.1\";\r\n\r\n\
      }"
   let emptyGraphWithMaxIter = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20maxiter = 999;\r\n\r\n\
      }"
   let emptyGraphWithMcLimit = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20mclimit = 1.5;\r\n\r\n\
      }"
   let emptyGraphWithMinDist = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20mindist = 0.4;\r\n\r\n\
      }"
   // Major is the default
   let emptyGraphWithModeMajor = 
      "graph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let emptyGraphWithModeKK = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20mode = \"KK\";\r\n\r\n\
      }"
   let emptyGraphWithModeHier = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20mode = \"hier\";\r\n\r\n\
      }"
   let emptyGraphWithModeIpSep = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20mode = \"ipsep\";\r\n\r\n\
      }"
   let emptyGraphWithModeSpring = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20mode = \"spring\";\r\n\r\n\
      }"
   let emptyGraphWithModeMaxEnt = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20mode = \"maxent\";\r\n\r\n\
      }"
   // Shortpath is the default
   let emptyGraphWithModelShortPath = 
      "graph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let emptyGraphWithModelCircuit = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20model = \"circuit\";\r\n\r\n\
      }"
   let emptyGraphWithModelSubset = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20model = \"subset\";\r\n\r\n\
      }"
   let emptyGraphWithModelMds = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20model = \"mds\";\r\n\r\n\
      }"
   let emptyGraphWithMosek = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20mosek = true;\r\n\r\n\
      }"
   let emptyGraphWithNodeSep = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20nodesep = 0.8;\r\n\r\n\
      }"
   let emptyGraphWithNoJustify = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20nojustify = true;\r\n\r\n\
      }"
   let emptyGraphWithNormalizeTrue = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20normalize = true;\r\n\r\n\
      }"
   let emptyGraphWithNormalizeNumber = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20normalize = 22.5;\r\n\r\n\
      }"
   let emptyGraphWithNoTranslate = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20notranslate = true;\r\n\r\n\
      }"
   let emptyGraphWithNsLimit = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20nslimit = 3.2;\r\n\r\n\
      }"
   let emptyGraphWithNsLimit1 = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20nslimit1 = 5.4;\r\n\r\n\
      }"
   let emptyGraphWithOrderingOut = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20ordering = \"out\";\r\n\r\n\
      }"
   let emptyGraphWithOrderingIn = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20ordering = \"in\";\r\n\r\n\
      }"
   let emptyGraphWithOrientation = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20orientation = 22.5;\r\n\r\n\
      }"
   // breadthfirst is the default:
   let emptyGraphWithOutputOrderBreadthFirst = 
      "graph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let emptyGraphWithOutputOrderNodesFirst = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20outputorder = \"nodesfirst\";\r\n\r\n\
      }"
   let emptyGraphWithOutputOrderEdgesFirst = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20outputorder = \"edgesfirst\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapTrue = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = true;\r\n\r\n\
      }"
   let emptyGraphWithOverLapFalse = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = false;\r\n\r\n\
      }"
   let emptyGraphWithOverLapScale = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"scale\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapPrism = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"prism\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapPrismSuffix = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"prism3\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapVoronoi = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"voronoi\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapscaleXY = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"scalexy\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapCompress = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"compress\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapVpsc = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"vpsc\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapOrtho = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"ortho\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapOrthoXY = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"orthoxy\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapOrthoYX = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"orthoyx\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapOrtho_YX = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"ortho_yx\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapPOrtho = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"portho\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapPOrthoXY = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"porthoxy\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapPOrthoYX = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"porthoyx\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapPOrtho_YX = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"portho_yx\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapIpSep = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"ipsep\";\r\n\r\n\
      }"
   let emptyGraphWithOverLapTruePrefix = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap = \"2:true\";\r\n\r\n\
      }"
   // -4 is the default
   let emptyGraphWithOverLapScalingMinus4 = 
      "graph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let emptyGraphWithOverLapScaling = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap_scaling = 3;\r\n\r\n\
      }"
   let emptyGraphWithOverLapShrink = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20overlap_shrink = false;\r\n\r\n\
      }"
   // false is the default
   let emptyGraphWithPackFalse = 
      "graph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let emptyGraphWithPackTrue = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pack = true;\r\n\r\n\
      }"
   let emptyGraphWithPackN = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pack = 12;\r\n\r\n\
      }"
   // This is the default
   let emptyGraphWithPackModeNode = 
      "graph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let emptyGraphWithPackModeClust = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"clust\";\r\n\r\n\
      }"
   let emptyGraphWithPackModeGraph = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"graph\";\r\n\r\n\
      }"
   let emptyGraphWithPackModeArray = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"array\";\r\n\r\n\
      }"
   let emptyGraphWithPackModeArrayC = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"array_c\";\r\n\r\n\
      }"
   let emptyGraphWithPackModeArrayCN = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"array_c5\";\r\n\r\n\
      }"
   let emptyGraphWithPackModeArrayT = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"array_t\";\r\n\r\n\
      }"
   let emptyGraphWithPackModeArrayB = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"array_b\";\r\n\r\n\
      }"
   let emptyGraphWithPackModeArrayL = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"array_l\";\r\n\r\n\
      }"
   let emptyGraphWithPackModeArrayR = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"array_r\";\r\n\r\n\
      }"
   let emptyGraphWithPackModeArrayCNR = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"array_c3r\";\r\n\r\n\
      }"
   let emptyGraphWithPackModeArrayU = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"array_u\";\r\n\r\n\
      }"
   let emptyGraphWithPackModeArrayCNRU = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20packmode = \"array_c3ru\";\r\n\r\n\
      }"
   let emptyGraphWithPadN = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pad = 1.2;\r\n\r\n\
      }"
   let emptyGraphWithPadXY = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pad = \"1.2,3.4\";\r\n\r\n\
      }"
   let emptyGraphWithPageN = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20page = 4.5;\r\n\r\n\
      }"
   let emptyGraphWithPageXY = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20page = \"4.5,6.7\";\r\n\r\n\
      }"
   // This is the default:
   let emptyGraphWithPageDirBL = 
      "graph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let emptyGraphWithPageDirBR = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pagedir = \"BR\";\r\n\r\n\
      }"
   let emptyGraphWithPageDirTL = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pagedir = \"TL\";\r\n\r\n\
      }"
   let emptyGraphWithPageDirTR = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pagedir = \"TR\";\r\n\r\n\
      }"
   let emptyGraphWithPageDirRB = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pagedir = \"RB\";\r\n\r\n\
      }"
   let emptyGraphWithPageDirRT = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pagedir = \"RT\";\r\n\r\n\
      }"
   let emptyGraphWithPageDirLB = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pagedir = \"LB\";\r\n\r\n\
      }"
   let emptyGraphWithPageDirLT = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pagedir = \"LT\";\r\n\r\n\
      }"
   let emptyGraphWithPenColorNamed = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pencolor = \"PeachPuff\";\r\n\r\n\
      }"
   let emptyGraphWithPenColorArgb = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pencolor = \"#01020304\";\r\n\r\n\
      }"
   let emptyGraphWithPenColorList = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pencolor = \"PeachPuff;0.4:#01020304;0.6\";\r\n\r\n\
      }"
   let emptyGraphWithPenColorListNoWeighting = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20pencolor = \"PeachPuff:#01020304\";\r\n\r\n\
      }"
   // This is the default
   let emptyGraphWithQuadtreeNormal = 
      "graph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let emptyGraphWithQuadtreeNone = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20quadtree = \"none\";\r\n\r\n\
      }"
   let emptyGraphWithQuadtreeFast = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20quadtree = \"fast\";\r\n\r\n\
      }"
   let emptyGraphWithQuantum = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20quantum = 1.23;\r\n\r\n\
      }"
   // This is the default:
   let emptyGraphWithRankdirTB = 
      "graph \"id\"\r\n\
      {\r\n\r\n\
      }"
   let emptyGraphWithRankdirLR = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20rankdir = \"LR\";\r\n\r\n\
      }"
   let emptyGraphWithRankdirBT = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20rankdir = \"BT\";\r\n\r\n\
      }"
   let emptyGraphWithRankdirRL = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20rankdir = \"RL\";\r\n\r\n\
      }"
   let emptyGraphWithRankSepOne = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20ranksep = 1.2;\r\n\r\n\
      }"
   let emptyGraphWithRankSepSeveral = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20ranksep = \"1.2:3.4:5.6\";\r\n\r\n\
      }"
   let emptyGraphWithRankSepOneEqually = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20ranksep = \"1.2 equally\";\r\n\r\n\
      }"
   let emptyGraphWithRatioN = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20ratio = 1.2;\r\n\r\n\
      }"
   let emptyGraphWithRatioFill = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20ratio = \"fill\";\r\n\r\n\
      }"
   let emptyGraphWithRatioCompress = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20ratio = \"compress\";\r\n\r\n\
      }"
   let emptyGraphWithRatioExpand = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20ratio = \"expand\";\r\n\r\n\
      }"
   let emptyGraphWithRatioAuto = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20ratio = \"auto\";\r\n\r\n\
      }"
   let emptyGraphWithReMinCross = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20remincross = false;\r\n\r\n\
      }"
   let emptyGraphWithRepulsiveForce = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20repulsiveforce = 1.9;\r\n\r\n\
      }"
   // Resolution is a synonym for DPI
   let emptyGraphWithResolution = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20dpi = 456.7;\r\n\r\n\
      }"
   let emptyGraphWithRoot = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20root = \"node99\";\r\n\r\n\
      }"
   // Rotate.Landscape (90) is a synonym for Rotation = 90
   let emptyGraphWithRotate = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20rotation = 90;\r\n\r\n\
      }"
   let emptyGraphWithScaleN = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20scale = 1.2;\r\n\r\n\
      }"
   let emptyGraphWithScaleXY = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20scale = \"1.2,3.4\";\r\n\r\n\
      }"
   let emptyGraphWithSearchSize = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20searchsize = 35;\r\n\r\n\
      }"
   let emptyGraphWithSepN = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20sep = 10;\r\n\r\n\
      }"
   let emptyGraphWithSepXY = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20sep = \"10,12\";\r\n\r\n\
      }"
   let emptyGraphWithSepPlusN = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20sep = +10;\r\n\r\n\
      }"
   let emptyGraphWithSepPlusXY = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20sep = \"+10,12\";\r\n\r\n\
      }"
   let emptyGraphWithShowBoxesBeginning = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20showboxes = 1;\r\n\r\n\
      }"
   let emptyGraphWithShowBoxesEnd = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20showboxes = 2;\r\n\r\n\
      }"
   let emptyGraphWithSizeN = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20size = 2.1;\r\n\r\n\
      }"
   let emptyGraphWithSizeXY = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20size = \"2.1,3.4\";\r\n\r\n\
      }"
   let emptyGraphWithSizeNDesired = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20size = \"2.1!\";\r\n\r\n\
      }"
   let emptyGraphWithSizeXYDesired = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20size = \"2.1,3.4!\";\r\n\r\n\
      }"
   let emptyGraphWithSmoothingAverageDistance = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20smoothing = \"avg_dist\";\r\n\r\n\
      }"
   let emptyGraphWithSmoothingGraphDistance = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20smoothing = \"graph_dist\";\r\n\r\n\
      }"
   let emptyGraphWithSmoothingPowerDistance = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20smoothing = \"power_dist\";\r\n\r\n\
      }"
   let emptyGraphWithSmoothingRandom = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20smoothing = \"rng\";\r\n\r\n\
      }"
   let emptyGraphWithSmoothingSpring = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20smoothing = \"spring\";\r\n\r\n\
      }"
   let emptyGraphWithSmoothingTriangle = 
      "graph \"id\"\r\n\
      {\r\n\
      \x20\x20smoothing = \"triangle\";\r\n\r\n\
      }"

[<TestFixture>]
type GraphTests() =

   [<Test>]
   member __.``An empty graph has the correct source``() =
      let expected = emptyGraph
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``Any empty digraph has the correct source``() =
      let expected = emptyDigraph
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Digraph)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``An empty strict graph has the correct source``() =
      let expected = emptyStrictGraph
      let sut = Graph(Id "id", Strictness.Strict, GraphKind.Graph)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``A one-node graph has the correct source``() =
      let expected = oneNodeGraph
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph)
            .WithStatement(NodeStatement(GraphNode.GraphNode(Id "node1")))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``A two-node graph has the correct source``() =
      let expected = twoNodeGraph
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph)
            .WithStatement(NodeStatement(GraphNode.GraphNode(Id "node1")))
            .WithStatement(NodeStatement(GraphNode.GraphNode(Id "node2")))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``A one-edge graph has the correct source``() =
      let expected = oneEdgeGraph
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph)
            .WithStatement(EdgeStatement("", GraphNode.GraphNode(Id "node1"), GraphNode.GraphNode(Id "node2"), Directionality.Undirected))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``A two-edge graph has the correct source``() =
      let expected = twoEdgeGraph
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph)
            .WithStatement(EdgeStatement("", GraphNode.GraphNode(Id "node1"), GraphNode.GraphNode(Id "node2"), Directionality.Undirected))
            .WithStatement(EdgeStatement("", GraphNode.GraphNode(Id "node3"), GraphNode.GraphNode(Id "node4"), Directionality.Undirected))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``A two-edge directed graph has the correct source``() =
      let expected = twoEdgeDigraph
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Digraph)
            .WithStatement(EdgeStatement("", GraphNode.GraphNode(Id "node1"), GraphNode.GraphNode(Id "node2"), Directionality.Directed))
            .WithStatement(EdgeStatement("", GraphNode.GraphNode(Id "node3"), GraphNode.GraphNode(Id "node4"), Directionality.Directed))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the Damping attribute to a non default value``() =
      let expected = emptyGraphWithDamping
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Damping = 0.5)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the Damping attribute to its default value and it is not included in the source``() =
      let expected = emptyGraph
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Damping = 0.99)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the K attribute to a non default value``() =
      let expected = emptyGraphWithK
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, K = 1.1)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the Damping attribute and the K attribute to non default values``() =
      let expected = emptyGraphWithDampingAndK
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, K = 1.1, Damping = 0.5)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the Url attribute to a non default value``() =
      let expected = emptyGraphWithUrl
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Url = "www.kiteason.com")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the _background attribute to a non default value``() =
      let expected = emptyGraphWithBackground
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Background = "background xdot")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the Bb attribute to a non default value``() =
      let expected = emptyGraphWithBb  
      let bb = { llx = 0.0; lly = 1.11; urx = 2.222; ury = 3.3333 }
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Bb = Some bb)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the bgcolor attribute to a non default value using a color name``() =
      let expected = emptyGraphWithBgColorNamed
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, BgColor = Some(GraphColor.SingleColor(Color.PeachPuff)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the bgcolor attribute to a non default value using an ARGB value``() =
      let expected = emptyGraphWithBgColorArgb
      let color = GraphColor.SingleColor(Color.FromArgb(1, 2, 3, 4))
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, BgColor = Some(color))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the bgcolor attribute to a color list value``() =
      let expected = emptyGraphWithBgColorList
      let color1 = { WColor = Color.PeachPuff; Weight = Some 0.4 }
      let color2 = { WColor = Color.FromArgb(1, 2, 3, 4); Weight = Some 0.6 }
      let colors = WeightedColorList([color1; color2])
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, BgColor = Some(GraphColor.ColorList(colors)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the bgcolor attribute to a color list value without weightings``() =
      let expected = emptyGraphWithBgColorListNoWeighting
      let color1 = { WColor = Color.PeachPuff; Weight = None }
      let color2 = { WColor = Color.FromArgb(1, 2, 3, 4); Weight = None }
      let colors = WeightedColorList([color1; color2])
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, BgColor = Some(GraphColor.ColorList(colors)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the center attribute to a non default value``() =
      let expected = emptyGraphWithCenter
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Center = true)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the charset attribute to a non default value``() =
      let expected = emptyGraphWithCharset
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Charset = "UTF-16")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the clusterrank attribute to a non default value``() =
      let expected = emptyGraphWithClusterRank
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, ClusterRank = "global")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the color attribute to a non default value using a color name``() =
      let expected = emptyGraphWithColorNamed
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Color = GraphColor.SingleColor(Color.PeachPuff))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the color attribute to a non default value using an ARGB value``() =
      let expected = emptyGraphWithColorArgb
      let color = GraphColor.SingleColor(Color.FromArgb(1, 2, 3, 4))
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Color = color)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the color attribute to a color list value``() =
      let expected = emptyGraphWithColorList
      let color1 = { WColor = Color.PeachPuff; Weight = Some 0.4 }
      let color2 = { WColor = Color.FromArgb(1, 2, 3, 4); Weight = Some 0.6 }
      let colors = WeightedColorList([color1; color2])
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Color = GraphColor.ColorList(colors))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the colorscheme attribute to a non default value``() =
      let expected = emptyGraphWithColorScheme
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, ColorScheme = "color scheme")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the comment attribute to a non default value``() =
      let expected = emptyGraphWithComment
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Comment = "this is a comment")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the compound attribute to a non default value``() =
      let expected = emptyGraphWithCompound
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Compound = true)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the concentrate attribute to a non default value``() =
      let expected = emptyGraphWithConcentrate
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Concentrate = true)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the defaultdist attribute to a non default value``() =
      let expected = emptyGraphWithDefaultDistance
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, DefaultDistance = Some 3.142)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the dim attribute to a non default value``() =
      let expected = emptyGraphWithDim
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Dim = Dimension(3))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the dimen attribute to a non default value``() =
      let expected = emptyGraphWithDimen
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Dimen = Dimension(10))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the diredgeconstraints attribute to true``() =
      let expected = emptyGraphWithDirEdgeConstraintsTrue
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, DirEdgeConstraints = DirEdgeConstraints.True)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the diredgeconstraints attribute to 'hier'``() =
      let expected = emptyGraphWithDirEdgeConstraintsHier
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, DirEdgeConstraints = DirEdgeConstraints.Hier)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the dpi attribute to a non default value``() =
      let expected = emptyGraphWithDpi
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Dpi = 123.4)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the epsilon attribute to a non default value``() =
      let expected = emptyGraphWithEpsilon
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Epsilon = Some 1.2)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the esep attribute to a simple non additive value``() =
      let expected = emptyGraphWithESepSimpleNonAdditive
      let esep = Separation(1.2, false)
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, ESep = Some esep)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the esep attribute to a point non additive value``() =
      let expected = emptyGraphWithESepPointNonAdditive
      let esep = Separation(1.2, 3.4, false)
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, ESep = Some esep)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the esep attribute to a simple additive value``() =
      let expected = emptyGraphWithESepSimpleAdditive
      let esep = Separation(1.2, true)
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, ESep = Some esep)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the esep attribute to a point additive value``() =
      let expected = emptyGraphWithESepPointAdditive
      let esep = Separation(1.2, 3.4, true)
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, ESep = Some esep)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the fontcolor attribute to a non default value``() =
      let expected = emptyGraphWithFontColor
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, FontColor = FontColor(Color.PeachPuff))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the fontname attribute to a non default value``() =
      let expected = emptyGraphWithFontName
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, FontName = "Helvetica")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the fontnames attribute to a non default value``() =
      let expected = emptyGraphWithFontNames
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, FontNames = FontNames.Svg)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the fontpath attribute to a non default value``() =
      let expected = emptyGraphWithFontPath
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, FontPath = @"c:\fonts")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the fontsize attribute to a non default value``() =
      let expected = emptyGraphWithFontSize
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, FontSize = FontSize(11.5))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the forcelabels attribute to a non default value``() =
      let expected = emptyGraphWithForceLabels
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, ForceLabels = false)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the gradientangle attribute to a non default value``() =
      let expected = emptyGraphWithGradientAngle
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, GradientAngle = 180)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the href attribute to a non default value``() =
      let expected = emptyGraphWithUrl
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Href = "www.kiteason.com")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the id attribute to a non default value``() =
      let expected = emptyGraphWithId
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, IdAttribute = "id attribute")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the imagepath attribute to a non default value``() =
      let expected = emptyGraphWithImagePath
      let imagePath = ImagePath([|"element1"; "element2"|])
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, ImagePath = imagePath)
      let actual = sut.ToString()
      actual |> should equal expected
   
   [<Test>]
   member __.``We can set the inputscale attribute to a non default value``() =
      let expected = emptyGraphWithInputScale
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, InputScale = 36.)
      let actual = sut.ToString()
      actual |> should equal expected
   
   [<Test>]
   member __.``We can set the label attribute to a non default value``() =
      let expected = emptyGraphWithLabel
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Label = "graph label")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the labelscheme attribute to a value of 'penalty center' (1)``() =
      let expected = emptyGraphWithLabelSchemePenaltyCenter
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LabelScheme = LabelScheme.PenaltyCenter)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the labelscheme attribute to a value of 'penalty old center' (2)``() =
      let expected = emptyGraphWithLabelSchemePenaltyOldCenter
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LabelScheme = LabelScheme.PenaltyCenterOld)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the labelscheme attribute to a value of 'two step' (3)``() =
      let expected = emptyGraphWithLabelSchemeTwoStep
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LabelScheme = LabelScheme.TwoStep)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the labeljust attribute to a value of 'right'``() =
      let expected = emptyGraphWithLabelJustRight
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LabelJust = LabelJust.Right)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the labeljust attribute to a value of 'left'``() =
      let expected = emptyGraphWithLabelJustLeft
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LabelJust = LabelJust.Left)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the labelloc attribute to a value of 'top'``() =
      let expected = emptyGraphWithLabelLocTop
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LabelLoc = Some LabelLoc.Top)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the labelloc attribute to a value of 'center'``() =
      let expected = emptyGraphWithLabelLocCenter
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LabelLoc = Some LabelLoc.Center)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the labelloc attribute to a value of 'bottom'``() =
      let expected = emptyGraphWithLabelLocBottom
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LabelLoc = Some LabelLoc.Bottom)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the landscape attribute to true causing a rotation of 90``() =
      let expected = emptyGraphWithLandscape
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Landscape = true)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the layerlistsep attribute to a non default value``() =
      let expected = emptyGraphWithLayerListSep
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LayerListSep = ";")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the layers attribute to a non default value (and default layersep)``() =
      let expected = emptyGraphWithLayersUsingDefaultSep
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            Layers = Layers([|Layer("layer1", true); Layer("layer2", true)|]))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the layers attribute to a non default value (and non default layersep)``() =
      let expected = emptyGraphWithLayersUsingNonDefaultSep
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            Layers = Layers([|Layer("layer1", true); Layer("layer2", true)|], "|"))
      let actual = sut.ToString()
      actual |> should equal expected
      
   [<Test>]
   member __.``We can set the layersep attribute to a non default value``() =
      let expected = emptyGraphWithLayerSep
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph)
      sut.Layers.LayerSep <- "|"
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the layerselect attribute to a non default value``() =
      let expected = emptyGraphWithLayerSelectSome
      let layers =
         Layers(
            [|
               Layer("layer1", true)
               Layer("layer2", false)
               Layer("layer3", true)
            |]
         )
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Layers = layers)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the layerselect attribute to a non default value (with a non default layerlistsep attribute)``() =
      let expected = emptyGraphWithLayerSelectSomeNonDefaultLayerListSep
      let layers =
         Layers(
            [|
               Layer("layer1", true)
               Layer("layer2", false)
               Layer("layer3", true)
            |]
         )
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            Layers = layers, LayerListSep = "|")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the layout attribute to a non default value``() =
      let expected = emptyGraphWithLayout
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Layout = "neato")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the levels attribute to a non default value``() =
      let expected = emptyGraphWithLevels
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Levels = 99)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the levelsgap attribute to a non default value``() =
      let expected = emptyGraphWithLevelsGap
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LevelsGap = 0.1)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the levelheight attribute to a non default value``() =
      let expected = emptyGraphWithLHeight
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LHeight = 1.2)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the lp attribute to a non default value``() =
      let expected = emptyGraphWithLp
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Lp = Some(GraphPoint(1.2, 3.4)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the lwidth attribute to a non default value``() =
      let expected = emptyGraphWithLWidth
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, LWidth = 5.6)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the margin attribute to a single non default value``() =
      let expected = emptyGraphWithSingleMargin 
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Margin = Margin(7.8))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the margin attribute to an x,y non default value``() =
      let expected = emptyGraphWithXYMargin 
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Margin = Margin(7.8, 9.1))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the maxiter attribute to a non default value``() =
      let expected = emptyGraphWithMaxIter 
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, MaxIter = 999)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the mclimit attribute to a non default value``() =
      let expected = emptyGraphWithMcLimit 
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, McLimit = 1.5)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the mindist attribute to a non default value``() =
      let expected = emptyGraphWithMinDist
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, MinDist = 0.4)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the mode attribute to a value of 'major'``() =
      let expected = emptyGraphWithModeMajor
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Mode = Mode.Major)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the mode attribute to a value of 'KK'``() =
      let expected = emptyGraphWithModeKK
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Mode = Mode.KK)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the mode attribute to a value of 'hier'``() =
      let expected = emptyGraphWithModeHier
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Mode = Mode.Hier)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the mode attribute to a value of 'ipsep'``() =
      let expected = emptyGraphWithModeIpSep
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Mode = Mode.IpSep)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the mode attribute to a value of 'spring'``() =
      let expected = emptyGraphWithModeSpring
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Mode = Mode.Spring)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the mode attribute to a value of 'maxent'``() =
      let expected = emptyGraphWithModeMaxEnt
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Mode = Mode.MaxEnt)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the model attribute to a value of 'shortpath'``() =
      let expected = emptyGraphWithModelShortPath
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Model = Model.ShortPath)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the model attribute to a value of 'circuit'``() =
      let expected = emptyGraphWithModelCircuit
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Model = Model.Circuit)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the model attribute to a value of 'subset'``() =
      let expected = emptyGraphWithModelSubset
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Model = Model.Subset)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the model attribute to a value of 'mds'``() =
      let expected = emptyGraphWithModelMds
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Model = Model.Mds)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the mosek attribute to a non default value``() =
      let expected = emptyGraphWithMosek
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Mosek = true)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the nodesep attribute to a non default value``() =
      let expected = emptyGraphWithNodeSep
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, NodeSep = NodeSep(0.8))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the nojustify attribute to a non default value``() =
      let expected = emptyGraphWithNoJustify
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, NoJustify = true)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the normalize attribute to a true``() =
      let expected = emptyGraphWithNormalizeTrue
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Normalize = Normalize.Bool(true))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the normalize attribute to a number``() =
      let expected = emptyGraphWithNormalizeNumber
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Normalize = Normalize.Degrees(22.5))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the notranslate element to a non default value``() =
      let expected = emptyGraphWithNoTranslate
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, NoTranslate = true)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the nslimit element to a non default value``() =
      let expected = emptyGraphWithNsLimit
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, NsLimit = 3.2)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the nslimit1 element to a non default value``() =
      let expected = emptyGraphWithNsLimit1
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, NsLimit1 = 5.4)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the ordering element to 'out'``() =
      let expected = emptyGraphWithOrderingOut
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Ordering = Some(Ordering.Out))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the ordering element to 'in'``() =
      let expected = emptyGraphWithOrderingIn
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Ordering = Some(Ordering.In))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the orientation element to a non default value``() =
      let expected = emptyGraphWithOrientation
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Orientation = 22.5)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the outputorder element to 'breadthfirst'``() =
      let expected = emptyGraphWithOutputOrderBreadthFirst
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, OutputOrder = OutputOrder.BreadthFirst)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the outputorder element to 'nodesfirst'``() =
      let expected = emptyGraphWithOutputOrderNodesFirst
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, OutputOrder = OutputOrder.NodesFirst)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the outputorder element to 'edgesfirst``() =
      let expected = emptyGraphWithOutputOrderEdgesFirst
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, OutputOrder = OutputOrder.EdgesFirst)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the overlap element to 'true'``() =
      let expected = emptyGraphWithOverLapTrue
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.True)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the overlap element to 'false'``() =
      let expected = emptyGraphWithOverLapFalse
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.False)
      let actual = sut.ToString()
      actual |> should equal expected
 
   [<Test>]
   member __.``We can set the overlap element to 'scale'``() =
      let expected = emptyGraphWithOverLapScale
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.OverlapScale)
      let actual = sut.ToString()
      actual |> should equal expected
 
   [<Test>]
   member __.``We can set the overlap element to 'prism'``() =
      let expected = emptyGraphWithOverLapPrism
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.Prism)
      let actual = sut.ToString()
      actual |> should equal expected
 
   [<Test>]
   member __.``We can set the overlap element to 'prism3'``() =
      let expected = emptyGraphWithOverLapPrismSuffix
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some (Overlap.PrismN(3)))
      let actual = sut.ToString()
      actual |> should equal expected
 
   [<Test>]
   member __.``We can set the overlap element to 'voronoi'``() =
      let expected = emptyGraphWithOverLapVoronoi
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.Voronoi)
      let actual = sut.ToString()
      actual |> should equal expected
 
   [<Test>]
   member __.``We can set the overlap element to 'scalexy``() =
      let expected = emptyGraphWithOverLapscaleXY
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.ScaleXY)
      let actual = sut.ToString()
      actual |> should equal expected
 
   [<Test>]
   member __.``We can set the overlap element to 'compress'``() =
      let expected = emptyGraphWithOverLapCompress
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.Compress)
      let actual = sut.ToString()
      actual |> should equal expected
  
   [<Test>]
   member __.``We can set the overlap element to 'vpsc'``() =
      let expected = emptyGraphWithOverLapVpsc
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.Vpsc)
      let actual = sut.ToString()
      actual |> should equal expected
 
   [<Test>]
   member __.``We can set the overlap element to 'ortho'``() =
      let expected = emptyGraphWithOverLapOrtho
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.Ortho)
      let actual = sut.ToString()
      actual |> should equal expected
  
   [<Test>]
   member __.``We can set the overlap element to 'orthoxy'``() =
      let expected = emptyGraphWithOverLapOrthoXY
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.OrthoXY)
      let actual = sut.ToString()
      actual |> should equal expected
  
   [<Test>]
   member __.``We can set the overlap element to 'orthoyx'``() =
      let expected = emptyGraphWithOverLapOrthoYX
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.OrthoYX)
      let actual = sut.ToString()
      actual |> should equal expected
  
   [<Test>]
   member __.``We can set the overlap element to 'ortho_yx'``() =
      let expected = emptyGraphWithOverLapOrtho_YX
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.Ortho_YX)
      let actual = sut.ToString()
      actual |> should equal expected
  
   [<Test>]
   member __.``We can set the overlap element to 'portho'``() =
      let expected = emptyGraphWithOverLapPOrtho
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.POrtho)
      let actual = sut.ToString()
      actual |> should equal expected
  
   [<Test>]
   member __.``We can set the overlap element to 'porthoxy'``() =
      let expected = emptyGraphWithOverLapPOrthoXY
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.POrthoXY)
      let actual = sut.ToString()
      actual |> should equal expected
 
   [<Test>]
   member __.``We can set the overlap element to 'porthoyx'``() =
      let expected = emptyGraphWithOverLapPOrthoYX
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.POrthoYX)
      let actual = sut.ToString()
      actual |> should equal expected
  
   [<Test>]
   member __.``We can set the overlap element to 'portho_yx'``() =
      let expected = emptyGraphWithOverLapPOrtho_YX
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.POrtho_YX)
      let actual = sut.ToString()
      actual |> should equal expected
  
   [<Test>]
   member __.``We can set the overlap element to 'ipsep'``() =
      let expected = emptyGraphWithOverLapIpSep
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.IpSep)
      let actual = sut.ToString()
      actual |> should equal expected
  
   [<Test>]
   member __.``We can set the overlap element to '2:true'``() =
      let expected = emptyGraphWithOverLapTruePrefix
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Overlap = Some Overlap.True, OverlapPrefix = 2)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the overlap_scaling element to the default``() =
      let expected = emptyGraphWithOverLapScalingMinus4
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, OverlapScaling = -4.0)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the overlap_scaling element to a non default value``() =
      let expected = emptyGraphWithOverLapScaling
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, OverlapScaling = 3.0)
      let actual = sut.ToString()
      actual |> should equal expected
  
   [<Test>]
   member __.``We can set the overlap_shrink element to a non default value``() =
      let expected = emptyGraphWithOverLapShrink
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, OverlapShrink = false)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack element to false``() =
      let expected = emptyGraphWithPackFalse
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Pack = Pack.False)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack element to true with no specified margin``() =
      let expected = emptyGraphWithPackTrue
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Pack = Pack.True(None))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack element to true with a specified margin``() =
      let expected = emptyGraphWithPackN
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Pack = Pack.True(Some 12))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'node'``() =
      let expected = emptyGraphWithPackModeNode
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PackMode = PackMode.Node)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'clust'``() =
      let expected = emptyGraphWithPackModeClust
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PackMode = PackMode.Clust)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'graph'``() =
      let expected = emptyGraphWithPackModeGraph
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PackMode = PackMode.Graph)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'array'``() =
      let expected = emptyGraphWithPackModeArray
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            PackMode = PackMode.Array(PackModeOrdering.Default, 
                                      PackModeAlignment.Default,
                                      PackModeInsertionOrder.Default))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'array_c'``() =
      let expected = emptyGraphWithPackModeArrayC
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            PackMode = PackMode.Array(PackModeOrdering.ColumnMajor, 
                                      PackModeAlignment.Default,
                                      PackModeInsertionOrder.Default))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'array_c5'``() =
      let expected = emptyGraphWithPackModeArrayCN
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            PackMode = PackMode.Array(PackModeOrdering.ColumnMajorN(5), 
                                      PackModeAlignment.Default,
                                      PackModeInsertionOrder.Default))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'array_t'``() =
      let expected = emptyGraphWithPackModeArrayT
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            PackMode = PackMode.Array(PackModeOrdering.Default, 
                                      PackModeAlignment.Top,
                                      PackModeInsertionOrder.Default))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'array_b'``() =
      let expected = emptyGraphWithPackModeArrayB
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            PackMode = PackMode.Array(PackModeOrdering.Default, 
                                      PackModeAlignment.Bottom,
                                      PackModeInsertionOrder.Default))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'array_l'``() =
      let expected = emptyGraphWithPackModeArrayL
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            PackMode = PackMode.Array(PackModeOrdering.Default, 
                                      PackModeAlignment.Left,
                                      PackModeInsertionOrder.Default))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'array_r'``() =
      let expected = emptyGraphWithPackModeArrayR
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            PackMode = PackMode.Array(PackModeOrdering.Default, 
                                      PackModeAlignment.Right,
                                      PackModeInsertionOrder.Default))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'array_c3r'``() =
      let expected = emptyGraphWithPackModeArrayCNR
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            PackMode = PackMode.Array(PackModeOrdering.ColumnMajorN(3), 
                                      PackModeAlignment.Right,
                                      PackModeInsertionOrder.Default))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'array_u'``() =
      let expected = emptyGraphWithPackModeArrayU
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            PackMode = PackMode.Array(PackModeOrdering.Default,
                                      PackModeAlignment.Default,
                                      PackModeInsertionOrder.User))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pack mode element to 'array_c3ru'``() =
      let expected = emptyGraphWithPackModeArrayCNRU
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, 
            PackMode = PackMode.Array(PackModeOrdering.ColumnMajorN(3),
                                      PackModeAlignment.Right,
                                      PackModeInsertionOrder.User))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pad element to a single value``() =
      let expected = emptyGraphWithPadN
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Pad = Pad(1.2))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pad element to a pair of values``() =
      let expected = emptyGraphWithPadXY
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Pad = Pad(1.2, 3.4))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the page element to a single value``() =
      let expected = emptyGraphWithPageN
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Page = (Page(4.5) |> Some))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the page element to a pair of values``() =
      let expected = emptyGraphWithPageXY
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Page = (Page(4.5, 6.7) |> Some))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pagedir element to 'BL'``() =
      let expected = emptyGraphWithPageDirBL
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PageDir = PageDir.BottomLeft)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pagedir element to 'BR'``() =
      let expected = emptyGraphWithPageDirBR
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PageDir = PageDir.BottomRight)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pagedir element to 'TL'``() =
      let expected = emptyGraphWithPageDirTL
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PageDir = PageDir.TopLeft)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pagedir element to 'TR'``() =
      let expected = emptyGraphWithPageDirTR
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PageDir = PageDir.TopRight)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pagedir element to 'RB'``() =
      let expected = emptyGraphWithPageDirRB
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PageDir = PageDir.RightBottom)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pagedir element to 'RT'``() =
      let expected = emptyGraphWithPageDirRT
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PageDir = PageDir.RightTop)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pagedir element to 'LB'``() =
      let expected = emptyGraphWithPageDirLB
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PageDir = PageDir.LeftBottom)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pagedir element to 'LT'``() =
      let expected = emptyGraphWithPageDirLT
      let sut = 
         Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PageDir = PageDir.LeftTop)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pencolor attribute to a non default value using a color name``() =
      let expected = emptyGraphWithPenColorNamed
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PenColor = GraphColor.SingleColor(Color.PeachPuff))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pencolor attribute to a non default value using an ARGB value``() =
      let expected = emptyGraphWithPenColorArgb
      let color = GraphColor.SingleColor(Color.FromArgb(1, 2, 3, 4))
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PenColor = color)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pencolor attribute to a color list value``() =
      let expected = emptyGraphWithPenColorList
      let color1 = { WColor = Color.PeachPuff; Weight = Some 0.4 }
      let color2 = { WColor = Color.FromArgb(1, 2, 3, 4); Weight = Some 0.6 }
      let colors = WeightedColorList([color1; color2])
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PenColor = GraphColor.ColorList(colors))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the pencolor attribute to a color list value without weightings``() =
      let expected = emptyGraphWithPenColorListNoWeighting
      let color1 = { WColor = Color.PeachPuff; Weight = None }
      let color2 = { WColor = Color.FromArgb(1, 2, 3, 4); Weight = None }
      let colors = WeightedColorList([color1; color2])
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, PenColor = GraphColor.ColorList(colors))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the quadtree attribute to the default``() =
      let expected = emptyGraphWithQuadtreeNormal
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Quadtree = Quadtree.Normal)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the quadtree attribute to Fast``() =
      let expected = emptyGraphWithQuadtreeFast
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Quadtree = Quadtree.Fast)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the quadtree attribute to None``() =
      let expected = emptyGraphWithQuadtreeNone
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Quadtree = Quadtree.None_)
      let actual = sut.ToString()
      actual |> should equal expected


   [<Test>]
   member __.``We can set the quantum attribute to a non default value``() =
      let expected = emptyGraphWithQuantum
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Quantum = 1.23)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the rankDir attribute to 'TB'``() =
      let expected = emptyGraphWithRankdirTB
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, RankDir = RankDir.TopBottom)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the rankDir attribute to 'LR'``() =
      let expected = emptyGraphWithRankdirLR
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, RankDir = RankDir.LeftRight)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the rankDir attribute to 'BT'``() =
      let expected = emptyGraphWithRankdirBT
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, RankDir = RankDir.BottomTop)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the rankDir attribute to 'RL'``() =
      let expected = emptyGraphWithRankdirRL
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, RankDir = RankDir.RightLeft)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the rankSep attribute to a single value``() =
      let expected = emptyGraphWithRankSepOne
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, RankSep = Some(RankSep.Single(1.2)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the rankSep attribute to a list of values``() =
      let expected = emptyGraphWithRankSepSeveral
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, RankSep = Some(RankSep.List(DoubleList([|1.2;3.4;5.6|]))))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the rankSep attribute to a value using 'equally'``() =
      let expected = emptyGraphWithRankSepOneEqually
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, RankSep = Some(RankSep.Equally(1.2)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the ratio attribute to a numeric value``() =
      let expected = emptyGraphWithRatioN
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Ratio = Some(Ratio.Numeric(1.2)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the ratio attribute to a value of 'fill'``() =
      let expected = emptyGraphWithRatioFill
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Ratio = Some(Ratio.Fill))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the ratio attribute to a value of 'compress'``() =
      let expected = emptyGraphWithRatioCompress
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Ratio = Some(Ratio.Compress))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the ratio attribute to a value of 'expand'``() =
      let expected = emptyGraphWithRatioExpand
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Ratio = Some(Ratio.Expand))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the ratio attribute to a value of 'auto'``() =
      let expected = emptyGraphWithRatioAuto
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Ratio = Some(Ratio.Auto))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the remincross attribute a non default value``() =
      let expected = emptyGraphWithReMinCross
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, ReMinCross=false)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the repulsiveforce attribute a non default value``() =
      let expected = emptyGraphWithRepulsiveForce
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, RepulsiveForce = 1.9)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the resolution attribute a non default value``() =
      let expected = emptyGraphWithResolution
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Resolution = 456.7)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the root attribute a non default value``() =
      let expected = emptyGraphWithRoot
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Root = "node99")
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the rotate attribute to 90``() =
      let expected = emptyGraphWithRotate
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Rotate=Rotate.Landscape)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the scale attribute to a single value``() =
      let expected = emptyGraphWithScaleN
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Scale = Some(Scale(1.2)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the scale attribute to a pair of values``() =
      let expected = emptyGraphWithScaleXY
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Scale = Some(Scale(1.2,3.4)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the searchsize attribute to a non default value``() =
      let expected = emptyGraphWithSearchSize
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, SearchSize = 35)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the sep attribute to a single value``() =
      let expected = emptyGraphWithSepN
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Sep = Sep.SepScale(10.))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the sep attribute to a pair of values``() =
      let expected = emptyGraphWithSepXY
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Sep = Sep.SepScaleHW(10.,12.))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the sep attribute to a single 'plus' value``() =
      let expected = emptyGraphWithSepPlusN
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Sep = Sep.SepAdd(10.))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the sep attribute to a pair of 'plus' values``() =
      let expected = emptyGraphWithSepPlusXY
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Sep = Sep.SepAddHW(10.,12.))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the showboxes attribute to a value of 'beginning'``() =
      let expected = emptyGraphWithShowBoxesBeginning
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, ShowBoxes = ShowBoxes.Beginning)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the showboxes attribute to a value of 'end'``() =
      let expected = emptyGraphWithShowBoxesEnd
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, ShowBoxes = ShowBoxes.End)
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the size attribute to a single value``() =
      let expected = emptyGraphWithSizeN
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Size=Some(Size.Max(2.1)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the size attribute to a pair of values``() =
      let expected = emptyGraphWithSizeXY
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Size=Some(Size.MaxHW(2.1,3.4)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the size attribute to a single 'desired' value``() =
      let expected = emptyGraphWithSizeNDesired
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Size=Some(Size.Desired(2.1)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the size attribute to a pair of 'desired' values``() =
      let expected = emptyGraphWithSizeXYDesired
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Size=Some(Size.DesiredHW(2.1,3.4)))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the smoothing attribute to a value of 'avg_dist'``() =
      let expected = emptyGraphWithSmoothingAverageDistance
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Smoothing=Some(Smoothing.AverageDistance))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the smoothing attribute to a value of 'graph_dist'``() =
      let expected = emptyGraphWithSmoothingGraphDistance
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Smoothing=Some(Smoothing.GraphDistance))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the smoothing attribute to a value of 'power_dist'``() =
      let expected = emptyGraphWithSmoothingPowerDistance
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Smoothing=Some(Smoothing.PowerDistance))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the smoothing attribute to a value of 'rng'``() =
      let expected = emptyGraphWithSmoothingRandom
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Smoothing=Some(Smoothing.Random))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the smoothing attribute to a value of 'spring'``() =
      let expected = emptyGraphWithSmoothingSpring
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Smoothing=Some(Smoothing.Spring))
      let actual = sut.ToString()
      actual |> should equal expected

   [<Test>]
   member __.``We can set the smoothing attribute to a value of 'triangle'``() =
      let expected = emptyGraphWithSmoothingTriangle
      let sut = Graph(Id "id", Strictness.NonStrict, GraphKind.Graph, Smoothing=Some(Smoothing.Triangle))
      let actual = sut.ToString()
      actual |> should equal expected

