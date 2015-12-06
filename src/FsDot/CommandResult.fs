namespace FsDot

type ResultFormat =
| Text
| Binary

type BinaryContent = array<byte>
type TextContent = string

type CommandResult =
| SuccessText of content:TextContent
| SuccessBinary of content:BinaryContent
| Failure of message:string

