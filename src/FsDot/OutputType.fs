namespace FsDot

type OutputType =
  | Svg
  | Gif
  | Jpg
  | Png
with 
  member x.resultFormat =
    match x with
    | Svg -> ResultFormat.Text
    | Gif
    | Jpg
    | Png -> ResultFormat.Binary