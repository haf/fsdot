#if INTERACTIVE
#else
namespace GraphVizWrapper
#endif

type Id =
| Id of string
   override this.ToString() =
      match this with
      | Id s -> s

