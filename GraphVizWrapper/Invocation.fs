﻿namespace GraphVizWrapper

open System
open System.IO
open Microsoft.FSharp.Reflection

module Invocation =

   // TODO make configurable
   let private graphVizPath = @"C:\Program Files (x86)\Graphviz2.38\bin\"

   let private getUnionCaseName (x:'a) = 
       match FSharpValue.GetUnionFields(x, typeof<'a>) with
       | case, _ -> case.Name  

   let toByteArray (sr : StreamReader) =
      use ms = new MemoryStream()
      sr.BaseStream.CopyTo(ms)
      ms.ToArray()
//      let buf = ms.GetBuffer()
//      let result = Array.zeroCreate buf.Length
//      buf.CopyTo(result, buf.Length)
//      result  

   // Shell out to run a command line program
   let private startProcessAndCaptureOutput cmd cmdParams (resultFormat : ResultFormat) (stdInContent : string) = 
      let si = 
         new System.Diagnostics.ProcessStartInfo(cmd, cmdParams,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true)
      use p = new System.Diagnostics.Process()
      p.StartInfo <- si
      if p.Start() then 
         p.StandardInput.Write(stdInContent)
         p.StandardInput.Close()
         use stdError = p.StandardError
         let message = stdError.ReadToEnd()
         if System.String.IsNullOrWhiteSpace message then
            // TODO there may be some encoding-sensitivity here when using
            // non-text graphics formats (eg. gif)
            match resultFormat with
            | Text ->
               let content = p.StandardOutput.ReadToEnd()
               CommandResult.SuccessText content
            | Binary -> 
               let content = p.StandardOutput |> toByteArray
               CommandResult.SuccessBinary content
         elif message.Contains "syntax error" then
            CommandResult.Failure (sprintf "Invalid input content: %s" message)
         else 
            CommandResult.Failure (sprintf "Unspecified error: %s" message)
      else 
         use stdError = p.StandardError
         let message = stdError.ReadToEnd()
         CommandResult.Failure message

   let Call (algo : Algo, outputType : OutputType, dotContent : string) =
      if dotContent = null then
         CommandResult.Failure "Null input"
      elif dotContent.Trim().Length = 0 then
         CommandResult.Failure "Empty input"
      else
         let commandName = (getUnionCaseName algo).ToLowerInvariant()
         let commandPath = Path.ChangeExtension(Path.Combine(graphVizPath, commandName), ".exe")
         let outputTypeParam = (getUnionCaseName outputType).ToLowerInvariant()
         let paramString = sprintf "-T%s" outputTypeParam
         startProcessAndCaptureOutput commandPath paramString (outputType.ToResultFormat()) dotContent
