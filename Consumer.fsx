#load "Common.fsx"

open System
open System.Threading
open Microsoft.WindowsAzure.Storage // Namespace for CloudStorageAccount
open Microsoft.WindowsAzure.Storage.Queue // Namespace for Queue storage types
open Common

let deleteMsg (queue: CloudQueue) = queue.DeleteMessageAsync >> Async.AwaitTask
let getNextMsg (queue: CloudQueue) (token: CancellationToken) () =
    queue.GetMessageAsync(token) |> Async.AwaitTask

let processMessages getNextMsg deleteMsg cancel =
    let rec loop () = async {
        if cancel ()
        then
            printfn "Stop processing"
            return ()
        else
            let! (msg: CloudQueueMessage) = getNextMsg ()
            printfn "Received: %s" msg.AsString
            do! msg |> deleteMsg
            return! loop ()
    }
    loop ()

let start () =
    use cts = new CancellationTokenSource()
    processMessages
        (getNextMsg Common.queue cts.Token)
        (deleteMsg Common.queue)
        (fun () -> cts.Token.IsCancellationRequested)
    |> Async.Start

    Console.ReadKey() |> ignore
    cts.Cancel()

start ()