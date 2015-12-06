namespace FsDot

type OutputType =
| Svg
| Gif
| Jpg
| Png
with 
   member this.ResultFormat() =
      match this with
      | Svg -> ResultFormat.Text
      | Gif
      | Jpg
      | Png -> ResultFormat.Binary