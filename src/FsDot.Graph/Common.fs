#if INTERACTIVE
#else
namespace FsDot
#endif

type Id =
  Id of string
with
  override x.ToString() =
    let (Id s) = x in s